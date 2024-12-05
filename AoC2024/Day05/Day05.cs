namespace AoC2024.Day05;

public class Day05 : IMDay
{
    public string FilePath { private get; init; } = "Day05\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var (rules, printedSets) = await GetInput();

        return printedSets
            .Where(p => IsInCorrectOrder(p, rules))
            .Sum(p => p[p.Length / 2])
            .ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var (rules, printedSets) = await GetInput();
        PageComparer comparer = new(rules);

        return printedSets
            .Where(p => !IsInCorrectOrder(p, rules))
            .Sum(p => p.Order(comparer).ToArray()[p.Length / 2])
            .ToString();
    }

    private static bool IsInCorrectOrder(int[] pages, Rule[] rules)
    {
        return Enumerable.Range(0, pages.Length)
            .All(i => rules.All(r => r.Right != pages[i] || !pages[(i + 1)..].Contains(r.Left)));
    }

    private async Task<(Rule[], int[][])> GetInput()
    {
        var (first, second) = await FileParser.ReadBlocksAsStringArray(FilePath);

        var rules = first!.Select(Parse).ToArray();
        var sets = second!.Select(l => l.ToIntArray(",")).ToArray();

        return (rules, sets);
    }

    private static Rule Parse(string rule)
    {
        var (left, right) = rule.ToIntArray("|");
        return new(left, right);
    }

    private record struct Rule(int Left, int Right);

    private class PageComparer(Rule[] rules) : IComparer<int>
    {
        public int Compare(int x, int y)
        {
            if (rules.Any(r => r.Left == x && r.Right == y))
                return -1;

            if (rules.Any(r => r.Left == y && r.Right == x))
                return 1;

            return 0;
        }
    }
}
