#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2024.Days;

public class Day11 : Day
{
    private const int YEAR = 2024;
    private const int DAY = 11;

    private const int Blinks = 25;

    private List<string> _data;
    private List<long> _stones;

    // memory
    private Dictionary<int, Dictionary<long, long>> _memory = new();
    private Dictionary<long, long> _memory25 = new();
    private Dictionary<(long, int), long> _cache = new();

    public Day11(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "233875";
        Expected2 = "277444936413293";
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
        SetMemory();

        Result = CountStones();
    }

    protected override void ComputePart2()
    {
        SetMemory();

        Result = CountStones2();
    }

    private void SetMemory()
    {
        _memory.Clear();
        _memory25.Clear();
        _cache.Clear();

        for (var i = 1; i <= 25; i++)
        {
            _memory[i] = new();
        }
    }

    private long CountStones()
    {
        var sum = 0L;

        foreach (var stone in _stones)
        {
            sum += ProcessStone(stone);
        }

        return sum;
    }

    private long CountStones2()
    {
        var sum = 0L;

        // FillCache();

        foreach (var stone in _stones)
        {
            sum += ProcessStone2(stone, 75);
        }

        return sum;
    }

    private void FillCache()
    {
        var values = new List<int>();
        for (var i = 0; i < 10; i++) values.Add(i);

        for (var b = 1; b < 75; b++)
        {
            for (var i = 0; i < 10; i++)
            {
                var sum = ProcessStone2(i, b);
                _cache[(i, b)] = sum;

                // $"Cache: [{i}, {b}] = [{sum}]".Dump();
            }
        }
    }

    private long ProcessStone(long stone)
    {
        var blinks = Blinks;
        var sum = 0L;

        var queue1 = new Queue<long>();
        var queue2 = new Queue<long>();

        var i = 1;
        queue1.Enqueue(stone);

        while (true)
        {
            var r = queue1.TryDequeue(out var s);
            if (!r)
            {
                i++;
                if (i > blinks)
                {
                    sum += queue2.Count;
                    break;
                }

                queue1 = queue2;
                queue2 = new();
                continue;
            }

            var stones = SplitStone(s);
            stones.ForEach(x => queue2.Enqueue(x));
        }

        return sum;
    }

    private long ProcessStone2(long stone, int blinks = Blinks)
    {
        var sum = 0L;

        var queue1 = new Queue<(long, int)>();

        queue1.Enqueue((stone, blinks));

        while (true)
        {
            var r = queue1.TryDequeue(out var sx);
            if (!r) break;

            var s = sx.Item1;
            sum++;
            // sum += sx.Item2;
            // (sx).Dump();

            for (var i = sx.Item2; i >= 1; i--)
            {
                var stones = SplitStone(s);
                if (stones.Count > 1)
                {
                    var x = (stones[1], i - 1);

                    if (_cache.ContainsKey(x)) sum += _cache[x];
                    // else queue1.Enqueue((stones[1], i - 1));
                    else
                    {
                        var ps2 = ProcessStone2(x.Item1, x.Item2);
                        _cache[x] = ps2;
                        sum += ps2;
                    }
                }
                s = stones[0];
            }
        }

        return sum;
    }

    private List<long> SplitStone(long stone)
    {
        var stones = new List<long>();
        var sText = $"{stone}";
        var sSize = sText.Length;

        if (stone == 0)
        {
            stones.Add(1);
        }
        else if (sSize % 2 == 0)
        {
            stones.Add(sText[0..(sSize / 2)].ToLong());
            stones.Add(sText[(sSize / 2)..].ToLong());
        }
        else
        {
            stones.Add(stone * 2024);
        }

        return stones;
    }

    private void AddToMemory(long stone, int stonesCount)
    {
        _memory25[stone] = stonesCount;
    }

    private void AddToMemory(int i, long stone, int stonesCount)
    {
        var dict = _memory[i];

        if (!dict.ContainsKey(stone)) dict[stone] = new();
        dict[stone] = stonesCount;
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        // Gromit do something!
        foreach (var line in _data)
        {
            _stones = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => x.ToLong()).ToList();
        }
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (!Log.EnableDebug) return;

        Debug();

        _data.DumpCollection();
    }
}

#if DUMP
#endif