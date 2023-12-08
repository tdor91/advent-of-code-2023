using System.Text.RegularExpressions;

var input = new Input("input.txt");

int step = 0;
var current = "AAA";
while (current != "ZZZ")
{
    var move = input.Moves[step % input.Moves.Length];
    current = move == 'L' ? input.Tree[current].Left : input.Tree[current].Right;
    step++;
}

Console.WriteLine(step);

Dictionary<string, long> cycleLengths = new();
foreach (var start in input.Tree.Keys.Where(node => node.EndsWith('A')))
{
    step = 0;
    current = start;
    while (!current.EndsWith('Z'))
    {
        var move = input.Moves[step % input.Moves.Length];
        current = move == 'L' ? input.Tree[current].Left : input.Tree[current].Right;
        step++;
    }
    cycleLengths.Add(start, step);
}

var lcm = MathUtil.LeastCommonMultiple(cycleLengths.Values);
Console.WriteLine(lcm);

// correct, but inefficient way. Do not even attempt it this way.
void BruteForcePart2(Input input)
{
    var currents = input.Tree.Keys.Where(node => node.EndsWith('A'));
    var step = 0;
    while (currents.Any(node => !node.EndsWith('Z')))
    {
        var move = input.Moves[step % input.Moves.Length];
        currents = currents.Select(node => move == 'L' ? input.Tree[node].Left : input.Tree[node].Right).ToArray();
        step++;
    }
    Console.WriteLine(step);
}

partial class Input
{
    public Input(string path)
    {
        var lines = File.ReadAllLines(path);
        Moves = lines.First();
        Tree = new();

        foreach (var line in lines.Skip(2))
        {
            var matches = InputRegex().Matches(line);
            Tree.TryAdd(matches[0].Value, (matches[1].Value, matches[2].Value));
        }
    }

    public string Moves { get; }
    public Dictionary<string, (string Left, string Right)> Tree { get; }

    [GeneratedRegex("[A-Z0-9]{3}", RegexOptions.None)]
    private static partial Regex InputRegex();
}

static class MathUtil
{
    public static long LeastCommonMultiple(IEnumerable<long> numbers)
    {
        return numbers.Aggregate(LeastCommonMultiple2);
    }

    public static long LeastCommonMultiple2(long a, long b)
    {
        return Math.Abs(a * b) / GreatestCommonDivisor(a, b);
    }

    public static long GreatestCommonDivisor(long a, long b)
    {
        return b == 0 ? a : GreatestCommonDivisor(b, a % b);
    }
}
