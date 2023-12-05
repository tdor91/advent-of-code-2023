using System.Collections.Concurrent;

var lines = File.ReadAllLines("input.txt");

var input = new Input(lines);

var minLocation = input.Seeds
    .Select(seed => input.MappingDefinitionGroups.Aggregate(seed, (a, b) => b.GetMappedValue(a)))
    .Min();

Console.WriteLine(minLocation);

// this is O(scary)
var seedsDefinitions = input.Seeds.Chunk(2);
ConcurrentBag<long> locations = new();
Parallel.ForEach(seedsDefinitions, (seedDef, _) =>
{
    long min = long.MaxValue;
    for (var i = seedDef[0]; i < seedDef[0] + seedDef[1]; i++)
    {
        var location = input.MappingDefinitionGroups.Aggregate(i, (a, b) => b.GetMappedValue(a));
        min = Math.Min(location, min);
    }
    locations.Add(min);
});

Console.WriteLine(locations.Min());

class Input
{
    public Input(string[] lines)
    {
        Seeds = lines[0].Replace("seeds: ", "").Split(" ").Select(long.Parse).ToList();
        MappingDefinitionGroups = new();

        List<string> currentMappings = new();
        foreach (var line in lines.Skip(1))
        {
            if (line.Contains("map:"))
            {
                currentMappings.Clear();
            }
            else if (string.IsNullOrWhiteSpace(line))
            {
                if (currentMappings.Any())
                {
                    MappingDefinitionGroups.Add(new MappingDefinitionGroup(currentMappings));
                }
            }
            else
            {
                currentMappings.Add(line);
            }
        }

        MappingDefinitionGroups.Add(new MappingDefinitionGroup(currentMappings));
    }

    public List<long> Seeds { get; }

    public List<MappingDefinitionGroup> MappingDefinitionGroups { get; }
}

class MappingDefinitionGroup
{
    public MappingDefinitionGroup(List<string> mappingDefinitions)
    {
        Mappings = mappingDefinitions.Select(def => new MappingDefinition(def)).OrderBy(x => x.Source).ToList();
    }

    public List<MappingDefinition> Mappings { get; }

    public long GetMappedValue(long source)
    {
        var mapping = Mappings.FirstOrDefault(mapping => source >= mapping.Source && source < mapping.Source + mapping.Length);
        if (mapping is null)
        {
            return source;
        }

        return mapping.Destination + (source - mapping.Source);
    }
}

record MappingDefinition
{
    public MappingDefinition(string line)
    {
        var parts = line.Split(" ");
        Destination = long.Parse(parts[0]);
        Source = long.Parse(parts[1]);
        Length = long.Parse(parts[2]);
    }

    public long Destination { get; init; }
    public long Source { get; init; }
    public long Length { get; set; }
}