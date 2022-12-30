#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2018.Days;

public class Day05 : Day
{
    private const int YEAR = 2018;
    private const int DAY = 05;

    private List<string> _data;
    private string _chars;

    public Day05(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "9822";
        Expected2 = "5726";
    }

    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Test;
        Parser.Type = ParserFileType.Real;

        _data = Parser.Parse();
    }

    #region Parse
    /// <inheritdoc />
    protected override void ProcessData()
    {
        _chars = _data[0];
    }
    #endregion

    #region Head
    protected override void ComputePart1()
    {
        Result = ProcessPolymer();
    }

    protected override void ComputePart2()
    {
        Result = long.MaxValue;
        ProcessPolymers();
    }
    #endregion

    #region Body
    private long ProcessPolymer()
    {
        return ProcessChars(_chars);
    }

    private void ProcessPolymers()
    {
        var range = Enumerable.Range('a', 26).Select(x => (char)x);
        foreach (var c in range)
        {
            var c1 = $"{c}";
            var c2 = c1.ToUpper();

            var text = _chars.Replace(c1, "").Replace(c2, "");
            var value = ProcessChars(text);
            Min(value, true);
        }
    }

    private long ProcessChars(string text)
    {
        var list = text.ToCharArray().ToList();

        while (true)
        {
            var reduced = false;
            var length = list.Count;

            for (var i = length - 2; i >= 0; i--)
            {
                var x = list[i];
                var y = list[i + 1];

                if (char.ToUpper(x) == char.ToUpper(y) && x != y)
                {
                    list.RemoveAt(i);
                    list.RemoveAt(i);
                    reduced = true;
                    i--;
                }
            }

            if (!reduced) break;
        }

        // list.DumpJson();
        return list.Count;
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
        // _chars.DumpJson();
    }
    #endregion
}

#if DUMP
#endif