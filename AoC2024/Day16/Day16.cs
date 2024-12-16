namespace AoC2024.Day16;

public class Day16 : IMDay
{
    private static readonly LinkedArray<Direction> _directions = new([
        Direction.North,
        Direction.East,
        Direction.South,
        Direction.West
    ]);

    public string FilePath { private get; init; } = "Day16\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var map = await GetInput();
        var start = map.First((_, v) => v == 'S');
        var end = map.First((_, v) => v == 'E');

        return map.GetShortestPath((start, Direction.East), end, GetNeighbors).ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var map = await GetInput();
        var start = map.First((_, v) => v == 'S');
        var end = map.First((_, v) => v == 'E');

        var shortest = map.GetShortestPath((start, Direction.East), end, GetNeighbors);
        var allShortestPaths = GetAllShortestPaths(map, start, Direction.East, end, shortest);

        return allShortestPaths
            .SelectMany(p => p)
            .Distinct()
            .Count()
            .ToString();
    }

    private static List<List<Point>> GetAllShortestPaths(Map<char> map, Point from, Direction startDirection, Point to, int costShortestPath)
    {
        List<List<Point>> finishedPaths = [];
        List<(List<Point>, Direction, int)> runningPaths = [([from], startDirection, 0)];
        Dictionary<(Point, Direction), int> currentCostPerPoint = new() { { (from, startDirection), 0 } };
        
        while (runningPaths.Count > 0)
        {
            List<(List<Point>, Direction, int)> newRunningPaths = [];

            foreach (var (path, direction, cost) in runningPaths)
            {
                var neighbors = GetNeighbors(map, (path[^1], direction));

                foreach (var (next, weightedPoint, extraCost) in neighbors)
                {
                    var (_, nextDirection) = weightedPoint;
                    var nextCost = cost + extraCost;

                    if (next == to && nextCost == costShortestPath)
                        finishedPaths.Add([..path, next]);
                    else if (nextCost < costShortestPath && (!currentCostPerPoint.TryGetValue((next, nextDirection), out var currentCost) || nextCost <= currentCost))
                    {
                        currentCostPerPoint.AddOrSet((next, nextDirection), nextCost);
                        newRunningPaths.Add(([..path, next], nextDirection, nextCost));
                    }
                }
            }

            runningPaths = newRunningPaths;
        }

        return finishedPaths;
    }

    private static List<(Point, (Point, Direction), int)> GetNeighbors(Map<char> map, (Point, Direction) point)
    {
        var (location, direction) = point;
        var next = location.Add(direction.ToPoint());

        List<(Point, (Point, Direction), int)> options = [
            (location, (location, _directions.GetPrevious(direction)), 1000),
            (location, (location, _directions.GetNext(direction)), 1000)
        ];

        if (map.GetValue(next) != '#')
            options.Add((next, (next, direction), 1));

        return options;
    }

    private async Task<Map<char>> GetInput() =>
        new(await FileParser.ReadLinesAsCharArray(FilePath));
}
