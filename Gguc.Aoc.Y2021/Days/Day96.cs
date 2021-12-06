#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Core.Templates;

public class Day96 : Day
{
    private const int YEAR = 2021;
    private const int DAY = 96;
    private List<List<int>> _source;
    private List<int> _data;

    public Day96(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Real;

        _source = Parser.Parse(ConvertInput);
    }

    /// <inheritdoc />
    protected override void ProcessData()
    {
        _data = _source[0];
    }

    protected override void ComputePart1()
    {
        var lanterns = _data.ToList();

        AdvanceDays(lanterns, 256);

        lanterns.Dump();

        Result = lanterns.Count;
    }

    protected override void ComputePart2()
    {
    }

    private void AdvanceDays(List<int> lanterns, int days)
    {
        for (int i = 0; i < days; i++)
        {
            AdvanceDay(lanterns);

            // lanterns.DumpJson("day");
        }
    }

    private void AdvanceDay(List<int> lanterns)
    {
        var count = lanterns.Count;

        for (int i = 0; i < count; i++)
        {
            var result = ProcessLantern(lanterns[i]);

            lanterns[i] = result.Item1;

            if (result.Item2) lanterns.Add(8);
        }
    }

    private (int, bool) ProcessLantern(int item)
    {
        var newLantern = item == 0 ? true : false;

        var result = item switch
        {
            0 => 6,
            _ => item - 1,
        };

        return (result, newLantern);
    }

    private List<int> ConvertInput(string input)
    {
        return input.Split(',').Select(x => x.ToInt()).ToList();
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

        _data.DumpJson("List");
    }
    #endregion Dump
}

#if DUMP

#endif