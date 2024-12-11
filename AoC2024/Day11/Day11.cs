namespace AoC2024.Day11;

public class Day11 : IMDay
{
    private static readonly Dictionary<(long, long), long> _cache = [];

    public string FilePath { private get; init; } = "Day11\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var input = await GetInput();

        return input.Sum(s => NumberOfStones(s, 25)).ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var input = await GetInput();

        return input.Sum(s => NumberOfStones(s, 75)).ToString();
    }

    private static long NumberOfStones(long stone, long numberOfRounds)
    {
        if (numberOfRounds == 0)
            return 1;

        if (_cache.TryGetValue((stone, numberOfRounds), out var result))
            return result;

        var stones = stone switch
        {
            0 => [1],
            long s when s.GetDigitCount().IsEven() => GetSplittedNumbers(stone),
            _ => [stone * 2024]
        };

        result = stones.Sum(s => NumberOfStones(s, numberOfRounds - 1));
        _cache.Add((stone, numberOfRounds), result);

        return result;
    }

    private static IEnumerable<long> GetSplittedNumbers(long stone)
    {
        var digits = stone.GetDigitCount() / 2;
        var divider = (long)Math.Pow(10, digits);

        return [stone / divider, stone % divider];
    }

    private async Task<long[]> GetInput() =>
        await FileParser.ReadLineAsLongArray(FilePath, " ");
}
