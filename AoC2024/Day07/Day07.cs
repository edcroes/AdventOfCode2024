namespace AoC2024.Day07;

public class Day07 : IMDay
{
    private static readonly Dictionary<char, Func<long, long, long>> _operations = new() {
        { '+', (a, b) => a + b },
        { '*', (a, b) => a * b },
        { '|', (a, b) => a.Join(b) }
    };

    public string FilePath { private get; init; } = "Day07\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var input = await GetInput();

        return input
            .Where(l => IsSolvable(l[0], l[1..], ['+', '*']))
            .Sum(l => l[0])
            .ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var input = await GetInput();

        return input
            .Where(l => IsSolvable(l[0], l[1..], ['+', '*', '|']))
            .Sum(l => l[0])
            .ToString();
    }

    private static bool IsSolvable(long answer, long[] input, char[] operators)
    {
        IEnumerable<long> options = [input[0]];

        foreach (var number in input[1..])
        {
            options = options.SelectMany(option =>
                operators
                    .Select(o => _operations[o](option, number))
                    .Where(o => o <= answer)
            );
        }

        return options.Contains(answer);
    }

    private async Task<long[][]> GetInput() =>
        await FileParser.ReadLinesAsLongArray(FilePath, [":", " "]);
}
