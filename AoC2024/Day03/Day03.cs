namespace AoC2024.Day03;

public partial class Day03 : IMDay
{
    private readonly Regex _mulRegex = MulRegex();
    public string FilePath { private get; init; } = "Day03\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var input = await GetInput();
        
        return input.Sum(m => m.X * m.Y).ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var input = await GetInput2();

        return input.Sum(m => m.X * m.Y).ToString();
    }

    private async Task<Mul[]> GetInput() =>
        await FileParser.ReadLineUsingRegex(FilePath, _mulRegex, ParseMul);

    private async Task<IEnumerable<Mul>> GetInput2()
    {
        var result = string.Join("", await FileParser.ReadLinesAsString(FilePath));
        return result
            .Split("do()")
            .Select(p => p.Split("don't()").First())
            .SelectMany(l => l.FromRegex(_mulRegex).Select(ParseMul));
    }

    private static Mul ParseMul(dynamic mul) =>
        new(int.Parse(mul.X), int.Parse(mul.Y));

    [GeneratedRegex(@"mul\((?<X>\d{1,3}),(?<Y>\d{1,3})\)")]
    private static partial Regex MulRegex();

    private record struct Mul(int X, int Y);
}
