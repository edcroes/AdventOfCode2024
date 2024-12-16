namespace AoC2024.Day14;

public partial class Day14 : IMDay
{
    private static readonly Regex _robotRegex = RobotRegex();

    public string FilePath { private get; init; } = "Day14\\input.txt";

    private bool IsTestInput => FilePath.Contains("test");

    public async Task<string> GetAnswerPart1()
    {
        var robots = await GetInput();
        var sizeX = IsTestInput ? 11 : 101;
        var sizeY = IsTestInput ? 7 : 103;

        robots = robots.Select(r => MoveRobot(r, 100, sizeX, sizeY)).ToArray();

        var leftUpper = robots.Count(r => r.Location.X < sizeX / 2 && r.Location.Y < sizeY / 2);
        var rightUpper = robots.Count(r => r.Location.X > sizeX / 2 && r.Location.Y < sizeY / 2);
        var leftLower = robots.Count(r => r.Location.X < sizeX / 2 && r.Location.Y > sizeY / 2);
        var rightLower = robots.Count(r => r.Location.X > sizeX / 2 && r.Location.Y > sizeY / 2);

        return (leftUpper * rightUpper * leftLower * rightLower).ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var robots = await GetInput();
        var sizeX = IsTestInput ? 11 : 101;
        var sizeY = IsTestInput ? 7 : 103;

        HashSet<Point> currentLocations = [];
        int times = 0;

        while (currentLocations.Count != robots.Length)
        {
            robots = robots.Select(r => MoveRobot(r, 1, sizeX, sizeY)).ToArray();
            currentLocations = robots.Select(r => r.Location).ToHashSet();
            times++;
        }

        DumpRobots(robots, sizeX, sizeY);

        return times.ToString();
    }

    private static Robot MoveRobot(Robot robot, int times, int fieldSizeX, int fieldSizeY)
    {
        var x = (robot.Location.X + robot.Velocity.X * times) % fieldSizeX;
        x = x < 0 ? x + fieldSizeX : x;

        var y = (robot.Location.Y + robot.Velocity.Y * times) % fieldSizeY;
        y = y < 0 ? y + fieldSizeY : y;

        return new(new(x, y), robot.Velocity);
    }

    private static void DumpRobots(Robot[] robots, int sizeX, int sizeY)
    {
        for (var y = 0; y < sizeY; y++)
        {
            for (var x = 0; x < sizeX; x++)
            {
                var c = robots.Count(r => r.Location.X == x && r.Location.Y == y);
                Console.Write(c == 0 ? "." : c.ToString());
            }
            Console.WriteLine();
        }

        Console.WriteLine();
    }

    private async Task<Robot[]> GetInput() =>
        (await FileParser.ReadLinesAsString(FilePath))
            .Select(Parse)
            .ToArray();

    private static Robot Parse(string line)
    {
        var match = _robotRegex.Match(line);
        return new(new(match.GetInt("PX"), match.GetInt("PY")), new(match.GetInt("VX"), match.GetInt("VY")));
    }

    [GeneratedRegex(@"^p=(?<PX>-?\d+),(?<PY>-?\d+) v=(?<VX>-?\d+),(?<VY>-?\d+)$")]
    private static partial Regex RobotRegex();

    private record struct Robot(Point Location, Point Velocity);
}
