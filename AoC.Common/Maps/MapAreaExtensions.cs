namespace AoC.Common.Maps;

public static class MapAreaExtensions
{
    public static Area<T> GetArea<T>(this Map<T> map, Point pointInArea) where T : notnull
    {
        Area<T> area = new() { Value = map.GetValue(pointInArea) };
        area.Add(pointInArea);

        map.ExpandArea(pointInArea, map.GetValue(pointInArea), area);
        
        return area;
    }

    private static void ExpandArea<T>(this Map<T> map, Point point, T value, Area<T> area) where T : notnull
    {
        var neighbors = map.GetStraightNeighbors(point).Where(n => !area.Contains(n) && map.GetValue(n).Equals(value));

        foreach (var neighbor in neighbors)
        {
            area.Add(neighbor);
            map.ExpandArea(neighbor, value, area);
        }
    }
}
