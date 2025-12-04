#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2024.Days;

public class Day13 : Day
{
    private const int YEAR = 2024;
    private const int DAY = 13;

    private List<string> _data;

    private List<Arcade> _arcades;

    public Day13(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        ExpectedProd1 = "36838";
        ExpectedProd2 = "83029436920891";
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
        Result = CalculateArcades1();
    }

    protected override void ComputePart2()
    {
        Result = CalculateArcades2();
    }

    private long CalculateArcades1()
    {
        var sum = 0L;

        foreach (var arcade in _arcades)
        {
            sum += CalculateArcadeMatrix(arcade);
        }

        return sum;
    }

    private long CalculateArcades2()
    {
        var sum = 0L;

        foreach (var arcade in _arcades)
        {
            arcade.PrizeX += 10000000000000;
            arcade.PrizeY += 10000000000000;
            sum += CalculateArcadeMatrix(arcade);
        }

        return sum;
    }

    private long CalculateArcadeForce(Arcade arcade)
    {
        var sum = 0L;

        var minX = Math.Max(arcade.PrizeX / arcade.Button1X, arcade.PrizeX / arcade.Button2X);
        var minY = Math.Max(arcade.PrizeY / arcade.Button1Y, arcade.PrizeY / arcade.Button2Y);
        var minZ = Math.Max(minX, minY);

        for (var i = 1; i <= minZ; i++)
        {
            var x = arcade.Button1X * i;
            if ((arcade.PrizeX - x) % arcade.Button2X != 0) continue;

            var y = arcade.Button1Y * i;
            if ((arcade.PrizeY - y) % arcade.Button2Y != 0) continue;

            var j1 = (arcade.PrizeX - x) / arcade.Button2X;
            var j2 = (arcade.PrizeY - y) / arcade.Button2Y;

            if (j1 != j2) continue;

            // $"i={i}::j={j1}::sum={i * 3 + j1}".Dump(); //--DUMP--

            return i * 3 + j1;
        }

        return sum;
    }

    private long CalculateArcadeMatrix(Arcade arcade)
    {
        var sum = 0L;

        var ax = arcade.Button1X;
        var bx = arcade.Button2X;

        var ay = arcade.Button1Y;
        var by = arcade.Button2Y;

        var xE = arcade.PrizeX;
        var yE = arcade.PrizeY;

        // -- matrix
        // ax | bx | xE
        // ay | by | yE
        // ------------
        //  1 |  0 |  X
        //  0 |  1 |  Y

        var m1 = (ax, bx, xE);
        var m2 = (ay, by, yE);

        // -- get X --
        var x = 0.0;
        {
            var fx = -by;
            var fy = bx;
            var mx = (ax * fx, bx * fx, xE * fx);
            var my = (ay * fy, by * fy, yE * fy);
            var ms = (mx.Item1 + my.Item1, mx.Item2 + my.Item2, mx.Item3 + my.Item3);
            x = (double)ms.Item3 / ms.Item1;

            if (!x.IsRound()) return 0L;
        }

        // -- get Y --
        var y = 0.0;
        {
            var fx = -ay;
            var fy = ax;
            var mx = (ax * fx, bx * fx, xE * fx);
            var my = (ay * fy, by * fy, yE * fy);
            var ms = (mx.Item1 + my.Item1, mx.Item2 + my.Item2, mx.Item3 + my.Item3);
            y = (double)ms.Item3 / ms.Item2;

            if (!y.IsRound()) return 0L;
        }
      
        sum = (long)(3 * x + y);

        // $"|{arcade.Button1X}|{arcade.Button2X}|{arcade.PrizeX}| => x=|{x}|".Dump(); //--DUMP--
        // $"|{arcade.Button1Y}|{arcade.Button2Y}|{arcade.PrizeY}| => y=|{y}|".Dump(); //--DUMP--
        // $"x=|{x}| *  y=|{y}| == [{x * y}] = [{sum}]".Dump(); //--DUMP--

        return sum;
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        _arcades = new List<Arcade>();
        for (int i = 0; i < _data.Count; i += 4)
        {
            var arcade = new Arcade();
            ProcessButton1(arcade, _data[i]);
            ProcessButton2(arcade, _data[i + 1]);
            ProcessPrize(arcade, _data[i + 2]);

            _arcades.Add(arcade);
        }
    }

    private void ProcessButton1(Arcade arcade, string s)
    {
        var pattern = @"Button .: X\+(\d+), Y\+(\d+)";
        var m = s.MatchAll(pattern).ToList();
        arcade.Button1X = m[1].ToInt();
        arcade.Button1Y = m[2].ToInt();
    }

    private void ProcessButton2(Arcade arcade, string s)
    {
        var pattern = @"Button .: X\+(\d+), Y\+(\d+)";
        var m = s.MatchAll(pattern).ToList();
        arcade.Button2X = m[1].ToInt();
        arcade.Button2Y = m[2].ToInt();
    }

    private void ProcessPrize(Arcade arcade, string s)
    {
        var pattern = @"Prize: X=(\d+), Y=(\d+)";
        var m = s.MatchAll(pattern).ToList();
        arcade.PrizeX = m[1].ToInt();
        arcade.PrizeY = m[2].ToInt();
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
        // _arcades.DumpCollection();
    }
}

#if DUMP
#endif