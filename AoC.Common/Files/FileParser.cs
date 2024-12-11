using System.Dynamic;
using System.Numerics;
using System.Text.RegularExpressions;

namespace AoC.Common.Files;

public static class FileParser
{
    public static async Task<int[][]> ReadBlocksAsIntArray(string filePath) =>
        (await File.ReadAllTextAsync(filePath))
            .SplitOnTwoNewLines()
            .Select(l => l.ToIntArray("\n"))
            .ToArray();

    public static async Task<string[][]> ReadBlocksAsStringArray(string filePath) =>
        (await File.ReadAllTextAsync(filePath))
            .SplitOnTwoNewLines()
            .Select(s => s.SplitOnNewLine())
            .ToArray();

    public static async Task<int[]> ReadLinesAsInt(string FilePath) =>
        (await File.ReadAllLinesAsync(FilePath))
            .Where(l => l.IsNotNullOrEmpty())
            .Select(int.Parse)
            .ToArray();

    public static async Task<long[]> ReadLinesAsLong(string FilePath) =>
        (await File.ReadAllLinesAsync(FilePath))
            .Where(l => l.IsNotNullOrEmpty())
            .Select(long.Parse)
            .ToArray();

    public static async Task<string[]> ReadLinesAsString(string filePath) =>
        (await File.ReadAllLinesAsync(filePath))
            .Where(l => l.IsNotNullOrEmpty())
            .ToArray();

    public static async Task<int[]> ReadLineAsIntArray(string filePath, string separator) =>
        (await File.ReadAllTextAsync(filePath))
            .ToIntArray(separator);

    public static async Task<long[]> ReadLineAsLongArray(string filePath, string separator) =>
        (await File.ReadAllTextAsync(filePath))
            .ToLongArray(separator);

    public static async Task<Point3D<T>[]> ReadLinesAsPoint3D<T>(string filePath, string separator) where T : INumber<T> =>
        (await File.ReadAllLinesAsync(filePath))
            .Select(l => l.ToPoint3D<T>(separator))
            .ToArray();

    public static async Task<string> ReadLineAsString(string filePath) =>
        (await File.ReadAllTextAsync(filePath)).Trim();

    public static async Task<int[][]> ReadLinesAsIntArray(string filePath) =>
        (await File.ReadAllLinesAsync(filePath))
            .Select(l => l.ToIntArray())
            .ToArray();

    public static async Task<int[][]> ReadLinesAsIntArray(string filePath, string separator) =>
        (await File.ReadAllLinesAsync(filePath))
            .Select(l => l.ToIntArray(separator))
            .ToArray();

    public static async Task<int[][]> ReadLinesAsIntArray(string filePath, string[] separator) =>
        (await File.ReadAllLinesAsync(filePath))
            .Select(l => l.ToIntArray(separator))
            .ToArray();

    public static async Task<long[][]> ReadLinesAsLongArray(string filePath, string[] separator) =>
        (await File.ReadAllLinesAsync(filePath))
            .Select(l => l.ToLongArray(separator))
            .ToArray();

    public static async Task<string[][]> ReadLinesAsStringArray(string filePath, string separator) =>
        (await File.ReadAllLinesAsync(filePath))
            .Where(l => l.IsNotNullOrEmpty())
            .Select(l => l.Split(separator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            .ToArray();

    public static async Task<char[][]> ReadLinesAsCharArray(string filePath) =>
        (await File.ReadAllLinesAsync(filePath))
            .Where(l => l.IsNotNullOrEmpty())
            .Select(l => l.ToCharArray())
            .ToArray();

    public static async Task<char[][]> ReadLinesAsCharArray(string filePath, string separator) =>
        (await File.ReadAllLinesAsync(filePath))
            .Where(l => l.IsNotNullOrEmpty())
            .Select(l => l
                .Split(separator, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s[0])
                .ToArray())
            .ToArray();

    public static async Task<bool[][]> ReadLinesAsBoolArray(string filePath, char trueValue) =>
        (await File.ReadAllLinesAsync(filePath))
            .Where(l => l.IsNotNullOrEmpty())
            .Select(l => l.ToBoolArray(trueValue))
            .ToArray();

    public static async Task<dynamic[]> ReadLineUsingRegex(string filePath, Regex regex) =>
        string.Join("", await File.ReadAllLinesAsync(filePath)).FromRegex(regex);

    public static async Task<dynamic[][]> ReadLinesUsingRegex(string filePath, Regex regex) =>
        (await File.ReadAllLinesAsync(filePath))
            .Select(l => l.FromRegex(regex))
            .ToArray();

    public static async Task<T[]> ReadLineUsingRegex<T>(string filePath, Regex regex, Func<dynamic, T> parser) =>
        string.Join("", await File.ReadAllLinesAsync(filePath))
            .FromRegex(regex)
            .Select(parser)
            .ToArray();

    public static async Task<T[][]> ReadLinesUsingRegex<T>(string filePath, Regex regex, Func<dynamic, T> parser) =>
        (await File.ReadAllLinesAsync(filePath))
            .Select(l => l
                .FromRegex(regex)
                .Select(parser)
                .ToArray())
            .ToArray();
}
