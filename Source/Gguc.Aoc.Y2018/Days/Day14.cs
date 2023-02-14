#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2018.Days;

public class Day14 : Day
{
    private const int YEAR = 2018;
    private const int DAY = 14;

    private List<string> _data;
    private List<int> _targetList;
    private int _targetValue;
    private string _targetString;

    public Day14(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "3138510102";
        Expected2 = "20179081";
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
        // Gromit do something!
        foreach (var line in _data)
        {
            _targetString = line;
            _targetValue = line.ToInt();
        }

        _targetList = new List<int> { 3, 7 };
    }
    #endregion

    #region Head
    protected override void ComputePart1()
    {
        Result = ImproveAfter(_targetValue);
    }

    protected override void ComputePart2()
    {
        Result = ImproveUntil(_targetString);
    }
    #endregion

    #region Body
    private long ImproveAfter(int target)
    {
        var recipes = _targetList.ToList();
        var a = 0;
        var b = 1;

        while (true)
        {
            Improve(ref recipes, ref a, ref b);

            if(recipes.Count >= target + 10) break;
        }

        var score = Score(recipes, target);
        Log.Info($"score={score}");

        return 0L;
    }
    private long ImproveUntil(string target)
    {
        var recipes = _targetList.ToList();
        var a = 0;
        var b = 1;
        var targetLast = target.Last().ToInt();

        while (true)
        {
            Improve(ref recipes, ref a, ref b);

            if (Contains(recipes, target, targetLast)) break;
        }

        Log.Info($"recipes.Count {recipes.Count}  targetValue {target}  length {target.Length}  targetLast {targetLast}");

        return Find(recipes, target);
    }

    private void Improve(ref List<int> recipes, ref int a, ref int b)
    {
        var temp = recipes;

        var v1 = temp[a];
        var v2 = temp[b];
        var sum = v1 + v2;
        temp.AddRange($"{sum}".Select(x => x.ToInt()));

        var length = temp.Count;
        a = (a + v1 + 1) % length;
        b = (b + v2 + 1) % length;

        recipes = temp;
    }

    private string Score(List<int> recipes, int skip)
    {
        var sb = new StringBuilder();
        recipes.Skip(skip).Take(10).ForEach(x => sb.Append(x));
        return sb.ToString();
    }

    private bool Contains(List<int> recipes, string target, int last)
    {
        var count = recipes.Count;
        if (last != recipes[count - 1] && last != recipes[count - 2]) return false;

        var sb = new StringBuilder();
        recipes.TakeLast(target.Length + 1).ForEach(x => sb.Append(x));
        return sb.ToString().Contains(target);
    }

    private int Find(List<int> recipes, string target)
    {
        var full = string.Join("", recipes);
        return full.IndexOf(target);
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
        _targetList.DumpCollection();
    }
    #endregion
}

#if DUMP
#endif