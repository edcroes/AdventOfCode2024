﻿using System.Drawing;

namespace AoC.Common.Maps;

public static class MapExtensions
{
    public static IEnumerable<TReturn> Select<T, TReturn>(this Map<T> map, Func<Point, T, TReturn> predicate) where T : notnull
    {
        for (var y = 0; y < map.SizeY; y++)
        {
            for (var x = 0; x < map.SizeX; x++)
            {
                var point = new Point(x, y);
                yield return predicate(point, map.GetValue(point));
            }
        }
    }

    public static Point First<T>(this Map<T> map, Func<Point, T, bool> predicate) where T : notnull
    {
        for (var y = 0; y < map.SizeY; y++)
        {
            for (var x = 0; x < map.SizeX; x++)
            {
                var point = new Point(x, y);
                if (predicate(point, map.GetValue(point)))
                {
                    return point;
                }
            }
        }

        throw new Exception();
    }

    public static Point Last<T>(this Map<T> map, Func<Point, T, bool> predicate) where T : notnull
    {
        for (var y = map.SizeY - 1; y >= 0; y--)
        {
            for (var x = map.SizeX - 1; x >= 0; x--)
            {
                var point = new Point(x, y);
                if (predicate(point, map.GetValue(point)))
                {
                    return point;
                }
            }
        }

        throw new Exception();
    }

    public static IEnumerable<Point> Where<T>(this Map<T> map, Func<Point, T, bool> predicate) where T : notnull
    {
        for (var y = 0; y < map.SizeY; y++)
        {
            for (var x = 0; x < map.SizeX; x++)
            {
                var point = new Point(x, y);
                if (predicate(point, map.GetValue(point)))
                {
                    yield return point;
                }
            }
        }
    }

    public static int Count<T>(this Map<T> map, T valueToMatch) where T : notnull
    {
        int count = 0;
        for (int y = 0; y < map.SizeY; y++)
        {
            for (int x = 0; x < map.SizeX; x++)
            {
                if (map.GetValue(x, y).Equals(valueToMatch))
                {
                    count++;
                }
            }
        }

        return count;
    }

    public static int Count<T>(this Map<T> map, Func<Point, T, bool> predicate) where T : notnull
    {
        int count = 0;
        for (int y = 0; y < map.SizeY; y++)
        {
            for (int x = 0; x < map.SizeX; x++)
            {
                if (predicate(new(x, y), map.GetValue(x, y)))
                {
                    count++;
                }
            }
        }

        return count;
    }

    public static bool All<T>(this Map<T> map, Func<Point, T, bool> predicate) where T : notnull
    {
        for (var y = 0; y < map.SizeY; y++)
        {
            for (var x = 0; x < map.SizeX; x++)
            {
                var point = new Point(x, y);
                if (!predicate(point, map.GetValue(point)))
                {
                    return false;
                }
            }
        }

        return true;
    }

