using AoC.Common.Extensions;

namespace AoC.Common.Tests.Extensions;

[TestClass]
public class IntExtensionsTests
{
    [TestMethod]
    public void GetDigitCountShouldReturnTheCorrectValue()
    {
        Assert.AreEqual(1, 0.GetDigitCount());
        Assert.AreEqual(2, 10.GetDigitCount());
        Assert.AreEqual(3, 100.GetDigitCount());
        Assert.AreEqual(4, 1000.GetDigitCount());
        Assert.AreEqual(5, 10000.GetDigitCount());
        Assert.AreEqual(6, 100000.GetDigitCount());
        Assert.AreEqual(7, 1000000.GetDigitCount());
        Assert.AreEqual(8, 10000000.GetDigitCount());
        Assert.AreEqual(9, 100000000.GetDigitCount());
        Assert.AreEqual(9, 999999999.GetDigitCount());
        Assert.AreEqual(10, 1000000000.GetDigitCount());
        Assert.AreEqual(10, int.MaxValue.GetDigitCount());

        Assert.AreEqual(1, (-1).GetDigitCount());
        Assert.AreEqual(2, (-10).GetDigitCount());
        Assert.AreEqual(3, (-100).GetDigitCount());
        Assert.AreEqual(4, (-1000).GetDigitCount());
        Assert.AreEqual(5, (-10000).GetDigitCount());
        Assert.AreEqual(6, (-100000).GetDigitCount());
        Assert.AreEqual(7, (-1000000).GetDigitCount());
        Assert.AreEqual(8, (-10000000).GetDigitCount());
        Assert.AreEqual(9, (-100000000).GetDigitCount());
        Assert.AreEqual(9, (-999999999).GetDigitCount());
        Assert.AreEqual(10, (-1000000000).GetDigitCount());
        Assert.AreEqual(10, int.MinValue.GetDigitCount());
    }
}
