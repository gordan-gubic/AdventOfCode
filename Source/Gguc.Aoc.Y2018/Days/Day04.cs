#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2018.Days;

public class Day04 : Day
{
    private const int YEAR = 2018;
    private const int DAY = 04;

    private List<string> _data;
    private List<(DateTime, string)> _events;
    private Dictionary<int, List<(DateTime, string)>> _guards;
    private Dictionary<int, long> _sleeps;
    private Dictionary<int, int> _minutes;

    public Day04(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "35184";
        Expected2 = "37886";
    }

    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Example;
        Parser.Type = ParserFileType.Test;
        Parser.Type = ParserFileType.Real;

        _data = Parser.Parse();
    }

    #region Parse
    protected override void ProcessData()
    {
        _events = new();
        _guards = new();

        var format = "yyyy-MM-dd HH:mm";

        // Gromit do something!
        foreach (var line in _data)
        {
            var parts = line.Split('[', ']');
            var datetime = parts[1].ToDateTime(format);
            var text = parts[2].Trim();
            _events.Add((datetime, text));
        }

        _events.Sort();
        // _events.DumpCollection();

        var pattern = @"Guard #(\d+) begins shift";
        var current = 0;

        foreach (var (datetime, text) in _events)
        {
            if (text.StartsWith("Guard"))
            {
                current = text.RegexValue(pattern).ToInt();
            }

            if (!_guards.ContainsKey(current)) _guards[current] = new();

            _guards[current].Add((datetime, text));
        }

        // _guards.DumpJson();

        _minutes = new ();
        var range = Enumerable.Range(0, 60);
        foreach (var i in range)
        {
            _minutes[i] = 0;
        }
    }
    #endregion Parse

    #region Head
    protected override void ComputePart1()
    {
        ProcessGuards();

        Result = ProcessGuardMaxSleep();
    }

    protected override void ComputePart2()
    {
        Result = ProcessGuardAny();
    }
    #endregion

    #region Body
    private void ProcessGuards()
    {
        _sleeps = new ();
        var current = default(DateTime);

        foreach (var (guard, info) in _guards)
        {
            var sum = 0L;

            foreach (var (datetime, text) in info)
            {
                if (text.StartsWith("Guard"))
                {
                    current = datetime;
                }
                else if (text == "falls asleep")
                {
                    current = datetime;
                }
                else if (text == "wakes up")
                {
                    sum += (long)(datetime - current).TotalMinutes;
                    current = datetime;
                }
            }

            _sleeps[guard] = sum;
            // Log.Debug($"Guard={guard}  Sum={sum}");
        }
    }

    private long ProcessGuardMaxSleep()
    {
        var target = _sleeps.OrderByDescending(x => x.Value).FirstOrDefault().Key;
        var minute = ProcessGuard(target);

        return target * minute.Item1;
    }


    private long ProcessGuardAny()
    {
        var targets = _sleeps.Keys;
        var target = 0;
        var minute = 0;
        var max = 0;

        foreach (var current in targets)
        {
            var (min, times) = ProcessGuard(current);

            if (times > max)
            {
                max = times;
                target = current;
                minute = min;
            }
        }

        return target * minute;
    }

    private (int, int) ProcessGuard(int guard)
    {
        Clear();

        var info = _guards[guard];
        var current = default(DateTime);

        foreach (var (datetime, text) in info)
        {
            if (text.StartsWith("Guard"))
            {
                current = datetime;
            }
            else if (text == "falls asleep")
            {
                current = datetime;
            }
            else if (text == "wakes up")
            {
                var total = (int)(datetime - current).TotalMinutes;
                var start = current.Minute;

                var range2 = Enumerable.Range(start, total);
                foreach (var i in range2)
                {
                    _minutes[i]++;
                }

                current = datetime;
            }
        }

        var minute = _minutes.OrderByDescending(x => x.Value).First();
        // Log.Debug($"Guard={guard}  minute={minute.Key}  times={_minutes[minute.Value]}");
        return (minute.Key, minute.Value);
    }

    private void Clear()
    {
        foreach (var k in _minutes.Keys)
        {
            _minutes[k] = 0;
        }
    }

    #endregion

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

        // _data[0].Dump("Item");
        // _data.DumpCollection("List");
    }
    #endregion Dump
}

#if DROP

#endif