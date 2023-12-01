var lines = File.ReadAllLines("input.txt");

var sum = lines.Sum(line =>
{
    var first = line.First(char.IsNumber);
    var last = line.Last(char.IsNumber);
    return int.Parse($"{first}{last}");
});

Console.WriteLine(sum);

Dictionary<string, int> mapping = new()
{
     ["1"] = 1, ["one"] = 1,
     ["2"] = 2, ["two"] = 2,
     ["3"] = 3, ["three"] = 3,
     ["4"] = 4, ["four"] = 4,
     ["5"] = 5, ["five"] = 5,
     ["6"] = 6, ["six"] = 6,
     ["7"] = 7, ["seven"] = 7,
     ["8"] = 8, ["eight"] = 8,
     ["9"] = 9, ["nine"] = 9,
};

var sum2 = lines.Sum(line =>
{
    var firstKey = mapping.Keys
        .Select(numberStr => (Index: line.IndexOf(numberStr), Key: numberStr))
        .Where(x => x.Index != -1)
        .MinBy(x => x.Index)
        .Key;

    var lastKey = mapping.Keys
        .Select(numberStr => (Index: line.LastIndexOf(numberStr), Key: numberStr))
        .Where(x => x.Index != -1)
        .MinBy(x => x.Index)
        .Key;

    return mapping[firstKey] * 10 + mapping[lastKey];
});

Console.WriteLine(sum2);