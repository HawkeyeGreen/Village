
namespace Village.VillageGame.BodySystem.Wounds
{
    struct LayerDamage
    {
        LayerDamageDepth depth;
        double area;
        WoundType type;

        public LayerDamageDepth Depth => depth;
        public double Area => area;
        public WoundType Type => type;

        public static LayerDamage NoDamage => new LayerDamage(LayerDamageDepth.Surface, 0, WoundType.Not_A_Touch);

        public LayerDamage(LayerDamageDepth depth, double area, WoundType type)
        {
            this.depth = depth;
            this.area = area;
            this.type = type;
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