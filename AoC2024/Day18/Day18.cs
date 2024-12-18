namespace AoC2024.Day18;

public class Day18 : IMDay
{
    public string FilePath { private get; init; } = "Day18\\input.txt";

    private bool IsTestInput => FilePath.Contains("test");

    public async Task<string> GetAnswerPart1()
    {
        var bytes = await GetInput();
        var size = IsTestInput ? 7 : 71;
        var numberOfBytesToDrop = IsTestInput ? 12 : 1024;
        Map<bool> map = new(size, size);

        foreach (var b in bytes.Take(numberOfBytesToDrop))
        {
            map.SetValue(b, true);
        }

        var shortestPath = map.GetShortestPath(new(0, 0), new(size - 1, size - 1), (map, _, to) => !map.GetValueOrDefault(to) && map.Contains(to));

        return shortestPath.ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var bytes = await GetInput();
        var size = IsTestInput ? 7 : 71;
        var currentDrop = IsTestInput ? 12 : 1024;
        Map<bool> map = new(size, size);

        foreach (var b in bytes.Take(currentDrop))
        {
            map.SetValue(b, true);
        }
        currentDrop--;

        int pathLength = -1;
        while (pathLength != int.MaxValue)
        {
            map.SetValue(bytes[++currentDrop], true);
            pathLength = map.GetShortestPath(new(0, 0), new(size - 1, size - 1), (map, _, to) => !map.GetValueOrDefault(to) && map.Contains(to));
        }

        return $"{bytes[currentDrop].X},{bytes[currentDrop].Y}";
    }

    private async Task<Point[]> GetInput() =>
        (await FileParser.ReadLinesAsIntArray(FilePath, ","))
            .Select(l => new Point(l[0], l[1]))
            .ToArray();
}
