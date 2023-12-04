using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");
var cards = lines.Select(line => new Card(line)).ToList();

var sumOfPoints = cards
    .Select(card => card.Points())
    .Sum();

Console.WriteLine(sumOfPoints);

// key: card id; value: number of copies of that card
Dictionary<int, int> copies = new();
cards.ForEach(card => copies.Add(card.Id, 0));
foreach (Card card in cards)
{
    var matches = card.Matches();
    for (int i = 1; i <= matches.Count && i < cards.Count; i++)
    {
        copies[card.Id + i] += 1 + copies[card.Id];
    }
}

var sumOfCards = cards.Count + copies.Values.Sum();

Console.WriteLine(sumOfCards);

class Card
{
    private readonly Regex IdRegex = new Regex("\\d+(?=:)");

    public Card(string card)
    {
        Id = int.Parse(IdRegex.Match(card).Value);
        var parts = card.Split([':', '|']);

        WinningNumbers = parts[1]
            .Split(' ')
            .Where(num => !string.IsNullOrWhiteSpace(num))
            .Select(int.Parse)
            .ToArray();

        Numbers = parts[2]
            .Split(' ')
            .Where(num => !string.IsNullOrWhiteSpace(num))
            .Select(int.Parse)
            .ToList();
    }

    public int Id { get; }
    public IReadOnlyList<int> WinningNumbers { get; }
    public IReadOnlyList<int> Numbers { get; }

    public IReadOnlyList<int> Matches() => WinningNumbers.Intersect(Numbers).ToArray();

    public int Points()
    {
        var matches = Matches();
        return matches.Any() ? (int)Math.Pow(2, matches.Count() - 1) : 0;
    }
}
