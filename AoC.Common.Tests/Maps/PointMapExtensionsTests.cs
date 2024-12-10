using AoC.Common.Maps;

namespace AoC.Common.Tests.Maps;

[TestClass]
public class PointMapExtensionsTests
{
    [TestMethod]
    public void GetPointsInDirectionShouldReturnTheCorrectPointsForAFixedSizeMap()
    {
        var map = GetMap(true);
        Point<int> start = new(3, 3);

        CollectionAssert.AreEquivalent(new List<Point<int>>() { new(3, 0), new(3, 1) }, map.GetPointsInDirection(start, Direction.North).ToList());
        CollectionAssert.AreEquivalent(new List<Point<int>>() { new(4, 2), new(6, 0) }, map.GetPointsInDirection(start, Direction.NorthEast).ToList());
        CollectionAssert.AreEquivalent(new List<Point<int>>() { new(5, 3) }, map.GetPointsInDirection(start, Direction.East).ToList());
        CollectionAssert.AreEquivalent(new List<Point<int>>() { new(4, 4), new(5, 5) }, map.GetPointsInDirection(start, Direction.SouthEast).ToList());
        CollectionAssert.AreEquivalent(new List<Point<int>>() { new(3, 4), new(3, 6) }, map.GetPointsInDirection(start, Direction.South).ToList());
        CollectionAssert.AreEquivalent(new List<Point<int>>() { new(2, 4), new(0, 6) }, map.GetPointsInDirection(start, Direction.SouthWest).ToList());
        CollectionAssert.AreEquivalent(new List<Point<int>>() { new(0, 3), new(2, 3) }, map.GetPointsInDirection(start, Direction.West).ToList());
        CollectionAssert.AreEquivalent(new List<Point<int>>() { new(0, 0), new(1, 1) }, map.GetPointsInDirection(start, Direction.NorthWest).ToList());
    }

    [TestMethod()]
    public void GetPointsInDirectionShouldReturnTheCorrectPointsForANonFixedSizeMap()
    {
        var map = GetMap(false);
        Point<int> start = new(3, 3);

        CollectionAssert.AreEquivalent(new List<Point<int>>() { new(3, 0), new(3, 1) }, map.GetPointsInDirection(start, Direction.North).ToList());
        CollectionAssert.AreEquivalent(new List<Point<int>>() { new(4, 2), new(6, 0) }, map.GetPointsInDirection(start, Direction.NorthEast).ToList());
        CollectionAssert.AreEquivalent(new List<Point<int>>() { new(5, 3) }, map.GetPointsInDirection(start, Direction.East).ToList());
        CollectionAssert.AreEquivalent(new List<Point<int>>() { new(4, 4), new(5, 5) }, map.GetPointsInDirection(start, Direction.SouthEast).ToList());
        CollectionAssert.AreEquivalent(new List<Point<int>>() { new(3, 4), new(3, 6) }, map.GetPointsInDirection(start, Direction.South).ToList());
        CollectionAssert.AreEquivalent(new List<Point<int>>() { new(2, 4), new(0, 6) }, map.GetPointsInDirection(start, Direction.SouthWest).ToList());
        CollectionAssert.AreEquivalent(new List<Point<int>>() { new(0, 3), new(2, 3) }, map.GetPointsInDirection(start, Direction.West).ToList());
        CollectionAssert.AreEquivalent(new List<Point<int>>() { new(0, 0), new(1, 1) }, map.GetPointsInDirection(start, Direction.NorthWest).ToList());
    }

    [TestMethod()]
    public void GetFirstPointInDirectionShouldReturnTheCorrectPoint()
    {
        var map = GetMap(true);

        Assert.AreEqual(new(4, 2), map.GetFirstPointInDirection(new(1, 2), Direction.East));
        Assert.IsNull(map.GetFirstPointInDirection(new(3, 2), Direction.West));
        Assert.IsNull(map.GetFirstPointInDirection(new(2, 4), Direction.NorthWest));
        Assert.AreEqual(new(0, 1), map.GetFirstPointInDirection(new(2, 3), Direction.NorthWest));
        Assert.AreEqual(new(1, 1), map.GetFirstPointInDirection(new(1, 4), Direction.North));
    }

