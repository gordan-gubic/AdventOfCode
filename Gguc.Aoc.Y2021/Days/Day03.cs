#define LOGx
#define STOPWATCH

namespace Gguc.Aoc.Y2021.Days;

public class Day03 : Day
{
    private const int YEAR = 2021;
    private const int DAY = 3;

    private List<string> _data;

    public Day03(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();
    }

    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Real;

        _data = Parser.Parse();
    }

    public override void DumpInput()
    {
        DumpData();
    }

    protected override void ComputePart1()
    {
        var temp1 = _data.TransposeListString();

        var x = CalculateBinaryString(temp1);
        var y = x.InverseBinaryString();

        Multiply(x.FromBinaryStringToInt());
        Multiply(y.FromBinaryStringToInt());
    }

    protected override void ComputePart2()
    {
        var x = OxygenGeneratorRating();
        var y = CO2ScrubberRating();

        Multiply(x.FromBinaryStringToInt());
        Multiply(y.FromBinaryStringToInt());
    }

    private string CalculateBinaryString(List<string> list)
    {
        var sb = new StringBuilder();

        foreach (var item in list)
        {
            var x = CalculateTarget(item);
            sb.Append(x);
        }

        return sb.ToString();
    }

    private string OxygenGeneratorRating()
    {
        var length0 = _data[0].Length;
        var list = _data.ToList();

        for (int i = 0; i < length0; i++)
        {
            var temp1 = list.TransposeListString();

            var target = CalculateTarget(temp1[i]);

            list = list.Where(x => x[i] == target).ToList();

            if (list.Count == 1) break;
        }

        return list[0];
    }

    private string CO2ScrubberRating()
    {
        var length0 = _data[0].Length;
        var list = _data.ToList();

        for (int i = 0; i < length0; i++)
        {
            var temp1 = list.TransposeListString();

            var target = CalculateTarget(temp1[i]);
            target = target.InverseBinaryChar();

            list = list.Where(x => x[i] == target).ToList();

            if (list.Count == 1) break;
        }

        return list[0];
    }

    private char CalculateTarget(string input)
    {
        char x = '0';
        char y = '1';

        var xcount = input.Count(s => s == x);
        var ycount = input.Count(s => s == y);

        return xcount > ycount ? x : y;
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (!Log.EnableDebug) return;

        Debug();

        //_data.DumpJson();
    }
}

#if DUMP
#endif