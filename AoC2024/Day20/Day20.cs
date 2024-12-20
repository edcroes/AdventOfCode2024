namespace AoC2024.Day20;

public class Day20 : IMDay
{
    public string FilePath { private get; init; } = "Day20\\input.txt";

    private bool IsTestInput => FilePath.Contains("test");

    public async Task<string> GetAnswerPart1()
    {
        var map = await GetInput();
        var start = map.First((_, v) => v == 'S');
        var end = map.First((_, v) => v == 'E');
        map.SetValue(start, '.');
        map.SetValue(end, '.');

        var threshold = IsTestInput ? 12 : 100;

        var costMap = map.GetShortestPathCostMap(start, end, (map, _, to) => map.GetValue(to) != '#');
        var cheatOptions = map.Select((p, v) => GetCheatOption(map, p, v)).Where(o => o is not null);

        return cheatOptions.Count(o => GetCheatProfit(costMap!, o!.Value) >= threshold).ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var map = await GetInput();
        var start = map.First((_, v) => v == 'S');
        var end = map.First((_, v) => v == 'E');
        map.SetValue(start, '.');
        map.SetValue(end, '.');

        var threshold = IsTestInput ? 60 : 100;

        var costMap = map.GetShortestPathCostMap(start, end, (map, _, to) => map.GetValue(to) != '#');
        var route = costMap!.Where((_, v) => v > 0).OrderBy(costMap!.GetValue).ToList();
        route.Insert(0, start);

        return Enumerable
            .Range(0, route.Count - 2)
            .Sum(i => route[(i + 2)..].Count(o => GetCheatProfit(costMap, route[i], o) >= threshold))
            .ToString();
    }

    private static int GetCheatProfit(Map<int> costMap, Point from, Point to)
    {
        var distance = from.GetManhattenDistance(to);

        if (distance > 20)
            return 0;

        return costMap.GetValue(to) - costMap.GetValue(from) - distance;
    }

    private static int GetCheatProfit(Map<int> costMap, (Point, Point) option)
    {
        var (first, second) = option;

        var firstCost = costMap.GetValue(first);
        var secondCost = costMap.GetValue(second);

        // - 1 because it will still cost one step to get there
        return Math.Abs(firstCost - secondCost) - 1;
    }

    private static (Point first, Point second)? GetCheatOption(Map<char> map, Point location, char value)
    {
        if (value != '#')
            return null;

        var pathNeighbors = map.GetStraightNeighbors(location).Where(n => map.GetValue(n) == '.');

        if (pathNeighbors.Contains(location.Add(Direction.North.ToPoint())) && pathNeighbors.Contains(location.Add(Direction.South.ToPoint())))
            return (location.Add(Direction.North.ToPoint()), location.Add(Direction.South.ToPoint()));

        if (pathNeighbors.Contains(location.Add(Direction.West.ToPoint())) && pathNeighbors.Contains(location.Add(Direction.East.ToPoint())))
            return (location.Add(Direction.West.ToPoint()), location.Add(Direction.East.ToPoint()));

        return null;
    }

    private async Task<Map<char>> GetInput() =>
        new(await FileParser.ReadLinesAsCharArray(FilePath));
}
