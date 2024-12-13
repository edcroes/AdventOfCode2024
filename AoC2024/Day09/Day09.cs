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
        
        List<Block> files = [];
        List<Block> freeSpace = [];

        var id = 0;
        var index = 0;

        for (var i = 0; i < input.Length; i++)
        {
            if (input[i] > 0)
            {
                if (i.IsEven())
                    files.Add(new(id++, input[i]) { Index = index });
                else
                    freeSpace.Add(new(-1, input[i]) { Index = index });
            }

            index += input[i];
        }

        CompactDrive(files, freeSpace);

        return files.Sum(f => f.ComputeChecksum()).ToString();
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

    private static void CompactDrive(List<Block> files, List<Block> freeSpace)
    {
        files.Reverse();
        foreach (var file in files)
        {
            if (freeSpace.Count == 0 || freeSpace[0].Index > file.Index)
                break;

            var freeBlock = freeSpace.FirstOrDefault(f => f.Length >= file.Length);

            if (freeBlock is not null && freeBlock.Index < file.Index)
            {
                file.Index = freeBlock.Index;

                var indexOfFreeBlock = freeSpace.IndexOf(freeBlock);
                freeSpace.Remove(freeBlock);

                if (freeBlock.Length > file.Length)
                {
                    freeSpace.Insert(indexOfFreeBlock, new(-1, freeBlock.Length - file.Length) { Index = freeBlock.Index + file.Length });
                }
            }
        }
    }

    private static long ComputeChecksum(List<int> drive) =>
        Enumerable.Range(0, drive.Count)
            .Sum(b => drive[b] == -1 ? 0L : drive[b] * b);

    private async Task<int[]> GetInput() =>
        (await FileParser.ReadLinesAsIntArray(FilePath))[0];

    private record Block(int Id, int Length)
    {
        public int Index { get; set; } = -1;

        public long ComputeChecksum() =>
            (long)((double)(Index * 2 + Length - 1) / 2 * Id * Length);
    }
}
