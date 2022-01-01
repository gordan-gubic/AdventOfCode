#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2021.Days;

public class Day2102 : Day
{
    private const int YEAR = 2021;
    private const int DAY = 2102;

    private const int TargetPoints = 21;

    private List<string> _source;
    private int _player1;
    private int _player2;

    private Dictionary<DiracGame, long> _cache = new Dictionary<DiracGame, long>();

    public Day2102(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();
    }

    #region Parse
    protected override void InitParser()
    {
        Parser.Day = 21;
        Parser.Type = ParserFileType.Real;

        _source = Parser.Parse();
    }

    protected override void ProcessData()
    {
        _player1 = _source[0].ToArray().LastOrDefault().ToInt() - 1;
        _player2 = _source[1].ToArray().LastOrDefault().ToInt() - 1;
    }
    #endregion Parse

    protected override void ComputePart1()
    {
    }

    protected override void ComputePart2()
    {
        _cache.Clear();

        var player1Game = new DiracGame { Dice = 0, Player1Position = _player1, Player2Position = _player2, IsPlayer1Turn = true };
        var player2Game = new DiracGame { Dice = 0, Player1Position = _player2, Player2Position = _player1, IsPlayer1Turn = false };

        var winner1 = CalcWinner(player1Game, true);
        var winner2 = CalcWinner(player2Game, true);

        var min = Math.Min(winner1, winner2);
        var max = Math.Max(winner1, winner2);

        min.Dump();
        max.Dump();

        Result = max;
    }

    private long CalcWinner(DiracGame game, bool firstTurn = false)
    {
        if(_cache.ContainsKey(game)) return _cache[game];

        var sum = 0L;

        var p1pos = game.Player1Position;
        var p2pos = game.Player2Position;
        var p1score = game.Player1Score;
        var p2score = game.Player2Score;
        var p1turn = game.IsPlayer1Turn;

        if (!firstTurn)
        {
            if (game.IsPlayer1Turn)
            {
                p1pos += game.Dice;
                p1pos = p1pos % 10;
                p1score += p1pos + 1;

                if (p1score >= TargetPoints) return 1L;
            }
            else
            {
                p2pos += game.Dice;
                p2pos = p2pos % 10;
                p2score += p2pos + 1;

                if (p2score >= TargetPoints) return 0L;
            }

            p1turn = !p1turn;
        }

        // Spawn games...

        var gameDice3 = CreateGame(3, p1pos, p2pos, p1score, p2score, p1turn);
        var gameDice4 = CreateGame(4, p1pos, p2pos, p1score, p2score, p1turn);
        var gameDice5 = CreateGame(5, p1pos, p2pos, p1score, p2score, p1turn);
        var gameDice6 = CreateGame(6, p1pos, p2pos, p1score, p2score, p1turn);
        var gameDice7 = CreateGame(7, p1pos, p2pos, p1score, p2score, p1turn);
        var gameDice8 = CreateGame(8, p1pos, p2pos, p1score, p2score, p1turn);
        var gameDice9 = CreateGame(9, p1pos, p2pos, p1score, p2score, p1turn);

        sum += CalcWinner(gameDice3) * 1L;
        sum += CalcWinner(gameDice4) * 3L;
        sum += CalcWinner(gameDice5) * 6L;
        sum += CalcWinner(gameDice6) * 7L;
        sum += CalcWinner(gameDice7) * 6L;
        sum += CalcWinner(gameDice8) * 3L;
        sum += CalcWinner(gameDice9) * 1L;

        _cache[game] = sum;

        return sum;
    }

    private DiracGame CreateGame(int dice, int p1pos, int p2pos, int p1score, int p2score, bool p1turn)
    {
        return new DiracGame
        {
            Dice = dice,
            Player1Position = p1pos,
            Player2Position = p2pos,
            Player1Score = p1score,
            Player2Score = p2score,
            IsPlayer1Turn = p1turn,
        };
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

        _player1.Dump("_player1");
        _player2.Dump("_player2");
    }
    #endregion Dump
}

#if DUMP

#endif