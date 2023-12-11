using Day10;
using Common;

var input = new Input("input.txt");
var pipeSystem = new PipeSystem(input.Matrix);

var loop = pipeSystem.FindLoop('S');
Console.WriteLine($"{loop.Points.Length / 2}");

var enclosedPoints = pipeSystem.Matrix
    .Points()
    .Except(loop.Points)
    .Where(p => GeometricUtility.IsInPolygon(loop.Points, p))
    .Count();

Console.WriteLine(enclosedPoints);

class PipeSystem
{
    private Dictionary<char, Point[]> connectionMapping = new()
    {
        ['|'] = [Directions.North, Directions.South],
        ['-'] = [Directions.West, Directions.East],
        ['L'] = [Directions.North, Directions.East],
        ['J'] = [Directions.North, Directions.West],
        ['7'] = [Directions.South, Directions.West],
        ['F'] = [Directions.South, Directions.East],
        ['.'] = [],
        // ['S'] = [Directions.North, Directions.East, Directions.South, Directions.West]
    };

    public PipeSystem(char[][] matrix)
    {
        Matrix = matrix;
    }

    public char[][] Matrix { get; }

    // only used for debugging
    public char[][] GetSimplifiedMatrix(Point[] pipePoints)
    {
        char[][] simplified = new char[Matrix.Length][];

        for (int y = 0; y < Matrix.Length; y++)
        {
            char[] row = new char[Matrix[y].Length];
            simplified[y] = row;
            for (int x = 0; x < Matrix[y].Length; x++)
            {
                var p = new Point(x, y);
                if (pipePoints.Contains(p))
                {
                    simplified[y][x] = '#';
                }
                else
                {
                    simplified[y][x] = '.';
                }
            }
        }

        return simplified;
    }

    private Point GetStartPoint(char start) => Matrix.Points().First(p => Matrix[p.Y][p.X] == start);

    public (Point Start, Point[] Points) FindLoop(char startSymbol)
    {
        var start = GetStartPoint(startSymbol);
        foreach (var direction in Directions.All)
        {
            var loop = FindLoop(start, direction);
            if (loop is not null)
            {
                return (start, loop);
            }
        }

        throw new Exception("No loop found.");
    }

    private Point[]? FindLoop(Point start, Point firstStep)
    {
        List<Point> loop = [start];

        var step = firstStep;
        var current = start + step;
        while (current != start)
        {
            if (!Matrix.Contains(current))
            {
                return null;
            }

            var possibleConnections = connectionMapping[Matrix.GetSymbol(current)];
            var backStep = step.Invert();
            if (!possibleConnections.Contains(backStep))
            {
                return null;
            }
            
            loop.Add(current);
            step = possibleConnections.Where(connection => connection != backStep).First();
            current += step;
        }

        return loop.ToArray();
    }
}

class Input
{
    public Input(string path)
    {
        var lines = File.ReadAllLines(path);
        Matrix = lines.Select(line => line.ToCharArray()).ToArray();
    }

    public char[][] Matrix { get; }
}

static class Directions
{
    public static Point North => new(0, -1);
    public static Point East => new(1, 0);
    public static Point South => new(0, 1);
    public static Point West => new(-1, 0);

    public static Point[] All => [North, East, South, West];
}