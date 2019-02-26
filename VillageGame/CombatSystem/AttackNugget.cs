using Village.VillageGame.Items;

namespace Village.VillageGame.CombatSystem
{
    struct AttackNugget
    {
        private float force;
        public float Force => force;

        private IWeapon weapon;
        public IWeapon Weapon => weapon;

        private double hitChance;
        public double HitChance => hitChance;


    }
}