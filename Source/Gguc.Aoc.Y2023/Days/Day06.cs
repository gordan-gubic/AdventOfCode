#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2023.Days;

public class Day06 : Day
{
    private const int YEAR = 2023;
    private const int DAY = 06;

    private List<string> _data;
    private List<int> _times;
    private List<int> _distances;

    public Day06(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "840336";
        Expected2 = "41382569";
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
        var result = CountWins();

        Result = result;
    }

    protected override void ComputePart2()
    {
        var result = CountWinsOneRace();

        Result = result;
    }

    private long CountWins()
    {
        var sum = 1L;

        for (var i = 0; i < _times.Count; i++)
        {
            // sum *= NumberOfFinishes(i);
            sum *= NumberOfFinishesAlgo(_times[i], _distances[i]);
        }

        return sum;
    }

    private long CountWinsOneRace()
    {
        var time = string.Join("", _times).ToLong();
        var distance = string.Join("", _distances).ToLong();

        time.Dump();
        distance.Dump();

        var count = 0L;

        count = NumberOfFinishesAlgo(time, distance);

        return count;
    }

    private long NumberOfFinishesAlgo(long time, long distance)
    {
        var half = time / 2;
        var end = CountDistance(time, half);

        end.Dump("end");
        if(end <= distance) return 0L;

        var currentWin = half;
        var currentFail = 0L;
        var temp = half;
        var reach = 0L;
        
        while (true)
        {
            reach = CountDistance(time, temp);

            if (reach > distance)
            {
                currentWin = temp;
                temp = (currentWin - currentFail) / 2 + currentFail;
            }
            else
            {
                currentFail = temp;
                temp = (currentWin - currentFail) / 2 + currentFail;
            }

            if (currentWin - currentFail == 1) break;
        }

        return (half - currentWin) * 2 + (1 + time % 2);
    }

    private long NumberOfFinishes(int index)
    {
        /*
         * Brute-Force algo
         */

        var races = new Dictionary<long, long>();

        for(var i = 1; i < _times[index]; i++)
        {
            races[i] = CountDistance(_times[index], i);
        }

        var count = races.Count(x => x.Value > _distances[index]);

        return count;
    }

    private long CountDistance(long total, long index)
    {
        return index * (total - index);
    }

    protected override void ProcessData()
    {
        /*
        Time:      7  15   30
        Distance:  9  40  200
         */

        base.ProcessData();

        _times = _data[0].Replace("Time:", "").Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => x.ToInt()).ToList();
        _distances = _data[1].Replace("Distance:", "").Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => x.ToInt()).ToList();
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

        //_data.DumpCollection();
        _times.DumpJson();
        _distances.DumpJson();
    }
}

#if DUMP
#endif