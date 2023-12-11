#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2023.Days;

public class Day13 : Day
{
    private const int YEAR = 2023;
    private const int DAY = 13;

    private List<string> _data;
    private List<Map<bool>> _mirrors;

    public Day13(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "31265";
        Expected2 = "39359";
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
        var result = 0L;

        foreach (var mirror in _mirrors)
        {
            var (c, r) = FindMirror(mirror);
            result += c + r * 100;
        }

        Result = result;
    }

    protected override void ComputePart2()
    {
        var result = 0L;

        foreach (var mirror in _mirrors)
        {
            var (c, r) = FindMirror(mirror);
            
            //mirror.MapBoolToString('#', '.').Dump("map-A", true);
            FixMirror(mirror);
            //mirror.MapBoolToString('#', '.').Dump("map-B", true);

            var (x, y) = FindMirror(mirror, c, r);

            // Log.Debug($"c,r=[{(c, r)}]  x,y=[{(x, y)}]");

            if (x != c) result += x;
            if(y != r) result += y * 100;
        }

        Result = result;
    }

    private void FixMirror(Map<bool> mirror)
    {
        {
            // columns
            var columns = GetColumns(mirror);

            var speckedLine = FindSpeckedLine(columns);
            if (speckedLine.Item1 != -1)
            {
                // Log.Debug($"specked column={speckedLine}");

                var col = speckedLine.Item1;
                var row = speckedLine.Item2;

                mirror[col, row] = !mirror[col, row];
                return;
            }
        }
        {
            // rows
            var rows = GetRows(mirror);

            var speckedLine = FindSpeckedLine(rows);
            if (speckedLine.Item1 != -1)
            {
                // Log.Debug($"specked row={speckedLine}");

                var row = speckedLine.Item1;
                var col = speckedLine.Item2;

                mirror[col, row] = !mirror[col, row];
                return;
            }
        }
    }

    private (int Column, int Row) FindMirror(Map<bool> mirror, int ignoreColumn = int.MaxValue, int ignoreRow = int.MaxValue)
    {
        var row = 0;
        var column = 0;

        {
            // rows
            var rows = GetRows(mirror);
            row = FindMirrorLine(rows, ignoreRow);
        }

        {
            // columns
            var columns = GetColumns(mirror);
            column = FindMirrorLine(columns, ignoreColumn);
        }

        return (column, row);
    }

    private List<List<bool>> GetColumns(Map<bool> mirror)
    {
        var columns = new List<List<bool>>();
        var width = mirror.Width;
        var height = mirror.Height;
        var line = new List<bool>();

        for (var column = 0; column < width; column++)
        {
            line.Clear();

            for (var i = 0; i < height; i++)
            {
                line.Add(mirror[column, i]);
            }

            columns.Add(line.ToList());
        }

        return columns;
    }

    private List<List<bool>> GetRows(Map<bool> mirror)
    {
        var rows = new List<List<bool>>();
        var width = mirror.Width;
        var height = mirror.Height;
        var line = new List<bool>();

        for (var row = 0; row < height; row++)
        {
            line.Clear();

            for (var i = 0; i < width; i++)
            {
                line.Add(mirror[i, row]);
            }

            rows.Add(line.ToList());
        }

        return rows;
    }

    private int FindMirrorLine(List<List<bool>> lines, int ignoreLine)
    {
        for (var i = 0; i < lines.Count; i++)
        {
            if(i + 1 == ignoreLine) continue;

            if (IsMirror(lines, i))
            {
                return i + 1;
            }
        }

        return 0;
    }

    private (int, int) FindSpeckedLine(List<List<bool>> lines)
    {
        for (var i = 0; i < lines.Count; i++)
        {
            var specked = IsSpecked(lines, i);
            if (specked.isOk)
            {
                return specked.Last;
            }
        }

        return (-1, -1);
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

    private (bool isOk, (int, int) Last) IsSpecked(List<List<bool>> lines, int i)
    {
        if (i + 1 >= lines.Count) return (false, (0, 0));

        var isOk = false;

        var specks = CountSpeck(lines[i], lines[i + 1], i + 1);
        var count = specks.Count;
        var last = specks.Last;

        if (count <= 1)
        {
            isOk = true;

            var k = i + 2;
            for (var j = i - 1; j >= 0; j--)
            {
                if (k >= lines.Count) break;

                specks = CountSpeck(lines[j], lines[k], k);
                count += specks.Count;
                if (specks.Count == 1) last = specks.Last;

                isOk = (count <= 1);
                if (!isOk) break;

                k++;
            }
        }

        isOk = count == 1;
        return (isOk, last);
    }

    private (int Count, (int, int) Last) CountSpeck(List<bool> previous, List<bool> line, int index)
    {
        if (previous.IsNullOrEmpty() || line.IsNullOrEmpty()) return (int.MaxValue, (0, 0));

        var count = 0;
        var last = 0;

        for (var i = 0; i < previous.Count; i++)
        {
            if (previous[i] != line[i])
            {
                count++;
                last = i;
            }
        }

        return (count, (index, last));
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