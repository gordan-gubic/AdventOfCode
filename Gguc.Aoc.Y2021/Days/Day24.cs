#define LOGx
#define STOPWATCH

namespace Gguc.Aoc.Y2021.Days;

public class Day24 : Day
{
    private const int YEAR = 2021;
    private const int DAY = 24;

    private List<string> _source;
    private List<List<List<string>>> _data;
    private Dictionary<int, Func<Alu, Alu>> _functions;

    private long _count = 0L;
    private long _min = long.MaxValue;
    private long _max;

    public Day24(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
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
        _data = new List<List<List<string>>>();
        var temp = new List<List<string>>();

        foreach (var line in _source)
        {
            if (line.StartsWith("inp") && !temp.IsNullOrEmpty())
            {
                _data.Add(temp.ToList());
                temp.Clear();
            }

            temp.Add(line.Split(' ').ToList());
        }

        _data.Add(temp.ToList());

        _functions = ProcessFunctions(_data);
    }

    private Dictionary<int, Func<Alu, Alu>> ProcessFunctions(List<List<List<string>>> data)
    {
        var functions = new Dictionary<int, Func<Alu, Alu>>();

        for (int i = 0; i < data.Count; i++)
        {
            var set = data[i];

            var x = set[5][2].ToInt();
            var y = set[15][2].ToInt();
            var z = set[4][2].ToInt();

            functions[i] = a => ProcessGen(a, x, y, z);
        }

        return functions;
    }
    #endregion Parse

    protected override void ComputePart1()
    {
        var alu = new Alu();
        ProcessSet(alu, 0);

        Result = _max;
    }

    protected override void ComputePart2()
    {
        Result = _min;
    }

    private void ProcessSet(Alu alu0, int level)
    {
        var alu = default(Alu);

        for (int i1 = 1; i1 <= 9; i1++)
        {
            alu = alu0.Copy();
            alu.Init = i1;
            alu.ProcessInput();

            var a = _functions[level](alu);
            if (a == null) continue;

            if (level < 13)
            {
                ProcessSet(alu, level + 1);
            }
            else
            {
                if (alu.Z == 0)
                {
                    AddToResult(alu.History);
                    _count++;
                }
            }
        }
    }

    private void AddToResult(List<int> history)
    {
        var result = string.Join("", history).ToLong();

        _min = Math.Min(result, _min);
        _max = Math.Max(result, _max);
    }

    private Alu ProcessGen(Alu alu, int xx, int yy, int zz)
    {
        var w = alu.Input.Dequeue();
        var z = alu.Z;
        var x = z % 26 + xx == w ? 0 : 1;
        z = z / zz;

        // THE LUCKY GUESS!
        if (zz == 26 && x == 1) return null;

        var y = (w + yy) * x;
        z = z * (25 * x + 1) + y;

        return SetAlu(alu, w, x, y, z);
    }

    private Alu SetAlu(Alu alu, int w, long x, long y, long z)
    {
        alu.History.Add(w);
        alu.W = w;
        alu.X = x;
        alu.Y = y;
        alu.Z = z;

        return alu;
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

        //_data[0].Dump("Item");
        _data.DumpJson("List");
    }
    #endregion Dump
}

#if DROP

    private void ProcessIns(Alu alu, List<string> ins)
    {
        switch (ins[0])
        {
            case "inp":
                ReadInput(alu, ins);
                break;
            case "add":
                AddValue(alu, ins);
                break;
            case "mul":
                MultiplyValue(alu, ins);
                break;
            case "div":
                DivideValue(alu, ins);
                break;
            case "mod":
                ModuloValue(alu, ins);
                break;
            case "eql":
                EqualValue(alu, ins);
                break;
        }
    }

    private void ReadInput(Alu alu, List<string> ins)
    {
        var r = alu.Input.Dequeue();
        alu.SetValue(ins[1], r);
        alu.History.Add(r);
    }

    private void AddValue(Alu alu, List<string> ins)
    {
        var r = alu.GetValue(ins[1]) + alu.GetValue(ins[2]);
        alu.SetValue(ins[1], r);
    }

    private void MultiplyValue(Alu alu, List<string> ins)
    {
        var r = alu.GetValue(ins[1]) * alu.GetValue(ins[2]);
        alu.SetValue(ins[1], r);
    }

    private void DivideValue(Alu alu, List<string> ins)
    {
        var r = alu.GetValue(ins[1]) / alu.GetValue(ins[2]);
        alu.SetValue(ins[1], r);
    }

    private void ModuloValue(Alu alu, List<string> ins)
    {
        var r = alu.GetValue(ins[1]) % alu.GetValue(ins[2]);
        alu.SetValue(ins[1], r);
    }

    private void EqualValue(Alu alu, List<string> ins)
    {
        var r = alu.GetValue(ins[1]) == alu.GetValue(ins[2]) ? 1L : 0L;
        alu.SetValue(ins[1], r);
    }

#endif