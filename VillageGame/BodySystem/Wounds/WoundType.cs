
namespace Village.VillageGame.BodySystem.Wounds
{
    enum WoundType
    {
        Scratch,
        Blunt,
        Cut,
        Lesion,
        Puncture
    }

    public static class WoundTypeConversion
    {
        private static string[] woundTypes = new string[5]{
            "Scratch",
            "Blunt",
            "Cut",
            "Lesion",
            "Puncture"
        };

        public static string[] WoundTypes => WoundTypes;

    }
}