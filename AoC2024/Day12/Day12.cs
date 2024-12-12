namespace AoC2024.Day12;

public class Day12 : IMDay
{
    private static readonly LinkedArray<Direction> _directions = new([
        Direction.North,
        Direction.East,
        Direction.South,
        Direction.West
    ]);

    public string FilePath { private get; init; } = "Day12\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var map = await GetInput();
        var areas = GetAreas(map);

        return areas
            .Sum(a => a.Count * a.GetBorderPoints().Count)
            .ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var map = await GetInput();
        var areas = GetAreas(map);

        return areas
            .Sum(a => a.Count * a.GetSides().Count)
            .ToString();
    }

    private static List<Area<char>> GetAreas(Map<char> map)
    {
        HashSet<Point> processedPoints = [];
        List<Area<char>> areas = [];

        map.ForEach((p, _) =>
        {
            if (!processedPoints.Contains(p))
            {
                var area = map.GetArea(p);
                processedPoints.AddRange(area.GetPoints());
                areas.Add(area);
            }
        });

        return areas;
    }

    private async Task<Map<char>> GetInput() =>
        new(await FileParser.ReadLinesAsCharArray(FilePath));
}
