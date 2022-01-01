#define LOGx
#define STOPWATCH

namespace Gguc.Aoc.Y2021.Days;

public class Day20 : Day
{
    private const int YEAR = 2021;
    private const int DAY = 20;

    private List<string> _source;
    private string _code;
    private Map<bool> _map;

    public Day20(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();
    }

    #region Parse
    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Real;

        _source = Parser.Parse();
    }

    protected override void ProcessData()
    {
        _code = _source[0];

        var mapRaw = _source.ToList();
        mapRaw.RemoveRange(0, 2);

        _map = new Map<bool>(mapRaw, x => x == '#');
    }
    #endregion Parse

    protected override void ComputePart1()
    {
        var map = ProcessMapLoop(_map, 2);

        Result = map.CountValues(true);
    }

    protected override void ComputePart2()
    {
        var map = ProcessMapLoop(_map, 50);

        Result = map.CountValues(true);
    }

    private Map<bool> ProcessMapLoop(Map<bool> map0, int pass)
    {
        var map = map0.Clone();

        for (int i = 0; i < pass; i++)
        {
            map = ProcessMap(map, i);
        }

        return map;
    }

    private Map<bool> ProcessMap(Map<bool> map, int pass)
    {
        var oddValue = (pass % 2) != 0;
        map = map.Expand(2, oddValue);

        var newmap = new Map<bool>(map.Width, map.Height);

        map.ForEach((x, y) =>
        {
            var value = ProcesPixel(map, x, y);
            newmap[x, y] = value;
        });

        newmap = newmap.Reduce();

        return newmap;
    }

    private bool ProcesPixel(Map<bool> map, int x0, int y0)
    {
        var sb = new StringBuilder();

        for (int y = y0 - 1; y <= y0 + 1; y++)
        {
            for (int x = x0 - 1; x <= x0 + 1; x++)
            {
                var ch = map.GetValue(x, y) ? "1" : "0";
                sb.Append(ch);
            }
        }

        var pixel = sb.ToString();
        var value = pixel.FromBinaryStringToInt();
        var newvalue = _code[value] == '#';

        return newvalue;
    }

    #region Dump
    public override void DumpInput()
    {
        DumpData();
    }

    public void DumpMap(Map<bool> map)
    {
        map.MapBoolToString('#', '.').Dump("Map", true);
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (!Log.EnableDebug) return;

        Debug();

        _code.Dump("Item");
        DumpMap(_map);
    }
    #endregion Dump
}

#if DUMP

#endif