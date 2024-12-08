﻿using System.Numerics;

namespace AoC.Common.Maps;

public class PointMap<TPointType, TValue> where TPointType : INumber<TPointType>
{
    private readonly Dictionary<Point<TPointType>, TValue> _points = [];
    private readonly bool _isFixedSize = false;
    private readonly TPointType? _sizeX = default;
    private readonly TPointType? _sizeY = default;

    public PointMap() { }

    public PointMap(TValue[][] values, TValue[] valuesToKeep, bool fixedSize = false)
    {
        if (valuesToKeep is null || valuesToKeep.Length == 0)
            throw new ArgumentNullException(nameof(valuesToKeep));

        for (var y = 0; y < values.Length; y++)
        {
            for (var x = 0; x < values.Min(l => l.Length); x++)
            {
                if (values[y][x] != null && valuesToKeep.Contains(values[y][x]))
                {
                    SetValue(new(TPointType.CreateChecked(x), TPointType.CreateChecked(y)), values[y][x]);
                }
            }
        }

        _isFixedSize = fixedSize;
        if (fixedSize)
        {
            _sizeX = TPointType.CreateChecked(values.Min(l => l.Length));
            _sizeY = TPointType.CreateChecked(values.Length);
        }
    }

    public PointMap(TValue[][] values, bool insertDefaultValue = false, TValue? defaultValue = default, bool fixedSize = false)
    {
        for (var y = 0; y < values.Length; y++)
        {
            for (var x = 0; x < values.Min(l => l.Length); x++)
            {
                if ((values[y][x] != null && !values[y][x]!.Equals(defaultValue)) || insertDefaultValue)
                {
                    SetValue(new(TPointType.CreateChecked(x), TPointType.CreateChecked(y)), values[y][x]);
                }
            }
        }

        _isFixedSize = fixedSize;
        if (fixedSize)
        {
            _sizeX = TPointType.CreateChecked(values.Min(l => l.Length));
            _sizeY = TPointType.CreateChecked(values.Length);
        }
    }

    public TPointType SizeX => _isFixedSize ? _sizeX! : GetBoundingRectangle().Width;

    public TPointType SizeY => _isFixedSize ? _sizeY! : GetBoundingRectangle().Height;

    public TPointType MinX => _isFixedSize ? TPointType.Zero : _points.Keys.Min(p => p.X);
    public TPointType MaxX => _isFixedSize ? SizeX - TPointType.One : _points.Keys.Max(p => p.X);
    public TPointType MinY => _isFixedSize ? TPointType.Zero : _points.Keys.Min(p => p.Y);
    public TPointType MaxY => _isFixedSize ? SizeY - TPointType.One : _points.Keys.Max(p => p.Y);

    public IReadOnlyList<Point<TPointType>> Points =>
        _points.Keys.ToList();

    public TValue GetValue(TPointType x, TPointType y) =>
        GetValue(new(x, y));

    public TValue GetValue(Point<TPointType> point) =>
        _points.TryGetValue(point, out TValue? value) ? value : throw new ArgumentOutOfRangeException(nameof(point));

    public TValue? GetValueOrDefault(TPointType x, TPointType y, TValue defaultValue = default) =>
        GetValueOrDefault(new(x, y), defaultValue);

    public TValue? GetValueOrDefault(Point<TPointType> point, TValue defaultValue = default) =>
        _points.TryGetValue(point, out TValue? value) ? value : defaultValue;

    public void SetValue(TPointType x, TPointType y, TValue value) =>
        SetValue(new(x, y), value);

    public void SetValue(Point<TPointType> point, TValue value) =>
        _points.AddOrSet(point, value);

    public void RemoveValue(Point<TPointType> point) =>
        _points.Remove(point);

    public IEnumerable<Point<TPointType>> GetStraightAndDiagonalNeighbors(Point<TPointType> point)
    {
        List<Point<TPointType>> neighbors = [];

        for (var y = point.Y - TPointType.One; y <= point.Y + TPointType.One; y++)
        {
            for (var x = point.X - TPointType.One; x <= point.X + TPointType.One; x++)
            {
                if (y == point.Y && x == point.X)
                {
                    continue;
                }

                neighbors.Add(new Point<TPointType>(x, y));
            }
        }

        return neighbors;
    }

    public int NumberOfStraightAndDiagonalNeighborsThatMatch(Point<TPointType> point, TValue valueToMatch) =>
        NumberOfStraightAndDiagonalNeighborsThatMatch(point, p => _points.TryGetValue(p, out TValue? value) && valueToMatch.Equals(value));

    public int NumberOfStraightAndDiagonalNeighborsThatMatch(Point<TPointType> point, Func<Point<TPointType>, bool> getMatch) =>
        GetStraightAndDiagonalNeighbors(point).Count(getMatch);

    public Rectangle<TPointType> GetBoundingRectangle()
    {
        if (_points.Keys.Count == 0)
            throw new InvalidOperationException("Unable to determine the bounding rectangle of a map without points");

        var minX = _points.Keys.Min(p => p.X);
        var maxX = _points.Keys.Max(p => p.X);
        var minY = _points.Keys.Min(p => p.Y);
        var maxY = _points.Keys.Max(p => p.Y);

        return new(minX!, minY!, maxX! - minX! + TPointType.One, maxY! - minY! + TPointType.One);
    }

    public bool Contains(Point<TPointType> point) =>
        point.X >= MinX && point.X <= MaxX && point.Y >= MinY && point.Y <= MaxY;
}
