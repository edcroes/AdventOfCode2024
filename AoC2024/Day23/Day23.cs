namespace AoC2024.Day23;

public class Day23 : IMDay
{
    public string FilePath { private get; init; } = "Day23\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var pairs = await GetInput();

        return pairs.Keys
            .Where(k => k[0] == 't')
            .SelectMany(p => GetCircleCount(p, pairs))
            .Distinct()
            .Count()
            .ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var pairs = await GetInput();

        List<HashSet<string>> sets = [];

        foreach (var key in pairs.Keys)
        {
            if (!sets.Any(s => s.Contains(key)))
                sets.Add([key]);

            var existingSets = sets.Where(s => s.Contains(key)).ToList();

            foreach (var other in pairs[key])
            {
                var currentSets = existingSets.Where(s => s.All(i => pairs[other].Contains(i))).ToList();
                if (currentSets.Count == 0)
                {
                    HashSet<string> newSet = [key, other];
                    existingSets.Add(newSet);
                    sets.Add(newSet);
                }
                else
                {
                    foreach (var set in currentSets)
                    {
                        set.Add(other);
                    }
                }
            }
        }

        return string.Join(",", sets.OrderBy(s => s.Count).Last().OrderBy(p => p));
    }

    private static HashSet<string> GetCircleCount(string start, Dictionary<string, HashSet<string>> pairs)
    {
        HashSet<string> circles = [];

        foreach (var next in pairs[start])
        {
            foreach (var after in pairs[next])
            {
                if (pairs[after].Contains(start))
                {
                    var circle = string.Join("", (new string[] { start, next, after }).OrderBy(s => s));
                    circles.Add(circle);
                }
            }
        }

        return circles;
    }

    private async Task<Dictionary<string, HashSet<string>>> GetInput()
    {
        Dictionary<string, HashSet<string>> pairs = [];
        var input = await FileParser.ReadLinesAsStringArray(FilePath, "-");

        foreach (var (first, second) in input)
        {
            pairs.AddOrUpdate(first!, second!);
            pairs.AddOrUpdate(second!, first!);
        }

        return pairs;
    }
}
