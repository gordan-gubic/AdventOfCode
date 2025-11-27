#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2024.Days;

public class Day24 : Day
{
    private const int YEAR = 2024;
    private const int DAY = 24;

    private List<string> _data;
    private Dictionary<string, bool> _values;
    private List<Operation> _operations;

    public Day24(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
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
    }

    /// <inheritdoc />
    public override void DumpInput()
    {
        DumpData();
    }

    protected override void ComputePart1()
    {
        Result = CalculateZ();
    }

    protected override void ComputePart2()
    {
        var result = 0L;

        Result = result;
    }

    private long CalculateZ()
    {
        var x = _values.Keys.Where(k => k.StartsWith("z")).ToList();
        x.Sort();
        x.Reverse();

        var sb = new StringBuilder();
        foreach (var x1 in x)
        {
            sb.Append(_values[x1] ? "1" : "0");
        }

        sb.Dump();

        return sb.ToString().FromBinaryStringToLong();
    }

    private void FillValues()
    {
        var ops = _operations.ToList();

        while (ops.Count > 0)
        {
            var o1 = ops.Where(o => _values.ContainsKey(o.X1) && _values.ContainsKey(o.X2)).ToList();

            foreach (var o2 in o1)
            {
                var x = SolveOperation(o2);
                _values[o2.R1] = x;

                ops.Remove(o2);
            }
        }

        // _values.DumpCollection();
    }

    private bool SolveOperation(Operation op)
    {
        return op.Op switch
        {
            "AND" => _values[op.X1] & _values[op.X2],
            "OR" => _values[op.X1] | _values[op.X2],
            "XOR" => _values[op.X1] ^ _values[op.X2],
            _ => false
        };
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        var isValues = true;
        var valueLines = new List<string>();
        var operationLines = new List<string>();

        // Gromit do something!
        foreach (var line in _data)
        {
            if (line.IsWhitespace())
            {
                isValues = false;
                continue;
            }

            if (isValues) valueLines.Add(line);
            else operationLines.Add(line);
        }

        _values = new();
        _operations = new List<Operation>();

        foreach (var line in valueLines)
        {
            var parts = line.Split(':', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            var x = parts[0];
            var y = parts[1] == "1";

            _values[x] = y;
        }

        foreach (var line in operationLines)
        {
            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var ops = new Operation { Op = parts[1], X1 = parts[0], X2 = parts[2], R1 = parts[4] };

            _operations.Add(ops);
        }

        FillValues();
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

        // _values.DumpCollection();
        // _operations.DumpCollection();
    }
}

#if DUMP
#endif