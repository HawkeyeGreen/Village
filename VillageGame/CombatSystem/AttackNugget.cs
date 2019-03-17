using Village.VillageGame.Items;

namespace Village.VillageGame.CombatSystem
{
    struct AttackNugget
    {
        private float force;
        public float Force { get => force; set => force = value; }

        private IWeapon weapon;
        public IWeapon Weapon => weapon;

        private double hitChance;
        public double HitChance => hitChance;

        private double hitArea;
        public double HitArea => hitArea;
    }
}