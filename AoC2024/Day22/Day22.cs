namespace AoC2024.Day22;

public class Day22 : IMDay
{
    public string FilePath { private get; init; } = "Day22\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var secrets = await GetInput();

        return secrets.Sum(s => GetNthSecretNumber(s, 2000)).ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var secrets = await GetInput();

        Dictionary<int, int> sequenceTotals = [];

        foreach (var secret in secrets)
        {
            var sequences = GetSequences(secret, 2000);
            foreach (var sequence in sequences.Keys)
            {
                sequenceTotals.AddOrUpdate(sequence, sequences[sequence]);
            }
        }

        return sequenceTotals.Values.Max().ToString();
    }

    private static Dictionary<int, int> GetSequences(long secret, int iterations)
    {
        Dictionary<int, int> sequences = [];

        List<int> changes = [];
        var last = (int)(secret % 10);

        for (var i = 0; i < iterations; i++)
        {
            secret = GetNextSecretNumber(secret);
            var current = (int)(secret % 10);
            changes.Add(current - last);

            if (changes.Count > 3)
            {
                var sequenceId = GetSequenceId(changes[^4], changes[^3], changes[^2], changes[^1]);
                _ = sequences.TryAdd(sequenceId, current);
            }

            last = current;
        }

        return sequences;
    }

    /// <summary>
    /// Get a unique id for the sequence combination. Makes each input positive by adding 9 (inputs are -9 >= and <= 9) and reserves 5 bits for each input (value 18 uses 5 bits).
    /// </summary>
    /// <param name="a">First number in the sequence</param>
    /// <param name="b">Second number in the sequnce</param>
    /// <param name="c">Third number in the sequence</param>
    /// <param name="d">Fourth number in the sequence</param>
    /// <returns>The unique id for this combination</returns>
    private static int GetSequenceId(int a, int b, int c, int d) =>
        ((a + 9) << 15) + ((b + 9) << 10) + ((c + 9) << 5) + d + 9;

    private static long GetNthSecretNumber(long secret, int n)
    {
        for (var i = 0; i < n; i++)
            secret = GetNextSecretNumber(secret);

        return secret;
    }

    private static long GetNextSecretNumber(long secret)
    {
        var next = ((secret << 6) ^ secret) & 16777215;
        next = ((next >> 5) ^ next) & 16777215;
        return ((next << 11) ^ next) & 16777215;
    }

    private async Task<long[]> GetInput() =>
        await FileParser.ReadLinesAsLong(FilePath);
}