    public static bool Any<T>(this Map<T> map, Func<Point, T, bool> predicate) where T : notnull
    {
        for (var y = 0; y < map.SizeY; y++)
        {
            for (var x = 0; x < map.SizeX; x++)
            {
                var point = new Point(x, y);
                if (predicate(point, map.GetValue(point)))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public static Map<T> GetSubMap<T>(this Map<T> map, Point from, Point to) where T : notnull
    {
        if (from.X < 0 || from.X >= map.SizeX || from.Y < 0 || from.Y >= map.SizeY)
        {
            throw new ArgumentException("Range is outside the map");
        }

        if (from.X > to.X || from.Y > to.Y)
        {
            throw new ArgumentException("'from' should come before 'to'");
        }

        var newSizeX = to.X - from.X + 1;
        var newSizeY = to.Y - from.Y + 1;
        Map<T> newMap = new(newSizeX, newSizeY);

        for (var y = from.Y; y <= to.Y; y++)
        {
            for (var x = from.X; x <= to.X; x++)
            {
                newMap.SetValue(x - from.X, y - from.Y, map.GetValue(x, y));
            }
        }

        return newMap;
    }

    public static bool HasValueOnBorder<T>(this Map<T> map, T value) where T : notnull =>
            map.GetLine(0, 0, map.SizeX - 1, 0).Contains(value) ||
            map.GetLine(0, 0, 0, map.SizeY - 1).Contains(value) ||
            map.GetLine(map.SizeX - 1, 0, map.SizeX - 1, map.SizeY - 1).Contains(value) ||
            map.GetLine(0, map.SizeY - 1, map.SizeX - 1, map.SizeY - 1).Contains(value);

    public static Map<T> EnlargeMapByOneOnEachSide<T>(this Map<T> map, T? valueForNewPoints) where T : notnull =>
        EnlargeMap(map, map.SizeX + 2, map.SizeY + 2, valueForNewPoints);

    public static Map<T> EnlargeMap<T>(this Map<T> map, int newSizeX, int newSizeY, T? valueForNewPoints) where T : notnull
    {
        if (newSizeX < map.SizeX || newSizeY < map.SizeY)
        {
            throw new ArgumentException($"{nameof(newSizeX)} and {nameof(newSizeY)} should be greater than or equal to the current size.");
        }

        Map<T> newMap = new(newSizeX, newSizeY);
        var startX = (newSizeX - map.SizeX) / 2;
        var startY = (newSizeY - map.SizeY) / 2;

        if (valueForNewPoints is not null)
        {
            for (var y = 0; y < newMap.SizeY; y++)
            {
                for (var x = 0; x < newMap.SizeX; x++)
                {
                    if (x < startX || x >= startX + map.SizeX ||
                        y < startY || y >= startY + map.SizeY)
                    {
                        newMap.SetValue(x, y, valueForNewPoints);
                    }
                }
            }
        }

        map.CopyTo(newMap, new Point(startX, startY));
        return newMap;
    }

    public static SpannedInt GetSpannedInt(this Map<char> map, Point point)
    {
        var value = map.GetValue(point);
        if (!value.IsNumber())
            throw new NotSupportedException($"{value} is not a number");

        var start = point;
        var totalValue = value.ToNumber();

        for (var x = point.X - 1; x >= 0; x--)
        {
            Point left = new(x, point.Y);
            var other = map.GetValue(left);
            if (other.IsNumber())
            {
                start = left;
                totalValue += other.ToNumber() * (int)Math.Pow(10, point.X - x);
            }
            else
                break;
        }

        for (var x = point.X + 1; x < map.SizeX; x++)
        {
            Point right = new(x, point.Y);
            var other = map.GetValue(right);
            if (other.IsNumber())
                totalValue = totalValue * 10 + other.ToNumber();
            else
                break;
        }

        return new(start, totalValue);
    }

    public static int GetMirrorOnX<T>(this Map<T> map, int skipX = -1, int faultsToAllow = 0) where T : notnull
    {
        for (var x = 0; x < map.SizeX - 1; x++)
        {
            if (x == skipX)
                continue;

            var current = map.GetLine(x, 0, x, map.SizeY - 1);
            var next = map.GetLine(x + 1, 0, x + 1, map.SizeY - 1);
            var nonMatching = current.GetNumberOfNonMatchingElements(next);

            if (nonMatching < faultsToAllow + 1)
            {
                var move = 1;
                while (x + move + 1 < map.SizeX && x - move >= 0)
                {
                    var left = map.GetLine(x - move, 0, x - move, map.SizeY - 1);
                    var right = map.GetLine(x + move + 1, 0, x + move + 1, map.SizeY - 1);

                    nonMatching += left.GetNumberOfNonMatchingElements(right);
                    if (nonMatching > faultsToAllow)
                        move = int.MaxValue;
                    else
                        move++;
                }

                if (move != int.MaxValue)
                    return x;
            }
        }

        return -1;
    }

    public static int GetMirrorOnY<T>(this Map<T> map, int skipY = -1, int faultsToAllow = 0) where T : notnull
    {
        var newMap = map.Clone();
        newMap.RotateLeft();
        return GetMirrorOnX(newMap, skipY, faultsToAllow);
    }

    public static SearchResult? Find<T>(this Map<T> map, T[] rangeToFind, Direction direction = Direction.All) where T : notnull
    {
        var directions = GetSuppliedDirections(direction);

        for (var y = 0; y < map.SizeY; y++)
        {
            for (var x = 0; x < map.SizeX; x++)
            {
                Point point = new(x, y);
                var foundDirection = directions.FirstOrDefault(d => HasSearchTerm(map, rangeToFind, point, d));
                if (foundDirection != Direction.None)
                {
                    return new(point, foundDirection);
                }
            }
        }

        return null;
    }

    public static IEnumerable<SearchResult> FindAll<T>(this Map<T> map, T[] rangeToFind, Direction direction = Direction.All) where T : notnull
    {
        var directions = GetSuppliedDirections(direction);
        List<SearchResult> results = [];

        map.ForEachPoint(p =>
        {
            var foundDirections = directions.Where(d => HasSearchTerm(map, rangeToFind, p, d));
            foreach (var dir in foundDirections)
            {
                results.Add(new(p, dir));
            }
        });

        return results;
    }

    private static Direction[] GetSuppliedDirections(Direction directions) =>
        Enum.GetValues<Direction>()
            .Where(e => !e.IsIn(Direction.All, Direction.None) && directions.HasFlag(e))
            .ToArray();

    private static bool HasSearchTerm<T>(Map<T> map, T[] searchTerm, Point start, Direction direction) where T : notnull
    {
        foreach (var item in searchTerm)
        {
            if (map.Contains(start) && item.Equals(map.GetValueOrDefault(start)))
            {
                start = start.Add(direction.ToPoint());
            }
            else
            {
                return false;
            }
        }

        return true;
    }
}