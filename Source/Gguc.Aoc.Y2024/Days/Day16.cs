#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2024.Days;

public class Day16 : Day
{
    private const int YEAR = 2024;
    private const int DAY = 16;

    private List<string> _data;
    private Map<char> _map;
    private Map<bool> _walls;
    private MazeSearch _mazeSearch;

    public Day16(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        ExpectedProd1 = "92432";
        ExpectedProd2 = "458";
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Test;
        Parser.Type = ParserFileType.Real;

        _map = Parser.ParseMapChar();
        _walls = Parser.ParseMapBool();
    }

    /// <inheritdoc />
    public override void DumpInput()
    {
        DumpData();
    }

    protected override void ComputePart1()
    {
        CalculateBestPath();

        var found = _mazeSearch.Result.MinBy(x => x.Value);
        var foundMin = found?.Value ?? 0L;

        Result = foundMin;
    }

    protected override void ComputePart2()
    {
        var found = _mazeSearch.Result.MinBy(x => x.Value);
        var foundMin = found?.Value ?? 0L;

        var seats = new HashSet<Point>();
        _mazeSearch.Result.Where(x => x.Value == foundMin).ForEach(m => m.Points.ForEach(p => seats.Add(p)));

        Result = seats.Count;
    }

    private void CalculateBestPath()
    {
        _mazeSearch = new MazeSearch();
        _mazeSearch.Map = _map;
        _mazeSearch.Walls = _walls;

        _mazeSearch.Find();
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (!Log.EnableDebug) return;

        Debug();

        // _data.DumpCollection();
        // _map.MapValueToString().Dump("map", true);
        // _walls.MapBoolToString().Dump("_walls", true);
    }
}

#if DUMP
#endif