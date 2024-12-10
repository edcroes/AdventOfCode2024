namespace AoC2024.Day06;

public class Day06 : IMDay
{
    private static readonly LinkedArray<Direction> _directions = new([
            Direction.North,
            Direction.East,
            Direction.South,
            Direction.West
        ]);

    public string FilePath { private get; init; } = "Day06\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var map = await GetInput();

        var start = map.First((_, v) => v == '^');
        var path = GetPathToEnd(map, start);

        return path.Count.ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var map = await GetInput();

        var start = map.First((_, v) => v == '^');
        var path = GetPathToEnd(map, start);
        _ = path.Remove(start);

        return path.Count(p => IsMapWithChangeALoop(map, p)).ToString();
    }

    private static HashSet<Point> GetPathToEnd(Map<char> map, Point start)
    {
        HashSet<Point> pointsHit = [];
        var currentDirection = Direction.North;
        var next = start;

        while (map.Contains(next.Add(_directions.GetPrevious(currentDirection).ToPoint())))
        {
            var path = map.MoveUntil(next, currentDirection, (_, v) => v == '#');
            pointsHit.AddRange(path);
            next = path[^1];
            currentDirection = _directions.GetNext(currentDirection);
        }

        return pointsHit;
    }

    private static bool IsMapWithChangeALoop(Map<char> map, Point pointToChange)
    {
        var newMap = map.Clone();
        newMap.SetValue(pointToChange, '#');

        return HasLoop(newMap);
    }

    private static bool HasLoop(Map<char> map)
    {
        var current = map.First((_, v) => v == '^');
        var next = current.Add(Direction.North.ToPoint());
        HashSet<(Point, Direction)> pointsHit = [(current, Direction.North)];

        var currentDirection = Direction.North;
        char nextValue = '.';

        while ((nextValue = map.GetValueOrDefault(next)) != default)
        {
            if (nextValue == '#')
            {
                currentDirection = _directions.GetNext(currentDirection);
            }
            else
            {
                current = next;
            }

            if (!pointsHit.Contains((current, currentDirection)))
            {
                pointsHit.Add((current, currentDirection));
            }
            else
            {
                return true;
            }

            next = current.Add(currentDirection.ToPoint());
        }

        return false;
    }

    private async Task<Map<char>> GetInput() =>
        new(await FileParser.ReadLinesAsCharArray(FilePath));
}
