var input = new Input("input.txt");

var sumOfNextPredictions = input.Histories
    .Select(history => history.NextPrediction())
    .Sum();

Console.WriteLine(sumOfNextPredictions);

var sumOfPreviousPredictions = input.Histories
    .Select(history => history.PreviousPrediction())
    .Sum();

Console.WriteLine(sumOfPreviousPredictions);

class Input
{
    public Input(string path)
    {
        Histories = File.ReadAllLines(path).Select(line => new History(line)).ToArray();
    }

    public History[] Histories { get; }
}

record History
{
    public History(string sensorValues)
    {
        var values = sensorValues.Split(' ').Select(int.Parse).ToArray();
        Values = [values];
        ReduceValuesUntilOnlyZeros();
    }

    public List<int[]> Values { get; }

    public int NextPrediction() => Values.Reverse<int[]>()
        .Aggregate(0, (prediction, values) => prediction + values.Last());

    public int PreviousPrediction() => Values.Reverse<int[]>()
        .Aggregate(0, (prediction, values) => values.First() - prediction);

    private void ReduceValuesUntilOnlyZeros()
    {
        while (Values.Last().Any(value => value != 0))
        {
            var lastValues = Values.Last();
            var newValues = lastValues.Zip(lastValues.Skip(1), (a, b) => b - a).ToArray();
            Values.Add(newValues);
        }
    }
}
