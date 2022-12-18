#define LOGx
#define STOPWATCH

namespace Gguc.Aoc.Y2022.Days;

public class Day06 : Day
{
    private const int YEAR = 2022;
    private const int DAY = 6;

    private List<string> _data;

    public Day06(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "1855";
        Expected2 = "3256";
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Test;
        Parser.Type = ParserFileType.Real;

        _data = Parser.Parse();
    }

    /// <inheritdoc />
    public override void DumpInput()
    {
        DumpData();
    }

    protected override void ComputePart1()
    {
        Result = FindMarker(4);
    }

    protected override void ComputePart2()
    {
        Result = FindMarker(14);
    }

    private long FindMarker(int input)
    {
        var line = _data[0];

        for (int i = 0; i < line.Length - input + 1; i++)
        {
            var chars = line.Skip(i).Take(input);
            var areDiff = AreDifferent(chars);

            if (areDiff) return i + input;
        }

        return 0L;
    }

    private bool AreDifferent(IEnumerable<char> chars)
    {
        var pod = new HashSet<char>(chars);
        return pod.Count == chars.Count();
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (!Log.EnableDebug) return;

        Debug();

        // _data.DumpCollection();

        Log.Info("Your puzzle answer was 1855.");
        Log.Info("Your puzzle answer was 3256.");
    }
}

#if DUMP
#endif