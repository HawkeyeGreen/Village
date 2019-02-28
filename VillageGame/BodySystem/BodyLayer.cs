using System.Collections.Generic;
using Village.VillageGame.World.ReactionSystem;
using Village.VillageGame.BodySystem.Wounds;
using Village.VillageGame.CombatSystem;

namespace Village.VillageGame.BodySystem
{
    struct BodyLayer
    {
        private List<LayerDamage> damages;
        private string[] features;
        private Material material;

        public string[] Features => features;
        public List<LayerDamage> Damages => damages;
        public Material Material => material;

        public BodyLayer(string materialName, List<string> _features, string materialDB = "")
        {
            damages = new List<LayerDamage>();

            if(materialDB != "")
            {
                material = new Material(materialName, materialDB);
            }
            else
            {
                material = new Material(materialName);
            }

            features = new string[_features.Count];
            _features.CopyTo(features);
            
        }

        public LayerDamage ApplyAttack(AttackNugget attack)
        {
            MaterialAnswer answer = material.ApplyForce(attack.Force, attack.Weapon.Sharpness, attack.Weapon.Material);
            LayerDamage damage;
            switch (answer)
            {
                case MaterialAnswer.Repelled:
                    return new LayerDamage(LayerDamageDepth.Surface, attack.HitArea , WoundType.Scratch);
                case MaterialAnswer.Withstand:
                    return LayerDamage.NoDamage;
                case MaterialAnswer.Penetrated:
                    damage = new LayerDamage(LayerDamageDepth.Penetrated, attack.HitArea, WoundType.Puncture);
                    damages.Add(damage);
                    return damage;
                case MaterialAnswer.Cutted:
                    damage = new LayerDamage(LayerDamageDepth.Deep, attack.HitArea, WoundType.Cut);
                    damages.Add(damage);
                    return damage;
                case MaterialAnswer.Bend:
                    damage = new LayerDamage(LayerDamageDepth.HalfDeep, attack.HitArea, WoundType.Blunt);
                    damages.Add(damage);
                    return damage;
                case MaterialAnswer.Broken:
                    damage = new LayerDamage(LayerDamageDepth.Deep, attack.HitArea, WoundType.Blunt);
                    damages.Add(damage);
                    return damage;
                default:
                    return LayerDamage.NoDamage;
            }
        }
    }
}