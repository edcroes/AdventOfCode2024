namespace AoC2024.Day25;

public class Day25 : IMDay
{
    public string FilePath { private get; init; } = "Day25\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var input = await GetInput();
        var locks = input
            .Where(m => m.Count((p, v) => p.Y == 0 && v == '#') == m.SizeX)
            .ToList();
        var locksPoints = locks
            .Select(l => l.Where((_, v) => v == '#').ToList())
            .ToList();

        var keysPoints = input
            .Where(m => !locks.Contains(m))
            .Select(m => m.Where((_, v) => v == '#').ToList())
            .ToList();

        return locksPoints.Sum(l => keysPoints.Count(k => k.All(p => !l.Contains(p)))).ToString();
    }

    public Task<string> GetAnswerPart2()
    {
        return Task.FromResult("DONE");
    }

    private async Task<Map<char>[]> GetInput() =>
        (await FileParser.ReadBlocksAsStringArray(FilePath))
            .Select(Parse)
            .ToArray();

    private static Map<char> Parse(string[] lines) =>
        new(lines.Select(l => l.ToCharArray()).ToArray());
}
