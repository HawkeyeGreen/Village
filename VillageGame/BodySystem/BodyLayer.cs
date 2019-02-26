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

        public void ApplyAttack(AttackNugget attack)
        {
            
        }
    }
}