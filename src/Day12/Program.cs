var input = new Input("input.txt");

long sum = 0;
foreach (var arrangement in input.SpringArrangements)
{
    sum += NumberOfOptions(arrangement.S, arrangement.Groups.ToArray(), new());
}
Console.WriteLine(sum);

sum = 0;
int i = 0;
foreach (var arrangement in input.SpringArrangements.Select(arrangement => arrangement.Unfold()))
{
    Console.WriteLine(i++);
    sum += NumberOfOptions(arrangement.S, arrangement.Groups.ToArray(), new());
}
Console.WriteLine(sum);

long NumberOfOptions(string input, int[] groups, Dictionary<(string Input, int[] Groups), long> cache)
{
    if (cache.ContainsKey((input, groups)))
    {
        return cache[(input, groups)];
    }

    var count = Calculate(input, groups, cache);
    cache[(input, groups)] = count;

    return count;
}

long Calculate(string input, int[] groups, Dictionary<(string Input, int[] Groups), long> cache)
{
    if (!groups.Any())
    {
        return input.Contains('#') ? 0 : 1;
    }

    if (input.Length < groups.Sum() + groups.Length - 1)
    {
        return 0;
    }

    switch (input[0])
    {
        case '.':
            return NumberOfOptions(input[1..], groups, cache);

        case '?':
            return NumberOfOptions("." + input[1..], groups, cache) + NumberOfOptions("#" + input[1..], groups, cache);

        case '#':
            var requiredSize = groups[0];

            var maxDeadSprings = input.TakeWhile(s => s != '.').Count();
            if (maxDeadSprings < requiredSize) // too small
            {
                return 0;
            }

            if (input.Length == requiredSize) // fits
            {
                return 1;
            }

            if (input[requiredSize] == '#') // too big
            {
                return 0;
            }

            return NumberOfOptions(input[(requiredSize + 1)..], groups[1..], cache);

        default:
            throw new Exception();
    };
}

class Input
{
    public Input(string path)
    {
        var lines = File.ReadAllLines(path);
        SpringArrangements = File.ReadAllLines(path).Select(line => new SpringArrangement(line)).ToList();
    }

    public IEnumerable<SpringArrangement> SpringArrangements { get; }
}

record SpringArrangement
{

    public SpringArrangement(string line)
    {
        var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        S = parts[0];
        Groups = parts[1].Split(',').Select(int.Parse).ToArray();
    }

    public SpringArrangement(string s, int[] groups)
    {
        S = s;
        Groups = groups;
    }

    public string S { get; }
    public int[] Groups { get; }

    public SpringArrangement Unfold()
    {
        string unfoldedS = "";
        List<int> unfoldedGroups = new();
        for (int i = 0; i < 5; i++)
        {
            unfoldedS += S + "?";
            unfoldedGroups.AddRange(Groups);
        }

        unfoldedS = unfoldedS[0..^1];

        return new SpringArrangement(unfoldedS, unfoldedGroups.ToArray());
    }
}
