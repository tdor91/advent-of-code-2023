using Common;

var input = new Input("input.txt");

var locations = input
    .ExpandedMatrix
    .Points()
    .Where(p => input.ExpandedMatrix.GetSymbol(p) == '#')
    .ToArray();

long sumOfDistances = 0;
for (int i = 0; i < locations.Length - 1; i++)
{
    for (int j = i + 1; j < locations.Length; j++)
    {
        var vec = locations[i].DistanceTo(locations[j]);
        sumOfDistances += vec.X + vec.Y;
    }
}
Console.WriteLine(sumOfDistances);

locations = input
    .RawMatrix
    .Points()
    .Where(p => input.RawMatrix.GetSymbol(p) == '#')
    .ToArray();

const int expansionSize = 1000000; // use 2 for result of part 1
sumOfDistances = 0;
for (int i = 0; i < locations.Length - 1; i++)
{
    for (int j = i + 1; j < locations.Length; j++)
    {
        var vec = locations[i].DistanceTo(locations[j]);

        var xExpansions = Math.Abs(
            input.EmptyColumnsAt.CountSmaller(locations[i].X) - 
            input.EmptyColumnsAt.CountSmaller(locations[j].X));
        var yExpansions = Math.Abs(
            input.EmptyRowsAt.CountSmaller(locations[i].Y) -
            input.EmptyRowsAt.CountSmaller(locations[j].Y));

        var dx = vec.X + xExpansions * (expansionSize - 1);
        var dy = vec.Y + yExpansions * (expansionSize - 1);

        sumOfDistances += dx + dy;
    }
}
Console.WriteLine(sumOfDistances);

class Input
{
    public Input(string path)
    {
        var lines = File.ReadAllLines(path);
        RawMatrix = lines.Select(line => line.ToCharArray()).ToArray();
        ExpandedMatrix = Expand(RawMatrix);

        EmptyRowsAt = FindEmptyRows(RawMatrix).ToList();
        EmptyColumnsAt = FindEmptyColumns(RawMatrix).ToList();
    }

    public char[][] RawMatrix { get; }

    public char[][] ExpandedMatrix { get; }

    public List<int> EmptyRowsAt { get; }
    public List<int> EmptyColumnsAt { get; }

    private char[][] Expand(char[][] matrix)
    {
        List<List<char>> columnExpandedResult = new();
        foreach (var row in matrix)
        {
            columnExpandedResult.Add(new());
        }

        for (int i = 0; i < matrix[0].Length; i++)
        {
            var column = matrix.Select(row => row[i]).ToArray();
            var isEmptyColumn = !column.Contains('#');
            for (int j = 0; j < columnExpandedResult.Count; j++)
            {
                columnExpandedResult[j].Add(column[j]);
                if (isEmptyColumn)
                {
                    columnExpandedResult[j].Add(column[j]);
                }
            }
        }

        List<List<char>> fullyExpandedResult = new();
        for (int i = 0; i < columnExpandedResult.Count; i++)
        {
            var row = columnExpandedResult[i];
            fullyExpandedResult.Add(row);
            if (!row.Contains('#'))
            {
                fullyExpandedResult.Add(row);
            }
        }

        return fullyExpandedResult.Select(row => row.ToArray()).ToArray();
    }

    private IEnumerable<int> FindEmptyRows(char[][] matrix)
    {
        for (int i = 0; i < matrix.Length; i++)
        {
            if (!matrix[i].Contains('#'))
            {
                yield return i;
            }
        }
    }

    private IEnumerable<int> FindEmptyColumns(char[][] matrix)
    {
        for (int i = 0; i < matrix[0].Length; i++)
        {
            var column = matrix.Select(row => row[i]);
            if (!column.Contains('#'))
            {
                yield return i;
            };
        }
    }
}

public static class EnumerableExtensions
{
    public static int CountSmaller(this IEnumerable<int> source, int max) => source.Count(x => x < max);
}
