using System.Numerics;

namespace AoC.Common.Maps;

public static class DirectionExtensions
{
    public static Point ToPoint(this Direction direction) =>
        direction switch
        {
            Direction.North => new(0, -1),
            Direction.NorthEast => new(1, -1),
            Direction.East => new(1, 0),
            Direction.SouthEast => new(1, 1),
            Direction.South => new(0, 1),
            Direction.SouthWest => new(-1, 1),
            Direction.West => new(-1, 0),
            Direction.NorthWest => new(-1, -1),
            _ => throw new ArgumentOutOfRangeException(nameof(direction))
        };

    public static Direction ToDirection(this Point direction) =>
        direction switch
        {
            { X:  0, Y: -1 } => Direction.North,
            { X:  1, Y: -1 } => Direction.NorthEast,
            { X:  1, Y:  0 } => Direction.East,
            { X:  1, Y:  1 } => Direction.SouthEast,
            { X:  0, Y:  1 } => Direction.South,
            { X: -1, Y:  1 } => Direction.SouthWest,
            { X: -1, Y:  0 } => Direction.West,
            { X: -1, Y: -1 } => Direction.NorthWest,
            _ => throw new ArgumentOutOfRangeException(nameof(direction))
        };

    public static Point<T> ToGenericPoint<T>(this Direction direction) where T : INumber<T>  =>
        direction switch
        {
            Direction.North => new(T.Zero, -T.One),
            Direction.NorthEast => new(T.One, -T.One),
            Direction.East => new(T.One, T.Zero),
            Direction.SouthEast => new(T.One, T.One),
            Direction.South => new(T.Zero, T.One),
            Direction.SouthWest => new(-T.One, T.One),
            Direction.West => new(-T.One, T.Zero),
            Direction.NorthWest => new(-T.One, -T.One),
            _ => throw new ArgumentOutOfRangeException(nameof(direction))
        };
}
