using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Village.VillageGame.DatabaseManagement;

namespace Village.VillageGame.World.ReactionSystem
{
    public class Material
    {
        private readonly string name;
        private string displayName_Key;
        private string displayDescr_Key;

        private static readonly string breakable = "Breakable";
        private static readonly string bendable = "Bendable";
        private static readonly string penetrable = "Penetrable";
        private static readonly string cutable = "Cutable";

        private Substance substance;

        public string Name => name;

        public string DisplayNameKey => displayName_Key;
        public string DisplayName => Localization.Local.GetInstance().GetString(displayName_Key);

        public string DisplayDescriptionKey => displayDescr_Key;
        public string DisplayDescription => Localization.Local.GetInstance().GetString(displayDescr_Key);

        public bool Breakable => tags.ContainsTag(breakable);
        public bool Bendable => tags.ContainsTag(bendable);
        public bool Penetrable => tags.ContainsTag(penetrable);
        public bool Cutable => tags.ContainsTag(cutable);

        public double ForceReduction => forceReductionPerCM;

        public Substance Substance => substance;

        // Die Menge an Kraft, welche pro cm Dicke aufgefangen wird
        private double forceReductionPerCM;

        // Ab welcher Schärfe dieses Material geschnitten werden kann
        private int cuttingPoint;

        /* 
         * Kraft, die fürs brechen etc. pro Flächeneinheit anliegen muss.
         */
        private double breakingForce;
        private double bendingForce;
        private double penetratingForce;

        private TagSystem.TagSet tags;

        /// <summary>
        /// Initialisiert das Material aus der Substance-DB im Main-Ordner.
        /// </summary>
        /// <param name="name">Der einzigartige Materialname.</param>
        public Material(string name)
        {
            tags = new TagSystem.TagSet();
            tags.FillTagSet(name, DBHelper.SUBSTANCES_DB_KEY);
            this.name = name;
            Load(DBHelper.ExecuteQuery("SELECT * FROM Materials WHERE name='" + name + "';", DBHelper.SUBSTANCES_DB_KEY).CreateDataReader());
        }

        /// <summary>
        /// Lädt das Material aus der angegebenen DB.
        /// </summary>
        /// <param name="name">Der einzigartige Materialname.</param>
        /// <param name="DB">Die Datenbank, in der das Material angelegt wurde.</param>
        public Material(string name, string DB)
        {
            tags = new TagSystem.TagSet();
            tags.FillTagSet(name, DB);
            this.name = name;
            Load(DBHelper.ExecuteQuery("SELECT * FROM Materials WHERE name='" + name + "';", DB).CreateDataReader());
        }

        private void Load(DataTableReader reader)
        {
            reader.Read();

            displayName_Key = Convert.ToString(reader.GetString(reader.GetOrdinal("displayKey")));
            displayDescr_Key = Convert.ToString(reader.GetString(reader.GetOrdinal("descrKey")));

            forceReductionPerCM = Convert.ToDouble(reader.GetDouble(reader.GetOrdinal("ForceReduction")));

            string subs = Convert.ToString(reader.GetString(reader.GetOrdinal("substance")));

            if (Cutable)
            {
                cuttingPoint = Convert.ToInt32(reader.GetInt64(reader.GetOrdinal("cuttingSharpness")));
            }

            if (Breakable)
            {
                breakingForce = Convert.ToDouble(reader.GetDouble(reader.GetOrdinal("breakingForce")));
            }

            if (Bendable)
            {
                bendingForce = Convert.ToDouble(reader.GetDouble(reader.GetOrdinal("bendingForce")));
            }

            if (Penetrable)
            {
                penetratingForce = Convert.ToDouble(reader.GetDouble(reader.GetOrdinal("penetratingForce")));
            }

            reader.Close();

            substance = Substance.GetSubstance(subs);
        }

        /// <summary>
        /// Überprüft, wie das Material reagiert, wenn die angegebene Form von Kraft mit der gegebenen Substance ausgeübt wird.
        /// </summary>
        /// <param name="forcePerUnit">Newton pro mm2</param>
        /// <param name="sharpness">Schärfegrad</param>
        /// <param name="substance2">Die Substanz aus der der Träger der Kraft besteht</param>
        /// <returns></returns>
        public MaterialAnswer ApplyForce(double forcePerUnit, int sharpness, Material material)
        {
            // Ist dieses Material härter, so wirft es den Angriff zurück
            if (substance.Hardness > material.Substance.Hardness)
            {
                return MaterialAnswer.Repelled;
            }

            // Die Substance ist scharf genug, um dieses Substance zu schneiden
            if (Cutable && (sharpness > cuttingPoint))
            {
                return MaterialAnswer.Cutted;
            }


            if (Breakable && (forcePerUnit > breakingForce)) { return MaterialAnswer.Broken; }
            else if (Penetrable && (forcePerUnit > penetratingForce)) { return MaterialAnswer.Penetrated; }
            else if (Bendable && (forcePerUnit > bendingForce)) { return MaterialAnswer.Bend; }

            return MaterialAnswer.Withstand;
        }


    }
}
