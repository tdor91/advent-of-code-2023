var lines = File.ReadAllLines("input.txt");

List<Number> numbers = new();
Dictionary<Point, char> symbols = new();

for (int y = 0; y < lines.Length; y++)
{
    var line = lines[y] + "."; // add a delimiter to simplify parsing at the line end ;)
    string? currentNumber = null;
    Point? start = null;
    for (int x = 0; x < line.Length; x++)
    {
        Point currentPosition = new(x, y);
        if (char.IsNumber(line[x]))
        {
            start ??= currentPosition;
            currentNumber += line[x];
            continue;
        }

        if (line[x] != '.')
        {
            symbols.Add(currentPosition, line[x]);
        }

        if (currentNumber is not null)
        {
            var value = int.Parse(currentNumber);
            var startPoint = start!.Value;
            var endPoint = startPoint.Add(currentNumber.Length - 1, 0);
            numbers.Add(new(value, startPoint, endPoint));
        }
        start = null;
        currentNumber = null;
    }
}

var sumOfPartNumbers = numbers
    .Where(number => symbols.Any(symbol => number.IsAdjacentTo(symbol.Key)))
    .Sum(number => number.Value);

Console.WriteLine(sumOfPartNumbers);

var sumOfGearRatios = symbols
    .Where(symbol => symbol.Value == '*')
    .Select(gearSymbol => numbers.Where(number => number.IsAdjacentTo(gearSymbol.Key)).ToArray())
    .Where(adjacentNumbers => adjacentNumbers.Length == 2)
    .Select(gearNumbers => gearNumbers[0].Value * gearNumbers[1].Value)
    .Sum();

Console.WriteLine(sumOfGearRatios);

struct Point(int x, int y)
{
    public int X { get; } = x;
    public int Y { get; } = y;

    public Point Add(int x, int y) => new(X + x, Y + y);
}

struct Number(int value, Point start, Point end)
{
    public int Value { get; } = value;
    public Point Start { get; } = start;
    public Point End { get; } = end;

    public bool IsAdjacentTo(Point p)
    {
        var xFrom = Math.Max(Start.X - 1, 0);
        var xTo = End.X + 1;

        var yFrom = Math.Max(Start.Y - 1, 0);
        var yTo = End.Y + 1;

        return p.X >= xFrom &&
            p.X <= xTo &&
            p.Y >= yFrom &&
            p.Y <= yTo;
    }
}
