namespace Gguc.Aoc.Core.Models;

public class Map<T>
{
    public Map(int size) : this(size, size)
    {
    }

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

        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                Values[x, y] = convert(lines[y][x]);
            }
        }
    }

    public T this[int x, int y]
    {
        get { return Values[x, y]; }
        set { Values[x, y] = value; }
    }

    public T[,] Values { get; set; }

    public int Width { get; }

    public int Height { get; }

    public Map<T> Clone()
    {
        var map = new Map<T>(Width, Height);

        ForEach((x, y) => map[x, y] = this[x, y]);

        return map;
    }

    public Map<T> Rotate()
    {
        var map = new Map<T>(Height, Width);

        ForEach((x, y) => map[Height - y - 1, x] = this[x, y]);

        return map;
    }

    public Map<T> FlipHorizontally()
    {
        var map = new Map<T>(Width, Height);

        ForEach((x, y) => map[Width - x - 1, y] = this[x, y]);

        return map;
    }

    public Map<T> FlipVertically()
    {
        var map = new Map<T>(Width, Height);

        ForEach((x, y) => map[x, Height - y - 1] = this[x, y]);

        return map;
    }

    public T GetValue(in int x, in int y)
    {
        if (x < 0 || y < 0 || x >= Width || y >= Height) return default;

        return this[x, y];
    }

    public int CountValues(T value = default)
    {
        var count = 0;

        ForEach((x, y) =>
        {
            if (GetValue(x, y).Equals(value)) count++;
        });

        return count;
    }

    public void ForEach(Action<int, int> action)
    {
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                action(x, y);
            }
        }
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
