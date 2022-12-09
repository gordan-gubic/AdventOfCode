#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2022.Days;

public class Day08 : Day
{
    private const int YEAR = 2022;
    private const int DAY = 8;

    private List<string> _data;
    private Map<int> _map;
    private int _width;
    private int _height;
    private long _sum;
    private Map<long> _location;

    public Day08(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "1672";
        Expected2 = "327180";
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
        _sum = 0L;
        _map.ForEach(IsVisible);

        Result = _sum;
    }

    protected override void ComputePart2()
    {
        _location = new Map<long>(_width, _height);
        _map.ForEach(VisibleFrom);

        Result = _location.MaxValue();
    }

    public void IsVisible(int x, int y)
    {
        if (x == 0 || y == 0 || x == _width - 1 || y == _height - 1)
        {
            _sum++;
            return;
        }

        if (IsVisibleLeft(x, y) || IsVisibleRight(x, y) || IsVisibleTop(x, y) || IsVisibleBottom(x, y))
        {
            _sum++;
            return;
        }
    }

    private bool IsVisibleTop(int x, int y)
    {
        var value = _map[x, y];

        for (int i = 0; i < y; i++)
        {
            if (_map[x, i] >= value) return false;
        }

        return true;
    }

    private bool IsVisibleBottom(int x, int y)
    {
        var value = _map[x, y];

        for (int i = y + 1; i < _height; i++)
        {
            if (_map[x, i] >= value) return false;
        }

        return true;
    }

    private bool IsVisibleLeft(int x, int y)
    {
        var value = _map[x, y];

        for (int i = 0; i < x; i++)
        {
            if (_map[i, y] >= value) return false;
        }

        return true;
    }

    private bool IsVisibleRight(int x, int y)
    {
        var value = _map[x, y];

        for (int i = x + 1; i < _width; i++)
        {
            if (_map[i, y] >= value) return false;
        }

        return true;
    }

    public void VisibleFrom(int x, int y)
    {
        var value = _map[x, y];

        _location[x, y] = VisibleFromTop(x, y) * VisibleFromBottom(x, y) * VisibleFromLeft(x, y) * VisibleFromRight(x, y);
    }

    private long VisibleFromTop(int x, int y)
    {
        var value = _map[x, y];
        var sum = 0;

        for (int i = y - 1; i >= 0; i--)
        {
            if (value <= _map[x, i])
            {
                sum++;
                return sum;
            }

            sum++;
        }

        return sum;
    }

    private long VisibleFromBottom(int x, int y)
    {
        var value = _map[x, y];
        var sum = 0;

        for (int i = y + 1; i < _height; i++)
        {
            if (value <= _map[x, i])
            {
                sum++;
                return sum;
            }

            sum++;
        }

        return sum;
    }

    private long VisibleFromLeft(int x, int y)
    {
        var value = _map[x, y];
        var sum = 0;

        for (int i = x - 1; i >= 0; i--)
        {
            if (value <= _map[i, y])
            {
                sum++;
                return sum;
            }

            sum++;
        }

        return sum;
    }

    private long VisibleFromRight(int x, int y)
    {
        var value = _map[x, y];
        var sum = 0;

        for (int i = x + 1; i < _width; i++)
        {
            if (value <= _map[i, y])
            {
                sum++;
                return sum;
            }

            sum++;
        }

        return sum;
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        _map = new Map<int>(_data, Converters.ToInt);
        _width = _map.Width;
        _height = _map.Height;

        //_map.Dump("map");
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