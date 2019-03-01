
using System;
using System.Collections.Generic;
using System.Data;
using Village.VillageGame.DatabaseManagement;

namespace Village.VillageGame.BodySystem.Wounds
{
    struct LayerDamage
    {
        private int ID;

        private LayerDamageDepth depth;
        private double area;
        private WoundType type;
        private List<string> damagedFeatures;

        public LayerDamageDepth Depth => depth;
        public double Area => area;
        public WoundType Type => type;
        public List<string> DamagedFeatures => damagedFeatures;


        public static LayerDamage NoDamage => new LayerDamage(LayerDamageDepth.NODAMAGE, 0, WoundType.Not_A_Touch, null);

        public static LayerDamage Repelled => new LayerDamage(LayerDamageDepth.REPELLED, 0, WoundType.Not_A_Touch, null);

        /// <summary>
        /// Erstellt einen Schaden. !ID ist zuerst auf -1 gesetzt. Bis der Schaden gespeichert wurde,
        /// hat er keine individuelle ID.
        /// </summary>
        /// <param name="depth">Die Tiefe des Schadens.</param>
        /// <param name="area">Die Fläche des Angriffs.</param>
        /// <param name="type">Die Art der Wunde, die verursacht werden kann.</param>
        /// <param name="dmgFeatures">Die verletzten Features in diesem Layer. Gesichert als strings.</param>
        public LayerDamage(LayerDamageDepth depth, double area, WoundType type, List<string> dmgFeatures)
        {
            ID = -1;
            this.depth = depth;
            this.area = area;
            this.type = type;
            damagedFeatures = dmgFeatures;
        }

        public LayerDamage(int ID, string DB)
        {
            this.ID = ID;
            // Lade Fläche, Tiefe und Typ aus der DB
            DataTableReader reader = DBHelper.ExecuteQuery("SELECT * FROM LayerDamages WHERE ID=" + ID + ";", DB).CreateDataReader();
            reader.Read();
            area = Convert.ToDouble(reader.GetDouble(reader.GetOrdinal("Area")));
            depth = LayerDamageDepthConversion.ConvertToLayerDamageDepth(Convert.ToString(reader.GetString(reader.GetOrdinal("DamageDepth"))));
            type = WoundTypeConversion.ConvertToWoundType(Convert.ToString(reader.GetString(reader.GetOrdinal("DamageDepth"))));
            reader.Close();

            // Lade alle beschädigten Features aus der DB
            reader = DBHelper.ExecuteQuery("SELECT * FROM LayerDamagesFeatures WHERE ID=" + ID + ";", DB).CreateDataReader();
            damagedFeatures = new List<string>();
            while (reader.Read())
            {
                damagedFeatures.Add(Convert.ToString(reader.GetString(reader.GetOrdinal("fName"))));
            }
            reader.Close();
        }
    }

    /// <summary>
    /// Ein enum zur Erleichterung der Unterscheidung von Wundtiefen.
    /// </summary>
    public enum LayerDamageDepth
    {
        NODAMAGE,
        REPELLED,
        Surface,
        Just_A_Scratch,
        HalfDeep,
        Deep,
        Penetrated
    }

    public static class LayerDamageDepthConversion
    {
        private static string[] layerDamageDepths = new string[7]{
            "NO_DAMAGE",
            "REPELLED",
            "Surface",
            "Just a Scratch",
            "Half Deep",
            "Deep",
            "Penetrated"
        };

        public static string[] LayerDamageDepths => layerDamageDepths;

        public static LayerDamageDepth ConvertToLayerDamageDepth(string name)
        {
            for (int i = 0; i < layerDamageDepths.Length; i++)
            {
                if (layerDamageDepths[i] == name) { return ConvertToLayerDamageDepth(i); }
            }

            return LayerDamageDepth.NODAMAGE;
        }

        public static LayerDamageDepth ConvertToLayerDamageDepth(int index)
        {
            if (index < layerDamageDepths.Length)
            {
                return (LayerDamageDepth)index;
            }
            return LayerDamageDepth.NODAMAGE;
        }
    }
}