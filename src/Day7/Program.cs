var lines = File.ReadAllLines("input.txt");
var input = new Input(lines);

var totalWinnings1 = input.Games
    .OrderBy(game => game.HandType)
        .ThenBy(game => game.Hand, HighCardComparer.Normal)
    .Select((game, i) => (game.Bid, Rank: i + 1))
    .Aggregate(0, (sum, game) => sum + game.Bid * game.Rank);

Console.WriteLine(totalWinnings1);

var totalWinnings2 = input.Games
    .OrderBy(game => game.HandTypeWithJokers)
        .ThenBy(game => game.Hand, HighCardComparer.WithJokers)
    .Select((game, i) => (game.Bid, Rank: i + 1))
    .Aggregate(0, (sum, game) => sum + game.Bid * game.Rank);

Console.WriteLine(totalWinnings2);

class Input
{
    public Input(string[] lines)
    {
        Games = lines.Select(line =>
        {
            var parts = line.Split(' ');
            return new Game(parts[0], int.Parse(parts[1]));
        }).ToList();
    }

    public List<Game> Games { get; }
}

record Game
{
    public Game(string hand, int bid)
    {
        Hand = hand;
        Bid = bid;
        HandType = GetHandType(hand);
        HandTypeWithJokers = GetHandTypeWithJokers(hand);
    }

    public string Hand { get; }
    public int Bid { get; }
    public HandType HandType { get; }
    public HandType HandTypeWithJokers { get; }

    private HandType GetHandType(string hand)
    {
        var groups = hand.GroupBy(card => card);

        if (groups.Any(grp => grp.Count() == 5)) return HandType.FiveOfAKind;
        if (groups.Any(grp => grp.Count() == 4)) return HandType.FourOfAKind;
        if (groups.Any(grp => grp.Count() == 3) && groups.Any(grp => grp.Count() == 2)) return HandType.FullHouse;
        if (groups.Any(grp => grp.Count() == 3)) return HandType.ThreeOfAKind;
        if (groups.Count(grp => grp.Count() == 2) == 2) return HandType.TwoPair;
        if (groups.Any(grp => grp.Count() == 2)) return HandType.OnePair;
        return HandType.HighCard;
    }

    private HandType GetHandTypeWithJokers(string hand)
    {
        if (!hand.Contains('J') || hand == "JJJJJ")
        {
            return GetHandType(hand);
        }

        var mostCommonNonJokerCard = hand
            .GroupBy(card => card)
            .Where(group => group.Key != 'J')
            .MaxBy(group => group.Count())!
            .First();

        var bestHand = hand.Replace('J', mostCommonNonJokerCard);
        return GetHandType(bestHand);
    }
}

enum HandType
{
    HighCard = 0,
    OnePair = 1,
    TwoPair = 2,
    ThreeOfAKind = 3,
    FullHouse = 4,
    FourOfAKind = 5,
    FiveOfAKind = 6
}

class HighCardComparer(Dictionary<char, int> mapping) : IComparer<string>
{
    private static Dictionary<char, int> normalMapping = new()
    {
        ['2'] = 1,
        ['3'] = 2,
        ['4'] = 3,
        ['5'] = 4,
        ['6'] = 5,
        ['7'] = 6,
        ['8'] = 7,
        ['9'] = 8,
        ['T'] = 9,
        ['J'] = 10,
        ['Q'] = 11,
        ['K'] = 12,
        ['A'] = 13,
    };

    private static Dictionary<char, int> jokerMapping = new()
    {
        ['J'] = 1,
        ['2'] = 2,
        ['3'] = 3,
        ['4'] = 4,
        ['5'] = 5,
        ['6'] = 6,
        ['7'] = 7,
        ['8'] = 8,
        ['9'] = 9,
        ['T'] = 10,
        ['Q'] = 11,
        ['K'] = 12,
        ['A'] = 13,
    };

    public static HighCardComparer Normal => new HighCardComparer(normalMapping);
    public static HighCardComparer WithJokers => new HighCardComparer(jokerMapping);

    public int Compare(string? x, string? y)
    {
        if (x == y) return 0;

        var pair = x!.Zip(y!).Where(pair => pair.First != pair.Second).First();
        return mapping[pair.First].CompareTo(mapping[pair.Second]);
    }
}
