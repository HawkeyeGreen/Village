using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Village.VillageGame.DatabaseManagement;
using Village.VillageGame.TagSystem;
using Zeus.Hermes;

namespace Village.VillageGame.World.ReactionSystem
{
    public class Substance
    {
        private string displayName;
        private static readonly string dbBaseKey = "Substances";
        private static Dictionary<string, Substance> instances = new Dictionary<string, Substance>();
        private string name;
        private TagSet tags = new TagSet();
        private Temperature meltingPoint;
        private Temperature boilingPoint;
        private Temperature ignitionPoint;

        private float viscosity = 0.0f;
        private float hardness = 0.0f;
        private float sDensity = 0.0f;
        private float fDensity = 0.0f;
        private float gDensity = 0.0f;

        public float Hardness => hardness;
        public float Viscosity => viscosity;
        public float SolidDensity => sDensity;
        public float FluidDensity => fDensity;
        public float GasDensity => gDensity;
        public string Name => name;
        public string DisplayName => Localization.Local.GetInstance().GetString(displayName);

        private Substance(string _Name)
        {
            name = _Name;
            Load();
            tags.FillTagSet(name, dbBaseKey);
        }

        private void Load()
        {
            DataTableReader reader = DBHelper.ExecuteQuery("SELECT * FROM Substances WHERE name='" + name + "';", dbBaseKey).CreateDataReader();
            if(reader.HasRows)
            {
                reader.Read();
                displayName = Convert.ToString(reader.GetString(reader.GetOrdinal("dName")));
                meltingPoint = Convert.ToDouble(reader.GetDouble(reader.GetOrdinal("Melting")));
                boilingPoint = Convert.ToDouble(reader.GetDouble(reader.GetOrdinal("Boiling")));
                ignitionPoint = Convert.ToDouble(reader.GetDouble(reader.GetOrdinal("Ignition")));

                hardness = Convert.ToSingle(reader.GetDouble(reader.GetOrdinal("Hardness")));
                viscosity = Convert.ToSingle(reader.GetDouble(reader.GetOrdinal("Viscosity")));
                sDensity = Convert.ToSingle(reader.GetDouble(reader.GetOrdinal("sDensity")));
                fDensity = Convert.ToSingle(reader.GetDouble(reader.GetOrdinal("fDensity")));
                gDensity = Convert.ToSingle(reader.GetDouble(reader.GetOrdinal("gDensity")));

            }
            reader.Close();
        }

        public static Substance GetSubstance(string name)
        {
            if (!instances.ContainsKey(name))
            {
                instances[name] = new Substance(name);
            }
            return instances[name];
        }

        public AggregatePhase GetAggregate(Temperature t)
        {
            switch (t)
            {
                case var _ when(t < meltingPoint):
                    return AggregatePhase.Solid;
                case var _ when (t > ignitionPoint):
                    return AggregatePhase.Plasma;
                case var _ when (t >= meltingPoint && t < boilingPoint):
                    return AggregatePhase.Liquid;
                case var _ when (t >= boilingPoint):
                    return AggregatePhase.Gaseous;
                default:
                    return AggregatePhase.NULL;
            }
        }

        public override string ToString()
        {
            string result = "{ Substanz: " + name + " | Name: " + Localization.Local.GetInstance().GetString(displayName) 
                + " | Mohs: " + hardness + " | Viskositaet: " + viscosity
                + " | Schmelzpunkt: " + meltingPoint + " | Dampfpunkt: " + boilingPoint + " }";
            return result;
        }
    }
}
