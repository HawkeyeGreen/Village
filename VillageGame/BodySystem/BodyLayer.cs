using System.Collections.Generic;
using Village.VillageGame.World.ReactionSystem;
using Village.VillageGame.BodySystem.Wounds;
using Village.VillageGame.CombatSystem;
using System.Data;
using Village.VillageGame.DatabaseManagement;

namespace Village.VillageGame.BodySystem
{
    struct BodyLayer
    {
        private int ID;

        private List<LayerDamage> damages;
        private BodyFeature[] features;
        private Material material;

        public BodyFeature[] Features => features;
        public List<LayerDamage> Damages => damages;
        public Material Material => material;

        public BodyLayer(int layerID, string materialName, string DB)
        {
            damages = new List<LayerDamage>();
            ID = layerID;

            DataTableReader reader;
            material = new Material(materialName, DB);
            reader = DBHelper.ExecuteQuery("SELECT * FROM BodyLayers WHERE ID=" + layerID + ";", DB).CreateDataReader();



        }

        public LayerDamage ApplyAttack(AttackNugget attack)
        {
            MaterialAnswer answer = material.ApplyForce(attack.Force, attack.Weapon.Sharpness, attack.Weapon.Material);
            LayerDamage damage;
            switch (answer)
            {
                case MaterialAnswer.Repelled:
                    return LayerDamage.NoDamage;
                case MaterialAnswer.Withstand:
                    return LayerDamage.NoDamage;
                case MaterialAnswer.Penetrated:
                    damage = new LayerDamage(LayerDamageDepth.Penetrated, attack.HitArea, WoundType.Puncture, CheckForFeatureDamages(WoundType.Puncture, attack.HitArea));
                    damages.Add(damage);
                    return damage;
                case MaterialAnswer.Cutted:
                    damage = new LayerDamage(LayerDamageDepth.Deep, attack.HitArea, WoundType.Cut, CheckForFeatureDamages(WoundType.Cut, attack.HitArea));
                    damages.Add(damage);
                    return damage;
                case MaterialAnswer.Bend:
                    damage = new LayerDamage(LayerDamageDepth.HalfDeep, attack.HitArea, WoundType.Blunt, CheckForFeatureDamages(WoundType.Blunt, attack.HitArea));
                    damages.Add(damage);
                    return damage;
                case MaterialAnswer.Broken:
                    damage = new LayerDamage(LayerDamageDepth.Deep, attack.HitArea, WoundType.Blunt, CheckForFeatureDamages(WoundType.Blunt, attack.HitArea));
                    damages.Add(damage);
                    return damage;
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
    }
}