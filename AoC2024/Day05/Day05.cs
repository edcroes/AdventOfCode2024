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

    private static bool IsInCorrectOrder(int[] pages, HashSet<Rule> rules)
    {
        return Enumerable.Range(0, pages.Length)
            .All(x => pages[(x + 1)..].All(y => !rules.Contains(new Rule(y, pages[x]))));
    }

    private async Task<(HashSet<Rule>, int[][])> GetInput()
    {
        var (first, second) = await FileParser.ReadBlocksAsStringArray(FilePath);

        HashSet<Rule> rules = new(first!.Select(Parse));
        var sets = second!.Select(l => l.ToIntArray(",")).ToArray();

        return (rules, sets);
    }

    private static Rule Parse(string rule)
    {
        var (left, right) = rule.ToIntArray("|");
        return new(left, right);
    }

    private record struct Rule(int Left, int Right);

    private class PageComparer(HashSet<Rule> rules) : IComparer<int>
    {
        public int Compare(int x, int y)
        {
            if (rules.Contains(new(x, y)))
                return -1;

            if (rules.Contains(new(y, x)))
                return 1;

            return 0;
        }
    }
}
