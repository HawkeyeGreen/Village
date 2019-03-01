
namespace Village.VillageGame.BodySystem.Wounds
{
    public enum WoundType
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

        public static WoundType ConvertToWoundType(string name)
        {
            for (int i = 0; i < woundTypes.Length; i++)
            {
                if (woundTypes[i] == name) { return ConvertToWoundType(i); }
            }

            return WoundType.Not_A_Touch;
        }

        public static WoundType ConvertToWoundType(int index)
        {
            if (index < woundTypes.Length)
            {
                return (WoundType)index;
            }
            return WoundType.Not_A_Touch;
        }
    }
}