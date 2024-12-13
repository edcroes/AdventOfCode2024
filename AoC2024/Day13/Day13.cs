namespace AoC2024.Day13;

public partial class Day13 : IMDay
{
    private static readonly Regex _buttonRegex = ButtonRegex();
    private static readonly Regex _prizeRegex = PrizeRegex();

    public string FilePath { private get; init; } = "Day13\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var clawMachines = await GetInput();

        return clawMachines
            .Select(GetButtonPresses)
            .Sum(p => p.aPresses * 3 + p.bPresses)
            .ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        const long addition = 10000000000000;
        var clawMachines = await GetInput();

        return clawMachines
            .Select(c => new ClawMachine(c.A, c.B, new(c.Prize.X + addition, c.Prize.Y + addition)))
            .Select(GetButtonPresses)
            .Sum(p => p.aPresses * 3 + p.bPresses)
            .ToString();
    }

    private static (long aPresses, long bPresses) GetButtonPresses(ClawMachine machine)
    {
        /*
        Button A: X+94, Y+34
        Button B: X+22, Y+67
        Prize: X=8400, Y=5400
        
        2 linear formulas:
        94a + 22b = 8400
        34a + 67b = 5400

        94a + 22b = 8400
        ----------------
        94a = 8400 - 22b
        a = (8400 - 22b) / 94     ----+
        If we know b, we can solve a. |  We now have formula for a which we can use in the next formula.
                                      |
        34a + 67b = 5400              |
        ----------------              |
        67b = 5400 - 34a              v
        67b = 5400 - 34 * ((8400 - 22b) / 94)
        67b = 5400 - ((285600 - 748b) / 94)    --> 34 * 8400 and 34 * 22
        
        5400 - 67b = (285600 - 748b) / 94
        94 * (5400 - 67b) = 285600 - 748b
        507600 - 6298b = 285600 - 748b
        507600 - 6298b + 748b = 285600
        507600 - 5550b = 285600
        5550b = 507600 - 285600
        b = 222000 / 5550
        
        > b = 40
        
        Now that we know b, solve the first formula
        a = (8400 - 22 * 40) / 94
        a = 7520 / 94
        
        > a = 80

        If a or b is not a whole number then the prize cannot be claimed.


        Bring this back to formulas with variables:

        a => p1
        b => p2

        94a + 22b = 8400 => ax * p1 + bx * p2 = px
        34a + 67b = 5400 => ay * p1 + by * p2 = py
        
        First formula:
        ax * p1 + bx * p2 = px
        ax * p1 = px - bx * p2
        p1 = (px - bx * p2) / ax   --+
        ----                         |
        Second formula:              |
        ay * p1 + by * p2 = py       |
        by * p2 = py - ay * p1       v
        by * p2 = py - ay * (px - bx * p2) / ax
        by * p2 = py - (ay * px - ay * bx * p2) / ax
        py - by * p2 = (ay * px - ay * bx * p2) / ax
        ax * (py - by * p2) = ay * px - ay * bx * p2
        ax * py - ax * by * p2 = ay * px - ay * bx * p2
        ax * py - ax * by * p2 + ay * bx * p2 = ay * px
        ax * py - (ax * by * p2 - ay * bx * p2) = ay * px
        (ax * by - ay * bx) * p2 = ax * py - ay * px
        p2 = (ax * py - ay * px) / (ax * by - ay * bx)
        */

        var bPresses = (machine.A.X * machine.Prize.Y - machine.A.Y * machine.Prize.X) / (machine.A.X * machine.B.Y - machine.A.Y * machine.B.X);
        var aPresses = (machine.Prize.X - machine.B.X * bPresses) / machine.A.X;

        // Check for rounding issues since the number of presses can be 8.1 for example which won't lead to a price
        if (machine.A.X * aPresses + machine.B.X * bPresses != machine.Prize.X ||
            machine.A.Y * aPresses + machine.B.Y * bPresses != machine.Prize.Y)
        {
            return (0, 0);
        }

        return (aPresses, bPresses);
    }

    private async Task<ClawMachine[]> GetInput() =>
        (await FileParser.ReadBlocksAsStringArray(FilePath)).Select(Parse).ToArray();

    private static ClawMachine Parse(string[] block)
    {
        var buttonAMatch = _buttonRegex.Match(block[0]);
        var buttonBMatch = _buttonRegex.Match(block[1]);
        var prizeMatch = _prizeRegex.Match(block[2]);

        return new(
            new(buttonAMatch.GetInt("X"), buttonAMatch.GetInt("Y")),
            new(buttonBMatch.GetInt("X"), buttonBMatch.GetInt("Y")),
            new(prizeMatch.GetInt("X"), prizeMatch.GetInt("Y")));
    }

    private record struct ClawMachine(Point<long> A, Point<long> B, Point<long> Prize);

    [GeneratedRegex(@"^Button (?:A|B): X\+(?<X>\d+), Y\+(?<Y>\d+)$")]
    private static partial Regex ButtonRegex();

    [GeneratedRegex(@"^Prize: X=(?<X>\d+), Y=(?<Y>\d+)$")]
    private static partial Regex PrizeRegex();
}
