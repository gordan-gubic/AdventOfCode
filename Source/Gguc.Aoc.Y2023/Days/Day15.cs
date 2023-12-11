#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2023.Days;

public class Day15 : Day
{
    private const int YEAR = 2023;
    private const int DAY = 15;

    private List<string> _data;
    private List<string> _sequence;

    public Day15(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "513643";
        Expected2 = "265345";
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
        var result = SumSequence();

        Result = result;
    }

    protected override void ComputePart2()
    {
        var result = FocusingPower();

        Result = result;
    }

    private long SumSequence()
    {
        var sum = 0L;

        foreach (var hash in _sequence)
        {
            sum += CalculateValue(hash);
        }

        return sum;
    }

    private long FocusingPower()
    {
        var sum = 0L;
        var boxes = new Dictionary<int, List<Len>>();

        for (var i = 0; i < 256; i++)
        {
            boxes[i] = new List<Len>();
        }

        foreach (var seq in _sequence)
        {
            var len = CreateLen(seq);
            ProcessLen(boxes, len);
        }

        foreach (var key in boxes.Keys)
        {
            var i = 1;
            foreach (var len in boxes[key])
            {
                var value = len.Length * (key + 1) * i;

                // Log.Debug($"Key=[{key}]... i=[{i}]... Len=[{len}]... Value=[{value}]");

                sum += value;
                i++;
            }
        }

        return sum;
    }

    private void ProcessLen(Dictionary<int, List<Len>> boxes, Len len)
    {
        var box = len.Box;
        var list = boxes[box];

        if (len.Operation == '-')
        {
            list.RemoveAll(x => x.Id == len.Id);
        }
        else
        {
            var current = list.FirstOrDefault(x => x.Id == len.Id);
            if (current == null)
            {
                list.Add(len);
            }
            else
            {
                current.Length = len.Length;
            }
        }
    }

    private Len CreateLen(string raw)
    {
        if (raw.Contains('='))
        {
            var parts = raw.Split('=', StringSplitOptions.RemoveEmptyEntries);
            var id = parts[0];
            var ln = parts[1].ToInt();

            return new Len { Id = id, Length = ln, Box = CalculateValue(id), Operation = '=' };
        }
        else
        {
            var parts = raw.Split('-', StringSplitOptions.RemoveEmptyEntries);
            var id = parts[0];

            return new Len { Id = id, Box = CalculateValue(id), Operation = '-' };
        }
    }

    private int CalculateValue(string hash)
    {
        var current = 0;

        foreach (var ch in hash)
        {
            var value = (int)ch;
            current += value;
            current *= 17;
            current %= 256;
        }

        return current;
    }

    protected override void ProcessData()
    {
        // rn=1,cm-,qp=3,cm=2,qp-,pc=4,ot=9,ab=5,pc-,pc=6,ot=7

        base.ProcessData();

        _sequence = _data[0].Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
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
        // _sequence.DumpCollection();
    }
}

#if DUMP
#endif