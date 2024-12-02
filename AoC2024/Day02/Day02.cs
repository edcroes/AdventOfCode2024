namespace AoC2024.Day02;

public class Day02 : IMDay
{
    public string FilePath { private get; init; } = "Day02\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var input = await GetInput();
        return input.Count(IsValid).ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var input = await GetInput();
        return input.Count(l => IsValid(l) || IsValidWithOneRemoval(l)).ToString();
    }

    private static bool IsValidWithOneRemoval(int[] list) =>
        Enumerable
            .Range(0, list.Length)
            .Select(i => list.WithRemovedIndex(i))
            .Any(IsValid);

    private static bool IsValid(int[] list)
    {
        var differences = GetDifferences(list);
        return differences.All(v => v is < 0 and >= -3) || differences.All(v => v is > 0 and <= 3);
    }

    private static int[] GetDifferences(int[] list) =>
        Enumerable
            .Range(1, list.Length - 1)
            .Select(i => list[i] - list[i - 1])
            .ToArray();

    private async Task<int[][]> GetInput() =>
        await FileParser.ReadLinesAsIntArray(FilePath, " ");
}
