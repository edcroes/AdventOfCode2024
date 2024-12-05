namespace AoC2024.Day04;

public class Day04 : IMDay
{
    public string FilePath { private get; init; } = "Day04\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var input = await GetInput();

        return input
            .FindAll("XMAS".ToCharArray())
            .Count()
            .ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var input = await GetInput();

        return input.Count((p, _) => IsXmas(input, p)).ToString();
    }

    private static bool IsXmas(Map<char> map, Point middle)
    {
        if (map.GetValue(middle) is not 'A')
            return false;

        var leftTop     = map.GetValueOrDefault(middle.Add(Direction.NorthWest.ToPoint()));
        var rightTop    = map.GetValueOrDefault(middle.Add(Direction.NorthEast.ToPoint()));
        var leftBottom  = map.GetValueOrDefault(middle.Add(Direction.SouthWest.ToPoint()));
        var rightBottom = map.GetValueOrDefault(middle.Add(Direction.SouthEast.ToPoint()));

        return
            ((leftTop == 'M' && rightBottom == 'S') || (leftTop == 'S' && rightBottom == 'M')) &&
            ((rightTop == 'M' && leftBottom == 'S') || (rightTop == 'S' && leftBottom == 'M'));
    }

    private async Task<Map<char>> GetInput() =>
        new(await FileParser.ReadLinesAsCharArray(FilePath));
}
