using AoC.Common.Extensions;

namespace AoC.Common.Tests.Extensions;

[TestClass]
public class LongExtensionsTests
{
    [TestMethod]
    public void GetDigitCountShouldReturnTheCorrectValue()
    {
        Assert.AreEqual(1, 0L.GetDigitCount());
        Assert.AreEqual(2, 10L.GetDigitCount());
        Assert.AreEqual(3, 100L.GetDigitCount());
        Assert.AreEqual(4, 1000L.GetDigitCount());
        Assert.AreEqual(5, 10000L.GetDigitCount());
        Assert.AreEqual(6, 100000L.GetDigitCount());
        Assert.AreEqual(7, 1000000L.GetDigitCount());
        Assert.AreEqual(8, 10000000L.GetDigitCount());
        Assert.AreEqual(9, 100000000L.GetDigitCount());
        Assert.AreEqual(9, 999999999L.GetDigitCount());
        Assert.AreEqual(10, 1000000000L.GetDigitCount());
        Assert.AreEqual(11, 10000000000L.GetDigitCount());
        Assert.AreEqual(12, 100000000000L.GetDigitCount());
        Assert.AreEqual(13, 1000000000000L.GetDigitCount());
        Assert.AreEqual(14, 10000000000000L.GetDigitCount());
        Assert.AreEqual(15, 100000000000000L.GetDigitCount());
        Assert.AreEqual(16, 1000000000000000L.GetDigitCount());
        Assert.AreEqual(17, 10000000000000000L.GetDigitCount());
        Assert.AreEqual(18, 100000000000000000L.GetDigitCount());
        Assert.AreEqual(19, 1000000000000000000L.GetDigitCount());
        Assert.AreEqual(19, long.MaxValue.GetDigitCount());

        Assert.AreEqual(1, (-1L).GetDigitCount());
        Assert.AreEqual(2, (-10L).GetDigitCount());
        Assert.AreEqual(3, (-100L).GetDigitCount());
        Assert.AreEqual(4, (-1000L).GetDigitCount());
        Assert.AreEqual(5, (-10000L).GetDigitCount());
        Assert.AreEqual(6, (-100000L).GetDigitCount());
        Assert.AreEqual(7, (-1000000L).GetDigitCount());
        Assert.AreEqual(8, (-10000000L).GetDigitCount());
        Assert.AreEqual(9, (-100000000L).GetDigitCount());
        Assert.AreEqual(9, (-999999999L).GetDigitCount());
        Assert.AreEqual(10, (-1000000000L).GetDigitCount());
        Assert.AreEqual(11, (-10000000000L).GetDigitCount());
        Assert.AreEqual(12, (-100000000000L).GetDigitCount());
        Assert.AreEqual(13, (-1000000000000L).GetDigitCount());
        Assert.AreEqual(14, (-10000000000000L).GetDigitCount());
        Assert.AreEqual(15, (-100000000000000L).GetDigitCount());
        Assert.AreEqual(16, (-1000000000000000L).GetDigitCount());
        Assert.AreEqual(17, (-10000000000000000L).GetDigitCount());
        Assert.AreEqual(18, (-100000000000000000L).GetDigitCount());
        Assert.AreEqual(19, (-1000000000000000000L).GetDigitCount());
        Assert.AreEqual(19, long.MinValue.GetDigitCount());
    }
}
