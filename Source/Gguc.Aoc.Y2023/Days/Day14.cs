#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2023.Days;

public class Day14 : Day
{
    private const int YEAR = 2023;
    private const int DAY = 14;

    private List<string> _data;
    private Map<char> _map;

    public Day14(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "106186";
        Expected2 = "106390";
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Test;
        Parser.Type = ParserFileType.Real;

        _map = Parser.ParseMapChar();
    }

    /// <inheritdoc />
    public override void DumpInput()
    {
        DumpData();
    }

    protected override void ComputePart1()
    {
        var map = _map.Clone();

        var result = CalculateWeightNorth(map);

        Result = result;
    }

    protected override void ComputePart2()
    {
        var map = _map.Clone();

        var weights = new List<long>();

        for (var i = 0; i < 200; i++)
        {
            var weight = CycleMap(map);
            weights.Add(weight);
        }

        // weights.DumpCollection("weights");

        // EXCEL
        var offset = 108;
        var mod = 12;

        var result = weights[offset + mod - 1];

        Result = result;
    }


    private long CalculateWeightNorth(Map<char> map)
    {
        var sum = 0L;

        // NORTH
        for (int i = 0; i < map.Width; i++)
        {
            var line = map.GetColumn(i);
            line.RemoveAt(line.Count - 1);
            var text = ProcessLine(line);
            ReplaceColumn(map, i, text);
        }

        // Weight
        for (int i = 0; i < map.Width; i++)
        {
            var line = map.GetColumn(i);
            line.RemoveAt(line.Count - 1);
            var text = LineToString(line);

            var weight = WeightLine(text);
            sum += weight;
        }

        return sum;
    }

    private long CycleMap(Map<char> map)
    {
        var sum = 0L;

        // NORTH
        for (int i = 0; i < map.Width; i++)
        {
            var line = map.GetColumn(i);
            line.RemoveAt(line.Count - 1);
            var text = ProcessLine(line);
            ReplaceColumn(map, i, text);
        }

        // WEST
        for (int i = 0; i < map.Height; i++)
        {
            var line = map.GetRow(i);
            line.RemoveAt(line.Count - 1);
            var text = ProcessLine(line);
            ReplaceRow(map, i, text);
        }

        // SOUTH
        for (int i = 0; i < map.Width; i++)
        {
            var line = map.GetColumn(i);
            line.RemoveAt(line.Count - 1);
            var text = ProcessLine(line, true);
            ReplaceColumn(map, i, text);
        }

        // EAST
        for (int i = 0; i < map.Height; i++)
        {
            var line = map.GetRow(i);
            line.RemoveAt(line.Count - 1);
            var text = ProcessLine(line, true);
            ReplaceRow(map, i, text);
        }

        // Weight
        for (int i = 0; i < map.Width; i++)
        {
            var line = map.GetColumn(i);
            line.RemoveAt(line.Count - 1);
            var text = LineToString(line);
            var weight = WeightLine(text);
            sum += weight;
        }

        return sum;
    }

    private long WeightLine(string text)
    {
        var sum = 0;

        var w = text.Length;
        for (var i = 0; i < text.Length; i++, w--)
        {
            if (text[i] == 'O') sum += w;
        }

        return sum;
    }

    private void ReplaceColumn(Map<char> map, int column, string line)
    {
        for (var i = 0; i < line.Length; i++)
        {
            map[column, i] = line[i];
        }
    }

    private void ReplaceRow(Map<char> map, int row, string line)
    {
        for (var i = 0; i < line.Length; i++)
        {
            map[i, row] = line[i];
        }
    }

    private string ProcessLine(List<(int, int, char)> line, bool reversed = false)
    {
        var sb = new StringBuilder();

        var text = LineToString(line);
        var parts = SplitLine(text);

        foreach (var part in parts)
        {
            var newText = ProcessPart(part, reversed);
            sb.Append(newText);
        }
        return sb.ToString();
    }

    private IEnumerable<string> SplitLine(string text)
    {
        var parts = new List<string>();
        var sb = new StringBuilder();

        foreach (var value in text)
        {
            if (value == '#')
            {
                if(sb.Length > 0) parts.Add(sb.ToString());
                sb.Clear();
                parts.Add("#");
            }
            else
            {
                sb.Append(value);
            }
        }

        if (sb.Length > 0) parts.Add(sb.ToString());
        return parts;
    }

    private string ProcessPart(string part, bool reversed = false)
    {
        if (part[0] == '#') return part;

        var count = part.Count(x => x == 'O');
        if (count == 0) return part;

        var text = !reversed
            ? $"".PadLeft(count, 'O').PadRight(part.Length, '.')
            : $"".PadRight(count, 'O').PadLeft(part.Length, '.');

        return text;
    }

    private string LineToString(List<(int, int, char)> line)
    {
        var sb = new StringBuilder();
        line.ForEach(x => sb.Append(x.Item3));
        return sb.ToString();
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (!Log.EnableDebug) return;

        Debug();

        // _map.MapValueToString().Dump("map", true);
    }
}

#if DUMP
#endif