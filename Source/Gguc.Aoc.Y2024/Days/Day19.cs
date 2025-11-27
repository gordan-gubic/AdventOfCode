#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2024.Days;

using System.IO.Enumeration;

public class Day19 : Day
{
    private const int YEAR = 2024;
    private const int DAY = 19;

    private List<string> _data;
    private HashSet<string> _patterns;
    private List<string> _requests;
    private List<string> _founded;

    public Day19(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "350";
        Expected2 = "";
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Real;
        Parser.Type = ParserFileType.Test;

        _data = Parser.Parse();
    }

    /// <inheritdoc />
    public override void DumpInput()
    {
        DumpData();
    }

    protected override void ComputePart1()
    {
        Result = ComputeRequests2();
    }

    protected override void ComputePart2()
    {
        // Result = ComputeRequests2(); ;
    }

    private long ComputeRequests1()
    {
        var sum = 0L;

        foreach (var request in _requests)
        {
            /*
            var ps = new PatternSearch { Patterns = _patterns, Request = request };
            ps.Find();
            if (!ps.Result.IsNullOrEmpty())
            {
                // ps.Result[0].Path.Dump("Found");
                sum++;
            }
            */

            var ps = new PatternRecursive1 { Patterns = _patterns, Request = request };
            ps.Find();
            if (ps.Result)
            {
                // ps.Result.Dump();
                sum++;
            }
            /*
            */
        }

        return sum;
    }

    private long ComputeRequests2()
    {
        var sum = 0L;

        foreach (var request in _requests)
        {
            var ps1 = new PatternRecursive1 { Patterns = _patterns, Request = request };
            ps1.Find();
            if (ps1.Result)
            {
                var ps2 = new PatternRecursive2 { Patterns = _patterns, Request = request };
                ps2.Find();
                ps2.Result.Dump();
                sum += ps2.Result;
            }
        }

        return sum;
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        var isPatterns = true;
        var patternsLines = new List<string>();
        var requestsLines = new List<string>();

        foreach (var line in _data)
        {
            if (line.IsWhitespace())
            {
                isPatterns = false;
                continue;
            }

            if (isPatterns) patternsLines.Add(line);
            else requestsLines.Add(line);
        }

        _patterns = new HashSet<string>();
        foreach (var pattern in patternsLines)
        {
            pattern.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList().ForEach(x => _patterns.Add(x));
        }

        _requests = requestsLines.ToList();
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

        // _patterns.DumpCollection("patterns");
        // _requests.DumpCollection("requests");
    }
}

#if DUMP
#endif