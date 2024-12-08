namespace AoC2024.Day08;

public class Day08 : IMDay
{
    public string FilePath { private get; init; } = "Day08\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var map = await GetInput();
        var allPoints = map.Points.ToArray();

        return Enumerable
            .Range(0, allPoints.Length - 1)
            .SelectMany(i => GetAntinodes(allPoints[i], allPoints[(i + 1)..].Where(p => map.GetValue(p) == map.GetValue(allPoints[i]))))
            .Where(map.Contains)
            .Distinct()
            .Count()
            .ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var map = await GetInput();
        var allPoints = map.Points.ToArray();

        return Enumerable
            .Range(0, allPoints.Length - 1)
            .SelectMany(i => GetAllAntinodes(map, allPoints[i], allPoints[(i + 1)..].Where(p => map.GetValue(p) == map.GetValue(allPoints[i]))))
            .Distinct()
            .Count()
            .ToString();
    }

    private static IEnumerable<Point<int>> GetAntinodes(Point<int> current, IEnumerable<Point<int>> others) =>
        others.SelectMany(o => GetAntinodes(current, o));

    private static IEnumerable<Point<int>> GetAntinodes(Point<int> current, Point<int> other)
    {
        var moveX = other.X - current.X;
        var moveY = other.Y - current.Y;

        return [other.Add(new(moveX, moveY)), current.Add(new(-moveX, -moveY))];
    }

    private static IEnumerable<Point<int>> GetAllAntinodes(PointMap<int, char> map, Point<int> current, IEnumerable<Point<int>> others) =>
        others.SelectMany(o => GetAllAntinodes(map, current, o));

    private static HashSet<Point<int>> GetAllAntinodes(PointMap<int, char> map, Point<int> current, Point<int> other)
    {
        HashSet<Point<int>> antinodes = [];
        var moveX = other.X - current.X;
        var moveY = other.Y - current.Y;

        var nextOther = other;
        while (map.Contains(nextOther))
        {
            antinodes.Add(nextOther);
            nextOther = nextOther.Add(new(moveX, moveY));
        }

        var nextCurrent = current;
        while (map.Contains(nextCurrent))
        {
            antinodes.Add(nextCurrent);
            nextCurrent = nextCurrent.Add(new(-moveX, -moveY));
        }

        return antinodes;
    }


    private async Task<PointMap<int, char>> GetInput() =>
        new(await FileParser.ReadLinesAsCharArray(FilePath), false, '.', true);
}
