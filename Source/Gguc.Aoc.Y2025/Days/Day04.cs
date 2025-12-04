#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2025.Days;

public class Day04 : Day
{
    private const int YEAR = 2025;
    private const int DAY = 04;

    private List<string> _raw;
    private Map<bool> _data;

    public Day04(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "1411";
        Expected2 = "8557";
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Test;
        Parser.Type = ParserFileType.Real;

        _raw = Parser.Parse();
        _data = Parser.ParseMapBool('@');
    }

    /// <inheritdoc />
    public override void DumpInput()
    {
        DumpData();
    }

    protected override void ComputePart1()
    {
        Result = CountRolls();
    }

    protected override void ComputePart2()
    {
        Result = CountRollsAndClean();
    }

    private long CountRolls()
    {
        var result = 0L;

        _data.ForEach((x, y) =>
        {
            if (!_data.GetValue(x, y)) return;

            var count = _data.CountNeighbors(x, y, n => n);

            if (count < 4) result++;
        });

        return result;
    }

    private long CountRollsAndClean()
    {
        var result = 0L;

        var map = _data.Clone();
        var removed = 1;

        while (removed > 0)
        {
            removed = 0;

            map.ForEach((x, y) =>
            {
                if (!map.GetValue(x, y)) return;

                var count = map.CountNeighbors(x, y, n => n);

                if (count < 4)
                {
                    removed++;
                    map[x, y] = false;
                    result++;
                }
            });
        }

        return result;
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        // Gromit do something!
        foreach (var line in _raw)
        {
        }
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (Parser.Type == ParserFileType.Real) Log.EnableDebug = false;

        if (!Log.EnableDebug) return;

        Debug();

        _data.MapBoolToString().Dump("Map", true);
    }
}

#if DUMP
#endif