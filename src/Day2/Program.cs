using System.Collections.Immutable;
using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");
var games = lines.Select(line => new Game(line));

var gameConfig = new GameConfiguration
{
    MaxRed = 12,
    MaxGreen = 13,
    MaxBlue = 14,
};

var sumOfPossibleGameIds = games
    .Where(game => game.IsPossible(gameConfig))
    .Sum(game => game.Id);

Console.WriteLine(sumOfPossibleGameIds);

var gamePower = games
    .Select(game => game.MinimumGameConfiguration().Power)
    .Sum();

Console.WriteLine(gamePower);

record GameConfiguration
{
    public int MaxRed { get; init; }
    public int MaxGreen { get; init; }
    public int MaxBlue { get; init; }

    public int Power => MaxRed * MaxGreen * MaxBlue;
}

record Game
{
    private static Regex GameIdRegex = new Regex("(?<=Game )\\d+");
    private static Regex GameSetsRegex = new Regex("(?<=[:;])[ a-z0-9,]+");

    public Game(string gameDef)
    {
        Id = GameIdRegex.Match(gameDef).Value.ToInt();
        var matches = GameSetsRegex.Matches(gameDef);
        Sets = matches.Select(match => new Set(match.Value)).ToImmutableList();
    }

    public int Id { get; init; }
    public ImmutableList<Set> Sets { get; init; }

    public bool IsPossible(GameConfiguration gameConfiguration)
    {
        return !Sets.Any(set =>
            set.Red > gameConfiguration.MaxRed ||
            set.Green > gameConfiguration.MaxGreen ||
            set.Blue > gameConfiguration.MaxBlue);
    }

    public GameConfiguration MinimumGameConfiguration()
    {
        return new GameConfiguration
        {
            MaxRed = Sets.Select(set => set.Red).Max(),
            MaxGreen = Sets.Select(set => set.Green).Max(),
            MaxBlue = Sets.Select(set => set.Blue).Max(),
        };
    }
}

record Set
{
    private static Regex SetDefRegex = new Regex("(?<count>\\d+) (?<color>[a-z]+)");
    public Set(string setDef)
    {
        var parts = setDef.Split(",");
        foreach (var part in parts)
        {
            var match = SetDefRegex.Match(part);

            switch (match.Groups["color"].Value)
            {
                case "red": Red = match.Groups["count"].Value.ToInt(); break;
                case "green": Green = match.Groups["count"].Value.ToInt(); break;
                case "blue": Blue = match.Groups["count"].Value.ToInt(); break;
            }
        }
    }

    public int Red { get; }
    public int Green { get; }
    public int Blue { get; }
}

public static class Extensions
{
    public static int ToInt(this string value) => int.Parse(value);
}