namespace Gguc.Aoc.Core.Models;

public class Space3d<T>
{
    public Space3d(int d0, int d1, int d2)
    {
        Length0 = d0;
        Length1 = d1;
        Length2 = d2;

        Values = new T[Length0, Length1, Length2];

        for (var z = 0; z < Length2; z++)
        {
            for (var y = 0; y < Length1; y++)
            {
                for (var x = 0; x < Length0; x++)
                {
                    Values[x, y, z] = default;
                }
            }
        }
    }

    public T[,,] Values { get; set; }

    public int Length0 { get; }
    public int Length1 { get; }
    public int Length2 { get; }

    public Space3d<T> Copy()
    {
        var space = new Space3d<T>(Length0, Length1, Length2);

        for (var z = 0; z < Length2; z++)
        {
            for (var y = 0; y < Length1; y++)
            {
                for (var x = 0; x < Length0; x++)
                {
                    space.Values[x, y, z] = Values[x, y, z];
                }
            }
        }

        return space;
    }

    public T[,,] CopyValue()
    {
        var space = new T[Length0, Length1, Length2];

        for (var z = 0; z < Length2; z++)
        {
            for (var y = 0; y < Length1; y++)
            {
                for (var x = 0; x < Length0; x++)
                {
                    space[x, y, z] = Values[x, y, z];
                }
            }
        }

        return space;
    }

    public T GetValue(in int x, in int y, in int z)
    {
        if (x < 0 || y < 0 || z < 0 || x >= Length0 || y >= Length1 || z >= Length2) return default;

        return Values[x, y, z];
    }

    public int CountValues(T value = default)
    {
        var count = 0;

        for (var z = 0; z < Length2; z++)
        {
            for (var y = 0; y < Length1; y++)
            {
                for (var x = 0; x < Length0; x++)
                {
                    if (GetValue(x, y, z).Equals(value)) count++;
                }
            }
        }

        return count;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        var sb = new StringBuilder();

        for (var z = 0; z < Length2; z++)
        {
            sb.AppendLine($"z={z}");

            for (var y = 0; y < Length1; y++)
            {
                for (var x = 0; x < Length0; x++)
                {
                    sb.Append($"{Values[x, y, z]}, ");
                }

                sb.AppendLine();
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }
}
