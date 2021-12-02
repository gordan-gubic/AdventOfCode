#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2021.Days;
public class Day01 : Day
{
    private List<int> _data;

    public Day01(ILog log, IParser parser) : base(log, parser)
    {
        EnableDebug();
        Initialize();
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Year = 2021;
        Parser.Day = 1;
        Parser.Type = ParserFileType.Real;

        _data = Parser.Parse(Convert);
    }

    /// <inheritdoc />
    public override void DumpInput()
    {
        //_data.DumpCollection("List");
    }

    protected override void ComputePart1()
    {
        var result = 0;
        var previous = 0;

        foreach (var item in _data)
        {
            if(previous == 0)
            {
                previous = item;
                continue;
            }

            if(item > previous)
            {
                result++;
            }

            previous = item;
        }

        Result = result;
    }

    protected override void ComputePart2()
    {
        var result = 0;
        var previous = Three(0);
        var current = previous;

        for (int i = 1; i < _data.Count - 2; i++)
        {
            current = Three(i);
            Log.Debug(current.ToString());
            if(current > previous)
            {
                result++;
            }
            previous = current;
        }

            Log.Debug(result.ToString());
        Result = result;
    }

    private int Three(int v)
    {
        return _data[v] + _data[v + 1] + _data[v + 2];
    }

    private int Convert(string input)
    {
        return input.ToInt();
    }

}

#if DUMP
#endif