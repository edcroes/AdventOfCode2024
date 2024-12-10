namespace AoC2024.Day09;

public class Day09 : IMDay
{
    public string FilePath { private get; init; } = "Day09\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var input = await GetInput();
        List<int> drive = [];

        var id = 0;

        for (var i = 0; i < input.Length; i++)
        {
            var toWrite = i.IsEven() ? id++ : -1;
            for (var size = 0; size < input[i]; size++)
            {
                drive.Add(toWrite);
            }
        }

        CompactDrive(drive);

        return ComputeChecksum(drive).ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var input = await GetInput();
        List<Block> drive = [];

        var id = 0;
        for (var i = 0; i < input.Length; i++)
        {
            drive.Add(new(i.IsEven() ? id++ : -1, input[i]));
        }

        CompactDrive(drive);

        return ComputeChecksum(drive).ToString();
    }

    private static void CompactDrive(List<int> drive)
    {
        int left = 0, right = drive.Count - 1;

        while (left < right)
        {
            if (drive[left] == -1)
            {
                while (drive[right] == -1)
                    right--;

                if (right <= left)
                    break;

                drive[left] = drive[right];
                drive[right] = -1;
            }

            left++;
        }
    }

    private static void CompactDrive(List<Block> drive)
    {
        HashSet<int> movedBlocks = [-1];

        for (var i = drive.Count - 1; i >= 0; i--)
        {
            var block = drive[i];
            if (movedBlocks.Contains(block.Id))
                continue;

            var emptyBlock = drive.FirstOrDefault(b => b.Id == -1 && b.Length >= block.Length);

            if (emptyBlock != default)
            {
                var emptyIndex = drive.IndexOf(emptyBlock);
                if (emptyIndex < i)
                {
                    drive.RemoveAt(i);
                    drive.RemoveAt(emptyIndex);
                    drive.Insert(emptyIndex, block);
                    drive.Insert(i, new(-1, block.Length));

                    if (block.Length < emptyBlock.Length)
                    {
                        drive.Insert(emptyIndex + 1, new(-1, emptyBlock.Length - block.Length));
                        i++;
                    }
                }
            }

            movedBlocks.Add(block.Id);
        }
    }

    private static long ComputeChecksum(List<int> drive) =>
        Enumerable.Range(0, drive.Count)
            .Sum(b => drive[b] == -1 ? 0L : drive[b] * b);

    private static long ComputeChecksum(List<Block> drive)
    {
        List<int> realDrive = [];

        foreach (var block in drive)
        {
            for (var i = 0; i < block.Length; i++)
            {
                realDrive.Add(block.Id);
            }
        }

        return ComputeChecksum(realDrive);
    }

    private static void DumpDrive(IEnumerable<int> drive)
    {
        foreach (var item in drive)
        {
            Console.Write(item == -1 ? "." : item);
        }

        Console.WriteLine();
    }

    private static void DumpDrive(IEnumerable<Block> drive)
    {
        foreach (var item in drive)
        {
            for (var i = 0; i < item.Length; i++)
            {
                Console.Write(item.Id == -1 ? "." : item.Id);
            }
        }

        Console.WriteLine();
    }

    private async Task<int[]> GetInput() =>
        (await FileParser.ReadLinesAsIntArray(FilePath))[0];

    private record struct Block(int Id, int Length);
}
