#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2018.Days;

using System.Drawing;

public class Day12 : Day
{
    private const int YEAR = 2018;
    private const int DAY = 12;

    private List<string> _data;
    private string _initial;
    private List<bool> _initialList;
    private Dictionary<string, char> _rules;
    private Dictionary<int, string> _buffer;
    private Dictionary<string, int> _cache;

    public Day12(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "2571";
        Expected2 = "3100000000655";
    }

    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Real;
        Parser.Type = ParserFileType.Test;

        _data = Parser.Parse();
    }

    #region Parse
    protected override void ProcessData()
    {
        var initial = _data.FirstOrDefault();
        initial = initial.Replace("initial state: ", "");

        var lines = _data.ToList();
        lines.RemoveAt(0);
        lines.RemoveAt(0);

        _initial = initial;
        _initialList = initial.ToBinaryList('#');
        _rules = new();

        _buffer = new();
        _cache = new();

        // Gromit do something!
        foreach (var line in lines)
        {
            var line1 = line.Replace(" => ", " ").Split(' ');
            var key = line1.First();
            var ch = line1.Last().Last();

            _rules[key] = ch;
        }
    }
    #endregion

    #region Head
    protected override void ComputePart1()
    {
        Result = ProcessPlants(20);
    }

    protected override void ComputePart2()
    {
        Result = ProcessPlantsRepeat(50000000000L);
    }
    #endregion

    #region Body
    private long ProcessPlants(int gen)
    {
        var state = _initial;

        for (var i = 0; i < gen; i++)
        {
            state = ProcessGen(state);
            // Log.Debug($"gen={i}  state={state}  sum={sum}");
        }

        return SumPlants(state, gen);
    }

    private long ProcessPlantsRepeat(long limit)
    {
        var state = _initial;
        var gen = 0;
        _buffer[0] = state;

        while (true)
        {
            gen++;
            state = ProcessGen(state);

            var (isx, genx) = FindRepetition(state, gen, 5);
            if (isx)
            {
                break;
            }

            if (gen > 1000) break;
        }

        var current = gen;
        var currentSum = SumPlants(state, current);
        state = ProcessGen(state);

        var next = current + 1;
        var nextSum = SumPlants(state, next);
        var diff = nextSum - currentSum;

        Log.Info($"Repeat!  current={current}, {currentSum}  next={next}, {nextSum}  diff={diff}");

        var iterations = limit - current;
        var iterationsSum = iterations * diff;
        var totalSum = iterationsSum + currentSum;

        Log.Info($"Repeat!  iterations={iterations}, {iterationsSum}  totalSum={totalSum}");

        return totalSum;
    }

    private string ProcessGen(string state)
    {
        state = $"....{state}....";
        var newState = new StringBuilder();

        for (var i = 0; i < state.Length - 4; i++)
        {
            var key = state.Substring(i, 5);
            newState.Append(GetRule(key));
        }

        state = newState.ToString();
        state = state.TrimEnd('.');
        return state;
    }

    private (bool, int) FindRepetition(string state, int gen, int size)
    {
        state = state.Trim('.');

        for (var i = gen - size; i < gen; i++)
        {
            if (i < 0) continue;

            if (!_buffer.ContainsKey(i)) _buffer[i] = "";

            _buffer[i] += state;
        }

        var diff = gen - size;
        if (_buffer.ContainsKey(diff))
        {
            var value = _buffer[diff];

            if (_cache.ContainsKey(value))
            {
                var key = _cache[value];
                Log.Warn($"Repeat!  state={state}  gen={gen}  size={size}  key={key}");
                return (true, key);
            }

            _cache[value] = diff;
            _buffer.Remove(diff);
        }

        return (false, -1);
    }

    private long SumPlants(string state, int gen)
    {
        var list = state.ToBinaryList('#');
        var offset = gen * 2;

        var indexes = new List<int>();

        for (var i = 0; i < list.Count; i++)
        {
            if (list[i]) indexes.Add(i - offset);
        }

        return indexes.Sum();
    }

    private char GetRule(string key)
    {
        return _rules.ContainsKey(key) ? _rules[key] : '.';
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

        // _data.DumpCollection();
        //_rules.DumpCollection();
    }
    #endregion
}

#if DUMP
#endif