#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2019.Days;

public class Day01 : Day
{
    private const int YEAR = 2019;
    private const int DAY = 1;

    private List<int> _data;

    public Day01(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Test;

        _data = Parser.Parse(Convert);
    }

    /// <inheritdoc />
    public override void DumpInput()
    {
        _data.Dump("List");
    }

    protected override void ComputePart1()
    {
        foreach (var record in _data)
        {
            Add(CalculateMass(record), true);
        }
    }

    protected override void ComputePart2()
    {
        foreach (var record in _data)
        {
            var mass = CalculateMass(record);

            do
            {
                Add(mass, true);

                mass = CalculateMass(mass);
            } while (mass > 0);
        }
    }

    private int Convert(string input)
    {
        return input.ToInt();
    }

    private long CalculateMass(long input)
    {
        // For a mass of 12, divide by 3 and round down to get 4, then subtract 2 to get 2.
        var result = (input / 3) - 2;

        // Log.DebugLog(ClassId, $"Input=[{input}]. Result=[{result}].");

        return result;
    }
}

#if DUMP
#endif