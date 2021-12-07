#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2020.Days;

public class Day11 : Day
{
    private Map<int> _source;
    private Map<int> _data;
    private int _width;
    private int _height;

    public Day11(ILog log, IParser parser) : base(log, parser)
    {
        EnableDebug();
        Initialize();
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Year = 2020;
        Parser.Day = 11;
        Parser.Type = ParserFileType.Real;

        var mapper = new Dictionary<char, int> { ['L'] = 0, ['.'] = -1 };
        _source = Parser.ParseMapInt(mapper);
        _width = _source.Width;
        _height = _source.Height;
    }

    /// <inheritdoc />
    protected override void ProcessData()
    {
    }

    /// <inheritdoc />
    public override void DumpInput()
    {
        DumpData();
    }

    protected override void ComputePart1()
    {
        Action fillSeats = FillSeats1;
        _data = _source.Clone();

        // var cycles = 5;
        // FillSeatsForCycles(fillSeats, cycles);
        // Result = CountSeats();

        FillSeatsUntilEquilibrium(fillSeats);

        //_data.DumpMap("Map");
    }

    protected override void ComputePart2()
    {
        Action fillSeats = FillSeats2;
        _data = _source.Clone();

        // var cycles = 5;
        // FillSeatsForCycles(fillSeats, cycles);
        // Result = CountSeats();

        FillSeatsUntilEquilibrium(fillSeats);

        //_data.DumpMap("Map");
    }

    private void FillSeatsForCycles(Action fillSeats, int cycles)
    {
        foreach (var _ in Enumerable.Range(0, cycles))
        {
            fillSeats();
        }
    }

    private void FillSeatsUntilEquilibrium(Action fillSeats)
    {
        var count = 0;
        do
        {
            fillSeats();
            var c = CountSeats();
            if (count == c)
            {
                Result = count;
                return;
            }

            count = c;
        } while (true);
    }

    private void FillSeats1()
    {
        var newSeats = _data.Clone();

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var sv = GetSeat(x, y);
                if (sv == -1)
                {
                    newSeats.Values[x, y] = -1;
                    continue;
                }

                if (IsSeatEncircled(x, y, 1, 4))
                {
                    newSeats.Values[x, y] = 0;
                    continue;
                }

                if (sv == 0 && !IsSeatEncircled(x, y, 1, 1))
                {
                    newSeats.Values[x, y] = 1;
                    continue;
                }
            }
        }

        _data = newSeats;
    }

    private void FillSeats2()
    {
        var newSeats = _data.Clone();

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var sv = GetSeat(x, y);
                if (sv == -1)
                {
                    newSeats.Values[x, y] = -1;
                    continue;
                }

                if (IsSeatEncircled(x, y, 0, 5))
                {
                    newSeats.Values[x, y] = 0;
                    continue;
                }

                if (sv == 0 && !IsSeatEncircled(x, y, 0, 1))
                {
                    newSeats.Values[x, y] = 1;
                    continue;
                }
            }
        }

        _data = newSeats;
    }

    private bool IsSeatEncircled(in int x, in int y, in int range = 0, in int target = 1)
    {
        var occupiedSeats = new List<bool>();

        if (range == 1)
        {
            occupiedSeats = new List<bool>
                {
                    IsOccupied(x - 1, y - 1),
                    IsOccupied(x - 1, y + 0),
                    IsOccupied(x - 1, y + 1),
                    IsOccupied(x + 0, y - 1),
                    IsOccupied(x + 0, y + 1),
                    IsOccupied(x + 1, y - 1),
                    IsOccupied(x + 1, y + 0),
                    IsOccupied(x + 1, y + 1),
                };
        }
        else
        {
            occupiedSeats = new List<bool>
                {
                    IsOccupiedDir(x, y, +0, -1),
                    IsOccupiedDir(x, y, +0, +1),
                    IsOccupiedDir(x, y, -1, +0),
                    IsOccupiedDir(x, y, +1, +0),
                    IsOccupiedDir(x, y, -1, -1),
                    IsOccupiedDir(x, y, +1, -1),
                    IsOccupiedDir(x, y, -1, +1),
                    IsOccupiedDir(x, y, +1, +1),
                };
        }

        return occupiedSeats.Count(s => s) >= target;
    }

    private bool IsOccupied(in int x, in int y)
    {
        return GetSeat(x, y) > 0;
    }

    private bool IsOccupiedDir(in int xx, in int yy, in int xdir, in int ydir)
    {
        var x = xx;
        var y = yy;

        for (x = xx + xdir, y = yy + ydir; (x >= 0 && y >= 0 && x < _width && y < _height); x = x + xdir, y = y + ydir)
        {
            var sv = GetSeat(x, y);
            if (sv == 1) return true;
            if (sv == 0) return false;
        }

        return false;
    }

    private int GetSeat(int x, int y)
    {
        if (x < 0 || y < 0 || x >= _width || y >= _height) return -1;

        return _data.Values[x, y];
    }

    private int CountSeats()
    {
        var r = 0;
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                r += (GetSeat(x, y) > 0).ToInt();
            }
        }

        return r;
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        Log.DebugLog(ClassId);

        //_data[0].Dump("Item");
        _data.Values.DumpMap("Map");
    }
}

#if DUMP
#endif