#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2023.DaysBackup;

public class Day13_01 : Day
{
    private const int YEAR = 2023;
    private const int DAY = 13_01;

    private List<string> _data;
    private List<Map<bool>> _mirrors;

    public Day13_01(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "31265";
        Expected2 = "";
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Real;
        Parser.Type = ParserFileType.Test;

        _data = Parser.Parse();
    }

    /// <inheritdoc />
    public override void DumpInput()
    {
        DumpData();
    }

    protected override void ComputePart1()
    {
        var result = FindMirrors();

        Result = result;
    }

    protected override void ComputePart2()
    {
        var result = 0L;

        Result = result;
    }

    private long FindMirrors()
    {
        var sum = 0L;

        foreach (var mirror in _mirrors)
        {
            sum += FindMirror(mirror);
        }

        return sum;
    }

    private long FindMirror(Map<bool> mirror)
    {
        var width = mirror.Width;
        var height = mirror.Height;

        var rows = new List<List<bool>>();
        var columns = new List<List<bool>>();
        var line = new List<bool>();


        // columns
        for (var column = 0; column < width; column++)
        {
            line.Clear();

            for (var i = 0; i < height; i++)
            {
                line.Add(mirror[column, i]);
            }

            columns.Add(line.ToList());
        }

        var x = FindMirrorLine(columns);
        if (x > 0) return x;

        // rows
        for (var row = 0; row < height; row++)
        {
            line.Clear();

            for (var i = 0; i < width; i++)
            {
                line.Add(mirror[i, row]);
            }

            rows.Add(line.ToList());
        }

        x = FindMirrorLine(rows);
        if (x > 0) return x * 100;

        line.Clear();

        return 0L;
    }

    private int FindMirrorLine(List<List<bool>> lines)
    {
        var founded = false;
        var line = new List<bool>();
        var previous = line.ToList();

        for (var i = 0; i < lines.Count; i++)
        {
            if (IsMirror(lines, i))
            {
                founded = true;
                return i + 1;
            }
        }

        return 0;
    }

    private bool IsMirror(List<List<bool>> lines, int i)
    {
        if (i + 1 >= lines.Count) return false;

        var isOk = false;

        if (IsEqual(lines[i], lines[i + 1]))
        {
            isOk = true;

            var k = i + 2;
            for (var j = i - 1; j >= 0; j--)
            {
                if (k >= lines.Count) break;

                isOk = IsEqual(lines[j], lines[k]);

                if (!isOk) break;

                k++;
            }
        }

        return isOk;
    }

    private bool IsEqual(List<bool> previous, List<bool> line)
    {
        if (previous.IsNullOrEmpty() || line.IsNullOrEmpty()) return false;

        for (var i = 0; i < previous.Count; i++)
        {
            if (previous[i] != line[i]) return false;
        }

        return true;
    }

    private bool Speck(List<bool> previous, List<bool> line)
    {
        if (previous.IsNullOrEmpty() || line.IsNullOrEmpty()) return false;

        for (var i = 0; i < previous.Count; i++)
        {
            if (previous[i] != line[i]) return false;
        }

        return true;
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        var queue = new Queue<string>(_data);

        var mirrors = new List<Map<bool>>();
        var lines = new List<string>();

        while (queue.Count > 0)
        {
            var line = queue.Dequeue();

            if (line.IsWhitespace())
            {
                mirrors.Add(ParseMapBool(lines));
                lines.Clear();
            }
            else
            {
                lines.Add(line);
            }
        }

        mirrors.Add(ParseMapBool(lines));

        _mirrors = mirrors;
    }

    public Map<bool> ParseMapBool(List<string> lines, char mapChar = '#')
    {
        var map = new Map<bool>(lines[0].Length, lines.Count);

        for (var y = 0; y < lines.Count; y++)
        {
            for (var x = 0; x < lines[y].Length; x++)
            {
                map.Values[x, y] = lines[y][x] == mapChar;
            }
        }

        return map;
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

        // _mirrors.ForEach(m => m.MapBoolToString('#', '.').Dump("map", true));
    }
}

#if DUMP
#endif