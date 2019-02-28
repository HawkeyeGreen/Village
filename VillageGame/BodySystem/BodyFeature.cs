using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Village.VillageGame.BodySystem.Wounds;
using Village.VillageGame.DatabaseManagement;
using Village.VillageGame.Helper;

namespace Village.VillageGame.BodySystem
{
    class BodyFeature
    {
        private static Dictionary<string, BodyFeature> instances = new Dictionary<string, BodyFeature>();

        private double hitChancePerHitArea = 0.00;
        private double hitMinimum = 30;
        private double hitMaximum = 75;

        private Dictionary<WoundType, double> modifierPerHitType = new Dictionary<WoundType, double>();

        private readonly string name;

        public string Name => name;

        /// <summary>
        /// Lädt das gecachte BodyFeature oder lädt es in den Cache.
        /// Dieser Aufruf geht davon aus, das unter dem Namen in der Body-DB ein Eintrag besteht.
        /// </summary>
        /// <param name="name">Der Name des BodyFeatures.</param>
        /// <returns>Das gesuchte BodyFeature.</returns>
        public static BodyFeature GetBodyFeature(string name)
        {
            return GetBodyFeature(name, DBHelper.BODY_DB_KEY);
        }

        /// <summary>
        /// Diese Methode ruft das BodyFeature entweder aus dem Cache oder aus der angegebenen DB ab.
        /// </summary>
        /// <param name="name">Der Name des BodyFeatures.</param>
        /// <param name="DB">Die Datenbank, in der </param>
        /// <returns>Das gesuchte BodyFeature.</returns>
        public static BodyFeature GetBodyFeature(string name, string DB)
        {
            if (!instances.ContainsKey(name))
            {
                instances[name] = new BodyFeature(name, DB);
            }
            return instances[name];
        }

        private BodyFeature(string name, string DBKey)
        {
            this.name = name;
            Load(name, DBKey);
        }

        private void Load(string name, string DBKey)
        {

        }

        public bool Hitted(double area, WoundType woundType, double multiplier = 1, double addition = 0)
        {
            double chanceToGetHit = area * hitChancePerHitArea;

            if (modifierPerHitType.ContainsKey(woundType))
            {
                chanceToGetHit *= modifierPerHitType[woundType];
            }

            if (chanceToGetHit < hitMinimum) { chanceToGetHit = hitMinimum; }
            if (chanceToGetHit > hitMaximum) { chanceToGetHit = hitMaximum; }

            chanceToGetHit *= multiplier;
            chanceToGetHit += addition;

            double chanceThrow = RandomHelper.random.NextDouble() * 100 + 1;

            if (chanceThrow > chanceToGetHit)
            {
                return true;
            }
            return false;
        }
    }
}
