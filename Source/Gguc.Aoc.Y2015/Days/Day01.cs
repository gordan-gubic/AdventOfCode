#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2015.Days;

public class Day01 : Day
{
    private const int YEAR = 2015;
    private const int DAY = 1;

    private List<string> _raw;
    private List<char[]> _data;

    public Day01(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "138";
        Expected2 = "1771";
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Test;
        Parser.Type = ParserFileType.Real;

        _raw = Parser.Parse();
    }

    /// <inheritdoc />
    public override void DumpInput()
    {
        DumpData();
    }

    protected override void ComputePart1()
    {
        var result = CalculateFloors();

        Result = result;
    }

    protected override void ComputePart2()
    {
        var result = FindBasement();

        Result = result;
    }

    private long CalculateFloors()
    {
        var result = 0L;

        foreach (var floor in _data)
        {
            var value = CalculateFloor(floor);
            result += value;
        }

        return result;
    }

    private long CalculateFloor(char[] floor)
    {
        var result = 0L;

        foreach (var ch in floor)
        {
            result += ch == '(' ? 1 : -1;
        }

        return result;
    }

    private long FindBasement()
    {
        var result = 0L;

        foreach (var floor in _data)
        {
            var value = FindBasement(floor);
            result += value;
        }

        return result;
    }

    private long FindBasement(char[] floor)
    {
        var result = 0L;
        var index = 0L;

        foreach (var ch in floor)
        {
            result += ch == '(' ? 1 : -1;
            index++;

            if (result == -1) break;
        }

        return index;
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        _data = new();

        // Gromit do something!
        foreach (var line in _raw)
        {
            _data.Add(line.ToCharArray());
        }
    }

    private int Convert(string input)
    {
        return input.ToInt();
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (Parser.Type == ParserFileType.Real) Log.EnableDebug = false;

        if (!Log.EnableDebug) return;

        Debug();

        _raw.DumpCollection();
        _data.DumpJson();
    }
}

#if DUMP
#endif