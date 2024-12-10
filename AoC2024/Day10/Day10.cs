namespace AoC2024.Day10;

public class Day10 : IMDay
{
    public string FilePath { private get; init; } = "Day10\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var map = await GetInput();

        Dictionary<(Point, Point), int> cache = [];
        var startPositions = map.Where((p, v) => v == 0);
        var endPositions = map.Where((p, v) => v == 9);

        return startPositions
            .Sum(s => endPositions.Count(e => map.GetNumberOfPaths(s, e, GetNeighbors, cache) > 0))
            .ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var map = await GetInput();

        Dictionary<Point, int> cache = [];
        var startPositions = map.Where((p, v) => v == 0);

        return startPositions
            .Sum(p => map.GetNumberOfPaths(p, 9, GetNeighbors, cache))
            .ToString();
    }

    private static IEnumerable<Point> GetNeighbors(Map<int> map, Point point, int value) =>
        map.GetStraightNeighbors(point).Where(n => map.GetValueOrDefault(n) - value == 1);

    private async Task<Map<int>> GetInput() =>
        new(await FileParser.ReadLinesAsIntArray(FilePath));
}
