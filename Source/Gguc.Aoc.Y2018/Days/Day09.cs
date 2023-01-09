#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2018.Days;

public class Day09 : Day
{
    private const int YEAR = 2018;
    private const int DAY = 09;

    private List<string> _data;
    private List<(int, int)> _games;
    private Dictionary<int, long> _players;

    public Day09(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "390592"; 
        Expected2 = "3277920293";
    }

    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Test;
        Parser.Type = ParserFileType.Real;

        _data = Parser.Parse();
    }

    #region Parse
    protected override void ProcessData()
    {
        _games = new();

        var pattern = @"(?'a'\d+) players; last marble is worth (?'b'\d+) pointsN*";

        // Gromit do something!
        foreach (var line in _data)
        {
            var a = line.RegexValue(pattern, "a").ToInt();
            var b = line.RegexValue(pattern, "b").ToInt();
            _games.Add((a, b));
        }
    }
    #endregion

    #region Head
    protected override void ComputePart1()
    {
        var result = 0L;

        foreach (var (players, steps) in _games)
        {
            InitPlayers(players);
            result = RunMarbles(steps, players);
            Log.Debug($"Players={players}  Steps={steps}  Result={result}");
        }

        Result = result;
    }

    protected override void ComputePart2()
    {
        var result = 0L;

        foreach (var (players, steps) in _games)
        {
            InitPlayers(players);
            result = RunMarbles(steps * 100, players);
            Log.Debug($"Players={players}  Steps={steps}  Result={result}");
        }

        Result = result;
    }
    #endregion

    #region Body

    private void InitPlayers(int players)
    {
        _players = new();

        for (int i = 1; i <= players; i++)
        {
            _players[i] = 0L;
        }
    }

    private long RunMarbles(int steps, int players)
    {
        var anchor = 0;
        var list = new LinkedList<int>();

        var node = list.AddFirst(anchor);

        for (var i = 1; i <= steps; i++)
        {
            var player = (i - 1) % players + 1;

            if (i % 23 == 0)
            {
                var temp = node;

                for (var j = 0; j < 7; j++)
                {
                    temp = temp.Previous;

                    if (temp.Value == 0)
                    {
                        temp = list.Last;
                        j++;
                    }
                }

                _players[player] += i;
                _players[player] += temp.Value;

                node = temp.Next;
                list.Remove(temp);
            }
            else
            {
                node = node.Next ?? list.First;
                node = list.AddAfter(node, i);
            }

            //Log.Debug($"Player={player}  Marble={i}  List={list.ToJson()}!");
        }

        return _players.Values.OrderByDescending(x => x).FirstOrDefault();
    }
    #endregion

    #region Dump
    /// <inheritdoc />
    public override void DumpInput()
    {
        DumpData();
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (!Log.EnableDebug) return;

        Debug();

        _data.DumpCollection();
    }
    #endregion
}

#if DUMP
#endif