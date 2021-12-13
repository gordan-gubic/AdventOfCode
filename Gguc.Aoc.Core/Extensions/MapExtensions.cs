namespace Gguc.Aoc.Core.Extensions;

public static class MapExtensions
{
    public static List<string> TransposeListString(this List<string> data)
    {
        var list = new List<string>();

        if (data == null || data.Count == 0 || data[0].Length == 0) return list;

        var rows = data.Count;
        var cols = data[0].Length;
        var sb = new StringBuilder();

        for (int i = 0; i < cols; i++)
        {
            sb.Clear();

            for (int j = 0; j < rows; j++)
            {
                sb.Append(data[j][i]);
            }

            list.Add(sb.ToString());
        }

        return list;
    }

    public static Map<bool> Union(this Map<bool> map1, Map<bool> map2)
    {
        var map = new Map<bool>(map1.Width, map1.Height);

        map.ForEach((x, y) => map[x, y] = map1[x, y] || map2[x, y]);

        return map;
    }

    public static string MapBoolToString(this Map<bool> map, char truech = '#', char falsech = ' ')
    {
        var sb = new StringBuilder();

        for (var y = 0; y < map.Height; y++)
        {
            for (var x = 0; x < map.Width; x++)
            {
                var value = map[x, y] ? truech : falsech;
                sb.Append(value);
            }
            sb.AppendLine();
        }

        return sb.ToString();
    }
}
