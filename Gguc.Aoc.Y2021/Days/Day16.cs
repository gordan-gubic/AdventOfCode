#define LOGx
#define STOPWATCH

namespace Gguc.Aoc.Core.Templates;

public class Day16 : Day
{
    private const int YEAR = 2021;
    private const int DAY = 16;

    private List<string> _source;
    private string _data;

    private Dictionary<char, string> _decode = new Dictionary<char, string>
    {
        ['0'] = "0000",
        ['1'] = "0001",
        ['2'] = "0010",
        ['3'] = "0011",
        ['4'] = "0100",
        ['5'] = "0101",
        ['6'] = "0110",
        ['7'] = "0111",
        ['8'] = "1000",
        ['9'] = "1001",
        ['A'] = "1010",
        ['B'] = "1011",
        ['C'] = "1100",
        ['D'] = "1101",
        ['E'] = "1110",
        ['F'] = "1111",
    };

    public Day16(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
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
        var sb = new StringBuilder();

        foreach (var ch in _source[0])
        {
            sb.Append(_decode[ch]);
        }

        _data = sb.ToString();
    }
    #endregion Parse

    protected override void ComputePart1()
    {
        var data = _data;

        var packet = new Packet(data);
        CalculatePacket1(packet);

        packet.Dump("packet", true);
    }

    protected override void ComputePart2()
    {
        var data = _data;

        var packet = new Packet(data);
        var result = CalculatePacket2(packet);

        packet.Dump("packet", true);

        Result = result;
    }

    private void CalculatePacket1(Packet packet)
    {
        $"CalculatePacket1. Packet=[{packet}]".Dump();
        Add(packet.Version);

        foreach(var child in packet.Children)
        {
            CalculatePacket1(child);
        }
    }

    private long CalculatePacket2(Packet packet)
    {
        $"CalculatePacket2. Packet=[{packet}]".Dump();

        foreach (var child in packet.Children)
        {
            if (child.Children.Count == 0) continue;
            child.Value = CalculatePacket2(child);
        }

        var operation = packet.TypeId;

        var result = operation switch
        {
            0 => CalcSum(packet.Children),
            1 => CalcProduct(packet.Children),
            2 => CalcMin(packet.Children),
            3 => CalcMax(packet.Children),
            5 => CalcGreaterThan(packet.Children),
            6 => CalcLessThan(packet.Children),
            7 => CalcEqual(packet.Children),
            _ => 0L,
        };

        return result;
    }

    private long CalcSum(List<Packet> children)
    {
        return children.Sum(x => x.Value);
    }

    private long CalcProduct(List<Packet> children)
    {
        var total = 1L;
        foreach (var child in children) total *= child.Value;
        return total;
    }

    private long CalcMin(List<Packet> children)
    {
        return children.Min(x => x.Value);
    }

    private long CalcMax(List<Packet> children)
    {
        return children.Max(x => x.Value);
    }

    private long CalcGreaterThan(List<Packet> children)
    {
        var value = children[0].Value > children[1].Value;
        return value ? 1 : 0;
    }

    private long CalcLessThan(List<Packet> children)
    {
        var value = children[0].Value < children[1].Value;
        return value ? 1 : 0;
    }

    private long CalcEqual(List<Packet> children)
    {
        var value = children[0].Value == children[1].Value;
        return value ? 1 : 0;
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

        _data.Dump("Decoded");
    }
    #endregion Dump
}

#if DUMP

#endif