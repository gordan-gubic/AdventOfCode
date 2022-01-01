#define LOGx
#define STOPWATCH

namespace Gguc.Aoc.Y2021.Days;

public class Day09 : Day
{
    private const int YEAR = 2021;
    private const int DAY = 9;
    
    private List<string> _source;
    private Map<int> _data;
    private Map<bool> _basins;
    private int _width;
    private int _height;

    public Day09(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Real;

        _source = Parser.Parse();
    }

    /// <inheritdoc />
    protected override void ProcessData()
    {
        _width = _source[0].Length;
        _height = _source.Count;

        var map = new Map<int>(_width, _height);
        var basins = new Map<bool>(_width, _height);

        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                var value = $"{_source[y][x]}".ToInt();

                map.Values[x, y] = value;

                if (value < 9) basins.Values[x, y] = true;
            }
        }

        _data = map;
        _basins = basins;
    }

    protected override void ComputePart1()
    {
        Result = 0;
        var map = _data;

        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                ProcessPoint(map, x, y);
            }
        }
    }

    protected override void ComputePart2()
    {
        Result = 1;
        List<int> basinSizes = new List<int>();
        var basins = _basins.Clone();

        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                var size = ProcessBasin(basins, x, y);

                if(size > 0) basinSizes.Add(size);
            }
        }

        basinSizes.DumpJson();

        FindLargests(basinSizes);
    }

    private void FindLargests(List<int> basinSizes)
    {
        var ordered = basinSizes.OrderByDescending(x => x).ToList();

        Multiply(ordered[0]);
        Multiply(ordered[1]);
        Multiply(ordered[2]);
    }

    private void ProcessPoint(Map<int> map, int x, int y)
    {
        var value = map.GetValue(x, y);
        var up = y - 1;
        var down = y + 1;
        var left = x - 1;
        var right = x + 1;

        if (up >= 0 && map.GetValue(x, up) <= value)
        {
            return;
        }
        if (down < _height && map.GetValue(x, down) <= value)
        {
            return;
        }
        if (left >= 0 && map.GetValue(left, y) <= value)
        {
            return;
        }
        if (right < _width && map.GetValue(right, y) <= value)
        {
            return;
        }

        Add(value + 1);
    }

    private int ProcessBasin(Map<bool> basins, int x, int y)
    {
        var value = basins.GetValue(x, y);
        if (!value) return 0;

        var total = 1;
        basins.Values[x, y] = false;

        var up = y - 1;
        var down = y + 1;
        var left = x - 1;
        var right = x + 1;

        if (up >= 0)
        {
            total += ProcessBasin(basins, x, up);
        }
        if (down < _height)
        {
            total += ProcessBasin(basins, x, down);
        }
        if (left >= 0)
        {
            total += ProcessBasin(basins, left, y);
        }
        if (right < _width)
        {
            total += ProcessBasin(basins, right, y);
        }

        return total;
    }

    #region Dump
    public override void DumpInput()
    {
        DumpData();
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (!Log.EnableDebug) return;

        Debug();

        _data.Dump("Map", true);
        _basins.Dump("basins", true);
    }
    #endregion Dump
}

#if DUMP
#endif