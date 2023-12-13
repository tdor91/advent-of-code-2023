using Common;

var input = new Input("input.txt");

long sum = input.Matrices
    .Select(matrix =>
    {
        var mirrorRowIndex = IndexOfMirror(matrix.StringRows(), requireError: false);
        var mirrorColummIndex = IndexOfMirror(matrix.StringColumns(), requireError: false);
        return (RowsBeforeMirror: mirrorRowIndex + 1, ColumnsBeforeMirror: mirrorColummIndex + 1);
    })
    .Sum(value => value.RowsBeforeMirror * 100 + value.ColumnsBeforeMirror);
Console.WriteLine(sum);

sum = input.Matrices
    .Select(matrix =>
    {
        var mirrorRowIndex = IndexOfMirror(matrix.StringRows(), requireError: true);
        var mirrorColummIndex = IndexOfMirror(matrix.StringColumns(), requireError: true);
        return (RowsBeforeMirror: mirrorRowIndex + 1, ColumnsBeforeMirror: mirrorColummIndex + 1);
    })
    .Sum(value => value.RowsBeforeMirror * 100 + value.ColumnsBeforeMirror);
Console.WriteLine(sum);

static int IndexOfMirror(IEnumerable<string> lines, bool requireError)
{
    var list = lines.ToList();

    for (var i = 0; i < list.Count - 1; i++)
    {
        var diffFound = false;

        var differences = Differences(list[i], list[i + 1]);
        if (differences > 1 || differences == 1 && !requireError)
        {
            continue;
        }

        if (differences == 1)
        {
            diffFound = true;
        }

        if (IsMirrorIndex(list, i, diffFound, requireError))
        {
            return i;
        }
    }

    return -1;
}

static bool IsMirrorIndex(List<string> lines, int i, bool initialDiffFound, bool requireError)
{
    var diffFound = initialDiffFound;
    var offset = 1;
    while (i - offset >= 0 && i + offset < lines.Count - 1)
    {
        var differences = Differences(lines[i - offset], lines[i + 1 + offset]);
        if (differences > 1 ||
            differences == 1 && !requireError ||
            differences == 1 && diffFound)
        {
            return false;
        }

        if (differences == 1)
        {
            diffFound = true;
        }

        offset++;
    }

    return requireError ? diffFound : !diffFound;
}

static int Differences(string a, string b)
{
    return a.Zip(b).Count(x => x.First != x.Second);
}

class Input
{
    public Input(string path)
    {
        var lines = File.ReadAllLines(path);

        var i = 0;
        while (i < lines.Length)
        {
            var matrix = lines
                .Skip(i)
                .TakeWhile(line => !string.IsNullOrEmpty(line))
                .Select(line => line.ToCharArray())
                .ToArray();
            Matrices.Add(matrix);

            i += matrix.Length + 1;
        }
    }

    public List<char[][]> Matrices { get; set; } = new();
}