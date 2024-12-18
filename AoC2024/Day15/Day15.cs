namespace AoC2024.Day15;

public class Day15 : IMDay
{
    public string FilePath { private get; init; } = "Day15\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var (map, directions) = await GetInput();

        var robot = map.First((_, v) => v == '@');
        var crates = map.Where((p, v) => v == 'O').ToHashSet();

        foreach (var movement in directions)
        {
            if (Move(map, robot, movement, crates))
                robot = robot.Add(movement.ToPoint());
        }

        return GetGpsCoordinates(crates).ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var (map, directions) = await GetChangedInput();

        var robot = map.First((_, v) => v == '@');
        var crates = map.Where((p, v) => v == '[').ToHashSet();

        foreach (var movement in directions)
        {
            var (points, canMove) = CanMove(map, robot, movement);
            if (canMove)
            {
                robot = robot.Add(movement.ToPoint());
                var orderedPoints = movement switch
                {
                    Direction.North => points.OrderBy(p => p.Y),
                    Direction.East => points.OrderByDescending(p => p.X),
                    Direction.South => points.OrderByDescending(p => p.Y),
                    Direction.West => points.OrderBy(p => p.X)
                };

                foreach (var point in orderedPoints)
                {
                    var next = point.Add(movement.ToPoint());
                    var currentObject = map.GetValue(point);

                    map.SetValue(next, currentObject);
                    map.SetValue(point, '.');

                    if (currentObject == '[')
                    {
                        crates.Remove(point);
                        crates.Add(next);
                    }
                }
            }
        }

        return GetGpsCoordinates(crates).ToString();
    }

    private static (HashSet<Point>, bool) CanMove(Map<char> map, Point point, Direction direction)
    {
        HashSet<Point> allPointsToMove = [];

        var value = map.GetValue(point);

        if (value == '#')
            return ([], false);
        if (value == '.')
            return ([], true);

        allPointsToMove.Add(point);
        var currentObject = map.GetValue(point);

        var next = point.Add(direction.ToPoint());
        var (pointsToMove, canMove) = CanMove(map, next, direction);

        if (!canMove)
            return ([], false);

        allPointsToMove.AddRange(pointsToMove);

        if (currentObject == '@')
            return (allPointsToMove, canMove);

        point = currentObject == '['
            ? point.Add(Direction.East.ToPoint())
            : point.Add(Direction.West.ToPoint());
        allPointsToMove.Add(point);

        // East and West are already handled by the normal move and adding the other part of the crate
        if (direction.IsIn(Direction.North, Direction.South))
        {
            next = point.Add(direction.ToPoint());
            (pointsToMove, canMove) = CanMove(map, next, direction);

            if (!canMove)
                return ([], false);

            allPointsToMove.AddRange(pointsToMove);
        }

        return (allPointsToMove, canMove);
    }

    private static bool Move(Map<char> map, Point point, Direction direction, HashSet<Point> crates)
    {
        var value = map.GetValue(point);

        if (value == '#')
            return false;
        if (value == '.')
            return true;

        var next = point.Add(direction.ToPoint());
        var currentObject = map.GetValue(point);

        if (!Move(map, next, direction, crates))
            return false;

        map.SetValue(next, currentObject);
        map.SetValue(point, '.');

        if (currentObject == 'O')
        {
            crates.Remove(point);
            crates.Add(next);
        }

        return true;
    }

    private static int GetGpsCoordinates(HashSet<Point> crates) =>
        crates.Sum(c => 100 * c.Y + c.X);

    private async Task<(Map<char>, Direction[])> GetInput()
    {
        var (mapInput, directionsInput) = await FileParser.ReadBlocksAsStringArray(FilePath);
        Map<char> map = new(mapInput!.Select(l => l.ToCharArray()).ToArray());
        Direction[] directions = string.Join("", directionsInput!).Select(d => d switch
        {
            '^' => Direction.North,
            '>' => Direction.East,
            'v' => Direction.South,
            '<' => Direction.West
        }).ToArray();

        return (map, directions);
    }

    private async Task<(Map<char>, Direction[])> GetChangedInput()
    {
        var (mapInput, directionsInput) = await FileParser.ReadBlocksAsStringArray(FilePath);
        Map<char> map = new(mapInput!.Select(l => l
                .Replace("#", "##")
                .Replace(".", "..")
                .Replace("O", "[]")
                .Replace("@", "@.")
                .ToCharArray()
            ).ToArray());
        Direction[] directions = string.Join("", directionsInput!).Select(d => d switch
        {
            '^' => Direction.North,
            '>' => Direction.East,
            'v' => Direction.South,
            '<' => Direction.West
        }).ToArray();

        return (map, directions);
    }
}
