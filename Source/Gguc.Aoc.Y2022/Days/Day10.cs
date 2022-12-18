#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2022.Days;

public class Day10 : Day
{
    private const int YEAR = 2022;
    private const int DAY = 10;

    private List<string> _data;
    private List<(string, int)> _instructions;
    private List<int> _signals;
    private int _current;
    private Map<bool> _map;

    public Day10(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "14780";
        Expected2 = "ELPLZGZL";
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
        _signals = new();

        ProcessInstructions();

        Result = CalculateSignals(220);
    }

    protected override void ComputePart2()
    {
        _signals = new();
        _map = new(40, 6);

        ProcessInstructions();
        ProcessMap();

        Result = 0L;
    }

    private void ProcessInstructions()
    {
        _current = 1;
        _signals.Add(_current);

        foreach (var (ins, value) in _instructions)
        {
            switch (ins)
            {
                case "noop":
                    DoNoop();
                    break;

                case "addx":
                    DoAddx(value);
                    break;
            }
        }

        // _signals.DumpCollection();
    }

    private void ProcessMap()
    {
        _map.ForEach(ProcessPoint);

        Log.Info($"Map:\n{_map.MapBoolToString()}");
    }

    private void ProcessPoint(int x, int y)
    {
        var mapFlat = x + y * 40;
        var signal = _signals[mapFlat] + y * 40;

        if (mapFlat >= signal - 1 && mapFlat <= signal + 1) _map[x, y] = true;
    }

    private long CalculateSignals(int limit)
    {
        var total = 0L;

        for (var i = 20; i <= limit; i += 40)
        {
            var s = _signals[i - 1] * i;
            total += s;
        }

        return total;
    }

    private void DoNoop()
    {
        _signals.Add(_current);
    }

    private void DoAddx(int value)
    {
        _signals.Add(_current);
        _current += value;
        _signals.Add(_current);
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        _instructions = new ();
        foreach (var line in _data)
        {
            var parts = line.Split(' ');
            _instructions.Add((parts.First(), parts.Last().ToInt()));
        }
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (!Log.EnableDebug) return;

        Debug();

        // _data.DumpCollection();
    }
}

#if DUMP
#endif