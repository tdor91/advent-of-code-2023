var lines = File.ReadAllLines("input.txt");
var input = new Input(lines);

var result1 = input.Times.Zip(input.Distances)
    .Select(pair => (Time: pair.First, Distance: pair.Second))
    .Select(pair => GetWinningOptions(pair.Time, pair.Distance))
    .Aggregate(1L, (a, b) => a * b);
Console.WriteLine(result1);

var result2 = GetWinningOptions(input.Time2, input.Distance2);
Console.WriteLine(result2);

static long GetWinningOptions(long time, long distance)
{
    long firstSuccess = 0;
    for (long i = 1; i < time; i++)
    {
        var d = (time - i) * i;
        if (d > distance)
        {
            firstSuccess = i;
            break;
        }
    }

    long lastSuccess = time;
    for (long i = time; i > 0; i--)
    {
        var d = (time - i) * i;
        if (d > distance)
        {
            lastSuccess = i;
            break;
        }
    }

    return lastSuccess - firstSuccess + 1;
}

class Input
{
    public Input(string[] lines)
    {
        Times = lines[0]
            .Replace("Time: ", "")
            .Split(" ", StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToList();
        Distances = lines[1]
            .Replace("Distance: ", "")
            .Split(" ", StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToList();

        Time2 = long.Parse(string.Join("", Times));
        Distance2 = long.Parse(string.Join("", Distances));
    }

    public List<int> Times { get; }
    public List<int> Distances { get; }

    public long Time2 { get; }
    public long Distance2 { get; }
}