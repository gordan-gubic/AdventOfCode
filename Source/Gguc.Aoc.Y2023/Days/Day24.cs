#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2023.Days;

public class Day24 : Day
{
    private const int YEAR = 2023;
    private const int DAY = 24;

    private List<string> _data;
    private List<Hailstone> _hailstones;
    private (long, long) _testArea;

    public Day24(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "16727";
        Expected2 = "606772018765659";
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Test;
        Parser.Type = ParserFileType.Real;

        _data = Parser.Parse();

        _testArea = Parser.Type == ParserFileType.Test ? (7L, 27L) : (200000000000000L, 400000000000000L);
        // _testArea = (200000000000000L, 400000000000000L);
    }

    /// <inheritdoc />
    public override void DumpInput()
    {
        DumpData();
    }

    protected override void ComputePart1()
    {
        var result = CountIntersections();

        Result = result;
    }

    protected override void ComputePart2()
    {
        var result = CalculateRock();

        Result = result;
    }

    private long CountIntersections()
    {
        var total = 0L;
        var sum = 0L;

        for (var i = 0; i < _hailstones.Count - 1; i++)
        {
            for (var j = i + 1; j < _hailstones.Count; j++)
            {
                total++;

                var ha = _hailstones[i];
                var hb = _hailstones[j];

                var intersection = IntersectionAt(ha, hb);
                var test = Verify(intersection, ha, hb, _testArea);

                // Log.Debug($"intersection=[{intersection}]  test=[{test}]"); //  [{ha.ToJson()}] [{hb.ToJson()}]

                if (test) sum++;
            }
        }

        Log.Debug($"total=[{total}]  sum=[{sum}]");

        return sum;
    }

    private long CalculateRock()
    {
        var total = 0L;
        var sum = 0L;
        var size = _hailstones.Count;
        size = 5;

        var matrixXY = new List<List<long>>();
        var matrixXZ = new List<List<long>>();

        "(dy'-dy) X + (dx-dx') Y + (y-y') DX + (x'-x) DY = x' dy' - y' dx' - x dy + y dx".Dump();
        " -------- ".Dump();

        for (var i = 0; i < size - 1; i++)
        {
            for (var j = i + 1; j < size; j++)
            {
                var ha = _hailstones[i];
                var hb = _hailstones[j];

                /*
                //  (dy'-dy) X + (dx-dx') Y + (y-y') DX + (x'-x) DY = x' dy' - y' dx' - x dy + y dx
                var eq1 = $"({hb.VY}-{ha.VY}) X + ({ha.VX}-{hb.VX}) Y + ({ha.Y}-{hb.Y}) DX + ({hb.X}-{ha.X}) DY = {hb.X} {hb.VY} - {hb.Y} {hb.VX} - {ha.X} {ha.VY} + {ha.Y} {ha.VX}";
                var eq2 = $"({hb.VY-ha.VY}) X + ({ha.VX-hb.VX}) Y + ({ha.Y-hb.Y}) DX + ({hb.X-ha.X}) DY = {hb.X*hb.VY-hb.Y*hb.VX-ha.X*ha.VY+ha.Y*ha.VX}";
                var eq3 = $"{hb.VY-ha.VY}\t{ha.VX-hb.VX}\t{ha.Y-hb.Y}\t{hb.X-ha.X}\t{hb.X*hb.VY-hb.Y*hb.VX-ha.X*ha.VY+ha.Y*ha.VX}";
                */

                var array = new List<long>
                {
                    (hb.VY-ha.VY),
                    (ha.VX-hb.VX),
                    (ha.Y-hb.Y),
                    (hb.X-ha.X),
                    (hb.X*hb.VY-hb.Y*hb.VX-ha.X*ha.VY+ha.Y*ha.VX),
                };
                matrixXY.Add(array);

                array = new List<long>
                {
                    (hb.VZ-ha.VZ),
                    (ha.VX-hb.VX),
                    (ha.Z-hb.Z),
                    (hb.X-ha.X),
                    (hb.X*hb.VZ-hb.Z*hb.VX-ha.X*ha.VZ+ha.Z*ha.VX),
                };
                matrixXZ.Add(array);
            }

            break;
        }

        matrixXY.Swap(1, 2);
        matrixXY.DumpJson();

        matrixXZ.Swap(1, 2);
        matrixXZ.Swap(0, 1);
        matrixXZ.DumpJson();

        var rock = new Hailstone();

        var g = new GaussianElimination();
        g.SetMatrix(matrixXY);
        g.Calculate();

        var rc = g.ResultConverted;
        rock.X = rc[0];
        rock.Y = rc[1];
        rock.VX = rc[2];
        rock.VY = rc[3];

        g.SetMatrix(matrixXZ);
        g.Calculate();

        rc = g.ResultConverted;
        rock.Z = rc[1];
        rock.VZ = rc[3];

        rock.DumpJson("rock");

        sum = rock.X + rock.Y + rock.Z;
        Log.Debug($" sum=[{sum}]");

        return sum;
    }

