#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Core.Templates;

public class Day15 : Day
{
    private const int YEAR = 2021;
    private const int DAY = 15;

    private List<string> _source;
    private Map<int> _map;
    private int _width;
    private int _height;

    /**** Not 2816 But 2809 !?! ****/

    public Day15(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();
    }

    #region Parse
    protected override void InitParser()
    {
        Parser.Day = 15;
        Parser.Type = ParserFileType.Real;

        _source = Parser.Parse();
    }

    protected override void ProcessData()
    {
        _width = _source[0].Length;
        _height = _source.Count;
        _map = new Map<int>(_source, x => x.ToInt());
    }
    #endregion Parse

    protected override void ComputePart1()
    {
        var map = _map.Clone();
        var results = new Map<long>(_width, _height);

        Pathfind(map, results);

        Result = results[results.Width - 1, results.Height - 1];

        // results.Dump("r-1", true);
    }

    protected override void ComputePart2()
    {
        //return;

        var orig = _map.Clone();
        var map = new Map<int>(_width * 5, _height * 5);
        var results = new Map<long>(_width * 5, _height * 5);

        for (int fy = 0; fy < 5; fy++)
        {
            for (int fx = 0; fx < 5; fx++)
            {
                for (int y = 0; y < _height; y++)
                {
                    for (int x = 0; x < _width; x++)
                    {
                        var newvalue = orig[x, y] + fx + fy;
                        if (newvalue > 9) newvalue -= 9;
                        map[x + (fx * _width), y + (fy * _height)] = newvalue;
                    }
                }
            }
        }

        Pathfind(map, results);

        Result = results[results.Width - 1, results.Height - 1];

        // map.Dump("map-2", true);
        // map.MapIntToString().Dump("map-2", true);
        // results.Dump("r-1", true);
    }

    private void Pathfind(Map<int> map, Map<long> results)
    {
        results[0, 0] = 0L;

        for (int x = 1; x < map.Width; x++)
        {
            results[x, 0] = results[x - 1, 0] + map[x, 0];
        }

        for (int y = 1; y < map.Height; y++)
        {
            results[0, y] = results[0, y - 1] + map[0, y];
        }

        for (int y = 1; y < map.Height; y++)
        {
            for (int x = 1; x < map.Width; x++)
            {
                var a = results[x - 1, y] + map[x, y];
                var b = results[x, y - 1] + map[x, y];
                var newvalue = Math.Min(a, b);
                results[x, y] = newvalue;

                // TODO - fix backtrace - moving direction up and left...
                if((results[x - 1, y]) > (newvalue + map[x - 1, y]))
                {
                    // $"Corrected: {x - 1},{y}: {results[x - 1, y]} to {newvalue + map[x - 1, y]}".Dump();
                    results[x - 1, y] = newvalue + map[x - 1, y];
                }
            }
        }
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

        // _map.Dump("Map");
    }
    #endregion Dump
}

#if DUMP

#endif