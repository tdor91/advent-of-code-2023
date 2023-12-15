using System.Text;
using System.Text.RegularExpressions;

var input = new Input("/home/thomas/dev/advent-of-code-2023/src/Day15/input.txt");

long sum = input.Sequences
    .Select(Hash)
    .Sum();
Console.WriteLine(sum);

int Hash(string seq) => Encoding.ASCII.GetBytes(seq).Aggregate(0, (sum, x) => sum = (sum + x) * 17 % 256);

var lenses = input.Sequences
    .Select(seq => new Lens(seq, Hash))
    .ToList();

Dictionary<byte, List<(string Label, int FocLen)>> boxes = new();
foreach (var lens in lenses)
{
    if (!boxes.TryGetValue(lens.BoxId, out var box))
    {
        box = new();
        boxes.Add(lens.BoxId, box);
    }

    var lensIndex = box.FindIndex(b => b.Label == lens.Label);
    if (lens.Operation == "=")
    {
        if (lensIndex == -1)
        {
            box.Add((lens.Label, lens.FocalLength));
        }
        else
        {
            box[lensIndex] = (lens.Label, lens.FocalLength);
        }
    }
    else if (lens.Operation == "-" && lensIndex != -1)
    {
        box.RemoveAt(lensIndex);
    }
}

sum = boxes
    .Select(box => box.Value.Select((lens, i) => (box.Key + 1) * (i + 1) * lens.FocLen).Sum())
    .Sum();
Console.WriteLine(sum);

record Lens
{
    private readonly Regex regex = new Regex(@"([a-z]+)(=|-)(\d+)?");

    public Lens(string seq, Func<string, int> hash)
    {
        var match = regex.Match(seq);
        Label = match.Groups[1].Value;
        Operation = match.Groups[2].Value;
        FocalLength = int.TryParse(match.Groups[3].Value, out var val) ? val : 0;

        BoxId = (byte)hash(Label);
    }

    public string Label { get; }
    public string Operation { get; }
    public int FocalLength { get; }
    public byte BoxId { get; }
}

class Input
{
    public Input(string path)
    {
        var text = File.ReadAllText(path);
        Sequences = text.Split(',', StringSplitOptions.RemoveEmptyEntries);
    }

    public string[] Sequences { get; }
}
