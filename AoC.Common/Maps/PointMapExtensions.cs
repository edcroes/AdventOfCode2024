using System.Numerics;

namespace AoC.Common.Maps;

public static class PointMapExtensions
{
    public static Point<TPointType> First<TPointType, TValue>(this PointMap<TPointType, TValue> map, Func<Point<TPointType>, TValue, bool> predicate) where TPointType : INumber<TPointType>
    {
        return map.Points.First(p => predicate(p, map.GetValue(p)));
    }

    public static Point<TPointType> MoveUntil<TPointType, TValue>(this PointMap<TPointType, TValue> map, Point<TPointType> start, Direction direction, Func<Point<TPointType>, TValue, bool> shouldStopMoving) where TPointType : INumber<TPointType>
    {
        var next = map.GetFirstPointInDirection(start, direction);

        while (next is not null && !shouldStopMoving(next.Value, map.GetValue(next.Value)))
        {
            next = map.GetFirstPointInDirection(next.Value, direction);
        }

        if (next is not null)
            return next.Value.Subtract(direction.ToGenericPoint<TPointType>());

        return direction switch
        {
            Direction.North => new(start.X, map.MinY),
            Direction.NorthEast => new(start.X + TPointType.Min(map.MaxX - start.X, start.Y - map.MinY), start.Y - TPointType.Min(map.MaxX - start.X, start.Y - map.MinY)),
            Direction.East => new(map.MaxX, start.Y),
            Direction.SouthEast => new(start.X + TPointType.Min(map.MaxX - start.X, map.MaxY - start.Y), start.Y + TPointType.Min(map.MaxX - start.X, map.MaxY - start.Y)),
            Direction.South => new(start.X, map.MaxY),
            Direction.SouthWest => new(start.X - TPointType.Min(start.X - map.MinX, map.MaxY - start.Y), start.Y + TPointType.Min(start.X - map.MinX, map.MaxY - start.Y)),
            Direction.West => new(map.MinX, start.Y),
            Direction.NorthWest => new(start.X - TPointType.Min(start.X - map.MinX, start.Y - map.MinY), start.Y - TPointType.Min(start.X - map.MinX, start.Y - map.MinY)),
            _ => throw new ArgumentOutOfRangeException(nameof(direction))
        };
    }

    public static Point<TPointType>? GetFirstPointInDirection<TPointType, TValue>(this PointMap<TPointType, TValue> map, Point<TPointType> start, Direction direction) where TPointType : INumber<TPointType>
    {
        var points = map.GetPointsInDirection(start, direction).OrderBy(start.GetManhattenDistance);

        if (points.Any())
            return points.First();

        return null;
    }

    public static IEnumerable<Point<TPointType>> GetAllPoints<TPointType, TValue>(this PointMap<TPointType, TValue> map, Point<TPointType> from, Point<TPointType> to) where TPointType: INumber<TPointType>
    {
        if (from.X != to.X && from.Y != to.Y && TPointType.Abs(from.Y - to.Y) != TPointType.Abs(from.X - to.X))
            throw new ArgumentException("Unable to get points that are not in a horizontal, vertical or 45 degrees line");

        HashSet<Point<TPointType>> points = [];

        var moveX = (to.X - from.X) switch
        {
            < 0 => -TPointType.One,
            > 0 => TPointType.One,
            _ => TPointType.Zero
        };

        var moveY = (to.Y - from.Y) switch
        {
            < 0 => -TPointType.One,
            > 0 => TPointType.One,
            _ => TPointType.Zero
        };

        Point<TPointType> movement = new(moveX, moveY);
        while (from != to)
        {
            points.Add(from);
            from = from.Add(movement);
        }

        points.Add(to);

        return points;
    }
}