    private (double, double) IntersectionAt(Hailstone ha, Hailstone hb)
    {
        var s1 = (((double)hb.Y - ha.Y) / (double)ha.VY) - (((double)hb.X - ha.X) / (double)ha.VX);
        var s2 = ((double)hb.VX / ha.VX) - ((double)hb.VY / ha.VY);
        var s3 = s1 * (1 / s2);

        var t1 = (((double)hb.X - ha.X) / (double)ha.VX) + s3 * ((double)hb.VX / (double)ha.VX);
        // var t2 = (((double)hb.Y - ha.Y) / ha.VY) + s3 * ((double)hb.VY / ha.VY);

        var ax = (double)ha.X + (t1 * (double)ha.VX);
        // var bx = (double)hb.X + (s3 * hb.VX);

        var ay = (double)ha.Y + (t1 * (double)ha.VY);
        // var by = (double)hb.Y + (s3 * hb.VY);

        // s1.Dump("s1");
        // s2.Dump("s2");
        // s3.Dump("s3");
        // t1.Dump("t1");
        // t2.Dump("t2");
        // ax.Dump("ax");
        // bx.Dump("bx");
        // ay.Dump("ay");
        // by.Dump("by");

        return (ax, ay);
    }

    private bool Verify((double x, double y) intersection, Hailstone a, Hailstone b, (long min, long max) test)
    {
        var dirAX = a.VX / Math.Abs(a.VX);
        var dirBX = b.VX / Math.Abs(b.VX);
        var dirAY = a.VY / Math.Abs(a.VY);
        var dirBY = b.VY / Math.Abs(b.VY);

        var validAX = (dirAX > 0) && (intersection.x > a.X) || (dirAX < 0) && (intersection.x < a.X);
        var validBX = (dirBX > 0) && (intersection.x > b.X) || (dirBX < 0) && (intersection.x < b.X);
        var validAY = (dirAY > 0) && (intersection.y > a.Y) || (dirAY < 0) && (intersection.y < a.Y);
        var validBY = (dirBY > 0) && (intersection.y > b.Y) || (dirBY < 0) && (intersection.y < b.Y);

        return validAX && validBX && validAY && validBY 
               && intersection.x >= test.min && intersection.x <= test.max 
               && intersection.y >= test.min && intersection.y <= test.max;
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        var hailstones = new List<Hailstone>();
        foreach (var line in _data)
        {
            var parts = line.Split(new[] { ' ', ',', '@' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.ToLong()).ToList();

            var hailstone = new Hailstone
            {
                X = parts[0],
                Y = parts[1],
                Z = parts[2],
                VX = parts[3],
                VY = parts[4],
                VZ = parts[5],
            };
            hailstones.Add(hailstone);
        }

        _hailstones = hailstones;
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
        // _hailstones.DumpJson();
        _testArea.Dump();
    }
}

#if DUMP
#endif