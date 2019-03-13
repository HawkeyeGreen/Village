/// <summary>
/// Stellt alle vorkommenden Aggregatszustände von Stoffen im Spiel dar.
/// </summary>
public enum Aggregate
{
    NULL,
    Solid,
    Liquid,
    Gaseous,
    Plasma
}

/// <summary>
/// Stellt einen Aggegratszustandsübergang dar.
/// </summary>
public enum AggregateChange
{
    Stable, // Keine Änderung
    Liquifying, // Verflüssigen
    Solidifying, // Verfestigen
    Vaporizing, // Verdampfen
    Ionizating // Ionisierung zu Plasma
}

/// <summary>
/// Bietet eine Liste zur Übersetzung von Aggregatszuständen (und ihren Übergängen) in Form eines Strings dar.
/// </summary>
public static class AggregateConversion
{
    private static string[] aggregate = new string[5]{
            "NULL",
            "Solid",
            "Liquid",
            "Gaseous",
            "Plasma"
    };

    private static string[] changes = new string[5]{
            "Stable",
            "Liquifying",
            "Solidifying",
            "Vaporizing",
            "Ionizating"
    };

    public static string[] Aggregate { get => aggregate; }

    public static string[] Changes { get => changes; }
}