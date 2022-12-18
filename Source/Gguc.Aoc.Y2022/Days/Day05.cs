#define LOGx
#define STOPWATCH

namespace Gguc.Aoc.Y2022.Days;

public class Day05 : Day
{
    private const int YEAR = 2022;
    private const int DAY = 5;

    private List<string> _data;
    private int _stacksCount;
    private Dictionary<int, Stack<char>> _stacks;
    private List<(int, int, int)> _moves;

    public Day05(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "BWNCQRMDB";
        Expected2 = "NHWZCBNBF";
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
        ProcessData();
        ProcessMoves1();

        var result = ReadResult();
        Log.Warn(result);
    }

    protected override void ComputePart2()
    {
        ProcessData();
        ProcessMoves2();

        var result = ReadResult();
        Log.Warn(result);
    }

    private string ReadResult()
    {
        var result = "";

        foreach (var stack in _stacks)
        {
            result += stack.Value.Pop();
        }

        return result;
    }

    private void ProcessMoves1()
    {
        foreach (var (m1, m2, m3) in _moves)
        {
            for (var i = 0; i < m1; i++)
            {
                var value = _stacks[m2 - 1].Pop();
                _stacks[m3 - 1].Push(value);
            }
        }
    }

    private void ProcessMoves2()
    {
        foreach (var (m1, m2, m3) in _moves)
        {
            var temp = new Stack<char>();

            for (var i = 0; i < m1; i++)
            {
                var value = _stacks[m2 - 1].Pop();
                temp.Push(value);
            }

            for (var i = 0; i < m1; i++)
            {
                var value = temp.Pop();
                _stacks[m3 - 1].Push(value);
            }
        }
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        /*
         * Stacks
         */

        var header = new List<string>();

        foreach (var line in _data)
        {
            if (line.IsWhitespace()) break;

            header.Add(line);
        }

        _stacksCount = header.LastOrDefault().Split(' ', StringSplitOptions.RemoveEmptyEntries).LastOrDefault().ToInt();
        Log.Info($"Number of stacks=[{_stacksCount}]");

        _stacks = new();

        for (var i = 0; i < _stacksCount; i++)
        {
            _stacks[i] = new();

            for (var j = header.Count - 1; j >= 0; j--)
            {
                var value = header[j][i * 4 + 1];
                if (value == ' ') continue;
                _stacks[i].Push(value);
            }
        }

        // _stacks.DumpJson();

        /*
         * Moves
         */

        _moves = new();

        var moves = _data.Skip(header.Count + 1).ToList();

        foreach (var value in moves)
        {
            var ps = value.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            _moves.Add((ps[1].ToInt(), ps[3].ToInt(), ps[5].ToInt()));
        }

        // _moves.DumpCollection();
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (!Log.EnableDebug) return;

        Debug();

        // _data.DumpCollection();

        "2022-12-05 06:59:34.6653|WARN | 1| - BWNCQRMDB".Dump();
        "2022-12-05 06:59:34.6653|WARN | 1| - NHWZCBNBF".Dump();
    }
}

#if DUMP
#endif