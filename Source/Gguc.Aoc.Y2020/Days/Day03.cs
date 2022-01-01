#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2020.Days;

public class Day03 : Day
{
    private bool[,] _map;

    public Day03(ILog log, IParser parser) : base(log, parser)
    {
        EnableDebug();
        Initialize();
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Year = 2020;
        Parser.Day = 3;
        Parser.Type = ParserFileType.Test;

        _map = Parser.ParseMap();
    }

    /// <inheritdoc />
    public override void DumpInput()
    {
        Log.DebugLog(ClassId);

        _map.DumpMap("Map");
    }

    protected override void ComputePart1()
    {
        Add(CountHits(3, 1));
    }

    protected override void ComputePart2()
    {
        Result = 1;
        Multiply(CountHits(1, 1));
        Multiply(CountHits(3, 1));
        Multiply(CountHits(5, 1));
        Multiply(CountHits(7, 1));
        Multiply(CountHits(1, 2));
    }

    private long CountHits(int speedX, int speedY)
    {
        var width = _map.GetLength(0);
        var height = _map.GetLength(1);
        var row = 0;
        var column = 0;
        var result = 0L;

        do
        {
            result += _map[row, column].ToInt();
            row = (row + speedX) % width;
            column = (column + speedY);
        } while (column < height);

        return result;
    }
}


#if DUMP
#endif