namespace Gguc.Aoc.Core.Models;


public class Space4d<T>
{
    public Space4d(int d0, int d1, int d2, int d3)
    {
        Length0 = d0;
        Length1 = d1;
        Length2 = d2;
        Length3 = d3;

        Values = new T[Length0, Length1, Length2, Length3];

        for (var q = 0; q < Length3; q++)
        {
            for (var z = 0; z < Length2; z++)
            {
                for (var y = 0; y < Length1; y++)
                {
                    for (var x = 0; x < Length0; x++)
                    {
                        Values[x, y, z, q] = default;
                    }
                }
            }
        }
    }

    public T[,,,] Values { get; set; }

    public int Length0 { get; }
    public int Length1 { get; }
    public int Length2 { get; }
    public int Length3 { get; }

    public Space4d<T> Copy()
    {
        var space = new Space4d<T>(Length0, Length1, Length2, Length3);

        for (var q = 0; q < Length3; q++)
        {
            for (var z = 0; z < Length2; z++)
            {
                for (var y = 0; y < Length1; y++)
                {
                    for (var x = 0; x < Length0; x++)
                    {
                        space.Values[x, y, z, q] = Values[x, y, z, q];
                    }
                }
            }
        }

        return space;
    }

    public T[,,,] CopyValue()
    {
        var space = new T[Length0, Length1, Length2, Length3];

        for (var q = 0; q < Length3; q++)
        {
            for (var z = 0; z < Length2; z++)
            {
                for (var y = 0; y < Length1; y++)
                {
                    for (var x = 0; x < Length0; x++)
                    {
                        space[x, y, z, q] = Values[x, y, z, q];
                    }
                }
            }
        }

        return space;
    }

    public T GetValue(in int x, in int y, in int z, in int q)
    {
        if (x < 0 || y < 0 || z < 0 || q < 0 || x >= Length0 || y >= Length1 || z >= Length2 || q >= Length3) return default;

        return Values[x, y, z, q];
    }

    public int CountValues(T value = default)
    {
        var count = 0;

        for (var q = 0; q < Length3; q++)
        {
            for (var z = 0; z < Length2; z++)
            {
                for (var y = 0; y < Length1; y++)
                {
                    for (var x = 0; x < Length0; x++)
                    {
                        if (GetValue(x, y, z, q).Equals(value)) count++;
                    }
                }
            }
        }

        return count;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        var sb = new StringBuilder();

        for (var q = 0; q < Length3; q++)
        {
            sb.AppendLine($"q={q}");

            for (var z = 0; z < Length2; z++)
            {
                sb.AppendLine($"z={z}");

                for (var y = 0; y < Length1; y++)
                {
                    for (var x = 0; x < Length0; x++)
                    {
                        sb.Append($"{Values[x, y, z, q]}, ");
                    }

                    sb.AppendLine();
                }

                sb.AppendLine();
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }
}
