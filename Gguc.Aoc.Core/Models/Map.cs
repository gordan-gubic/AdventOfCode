namespace Gguc.Aoc.Core.Models;

public class Map<T>
{
    public Map(int width, int height)
    {
        Width = width;
        Height = height;
        Values = new T[Width, Height];

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Values[x, y] = default;
            }
        }
    }

    public Map(List<string> lines, Func<char, T> convert)
    {
        Width = lines[0].Length;
        Height = lines.Count;
        Values = new T[Width, Height];

        for (var y = 0; y < lines.Count; y++)
        {
            for (var x = 0; x < lines[y].Length; x++)
            {
                Values[x, y] = convert(lines[y][x]);
            }
        }
    }

    public T[,] Values { get; set; }

    public int Width { get; }

    public int Height { get; }

    public Map<T> Clone()
    {
        var map = new Map<T>(Width, Height);

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                map.Values[x, y] = Values[x, y];
            }
        }

        return map;
    }

    public Map<T> Rotate()
    {
        var map = new Map<T>(Height, Width);

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                map.Values[Height - y - 1, x] = Values[x, y];
            }
        }

        return map;
    }

    public Map<T> FlipHorizontally()
    {
        var map = new Map<T>(Width, Height);

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                map.Values[Width - x - 1, y] = Values[x, y];
            }
        }

        return map;
    }

    public Map<T> FlipVertically()
    {
        var map = new Map<T>(Width, Height);

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                map.Values[x, Height - y - 1] = Values[x, y];
            }
        }

        return map;
    }

    public T GetValue(in int x, in int y)
    {
        if (x < 0 || y < 0 || x >= Width || y >= Height) return default;

        return Values[x, y];
    }

    public int CountValues(T value = default)
    {
        var count = 0;

        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                if (GetValue(x, y).Equals(value)) count++;
            }
        }

        return count;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        var sb = new StringBuilder();

        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                sb.Append($"{Values[x, y]}, ");
            }
            sb.AppendLine();
        }

        return sb.ToString();
    }
}
