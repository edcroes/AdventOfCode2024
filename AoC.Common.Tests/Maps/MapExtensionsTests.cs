using AoC.Common.Maps;

namespace AoC.Common.Tests.Maps;

[TestClass]
public class MapExtensionsTests
{
    [TestMethod]
    public void FindForAllDirectionsShouldReturnTheCorrectResult()
    {
        var map = GetMap();

        var result = map.Find("WTF".ToCharArray());
        Assert.IsNotNull(result);
        Assert.AreEqual(new(new(3, 0), Direction.South), result);

        result = map.Find("BBQ".ToCharArray(), Direction.All);
        Assert.IsNotNull(result);
        Assert.AreEqual(new(new(2, 0), Direction.SouthWest), result);

        result = map.Find("NOT".ToCharArray());
        Assert.IsNull(result);
    }

    [TestMethod]
    public void FindForSpecificDirectionShouldReturnTheCorrectResult()
    {
        var map = GetMap();

        var result = map.Find("WTF".ToCharArray(), Direction.North | Direction.South);
        Assert.IsNotNull(result);
        Assert.AreEqual(new(new(3, 0), Direction.South), result);

        result = map.Find("WTF".ToCharArray(), Direction.North | Direction.East);
        Assert.IsNull(result);

        result = map.Find("BBQ".ToCharArray(), Direction.West);
        Assert.IsNotNull(result);
        Assert.AreEqual(new(new(2, 2), Direction.West), result);

        result = map.Find("BBQ".ToCharArray(), Direction.None);
        Assert.IsNull(result);
    }

    [TestMethod]
    public void FindAllForAllDirectionsShouldReturnTheCorrectResult1()
    {
        var map = GetMap();

        var results = map.FindAll("BBQ".ToCharArray()).ToList();
        Assert.AreEqual(4, results.Count);
        CollectionAssert.Contains(results, new SearchResult(new(2, 0), Direction.SouthWest));
        CollectionAssert.Contains(results, new SearchResult(new(1, 1), Direction.SouthEast));
        CollectionAssert.Contains(results, new SearchResult(new(1, 1), Direction.South));
        CollectionAssert.Contains(results, new SearchResult(new(2, 2), Direction.West));

        results = map.FindAll("WTF".ToCharArray()).ToList();
        Assert.AreEqual(1, results.Count);
        CollectionAssert.Contains(results, new SearchResult(new(3, 0), Direction.South));

        results = map.FindAll("NOT".ToCharArray()).ToList();
        Assert.AreEqual(0, results.Count);
    }

    [TestMethod]
    public void FindAllForAllDirectionsShouldReturnTheCorrectResult2()
    {
        Map<char> map = new([
            ['F', 'X', 'F', 'X', 'F'],
            ['X', 'T', 'T', 'T', 'X'],
            ['F', 'T', 'W', 'T', 'F'],
            ['X', 'T', 'T', 'T', 'X'],
            ['F', 'X', 'F', 'X', 'F']
        ]);

        var results = map.FindAll("WTF".ToCharArray()).ToList();

        Assert.AreEqual(8, results.Count);
        CollectionAssert.Contains(results, new SearchResult(new(2, 2), Direction.North));
        CollectionAssert.Contains(results, new SearchResult(new(2, 2), Direction.NorthEast));
        CollectionAssert.Contains(results, new SearchResult(new(2, 2), Direction.East));
        CollectionAssert.Contains(results, new SearchResult(new(2, 2), Direction.SouthEast));
        CollectionAssert.Contains(results, new SearchResult(new(2, 2), Direction.South));
        CollectionAssert.Contains(results, new SearchResult(new(2, 2), Direction.SouthWest));
        CollectionAssert.Contains(results, new SearchResult(new(2, 2), Direction.West));
        CollectionAssert.Contains(results, new SearchResult(new(2, 2), Direction.NorthWest));
    }

    [TestMethod]
    public void FindAllForSpecificDirectionShouldReturnTheCorrectResult()
    {
        var map = GetMap();

        var results = map.FindAll("BBQ".ToCharArray(), Direction.South | Direction.SouthEast | Direction.North).ToList();
        Assert.AreEqual(2, results.Count);
        CollectionAssert.Contains(results, new SearchResult(new(1, 1), Direction.SouthEast));
        CollectionAssert.Contains(results, new SearchResult(new(1, 1), Direction.South));

        results = map.FindAll("WTF".ToCharArray(), Direction.North).ToList();
        Assert.AreEqual(0, results.Count);
    }

    private static Map<char> GetMap() =>
        new([
            ['A', 'R', 'B',  'W'],
            ['B', 'B', 'Z',  'T'],
            ['Q', 'B', 'B',  'F'],
            ['Q', 'Q', 'G',  'Q']
        ]);
}