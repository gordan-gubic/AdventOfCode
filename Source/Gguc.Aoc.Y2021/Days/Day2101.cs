#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Core.Templates;

public class Day2101 : Day
{
    private const int YEAR = 2021;
    private const int DAY = 2101;

    private List<string> _source;
    private int _player1;
    private int _player2;
    private DiracGame _game;

    public Day2101(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
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
        _player1 = _source[0].ToArray().LastOrDefault().ToInt();
        _player2 = _source[1].ToArray().LastOrDefault().ToInt();
    }
    #endregion Parse

    protected override void ComputePart2()
    {
    }

    protected override void ComputePart1()
    {
        _game = new DiracGame();
        _game.Player1Position = _player1 - 1;
        _game.Player2Position = _player2 - 1;

        PlayGame();

        _game.DumpJson("Game");

        var min = Math.Min(_game.Player1Score, _game.Player2Score);
        Result = min * _game.DiceCount;
    }

    private void PlayGame()
    {
        while (!NextTurn())
        {
        }
    }

    private bool NextTurn()
    {
        var dice = SumDices(3);

        MovePlayer(1, dice);
        if (_game.Player1Score >= 1000) return true;

        dice = SumDices(3);

        MovePlayer(2, dice);
        if (_game.Player2Score >= 1000) return true;

        return false;
    }

    private void MovePlayer(int player, int dice)
    {
        var pos = (player == 1) ? _game.Player1Position : _game.Player2Position;
        var newPos = (pos + dice) % 10;

        if(player == 1)
        {
            _game.Player1Score += newPos + 1;
            _game.Player1Position = newPos;
        }
        else
        {
            _game.Player2Score += newPos + 1;
            _game.Player2Position = newPos;
        }
    }

    private int SumDices(int n)
    {
        var sum = 0;
        for (var i = 0; i < n; i++)
            sum += Roll();

        return sum;
    }

    private int Roll()
    {
        var r = _game.Dice + 1;
        _game.Dice++;
        _game.DiceCount++;
        return r;
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