namespace DuckPond;

/// <summary>The ducks on staff. Fixed list, no database needed.</summary>
public static class DuckRoster
{
    public static readonly IReadOnlyList<Duck> Ducks =
    [
        new(1, "Sir Quackington",      "Off-by-one errors",              "\U0001F986"),
        new(2, "Dr. Mallard",          "Race conditions",                "\U0001F9EA"),
        new(3, "Bathtub Betty",        "\"Works on my machine\"",        "\U0001F6C1"),
        new(4, "Admiral Featherstone", "Merge conflicts",                "\U00002693"),
        new(5, "Rubber Ducky Jr.",     "undefined is not a function",    "\U0001F423"),
        new(6, "Professor Waddles",    "It was DNS. It is always DNS.",  "\U0001F393")
    ];

    public static Duck? Find(int id) => Ducks.FirstOrDefault(duck => duck.Id == id);
}
