#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2023.Days;

public class Day05 : Day
{
    private const int YEAR = 2023;
    private const int DAY = 05;

    private List<string> _data;
    private List<long> _seeds;
    private List<List<ConversionMap>> _ranges;

    public Day05(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "510109797";
        Expected2 = "9622622";
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
        var result = MinimalLocation1();

        Result = result;
    }

    protected override void ComputePart2()
    {
        var result = MinimalLocation2();

        Result = result;
    }

    private long MinimalLocation1()
    {
        var minimum = long.MaxValue;

        foreach (var seed in _seeds)
        {
            var location = GetLocation(seed);
            minimum = Math.Min(minimum, location);
        }

        return minimum;
    }

    private long MinimalLocation2()
    {
        var pairs = new List<(long, long)>();

        for (int i = 0; i < _seeds.Count - 1; i += 2)
        {
            var a = _seeds[i];
            var b = _seeds[i] + _seeds[i+1] - 1;

            pairs.Add((a, b));
        }

        foreach (var range in _ranges)
        {
            pairs = ConvertPairs(pairs, range);
        }

        var locations = new List<long>();

        foreach (var pair in pairs)
        {
            locations.Add(pair.Item1);
            locations.Add(pair.Item2);
        }

        return locations.Min();
    }

    private List<(long, long)> ConvertPairs(List<(long, long)> pairs, List<ConversionMap> maps)
    {
        var newpairs1 = new List<(long, long)>();
        var newpairs2 = new List<(long, long)>();

        foreach (var pair in pairs)
        {
            newpairs1.AddRange(BreakPairs(pair, maps));
        }

        foreach (var pair in newpairs1)
        {
            var isInside = false;
            
            foreach (var map in maps)
            {
                if (IsInside(pair, map))
                {
                    isInside = true;
                    newpairs2.Add(ConvertPair(pair, map));
                    break;
                }
            }

            if(!isInside)
            {
                newpairs2.Add(pair);
            }
        }

        return newpairs2;
    }

    private (long, long) ConvertPair((long, long) pair, ConversionMap map)
    {
        return (pair.Item1 + map.Conversion, pair.Item2 + map.Conversion);
    }

    private bool IsInside((long, long) pair, ConversionMap map)
    {
        return (pair.Item1 >= map.Begin && pair.Item2 <= map.End);
    }

    private bool IsOutside((long, long) pair, ConversionMap map)
    {
        return (map.Begin > pair.Item2 || map.End < pair.Item1);
    }

    private IEnumerable<(long, long)> BreakPairs((long, long) pair, List<ConversionMap> maps)
    {
        var newpairs = new List<(long, long)>();

        var breaks = new List<long>();

        var m1 = maps.OrderBy(m => m.Begin).ToList();

        foreach (var m in m1)
        {
            breaks.Add(m.Begin);
            breaks.Add(m.End);
        }

        var points = new List<long>
        {
            pair.Item1,
            pair.Item2
        };

        foreach (var br in breaks)
        {
            if(br <= pair.Item1 || br >= pair.Item2) continue;

            points.Add(br);
        }

        points.Sort();

        for (var i = 0; i < points.Count - 1; i++)
        {
            newpairs.Add((points[i], points[i+1]));
        }

        return newpairs;
    }

    private long GetLocation(long seed)
    {
        var location = seed;

        foreach (var range in _ranges)
        {
            location = GetDestination(location, range);
        }

        return location;
    }

    private long GetDestination(long location, List<ConversionMap> maps)
    {
        foreach (var map in maps)
        {
            if (location >= map.Begin && location <= map.End)
            {
                location += map.Conversion;
                break;
            }
        }

        return location;
    }

    protected override void ProcessData()
    {
        /*
        seeds: 79 14 55 13

        seed-to-soil map:
        50 98 2
        52 50 48
         */

        base.ProcessData();

        var queue = new Queue<string>(_data);
        var line = queue.Dequeue();

        _seeds = GetSeeds(line);

        var ranges = new List<List<ConversionMap>>();
        var range = new List<ConversionMap>();

        queue.Dequeue();
        while (queue.Count > 0)
        {
            line = queue.Dequeue();

            if (line.IsWhitespace())
            {
                ranges.Add(range);
                range = new();
                continue;
            }

            if (char.IsLetter(line[0])) continue;

            var raw = line.Replace("seeds: ", "").Split(' ').Select(x => x.ToLong()).ToList();
            var map = new ConversionMap { Begin = raw[1], End = raw[1] + raw[2] - 1, Size = raw[2], Target = raw[0], Conversion = raw[0] - raw[1] };
            range.Add(map);
        }

        ranges.Add(range);

        _ranges = ranges;
    }

    private List<long> GetSeeds(string line)
    {
        var list = new List<long>();

        var raw = line.Replace("seeds: ", "").Split(' ');
        raw.ForEach(x => list.Add(x.ToLong()));

        return list;
    }

    private int Convert(string input)
    {
        return input.ToInt();
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (!Log.EnableDebug) return;

        Debug();

        // data.DumpCollection();

        // _seeds.DumpJson();
        // _ranges.DumpJson();
    }
}

#if DUMP
#endif