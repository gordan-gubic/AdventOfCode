#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2024.Days;

public class Day23 : Day
{
    private const int YEAR = 2024;
    private const int DAY = 23;

    private List<string> _data;
    private Dictionary<string, HashSet<string>> _comps;
    private HashSet<(string, string, string)> _lanSet;

    public Day23(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        ExpectedProd1 = "1308";
        ExpectedProd2 = "bu,fq,fz,pn,rr,st,sv,tr,un,uy,zf,zi,zy";
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
        Result = CountParty();
    }

    protected override void ComputePart2()
    {
        Result = FindPassword();
    }

    private long CountParty()
    {
        var tList = _comps.Keys.Where(x => x.StartsWith("t")).ToList();

        var lanSet = new HashSet<(string, string, string)>();

        foreach (var tComp in tList)
        {
            var tVal = _comps[tComp];

            foreach (var rComp in tVal.ToList())
            {
                var x1 = _comps[rComp].ToHashSet();
                var x2 = tVal.ToHashSet();

                x1.Remove(tComp);
                x2.Remove(rComp);

                var x3 = x1.Intersect(x2);

                foreach (var xComp in x3)
                {
                    // $"{tComp}:{rComp}::{xComp}".Dump();

                    var yList = new List<string>{ tComp, rComp, xComp };
                    yList.Sort();

                    lanSet.Add((yList[0], yList[1], yList[2]));
                }
            }
        }

        _lanSet = lanSet;

        // lanSet.DumpJson("lan");

        return lanSet.Count;
    }

    private long FindPassword()
    {
        var lRest = _lanSet.ToList();
        lRest.Sort();

        var lanList = new List<HashSet<string>>();

        while (lRest.Count > 0)
        {
            var x = lRest.FirstOrDefault();
            lRest.Remove(x);

            var lSet = new HashSet<string>
            {
                x.Item1,
                x.Item2,
                x.Item3
            };

            foreach (var rest in lRest.ToList())
            {
                var a = rest.Item1;
                var b = rest.Item2;
                var c = rest.Item3;

                var all = true;

                if(!lSet.Contains(a))
                {
                    all &= lSet.All(y => _comps[y].Contains(a));
                }
                if (!lSet.Contains(b))
                {
                    all &= lSet.All(y => _comps[y].Contains(b));
                }
                if (!lSet.Contains(c))
                {
                    all &= lSet.All(y => _comps[y].Contains(c));
                }

                if (all)
                {
                    lRest.Remove(rest);
                    lSet.Add(a);
                    lSet.Add(b);
                    lSet.Add(c);
                }
            }

            lanList.Add(lSet);
        }

        var lanX = lanList.MaxBy(s => s.Count).ToList();
        lanX.Sort();

        string.Join(',', lanX).Dump();

        return 1L;
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        _comps = new();

        // Gromit do something!
        foreach (var line in _data)
        {
            var parts = line.Split('-', StringSplitOptions.RemoveEmptyEntries);
            var x = parts[0];
            var y = parts[1];
            AddComp(x, y);
            AddComp(y, x);
        }
    }

    private void AddComp(string a, string b)
    {
        if (!_comps.ContainsKey(a)) _comps[a] = new();
        _comps[a].Add(b);
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

        // _data.DumpCollection();
        // _comps.DumpJson();
    }
}

#if DUMP
#endif