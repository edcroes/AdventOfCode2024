namespace AoC.Common.Extensions;

public static class ListExtensions
{
    public static void RemoveRange<T>(this List<T> list, IEnumerable<T> itemsToRemove)
    {
        foreach (var item in itemsToRemove)
        {
            list.Remove(item);
        }
    }

    public static void AddIfNotContains<T>(this List<T> list, T item)
    {
        if (!list.Contains(item))
        {
            list.Add(item);
        }
    }

    public static List<T> WithRemovedIndex<T>(this List<T> list, int index)
    {
        List<T> newList = new(list);

        newList.RemoveAt(index);

        return newList;
    }
}