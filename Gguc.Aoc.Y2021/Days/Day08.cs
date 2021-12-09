#define LOGx
#define STOPWATCH

namespace Gguc.Aoc.Y2021.Days;

public class Day08 : Day
{
    private const int YEAR = 2021;
    private const int DAY = 8;

    private List<string> _source;
    private List<List<string>> _data;
    private List<List<int>> _values;
    private List<List<string>> _sorted;

    public Day08(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
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
        _data = new List<List<string>>();

        _source.ForEach(x => _data.Add(x.Split(new char[] { ' ', '|' }, StringSplitOptions.RemoveEmptyEntries).ToList()));

        _values = Decode(_data);

        _sorted = Sorted(_data);
    }

    private List<List<int>> Decode(List<List<string>> data)
    {
        var result = new List<List<int>>();

        foreach (var row in data)
        {
            var r = new List<int>();

            foreach (var c in row)
            {
                r.Add(c.Length);
            }

            result.Add(r);
        }

        return result;
    }

    private List<List<string>> Sorted(List<List<string>> data)
    {
        var result = new List<List<string>>();

        foreach (var row in data)
        {
            var r = new List<string>();

            foreach (var text in row)
            {
                var x = text.ToList();
                x.Sort();

                r.Add(x.JoinToString());
            }

            result.Add(r);
        }

        return result;
    }

    protected override void ComputePart1()
    {
        Result = 0;

        var hash = new HashSet<int> { 2, 3, 4, 7 };

        foreach (var row in _values)
        {
            for (int i = 10; i <= 13; i++)
            {
                if (hash.Contains(row[i])) Add(1);
            }
        }
    }

    protected override void ComputePart2()
    {
        Result = 0;

        var rowscount = _sorted.Count;

        for (int i = 0; i < rowscount; i++)
        {
            var signals = _sorted[i].ToList();
            var map = CreateMap(signals);

            var sb = new StringBuilder();
            for (int j = 10; j <= 13; j++)
            {
                sb.Append(map[signals[j]]);
            }

            var output = sb.ToString().ToLong();
            Add(output);
        }
    }

    private Dictionary<string, int> CreateMap(List<string> signals)
    {
        var map = new Dictionary<string, int>();
        var hash = new HashSet<string>();

        for (int i = 0; i < 10; i++)
        {
            hash.Add(signals[i]);
        }

        AddValueToMap(1, hash, map, x => x.Length == 2);
        AddValueToMap(8, hash, map, x => x.Length == 7);

        var x4 = AddValueToMap(4, hash, map, x => x.Length == 4);
        var x7 = AddValueToMap(7, hash, map, x => x.Length == 3);

        AddValueToMap(9, hash, map, x => x.Length == 6 && Contains(x, x4) && Contains(x, x7));
        AddValueToMap(0, hash, map, x => x.Length == 6 && Contains(x, x7));

        var x6 = AddValueToMap(6, hash, map, x => x.Length == 6);

        AddValueToMap(3, hash, map, x => x.Length == 5 && Contains(x, x7));
        AddValueToMap(5, hash, map, x => x.Length == 5 && Contains(x6, x));
        AddValueToMap(2, hash, map, x => x.Length == 5);

        return map;
    }

    private string AddValueToMap(int value, HashSet<string> hash, Dictionary<string, int> map, Func<string, bool> predicate)
    {
        var code = hash.Where(predicate).FirstOrDefault();
        map[code] = value;
        hash.Remove(code);
        return code;
    }

    private bool Contains(string x, string y)
    {
        var x1 = x.ToList();
        var y1 = y.ToList();

        return y1.Except(x1).Count() == 0;
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

        _data[0].Dump("Item");
        _data.DumpJson("List");

        _values.DumpJson("Values");
        _sorted.DumpJson("Sorted");
    }
    #endregion Dump
}

#if DUMP
#endif