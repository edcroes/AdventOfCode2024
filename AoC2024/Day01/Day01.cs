namespace AoC2024.Day01;

public class Day01 : IMDay
{
    public string FilePath { private get; init; } = "Day01\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var (first, second) = await GetInput();
        return Enumerable
            .Range(0, first.Length)
            .Sum(i => Math.Abs(first[i] - second[i]))
            .ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var (first, second) = await GetInput();
        return first
            .Sum(n => n * second.Count(sn => sn == n))
            .ToString();
    }

    private async Task<(int[], int[])> GetInput()
    {
        var lines = await FileParser.ReadLinesAsIntArray(FilePath, " ");
        var first = lines.Select(l => l[0]).OrderBy(i => i).ToArray();
        var second = lines.Select(l => l[1]).OrderBy(i => i).ToArray();

        return (first, second);
    }
}
