#define LOGx
#define STOPWATCH

namespace Gguc.Aoc.Y2021.Days;

public class Day04 : Day
{
    private const int YEAR = 2021;
    private const int DAY = 4;

    private List<string> _data;
    private List<int> _numbers;
    private List<BingoBoard> _boards;

    public Day04(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
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

    protected override void ProcessData()
    {
        _numbers = new List<int>();
        _data[0].Split(',', StringSplitOptions.RemoveEmptyEntries).ForEach(x => _numbers.Add(x.ToInt()));
        _numbers.DumpJson();

        _boards = new List<BingoBoard>();
        var sb = new StringBuilder();
        var temp = new List<string>();

        for (int i = 2; i < _data.Count; i++)
        {
            var line = _data[i];

            if (line.IsWhitespace())
            {
                _boards.Add(new BingoBoard(temp.ToList()));
                temp.Clear();
            }
            else
            {
                temp.Add(line);
            }
        }

        if (temp.Count > 0)
        {
            _boards.Add(new BingoBoard(temp.ToList()));
            temp.Clear();
        }

        _boards.DumpCollection();
    }

    protected override void ComputePart1()
    {
        var winner = false;

        foreach (var n in _numbers)
        {
            foreach (var board in _boards)
            {
                winner = board.ExecuteNumber(n);

                if (winner)
                {
                    $"[{n}] - Winner! - [{board.GetHashCode()}][{board.Sum}]".Dump();
                    Result = n * board.Sum;
                    break;
                }
            }

            if (winner) break;
        }
    }

    protected override void ComputePart2()
    {
        var winner = false;

        _boards.ForEach(board => board.Reset());
        var tempBoards = _boards.ToList();

        foreach (var n in _numbers)
        {
            for (int i = tempBoards.Count - 1; i >= 0; i--)
            {
                var board = tempBoards[i];
                winner = board.ExecuteNumber(n);

                if (winner)
                {
                    $"[{n}] - Winner! - [{board.GetHashCode()}][{board.Sum}]".Dump();
                    Result = n * board.Sum;
                    tempBoards.RemoveAt(i);
                }
            }

            if (tempBoards.Count == 0) break;
        }
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (!Log.EnableDebug) return;

        Debug();

        _data.DumpJson();
    }
}

#if DUMP
#endif