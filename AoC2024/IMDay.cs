namespace AoC2024;

public interface IMDay
{
    string FilePath { init; }
    Task<string> GetAnswerPart1();
    Task<string> GetAnswerPart2();
}
