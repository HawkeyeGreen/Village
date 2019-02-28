
namespace Village.VillageGame.BodySystem.Wounds
{
    enum WoundType
    {
        Not_A_Touch,
        Scratch,
        Blunt,
        Cut,
        Lesion,
        Puncture
    }

    public static class WoundTypeConversion
    {
        private static string[] woundTypes = new string[6]{
            "Not a touch",
            "Scratch",
            "Blunt",
            "Cut",
            "Lesion",
            "Puncture"
        };

        public static string[] WoundTypes => WoundTypes;

    }
}