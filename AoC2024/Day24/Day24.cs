namespace AoC2024.Day24;

public partial class Day24 : IMDay
{
    private const string XOR = "XOR";
    private const string AND = "AND";
    private const string OR  = "OR";

    public string FilePath { private get; init; } = "Day24\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var input = await GetInput();
        var gates = input
            .Select(g => new KeyValuePair<string, Gate>(g.Output, g))
            .ToDictionary();

        return gates.Keys
            .Where(g => g[0] == 'z')
            .Select(g => (gates[g], GetOutput(gates[g], gates)))
            .OrderByDescending(g => g.Item1.Output)
            .Aggregate(0L, (total, next) => total * 2 + (next.Item2 ? 1 : 0))
            .ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var gates = await GetInput();
        var inputs = gates.Count(g => g.Output[0] == 'x');

        Dictionary<string, string> errors = [];

        var firstOutput = gates.Single(g => HasInputs(g, "x00", "y00") && g.GateType == XOR);
        var carry00 = gates.Single(g => g.Left.IsIn("x00", "y00") && g.Right.IsIn("x00", "y00") && g.GateType == AND);
        var previousCarry = carry00.Output;

        if (firstOutput.Output != "z00" || carry00.Output == "z00")
        {
            errors.Add(firstOutput.Output, carry00.Output);
            errors.Add(carry00.Output, firstOutput.Output);
            previousCarry = firstOutput.Output;
        }

        for (var i = 1; i < inputs; i++)
        {
            var x = $"x{i:D2}";
            var y = $"y{i:D2}";
            var realOutput = $"z{i:D2}";

            var xorXY = gates.Single(g => HasInputs(g, x, y) && g.GateType == XOR);
            var andXY = gates.Single(g => HasInputs(g, x, y) && g.GateType == AND);
            var output = gates.Single(g => previousCarry.IsIn(g.Left, g.Right) && g.GateType == XOR);

            var realXorXYOutput = xorXY.Output.IsIn(output.Left, output.Right)
                ? xorXY.Output
                : GetOtherInput(output, previousCarry);

            if (realXorXYOutput != xorXY.Output)
            {
                errors.Add(xorXY.Output, realXorXYOutput);
            }

            if (realOutput != output.Output)
            {
                errors.Add(output.Output, realOutput);
            }

            var carryLeft = gates.Single(g => HasInputs(g, realXorXYOutput, previousCarry) && g.GateType == AND);
            var carry = gates.Single(g => (carryLeft.Output.IsIn(g.Left, g.Right) || andXY.Output.IsIn(g.Left, g.Right)) && g.GateType == OR);

            var realAndXYOutput = andXY.Output.IsIn(carry.Left, carry.Right)
                ? andXY.Output :
                GetOtherInput(carry, carryLeft.Output);
            var realCarryLeftOutput = carryLeft.Output.IsIn(carry.Left, carry.Right)
                ? carryLeft.Output
                : GetOtherInput(carry, realAndXYOutput);

            if (realAndXYOutput != andXY.Output)
            {
                errors.Add(andXY.Output, realAndXYOutput);
            }

            if (realCarryLeftOutput != carryLeft.Output)
            {
                errors.Add(carryLeft.Output, realCarryLeftOutput);
            }

            var realCarryOutput = errors.ContainsValue(carry.Output)
                ? errors.Keys.Single(e => errors[e] == carry.Output)
                : carry.Output;

            if (realCarryOutput != carry.Output)
            {
                errors.Add(carry.Output, realCarryOutput);
            }

            previousCarry = realCarryOutput;
        }

        return string.Join(',', errors.Keys.OrderBy(e => e));
    }

    private static bool GetOutput(Gate gate, Dictionary<string, Gate> gates)
    {
        if (gate.GateType == "Direct")
            return gate.Left.ParseToBool();

        var left = GetOutput(gates[gate.Left], gates);
        var right = GetOutput(gates[gate.Right!], gates);

        return gate.GateType switch
        {
            AND => left & right,
            OR => left | right,
            XOR => left ^ right,
            _ => throw new NotSupportedException()
        };
    }

    private static bool HasInputs(Gate gate, string first, string second) =>
        (gate.Left == first && gate.Right == second) || (gate.Left == second && gate.Right == first);

    private static string GetOtherInput(Gate gate, string input) =>
        gate.Left == input ? gate.Right! : gate.Left;

    private async Task<Gate[]> GetInput()
    {
        var (first, second) = await FileParser.ReadBlocksAsStringArray(FilePath);

        var firstGates = first!.Select(l =>
        {
            var (output, left) = l.Split(": ");
            return new Gate(left!, "Direct", null, output!);
        });

        var secondGates = second!.Select(Parse);

        return firstGates.Union(secondGates).ToArray();
    }

    private static Gate Parse(string line)
    {
        var regex = GateRegex();
        var match = regex.Match(line);
        return new Gate(match.GetString("Left"), match.GetString("Op"), match.GetString("Right"), match.GetString("Output"));
    }

    [GeneratedRegex("^(?<Left>[a-z0-9]+) (?<Op>AND|OR|XOR) (?<Right>[a-z0-9]+) -> (?<Output>[a-z0-9]+)$")]
    private static partial Regex GateRegex();

    private record struct Gate(string Left, string GateType, string? Right, string Output);
}
