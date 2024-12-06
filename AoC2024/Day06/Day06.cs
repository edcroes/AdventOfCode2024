namespace AoC2024.Day06;

public class Day06 : IMDay
{
    private readonly LinkedArray<Direction> _directions = new([
            Direction.North,
            Direction.East,
            Direction.South,
            Direction.West
        ]);

    public string FilePath { private get; init; } = "Day06\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var map = await GetInput();

        var current = map.First((_, v) => v == '^');
        var next = current.Add(Direction.North.ToPoint());
        HashSet<Point> pointsHit = [current];

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

            pointsHit.Add(current);

            next = current.Add(currentDirection.ToPoint());
        }

        return pointsHit.Count.ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var map = await GetInput();

        return map.Count((p, _) => IsMapWithChangeALoop(map, p)).ToString();
    }

    private bool IsMapWithChangeALoop(Map<char> map, Point pointToChange)
    {
        var valueAtPoint = map.GetValue(pointToChange);
        if (valueAtPoint.IsIn('#', '^'))
            return false;

        var newMap = map.Clone();
        newMap.SetValue(pointToChange, '#');

        return HasLoop(newMap);
    }

    private bool HasLoop(Map<char> map)
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
