
using System.Collections.Generic;

namespace Village.VillageGame.BodySystem.Wounds
{
    struct LayerDamage
    {
        private LayerDamageDepth depth;
        private double area;
        private WoundType type;
        private List<string> damagedFeatures;

        public LayerDamageDepth Depth => depth;
        public double Area => area;
        public WoundType Type => type;
        public List<string> DamagedFeatures => damagedFeatures;


        public static LayerDamage NoDamage => new LayerDamage(LayerDamageDepth.Surface, 0, WoundType.Not_A_Touch, null);

        public LayerDamage(LayerDamageDepth depth, double area, WoundType type, List<string> dmgFeatures)
        {
            this.depth = depth;
            this.area = area;
            this.type = type;
            damagedFeatures = dmgFeatures;
        }

    }

    /// <summary>
    /// Ein enum zur Erleichterung der Unterscheidung von Wundtiefen.
    /// </summary>
    enum LayerDamageDepth
    {
        Surface,
        Just_A_Scratch,
        HalfDeep,
        Deep,
        Penetrated
    }

    public static class LayerDamageDepthConversion
    {
        private static string[] layerDamageDepths = new string[5]{
            "Surface",
            "Just a Scratch",
            "Half Deep",
            "Deep",
            "Penetrated"
        };

        public static string[] LayerDamageDepths => layerDamageDepths;

    }
}