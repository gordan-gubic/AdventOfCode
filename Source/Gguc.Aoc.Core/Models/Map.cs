namespace Gguc.Aoc.Core.Models;

public class Map<T>
{
    public Map(int width, int height, T defaultValue = default)
    {
        Width = width;
        Height = height;
        Values = new T[Width, Height];

        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                Values[x, y] = defaultValue;
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

    public void Reset(T value = default)
    {
        ForEach((x, y) => Values[x, y] = value);
    }

    public Map<T> Sub(int x1, int y1, int x2, int y2)
    {
        var width = x2 - x1 + 1;
        var height = y2 - y1 + 1;

        var map = new Map<T>(width, height);

        for (var y = y1; y <= y2; y++)
        {
            for (var x = x1; x <= x2; x++)
            {
                map[x - x1, y - y1] = this[x, y];
            }
        }

        return map;
    }

    public Map<T> Expand(int n = 1, T value = default)
    {
        var map = new Map<T>(Width + 2 * n, Height + 2 * n);

        map.ForEach((x, y) => map[x, y] = value);

        this.ForEach((x, y) => map[x + n, y + n] = this[x, y]);

        return map;
    }

    public Map<T> Reduce(int n = 1)
    {
        return this.Sub(n, n, Height - n - 1, Width - n - 1);
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
        if (!Contains(x, y)) return default;

        return this[x, y];
    }

    public List<(int, int, T)> GetRow(int y)
    {
        var list = new List<(int, int, T)>();

        for (var x = 0; x <= Width; x++)
        {
            list.Add((x, y, GetValue(x, y)));
        }

        return list;
    }

    public List<(int, int, T)> GetColumn(int x)
    {
        var list = new List<(int, int, T)>();

        for (var y = 0; y <= Height; y++)
        {
            list.Add((x, y, GetValue(x, y)));
        }

        return list;
    }

    public bool Contains(int x, int y) => !(x < 0 || y < 0 || x >= Width || y >= Height);

    public int CountValues(T value = default)
    {
        var count = 0;

        ForEach((x, y) =>
        {
            if (GetValue(x, y).Equals(value)) count++;
        });

        return count;
    }

    public int CountValues(Func<T, bool> func)
    {
        var count = 0;

        ForEach((x, y) =>
        {
            if (func(GetValue(x, y))) count++;
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

    public (bool, int, int) Find(T value)
    {
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                if (GetValue(x, y).Equals(value)) return (true, x, y);
            }
        }

        return (false, -1, -1);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        var sb = new StringBuilder();

        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                sb.Append($"{this[x, y]}, ");
            }
            sb.AppendLine();
        }

        return sb.ToString();
    }
}
