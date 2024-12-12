using AoC.Common.Collections;

namespace AoC.Common.Maps;

public class Area<T> where T : notnull
{
    private static readonly LinkedArray<Direction> _directions = new([
        Direction.North,
        Direction.East,
        Direction.South,
        Direction.West
    ]);
    private readonly HashSet<Point> _points = [];

    public required T Value { get; init; }
    public int Count => _points.Count;


    public void Add(Point point)
    {
        if (_points.Count > 0 &&
            !_points.Any(p =>
                p == point.Add(Direction.North.ToPoint()) ||
                p == point.Add(Direction.East.ToPoint()) ||
                p == point.Add(Direction.South.ToPoint()) ||
                p == point.Add(Direction.West.ToPoint())))
        {
            throw new ArgumentOutOfRangeException(nameof(point), $"Point {point} doesn't touch any existing point of the area");
        }

        _points.Add(point);
    }

    public bool Contains(Point point) => _points.Contains(point);

    public HashSet<Point> GetPoints() => new(_points);

    public List<HashSet<Point>> GetSides()
    {
        var borderPoints = GetBorderPoints();
        List<HashSet<Point>> sides = [];

        while (borderPoints.Count > 0)
        {
            HashSet<Point> side = [];
            var (start, direction) = borderPoints.First();

            var next = start;
            while (borderPoints.Contains((next, direction)))
            {
                borderPoints.Remove((next, direction));
                side.Add(next);
                next = next.Add(_directions.GetNext(direction).ToPoint());
            }

            var previous = start.Add(_directions.GetPrevious(direction).ToPoint());
            while (borderPoints.Contains((previous, direction)))
            {
                borderPoints.Remove((previous, direction));
                side.Add(previous);
                previous = previous.Add(_directions.GetPrevious(direction).ToPoint());
            }

            sides.Add(side);
        }

        return sides;
    }

    public HashSet<(Point Point, Direction Direction)> GetBorderPoints()
    {
        HashSet<(Point, Direction)> border = [];
        Direction[] directions = [Direction.North, Direction.East, Direction.South, Direction.West];

        foreach (var p in _points)
        {
            foreach (var d in directions)
            {
                if (!_points.Contains(p.Add(d.ToPoint())))
                    border.Add((p, d));
            }
        }

        return border;
    }
}
