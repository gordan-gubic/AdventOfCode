﻿#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Core.Templates;


public class Day00 : Day
{
    private List<string> _source;
    private List<string> _data;

    public Day00(ILog log, IParser parser) : base(log, parser)
    {
        EnableDebug();
        Initialize();
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Year = 2020;
        Parser.Day = 1;
        Parser.Type = ParserFileType.Test;

        _source = Parser.Parse(ConvertInput);
    }

    /// <inheritdoc />
    protected override void ProcessData()
    {
        _data = _source;
    }

    /// <inheritdoc />
    public override void DumpInput()
    {
        DumpData();
    }

    protected override void ComputePart1()
    {
    }

    protected override void ComputePart2()
    {
    }

    private string ConvertInput(string input)
    {
        return input;
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (!Log.EnableDebug) return;

        Debug();

        _data[0].Dump("Item");
        _data.DumpCollection("List");
    }
}

#if DUMP

#endif