    [TestMethod]
    public void MoveUntilStopsAtTheEndOfTheMap()
    {
        var map = GetMoveTestMap();

        Assert.AreEqual(new(2, 0), map.MoveUntil(new(2, 4), Direction.North, (p, v) => v == '#'));
        Assert.AreEqual(new(4, 1), map.MoveUntil(new(2, 3), Direction.NorthEast, (p, v) => v == '#'));
        Assert.AreEqual(new(4, 3), map.MoveUntil(new(0, 3), Direction.East, (p, v) => v == '#'));
        Assert.AreEqual(new(3, 4), map.MoveUntil(new(1, 2), Direction.SouthEast, (p, v) => v == '#'));
        Assert.AreEqual(new(1, 4), map.MoveUntil(new(1, 2), Direction.South, (p, v) => v == '#'));
        Assert.AreEqual(new(1, 4), map.MoveUntil(new(4, 1), Direction.SouthWest, (p, v) => v == '#'));
        Assert.AreEqual(new(0, 2), map.MoveUntil(new(3, 2), Direction.West, (p, v) => v == '#'));
        Assert.AreEqual(new(0, 2), map.MoveUntil(new(2, 4), Direction.NorthWest, (p, v) => v == '#'));
    }

    [TestMethod]
    public void MoveUntilStopsAtAnObstacle()
    {
        var map = GetMoveTestMap();

        Assert.AreEqual(new(1, 2), map.MoveUntil(new(1, 2), Direction.North, (p, v) => v == '#'));
        Assert.AreEqual(new(1, 2), map.MoveUntil(new(1, 4), Direction.North, (p, v) => v == '#'));
        Assert.AreEqual(new(2, 1), map.MoveUntil(new(0, 3), Direction.NorthEast, (p, v) => v == '#'));
        Assert.AreEqual(new(3, 2), map.MoveUntil(new(0, 2), Direction.East, (p, v) => v == '#'));
        Assert.AreEqual(new(1, 3), map.MoveUntil(new(0, 2), Direction.SouthEast, (p, v) => v == '#'));
        Assert.AreEqual(new(0, 2), map.MoveUntil(new(0, 0), Direction.South, (p, v) => v == '#'));
        Assert.AreEqual(new(1, 2), map.MoveUntil(new(2, 1), Direction.SouthWest, (p, v) => v == '#'));
        Assert.AreEqual(new(1, 3), map.MoveUntil(new(4, 3), Direction.West, (p, v) => v == '#'));
        Assert.AreEqual(new(2, 1), map.MoveUntil(new(4, 3), Direction.NorthWest, (p, v) => v == '#'));
    }

    private static PointMap<int, char> GetMap(bool isFixedSize)
    {
        char[][] map = [
            ['X', 'X', 'X', 'X', 'X', 'X', 'X'],
            ['X', 'X', 'X', 'X', 'X', '.', 'X'],
            ['.', '.', '.', '.', 'X', 'X', 'X'],
            ['X', '.', 'X', 'X', '.', 'X', '.'],
            ['X', 'X', 'X', 'X', 'X', 'X', 'X'],
            ['X', '.', 'X', '.', 'X', 'X', 'X'],
            ['X', 'X', 'X', 'X', 'X', 'X', '.']
        ];

        return new(map, ['X'], isFixedSize);
    }

    private static PointMap<int, char> GetMoveTestMap()
    {
        char[][] map = [
            ['.', '#', '.', '#', '.'],
            ['.', '#', '.', '.', '.'],
            ['.', '.', '.', '.', '#'],
            ['#', '.', '.', '.', '.'],
            ['.', '.', '#', '.', '.'],
        ];

        return new(map, ['#'], true);
    }
}
