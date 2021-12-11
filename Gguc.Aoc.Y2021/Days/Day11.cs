#define LOGx
#define STOPWATCH

namespace Gguc.Aoc.Core.Templates;

public class Day11 : Day
{
    private const int YEAR = 2021;
    private const int DAY = 11;

    private List<string> _source;
    private Map<int> _data;
    private int _width;
    private int _height;

    public Day11(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
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

        _data = new Map<int>(_source, Converters.ToInt);
    }

    protected override void ComputePart1()
    {
        var map = _data.Clone();

        var total = ProcessMap1(map, 100);

        Result = total;
    }

    protected override void ComputePart2()
    {
        var map = _data.Clone();

        var total = ProcessMap2(map);

        Result = total;
    }

    private long ProcessMap1(Map<int> map, int steps)
    {
        var total = 0L;

        for (int i = 0; i < steps; i++)
        {
            total += ProcessMap(map);
        }

        return total;
    }

    private long ProcessMap2(Map<int> map)
    {
        var i = 0;
        
        while(true)
        {
            i++;
            
            var total = ProcessMap(map);

            if (total == 100) break;
        }

        return i;
    }

    private long ProcessMap(Map<int> map)
    {
        var flashes = new Queue<(int, int)>();

        IncreaseValues(map);

        InitFlashes(map, flashes);

        ProcessFlashes(map, flashes);

        SetToZero(map);

        var total = map.CountValues(0);
        return total;
    }

    private void IncreaseValues(Map<int> map)
    {
        map.ForEach((x, y) => map[x, y]++);
    }

    private void InitFlashes(Map<int> map, Queue<(int, int)> flashes)
    {
        map.ForEach((x, y) => {
            if (map[x, y] > 9) flashes.Enqueue((x, y));
        });
    }

    private void SetToZero(Map<int> map)
    {
        map.ForEach((x, y) => {
            if (map[x, y] > 9) map[x, y] = 0;
        });
    }

    private void ProcessFlashes(Map<int> map, Queue<(int, int)> flashes)
    {
        while (flashes.Count > 0)
        {
            var flash = flashes.Dequeue();

            ProcessFlash(map, flashes, flash);
        }
    }

    private void ProcessFlash(Map<int> map, Queue<(int, int)> flashes, (int, int) flash)
    {
        var x = flash.Item1;
        var y = flash.Item2;

        IncreaseValue(map, flashes, x - 1, y - 1);
        IncreaseValue(map, flashes, x - 1, y + 0);
        IncreaseValue(map, flashes, x - 1, y + 1);
        IncreaseValue(map, flashes, x + 0, y - 1);
        IncreaseValue(map, flashes, x + 0, y + 1);
        IncreaseValue(map, flashes, x + 1, y - 1);
        IncreaseValue(map, flashes, x + 1, y + 0);
        IncreaseValue(map, flashes, x + 1, y + 1);
    }

    private void IncreaseValue(Map<int> map, Queue<(int, int)> flashes, int x, int y)
    {
        if (x < 0 || y < 0 || x >= _width || y >= _height) return;

        var value = map[x, y];
        value++;
        map[x, y] = value;
        if (value == 10) flashes.Enqueue((x, y));
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
    }
    #endregion Dump
}

#if DUMP
#endif