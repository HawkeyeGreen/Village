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
        private static Dictionary<string, Substance> instances = new Dictionary<string, Substance>();
        private string name;
        private TagSet tags = new TagSet();
        private Temperature meltingPoint;
        private Temperature boilingPoint;
        private Temperature ignitionPoint;

        private float mass = 1;
        private float viscosity = 0.0f;
        private float hardness = 0.0f;
        private float sDensity = 0.0f;
        private float fDensity = 0.0f;
        private float gDensity = 0.0f;

        public float Mass => mass;
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
            tags.FillTagSet(name, DBHelper.SUBSTANCES_DB_KEY);
        }

        private void Load()
        {
            DataTableReader reader = DBHelper.ExecuteQuery("SELECT * FROM Substances WHERE name='" + name + "';", DBHelper.SUBSTANCES_DB_KEY).CreateDataReader();
            if (reader.HasRows)
            {
                reader.Read();
                displayName = Convert.ToString(reader.GetString(reader.GetOrdinal("dName")));
                meltingPoint = Convert.ToDouble(reader.GetDouble(reader.GetOrdinal("Melting")));
                boilingPoint = Convert.ToDouble(reader.GetDouble(reader.GetOrdinal("Boiling")));
                ignitionPoint = Convert.ToDouble(reader.GetDouble(reader.GetOrdinal("Ignition")));

                mass = Convert.ToSingle(reader.GetDouble(reader.GetOrdinal("mass")));
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

        public Aggregate GetAggregate(Temperature t)
        {
            switch (t)
            {
                case var _ when (t < meltingPoint):
                    return Aggregate.Solid;
                case var _ when (t > ignitionPoint):
                    return Aggregate.Plasma;
                case var _ when (t >= meltingPoint && t < boilingPoint):
                    return Aggregate.Liquid;
                case var _ when (t >= boilingPoint):
                    return Aggregate.Gaseous;
                default:
                    return Aggregate.NULL;
            }
        }

        public float GetDensity(Aggregate aggregate)
        {
            switch (aggregate)
            {
                case Aggregate.Solid:
                    return SolidDensity;
                case Aggregate.Liquid:
                    return FluidDensity;
                case Aggregate.Gaseous:
                    return GasDensity;
                default:
                    return 1;
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
