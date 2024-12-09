#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2024.Days;

using System;

public class Day04A : Day
{
    private const int YEAR = 2024;
    private const int DAY = 4;

    private List<string> _data;
    private Map<char> _map;
    private int _height;
    private int _width;


    public Day04A(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "";
        Expected2 = "";
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Test;
        Parser.Type = ParserFileType.Real;

        _data = Parser.Parse();
        _map = Parser.ParseMapChar();
        _height = _map.Height;
        _width = _map.Width;
    }

    /// <inheritdoc />
    public override void DumpInput()
    {
        DumpData();
    }

    protected override void ComputePart1()
    {
        var result = 0L;

        Result = CountXmas();
    }

    protected override void ComputePart2()
    {
        var result = 0L;

        Result = CountCrossMas();
    }

    private long CountXmas()
    {
        var sum = 0L;

        _map.ForEach((x, y, value) => { sum += CountXmasAtPoint(x, y, value); });

        return sum;
    }

    private long CountCrossMas()
    {
        var sum = 0L;

        _map.ForEach((x, y, value) => { sum += CountCrossMasAtPoint(x, y, value); });

        return sum;
    }

    private long CountXmasAtPoint(int x, int y, char value)
    {
        if(value != 'X') return 0;

        var sum = 0L;

        for (var i = 0; i < 360; i += 45)
        {
            sum += CountXmasAtDir(x, y, i);
        }
        
        return sum;
    }

    private long CountCrossMasAtPoint(int x, int y, char value)
    {
        if (value != 'A') return 0;

        var sum = 0L;

        sum += (CountCrossMasDir(x, y, 45, 225) && CountCrossMasDir(x, y, 135, 315)) ? 1 : 0;

        return sum;
    }

    private long CountXmasAtDir(int x, int y, int dir)
    {
        var xDir = dir switch { 0 =>  0, 45 => +1, 90 => +1, 135 => +1, 180 =>  0, 225 => -1, 270 => -1, 315 => -1, _ => 0 };
        var yDir = dir switch { 0 => -1, 45 => -1, 90 =>  0, 135 => +1, 180 => +1, 225 => +1, 270 =>  0, 315 => -1, _ => 0 };

        var val = "";

        for (var i = 1; i <= 3; i++)
        {
            var v1 = _map.GetValue(x + xDir * i, y + yDir * i);

            val += v1;
        }

        return val == "MAS" ? 1 : 0;
    }

    private bool CountCrossMasDir(int x, int y, int dir1, int dir2)
    {
        var xDir1 = dir1 switch { 0 => 0, 45 => +1, 90 => +1, 135 => +1, 180 => 0, 225 => -1, 270 => -1, 315 => -1, _ => 0 };
        var yDir1 = dir1 switch { 0 => -1, 45 => -1, 90 => 0, 135 => +1, 180 => +1, 225 => +1, 270 => 0, 315 => -1, _ => 0 };

        var xDir2 = dir2 switch { 0 => 0, 45 => +1, 90 => +1, 135 => +1, 180 => 0, 225 => -1, 270 => -1, 315 => -1, _ => 0 };
        var yDir2 = dir2 switch { 0 => -1, 45 => -1, 90 => 0, 135 => +1, 180 => +1, 225 => +1, 270 => 0, 315 => -1, _ => 0 };

        var word = $"{_map.GetValue(x + xDir1, y + yDir1)}A{_map.GetValue(x + xDir2, y + yDir2)}";

        return word == "MAS" || word == "SAM";
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (!Log.EnableDebug) return;

        Debug();

        // _data.DumpCollection();

        // _map.Dump("Map", true);
        // _map.MapValueToString().Dump("map", true);
    }
}

#if DUMP
#endif