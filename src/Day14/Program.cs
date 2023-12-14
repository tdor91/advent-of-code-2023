var input = new Input("input.txt");

var matrix = input.Matrix;
TiltNorth(matrix);

var load = matrix
    .Reverse()
    .Select((row, i) => row.Count(c => c == 'O') * (i + 1))
    .Sum();
Console.WriteLine(load);


void TiltNorth(char[][] matrix)
{
    int[] spaces = new int[matrix[0].Length];

    for (int y = 0; y < matrix.Length; y++)
    {
        for (int x = 0; x < matrix[y].Length; x++)
        {
            switch (matrix[y][x])
            {
                case '.':
                    spaces[x]++;
                    break;
                case '#':
                    spaces[x] = 0;
                    break;
                case 'O':
                    matrix[y][x] = '.';
                    matrix[y - spaces[x]][x] = 'O';
                    break;
                default: throw new Exception();
            }
        }
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