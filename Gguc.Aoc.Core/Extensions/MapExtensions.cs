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
}
