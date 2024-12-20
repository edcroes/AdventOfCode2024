namespace AoC.Common.Maps;

public static class MapPathExtensions
{
    public static int GetShortestPath(this Map<int> map, Point toPoint) =>
        map.GetShortestPath(new Point(0, 0), toPoint);

    public static int GetShortestPath(this Map<int> map, Point fromPoint, Point toPoint) =>
        map.GetShortestPathCostMap(fromPoint, toPoint).GetValue(toPoint);

    public static Map<int> GetShortestPathCostMap(this Map<int> map, Point fromPoint, Point toPoint)
    {
        Map<int> currentCostPerPoint = new(map.SizeX, map.SizeY);
        HashSet<Point> visitedPoints = [];
        PriorityQueue<Point, int> openPositions = new();
        openPositions.Enqueue(fromPoint, 0);

        while (currentCostPerPoint.GetValue(toPoint) == 0)
        {
            var point = openPositions.Dequeue();
            var cost = currentCostPerPoint.GetValue(point);

            visitedPoints.Add(point);
            var nextPoints = map
                .GetStraightNeighbors(point)
                .Where(p => !visitedPoints.Contains(p));

            foreach (var nextPoint in nextPoints)
            {
                var nextCost = map.GetValue(nextPoint) + cost;
                var currentCost = currentCostPerPoint.GetValue(nextPoint);
                if (currentCost == 0 || nextCost < currentCost)
                {
                    currentCostPerPoint.SetValue(nextPoint, nextCost);
                    openPositions.Enqueue(nextPoint, nextCost);
                }
            }
        }

        return currentCostPerPoint;
    }

    public static int GetShortestPath<T>(this Map<T> map, Point fromPoint, Point toPoint, Func<Map<T>, Point, Point, bool> canMoveTo) where T : notnull =>
        map.GetShortestPathCostMap(fromPoint, toPoint, canMoveTo)?.GetValue(toPoint) ?? int.MaxValue;

    public static Map<int>? GetShortestPathCostMap<T>(this Map<T> map, Point fromPoint, Point toPoint, Func<Map<T>, Point, Point, bool> canMoveTo) where T : notnull
    {
        Map<int> currentCostPerPoint = new(map.SizeX, map.SizeY);
        HashSet<Point> visitedPoints = [];
        PriorityQueue<Point, int> openPositions = new();
        openPositions.Enqueue(fromPoint, 0);

        while (currentCostPerPoint.GetValue(toPoint) == 0)
        {
            if (openPositions.Count == 0)
            {
                // Dead end, no route possible
                return null;
            }

            var point = openPositions.Dequeue();
            var cost = currentCostPerPoint.GetValue(point);

            visitedPoints.Add(point);
            var nextPoints = map
                .GetStraightNeighbors(point)
                .Where(p => !visitedPoints.Contains(p) && canMoveTo(map, point, p));

            foreach (var nextPoint in nextPoints)
            {
                var nextCost = cost + 1;
                var currentCost = currentCostPerPoint.GetValue(nextPoint);
                if (currentCost == 0 || nextCost < currentCost)
                {
                    currentCostPerPoint.SetValue(nextPoint, nextCost);
                    openPositions.Enqueue(nextPoint, nextCost);
                }
            }
        }

        return currentCostPerPoint;
    }

    public static int GetShortestPath<TMap, TWeightedPoint>(this Map<TMap> map, TWeightedPoint fromWeighted, Point to, Func<Map<TMap>, TWeightedPoint, List<(Point, TWeightedPoint, int)>> getNeighbors)
        where TWeightedPoint : notnull
        where TMap : notnull
    {
        Dictionary<TWeightedPoint, int> currentCostPerPoint = new() { { fromWeighted, 0 } };
        HashSet<TWeightedPoint> visitedPoints = [];
        PriorityQueue<TWeightedPoint, int> openPositions = new();

        openPositions.Enqueue(fromWeighted, 0);
        var totalCost = -1;

        while (totalCost == -1)
        {
            var currentWeightedPoint = openPositions.Dequeue();
            visitedPoints.Add(currentWeightedPoint);
            var cost = currentCostPerPoint[currentWeightedPoint];

            var neighbors = getNeighbors(map, currentWeightedPoint);

            foreach (var (nextPoint, nextWeightedPoint, nextCost) in neighbors)
            {
                var newCost = cost + nextCost;

                if (nextPoint == to)
                {
                    totalCost = newCost;
                    break;
                }

                if (!currentCostPerPoint.TryGetValue(nextWeightedPoint, out var currentCost) || newCost < currentCost)
                {
                    currentCostPerPoint.AddOrSet(nextWeightedPoint, newCost);
                    openPositions.Enqueue(nextWeightedPoint, newCost);
                }
            }
        }

        return totalCost;
    }

    public static int GetLongestPath<T>(this Map<T> map, Point from, Point to, Func<Map<T>, Point, IEnumerable<Point>> getNeighbors) where T : notnull
    {
        List<List<Point>> routes = [[from]];
        var longestPath = 0;

        while (routes.Count > 0)
        {
            List<List<Point>> newRoutes = [];
            foreach (var route in routes)
            {
                var validNeighbors = getNeighbors(map, route[^1]).Where(n => !route.Contains(n));
                foreach (var neighbor in validNeighbors)
                {
                    if (neighbor == to)
                        longestPath = Math.Max(longestPath, route.Count);
                    else
                        newRoutes.Add(new(route) { neighbor });
                }
            }

            routes = newRoutes;
        }

        return longestPath;
    }

    public static int GetNumberOfPaths<T>(this Map<T> map, Point from, T toValue, Func<Map<T>, Point, T, IEnumerable<Point>> getNeighbors, Dictionary<Point, int>? cache = null) where T : notnull
    {
        cache ??= [];

        if (cache.TryGetValue(from, out int cachedPaths))
            return cachedPaths;

        var currentValue = map.GetValue(from);
        if (currentValue.Equals(toValue))
            return 1;

        var neighbors = getNeighbors(map, from, currentValue);
        var paths = neighbors.Sum(n => map.GetNumberOfPaths(n, toValue, getNeighbors, cache));

        cache.Add(from, paths);

        return paths;
    }

    public static int GetNumberOfPaths<T>(this Map<T> map, Point from, Point to, Func<Map<T>, Point, T, IEnumerable<Point>> getNeighbors, Dictionary<(Point, Point), int>? cache = null) where T : notnull
    {
        cache ??= [];

        if (cache.TryGetValue((from, to), out int cachedPaths))
            return cachedPaths;

        if (from == to)
            return 1;

        var neighbors = getNeighbors(map, from, map.GetValue(from));
        var paths = neighbors.Sum(n => map.GetNumberOfPaths(n, to, getNeighbors, cache));

        cache.Add((from, to), paths);

        return paths;
    }

    public static List<Point> MoveUntil<T>(this Map<T> map, Point from, Direction direction, Func<Point, T, bool> shouldStopMoving) where T : notnull
    {
        List<Point> path = [];
        var movement = direction.ToPoint();
        
        var next = from;
        while (map.Contains(next) && !shouldStopMoving(next, map.GetValue(next)))
        {
            path.Add(next);
            next = next.Add(movement);
        }

        return path;
    }
}