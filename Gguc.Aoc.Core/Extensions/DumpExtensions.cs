namespace Gguc.Aoc.Core.Extensions;

public static class DumpExtensions
{
    [Conditional("LOG")]
    public static void DumpCaller(this object input, [CallerMemberName] string method = null, string title = null, bool newLine = false)
    {
        DumpTitle(title);

        var nl = newLine ? Environment.NewLine : "";

        Trace.WriteLine($"{nl}{method}");
    }

    [Conditional("LOG")]
    public static void Dump(this object input, string title = null, bool newLine = false)
    {
        DumpTitle(title);

        var nl = newLine ? Environment.NewLine : "";

        Trace.WriteLine($"{nl}{input}");
    }

    [Conditional("LOG")]
    public static void DumpJson(this object input, string title = null, bool newLine = false)
    {
        DumpTitle(title);

        var nl = newLine ? Environment.NewLine : "";

        Trace.WriteLine($"{nl}{input.ToJson()}");
    }

    [Conditional("LOG")]
    public static void DumpJsonIndented(this object input, string title = null, bool newLine = false)
    {
        DumpTitle(title);

        var nl = newLine ? Environment.NewLine : "";

        Trace.WriteLine($"{nl}{input.ToJsonIndented()}");
    }

    [Conditional("LOG")]
    public static void DumpCollection(this IEnumerable input, string title = null)
    {
        DumpTitle(title);

        foreach (var value in input)
        {
            Trace.WriteLine(value);
        }
    }

    [Conditional("LOG")]
    public static void DumpMap(this bool[,] input, string title = null)
    {
        DumpTitle(title);

        // Trace.WriteLine(input.ToJson());

        var width = input.GetLength(0);
        var height = input.GetLength(1);

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                Console.Write($"{input[x, y].ToInt()}, ");
            }
            Console.WriteLine("");
        }
    }

    [Conditional("LOG")]
    public static void DumpMap(this int[,] input, string title = null, int format = 0, int offset = 0)
    {
        DumpTitle(title);

        // Trace.WriteLine(input.ToJson());
        var formatString = $"{{0,{format}}}, ";

        var width = input.GetLength(0);
        var height = input.GetLength(1);

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                Console.Write(formatString, input[x, y] + offset);
            }
            Console.WriteLine("");
        }
    }

    [Conditional("LOG")]
    public static void DumpSpace(this bool[,,] space, string title = null)
    {
        DumpTitle(title);

        var l0 = space.GetLength(0);
        var l1 = space.GetLength(1);
        var l2 = space.GetLength(2);

        for (int z = 0; z < l2; z++)
        {
            for (int y = 0; y < l1; y++)
            {
                for (int x = 0; x < l0; x++)
                {
                    Console.Write($"{space[x, y, z].ToInt()}, ");
                }
                Console.WriteLine("");
            }
            Console.WriteLine("");
            Console.WriteLine("");
        }
    }

    [Conditional("LOG")]
    public static void DumpSpace(this bool[,,,] space, string title = null)
    {
        DumpTitle(title);

        var l0 = space.GetLength(0);
        var l1 = space.GetLength(1);
        var l2 = space.GetLength(2);
        var l3 = space.GetLength(3);

        for (int q = 0; q < l3; q++)
        {
            for (int z = 0; z < l2; z++)
            {
                for (int y = 0; y < l1; y++)
                {
                    for (int x = 0; x < l0; x++)
                    {
                        Console.Write($"{space[x, y, z, q].ToInt()}, ");
                    }

                    Console.WriteLine("");
                }

                Console.WriteLine("");
                Console.WriteLine("");
            }
            Console.WriteLine("...");
            Console.WriteLine("");
        }
    }

    private static void DumpTitle(string title = null)
    {
        if (title.IsNotWhitespace())
        {
            Trace.WriteLine("".PadLeft(80, '-'));
            Trace.WriteLine($"--  {title}");
        }
    }
}
