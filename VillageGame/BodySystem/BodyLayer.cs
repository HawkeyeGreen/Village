﻿using System.Collections.Generic;
using Village.VillageGame.World.ReactionSystem;
using Village.VillageGame.BodySystem.Wounds;
using Village.VillageGame.CombatSystem;
using System.Data;
using Village.VillageGame.DatabaseManagement;
using System;

namespace Village.VillageGame.BodySystem
{
    struct BodyLayer
    {
        private int ID;

        private int thickness;
        private BodyFeature[] features;
        private Material material;

        public BodyFeature[] Features => features;
        public Material Material => material;
        public int Thickness { get => thickness; set => thickness = value; }

        public double ForceReduction => thickness * material.ForceReduction;

        /// <summary>
        /// Lädt ein Layer-Template aus der angegebenen Datenbank.
        /// </summary>
        /// <param name="layerTID">Kennung des Layers</param>
        /// <param name="DB">DB in der sich der Layer befindet</param>
        public BodyLayer(string DB, int layerTID)
        {
            ID = -1; //  Noch NICHT zugewiesen! Erst beim abspeichern entsteht die 'echte' ID
            DataTableReader reader;

            // Lade das Material aus der DB
            reader = DBHelper.ExecuteQuery("SELECT * FROM BodyLayerTemplates WHERE ID=" + layerTID + ";", DB).CreateDataReader();
            reader.Read();
            material = new Material(Convert.ToString(reader.GetString(reader.GetOrdinal("mName"))), DB);
            thickness = Convert.ToInt32(reader.GetInt32(reader.GetOrdinal("Thickness")));
            reader.Close();

            // Lade die Layer-Features aus der DB
            reader = DBHelper.ExecuteQuery("SELECT * FROM BodyLayerFeatureTemplates WHERE lID=" + layerTID + ";", DB).CreateDataReader();
            List<string> _features = new List<string>();
            while (reader.Read())
            {
                _features.Add(Convert.ToString(reader.GetString(reader.GetOrdinal("fName"))));
            }
            features = new BodyFeature[_features.Count];
            for (int i = 0; i < _features.Count; i++)
            {
                features[i] = BodyFeature.GetBodyFeature(_features[i], DB);
            }
            reader.Close();
        }

        /// <summary>
        /// Dieser Konstruktor lädt eine Layer-Instanz aus der angegebenen DB.
        /// </summary>
        /// <param name="layerID">Die ID des Layers.</param>
        /// <param name="DB">Die Datenbank, in der der Layer abegespeichert wurde.</param>
        public BodyLayer(int layerID, string DB)
        {
            ID = layerID;
            DataTableReader reader;

            // Lade das Material aus der DB
            reader = DBHelper.ExecuteQuery("SELECT * FROM BodyLayers WHERE ID=" + layerID + ";", DB).CreateDataReader();
            reader.Read();
            material = new Material(Convert.ToString(reader.GetString(reader.GetOrdinal("mName"))), Convert.ToString(reader.GetString(reader.GetOrdinal("mDB"))));
            thickness = Convert.ToInt32(reader.GetInt32(reader.GetOrdinal("Thickness")));
            reader.Close();

            // Lade die Layer-Features aus der DB
            reader = DBHelper.ExecuteQuery("SELECT * FROM BodyLayerFeatures WHERE lID=" + layerID + ";", DB).CreateDataReader();
            List<string> _features = new List<string>();
            List<string> _featureDBs = new List<string>();
            while (reader.Read())
            {
                _features.Add(Convert.ToString(reader.GetString(reader.GetOrdinal("fName"))));
                _featureDBs.Add(Convert.ToString(reader.GetString(reader.GetOrdinal("fDB"))));
            }
            features = new BodyFeature[_features.Count];
            for (int i = 0; i < _features.Count; i++)
            {
                features[i] = BodyFeature.GetBodyFeature(_features[i], _featureDBs[i]);
            }
            reader.Close();
        }

        public LayerDamage ApplyAttack(AttackNugget attack)
        {
            MaterialAnswer answer = material.ApplyForce(attack.Force, attack.Weapon.Sharpness, attack.Weapon.Material);
            switch (answer)
            {
                case MaterialAnswer.Repelled:
                    return LayerDamage.Repelled;
                case MaterialAnswer.Withstand:
                    return LayerDamage.NoDamage;
                case MaterialAnswer.Penetrated:
                    return new LayerDamage(LayerDamageDepth.Penetrated, attack.HitArea, WoundType.Puncture, CheckForFeatureDamages(WoundType.Puncture, attack.HitArea));
                case MaterialAnswer.Cutted:
                    return new LayerDamage(LayerDamageDepth.Deep, attack.HitArea, WoundType.Cut, CheckForFeatureDamages(WoundType.Cut, attack.HitArea));
                case MaterialAnswer.Bend:
                    return new LayerDamage(LayerDamageDepth.HalfDeep, attack.HitArea, WoundType.Blunt, CheckForFeatureDamages(WoundType.Blunt, attack.HitArea));
                case MaterialAnswer.Broken:
                    return new LayerDamage(LayerDamageDepth.Deep, attack.HitArea, WoundType.Blunt, CheckForFeatureDamages(WoundType.Blunt, attack.HitArea));
                default:
                    return LayerDamage.NoDamage;
            }
        }

        private List<string> CheckForFeatureDamages(WoundType wType, double hitArea)
        {
            List<string> dmgFeatures = new List<string>();

            foreach (BodyFeature feature in features)
            {
                if (feature.Hitted(hitArea, wType))
                {
                    dmgFeatures.Add(feature.Name);
                }
            }

            return dmgFeatures;
        }

        /// <summary>
        /// Erstellt in der gegebenen DB alle Tabellen, die BodyLayer verwendet.
        /// </summary>
        /// <param name="DB"></param>
        public static void CreateTablesInDB(string DB)
        {
            DBHelper.ExecuteCommandNonQuery("CREATE TABLE IF NOT EXISTS BodyLayers (" +
                "ID INTEGER NOT NULL PRIMARY KEY," +
                "mName TEXT NOT NULL," +
                "Thickness INTEGER NOT NULL,"+
                "mDB TEXT DEFAULT '" + DBHelper.SUBSTANCES_DB_KEY + "'" +
                ");", DB);

            DBHelper.ExecuteCommandNonQuery("CREATE TABLE IF NOT EXISTS BodyLayerFeatures (" +
                "lID INTEGER NOT NULL," +
                "fName TEXT NOT NULL," +
                "fDB TEXT DEFAULT '" + DBHelper.BODY_DB_KEY + "'," +
                "PRIMARY KEY(lID, fName)" +
                ");", DB);
        }
    }
}