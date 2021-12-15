#define LOGx

namespace Gguc.Aoc.Y2021.Models;

public class Packet
{
    private string _data;

    public Packet(string data)
    {
        _data = data;

        Values = new List<long>();
        Children = new List<Packet>();

        $"New packet: {GetHashCode()} From: [{_data}]".Dump();
        Process(_data);
    }

    public string Raw { get; private set; }

    public int Version { get; private set; }

    public int TypeId { get; private set; }

    public string Body { get; private set; }

    public int LengthId { get; private set; }

    public long Length { get; private set; }

    public long Number { get; private set; }

    public string Footer { get; private set; }

    public long Value { get; set; }

    public List<long> Values { get; private set; }

    public List<Packet> Children { get; private set; }

    private void Process(string data)
    {
        data.Dump("data");

        var version = data.Substring(0, 3);
        var typeId = data.Substring(3, 3);
        var body = data.Substring(6, data.Length - 6);

        DecodeVersion(version);
        DecodeTypeId(typeId);
        DecodeBody(body);

        Raw = version + typeId;
    }

    private void DecodeVersion(string version)
    {
        Version = version.FromBinaryStringToInt();
    }

    private void DecodeTypeId(string typeId)
    {
        TypeId = typeId.FromBinaryStringToInt();
    }

    private void DecodeBody(string body)
    {
        Body = body;

        if(TypeId == 4)
        {
            DecodeLiteral(body);
        }
        else
        {
            DecodeOperator(body);
        }
    }

    private void DecodeOperator(string input)
    {
        LengthId = input[0].ToInt();
        Raw += input[0];

        if (LengthId == 0)
        {
            Length = input.Substring(1, 15).FromBinaryStringToLong();
            var subpackets = input.Substring(16, input.Length - 16);
            DecodeSubPacketsByLength(subpackets, Length);
        }
        else
        {
            Number = input.Substring(1, 11).FromBinaryStringToLong();
            var subpackets = input.Substring(12, input.Length - 12);
            DecodeSubPacketsBySize(subpackets, Number);
        }
    }

    private void DecodeSubPacketsByLength(string subpackets, long length)
    {
        $"DecodeSubPacketsByLength() - length=[{length}]; subpackets=[{subpackets}]".Dump();

        var counter = 0;

        while (true)
        {
            subpackets.Dump("DecodeSubPacketsByLength-while-true");
            if (subpackets.IsWhitespace()) return;

            var packet = new Packet(subpackets);
            packet.Dump();

            Children.Add(packet);
            Values.Add(packet.Value);
            Raw += packet.Raw;

            var size = subpackets.Length - packet.Footer.Length;
            counter += size;

            subpackets = packet.Footer;
            $"DecodeSubPacketsByLength() - Footer=[{packet.Footer}]".Dump();

            if (counter >= length) break;
        }

        Footer = subpackets;
    }

    private void DecodeSubPacketsBySize(string subpackets, long number)
    {
        $"DecodeSubPacketsBySize() - number=[{number}]; subpackets=[{subpackets}]".Dump();

        var counter = 0;

        while (true)
        {
            subpackets.Dump("DecodeSubPacketsBySize-while-true");
            if (subpackets.IsWhitespace()) return;
 
            counter++;

            var packet = new Packet(subpackets);
            packet.Dump();

            Children.Add(packet);
            Values.Add(packet.Value);
            Raw += packet.Raw;

            subpackets = packet.Footer;
            $"DecodeSubPacketsBySize() - Footer=[{packet.Footer}]".Dump();

            if (counter >= number) break;
        }

        Footer = subpackets;
    }

    private void DecodeLiteral(string body)
    {
        $"DecodeLiteral() - body=[{body}]".Dump();

        var queue = body;
        var temp = "";
        var sb = new StringBuilder();

        while(queue.Length > 0)
        {
            temp = queue.Substring(0, 5);
            queue = queue.Remove(0, 5);

            sb.Append(temp.Substring(1, 4));
            Raw += temp;

            var isLast = temp[0] == '0' ? true : false;
            if (isLast) break;
            
        }

        var value = sb.ToString().FromBinaryStringToLong();
        Value = value;
        Footer = queue;
    }

    public override string ToString()
    {
        return $"Packet=[{GetHashCode()}]; Version=[{Version}]; TypeId=[{TypeId}]; Value=[{Value.ToJson()}]; Children=[{Children.Count}]";
        // return this.ToJson();
    }
}
