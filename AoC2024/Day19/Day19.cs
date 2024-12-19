namespace AoC2024.Day19;

public class Day19 : IMDay
{
    private readonly Dictionary<string, long> _cache = [];

    public string FilePath { private get; init; } = "Day19\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var (patterns, designs) = await GetInput();

        return designs
            .Count(d => GetPatternOptions(d, patterns) > 0)
            .ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var (patterns, designs) = await GetInput();

        return designs
            .Sum(d => GetPatternOptions(d, patterns))
            .ToString();
    }

    private long GetPatternOptions(string design, string[] patterns)
    {
        if (_cache.TryGetValue(design, out var count))
            return count;

        if (design.IsNullOrEmpty())
            return 1;

        var optionCount = patterns
            .Where(design.StartsWith)
            .Sum(p => GetPatternOptions(design[p.Length..], patterns));

        _cache.Add(design, optionCount);

        return optionCount;
    }

    private async Task<(string[], string[])> GetInput()
    {
        var input = await FileParser.ReadBlocksAsStringArray(FilePath);
        return (input[0][0].Split(", "), input[1]);
    }
}
