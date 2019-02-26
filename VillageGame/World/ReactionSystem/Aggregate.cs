public enum AggregatePhase
{
    NULL,
    Solid,
    Liquid,
    Gaseous,
    Plasma
}

public static class AggregateConversion
{
    private static string[] aggregate = new string[5]{
            "NULL",
            "Solid",
            "Liquid",
            "Gaseous",
            "Plasma"
    };

    public static string[] Aggregate { get => aggregate; }

}