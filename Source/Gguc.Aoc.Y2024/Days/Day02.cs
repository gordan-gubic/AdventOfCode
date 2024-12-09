#define LOGx
#define STOPWATCH

namespace Gguc.Aoc.Y2024.Days;

public class Day02 : Day
{
    private const int YEAR = 2024;
    private const int DAY = 02;

    private List<string> _data;
    private List<List<int>> _reports;

    public Day02(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "269";
        Expected2 = "337";
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
        Result = CalculateSafeReports();
    }

    protected override void ComputePart2()
    {
        Result = CalculateSuperSafeReports();
    }

    private int CalculateSafeReports()
    {
        var safe = 0;

        foreach (var report in _reports)
        {
            var isSafe = ProcessReport(report);
            if (isSafe) safe++;
        }

        return safe;
    }

    private int CalculateSuperSafeReports()
    {
        var safe = 0;

        foreach (var report in _reports)
        {
            var isSafe = ProcessReport(report);

            if(!isSafe) isSafe = ProcessReportDeep(report);

            if (isSafe) safe++;
        }

        return safe;
    }

    private bool ProcessReport(List<int> report)
    {
        if (report[1] == report[0]) return false;

        var ascending = report[1] > report[0];

        for (var i = 1; i < report.Count; i++)
        {
            var x = ascending ? report[i - 1] : report[i];
            var y = ascending ? report[i] : report[i - 1];

            if (x == y) return false;

            var distance = y - x;
            if (distance < 1 || distance > 3) return false;
        }

        return true;
    }

    private bool ProcessReportDeep(List<int> reportOrig)
    {
        var safe = 0;

        for (var i = 0; i < reportOrig.Count; i++)
        {
            var report = reportOrig.ToList();
            report.RemoveAt(i);

            var isSafe = ProcessReport(report);
            if (isSafe) safe++;
        }

        return safe > 0;
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        _reports = new();

        // Gromit do something!
        foreach (var line in _data)
        {
            var report = new List<int>();
            _reports.Add(report);

            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            report.AddRange(parts.Select(x => x.ToInt()));
        }
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

        _data.DumpCollection();
    }
}

#if DUMP
#endif