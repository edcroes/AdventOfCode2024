namespace AoC.Common.Extensions;
public static class IntExtensions
{
    public static int GetDigitCount(this int value)
    {
        return value switch
        {
            >= 0 and < 10 => 1,
            >= 10 and < 100 => 2,
            >= 100 and < 1000 => 3,
            >= 1000 and < 10000 => 4,
            >= 10000 and < 100000 => 5,
            >= 100000 and < 1000000 => 6,
            >= 1000000 and < 10000000 => 7,
            >= 10000000 and < 100000000 => 8,
            >= 100000000 and < 1000000000 => 9,
            >= 1000000000 => 10,
            < 0 and > -10 => 1,
            <= -10 and > -100 => 2,
            <= -100 and > -1000 => 3,
            <= -1000 and > -10000 => 4,
            <= -10000 and > -100000 => 5,
            <= -100000 and > -1000000 => 6,
            <= -1000000 and > -10000000 => 7,
            <= -10000000 and > -100000000 => 8,
            <= -100000000 and > -1000000000 => 9,
            <= -1000000000 => 10
        };
    }

    public static int Join(this int value, int other) =>
        value * (int)Math.Pow(10, other.GetDigitCount()) + other;

    public static bool IsEven(this int value) =>
        value % 2 == 0;
}
