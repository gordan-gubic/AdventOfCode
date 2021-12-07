#define LOGx
#define STOPWATCH

namespace Gguc.Aoc.Y2020.Days;

public class Day24 : Day
{
    private List<List<string>> _source;
    private List<List<string>> _data;

    Dictionary<Point, bool> _tiles = new Dictionary<Point, bool>();

    public Day24(ILog log, IParser parser) : base(log, parser)
    {
        EnableDebug();
        Initialize();
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Year = 2020;
        Parser.Day = 24;
        Parser.Type = ParserFileType.Real;

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
        FlipTilesFromInstructions();

        _tiles.DumpJson("_tiles");

        Result = _tiles.Count(x => x.Value);
    }

    protected override void ComputePart2()
    {
        var target = 100;

        // It's not clear, but tiles are left as finished after part 1
        InitTiles();
        FlipTilesFromInstructions();

        FlipTilesForCycles(target);

        Result = _tiles.Count(x => x.Value);
    }

    private void InitTiles(bool defaultvalue = false)
    {
        foreach (var address in _data)
        {
            var tile = FindTile(address);
            _tiles[tile] = defaultvalue;
        }
    }

    private void FlipTilesFromInstructions()
    {
        foreach (var address in _data)
        {
            FlipTile1(address);
        }
    }

    private void FlipTilesForCycles(in int target)
    {
        for (int i = 0; i < target; i++)
        {
            var tempTiles = _tiles.Copy();
            AddNeighborsForAll(tempTiles);

            foreach (var key in tempTiles.Keys.ToList())
            {
                FlipTile2(tempTiles, key, tempTiles[key]);
            }

            Debug($"Turn: {i + 1}, Count: {tempTiles.Count(x => x.Value)}");

            _tiles = tempTiles;
        }
    }

    private void FlipTile1(List<string> address)
    {
        var tile = FindTile(address);

        if (!_tiles.ContainsKey(tile)) _tiles[tile] = false;

        Debug($"tile: [{tile}]. Value=[{_tiles[tile]}]");

        _tiles[tile] = !_tiles[tile];
    }

    private void FlipTile2(Dictionary<Point, bool> tiles, in Point tile, in bool value)
    {
        var count = CountNeighbors(tile);

        if (!value && count == 0)
        {
            tiles.Remove(tile);
            return;
        }

        if (!value && count == 2)
        {
            tiles[tile] = true;
            return;
        }

        if (value && (count == 0 || count > 2))
        {
            tiles[tile] = false;
        }
    }

    private void AddNeighborsForAll(Dictionary<Point, bool> tempTiles)
    {
        foreach (var tile in tempTiles.Keys.ToList())
        {
            AddNeighbor(tempTiles, tile.X - 10, tile.Y);
            AddNeighbor(tempTiles, tile.X + 10, tile.Y);
            AddNeighbor(tempTiles, tile.X - 5, tile.Y - 10);
            AddNeighbor(tempTiles, tile.X + 5, tile.Y - 10);
            AddNeighbor(tempTiles, tile.X - 5, tile.Y + 10);
            AddNeighbor(tempTiles, tile.X + 5, tile.Y + 10);
        }
    }

    private void AddNeighbor(Dictionary<Point, bool> tiles, in int x, in int y)
    {
        var point = new Point(x, y);

        if (!tiles.ContainsKey(point)) tiles[point] = false;
    }

    private int CountNeighbors(in Point tile)
    {
        var validList = new List<bool>
            {
                GetValue(tile.X - 10, tile.Y),
                GetValue(tile.X + 10, tile.Y),
                GetValue(tile.X - 5, tile.Y - 10),
                GetValue(tile.X + 5, tile.Y - 10),
                GetValue(tile.X - 5, tile.Y + 10),
                GetValue(tile.X + 5, tile.Y + 10),
            };

        return validList.Count(x => x);
    }

    private bool GetValue(in int x, in int y)
    {
        var point = new Point(x, y);

        return _tiles.ContainsKey(point) && _tiles[point];
    }

    private Point FindTile(List<string> address)
    {
        var point = new Point(0, 0);

        foreach (var ins in address)
        {
            switch (ins)
            {
                case "e": point = new Point(+10 + point.X, +0 + point.Y); break;
                case "w": point = new Point(-10 + point.X, +0 + point.Y); break;
                case "ne": point = new Point(+5 + point.X, +10 + point.Y); break;
                case "se": point = new Point(+5 + point.X, -10 + point.Y); break;
                case "nw": point = new Point(-5 + point.X, +10 + point.Y); break;
                case "sw": point = new Point(-5 + point.X, -10 + point.Y); break;
            }
        }

        return point;
    }

    private List<string> ConvertInput(string input)
    {
        var chars = input.ToList();
        var instructions = new List<string>();

        var ins = string.Empty;
        foreach (var ch in chars)
        {
            if (ch == 'n' || ch == 's')
            {
                ins += ch;
                continue;
            }

            ins += ch;
            instructions.Add(ins);
            ins = string.Empty;
        }

        return instructions;
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (!Log.EnableDebug) return;

        Debug();

        _data.DumpJson("List");
    }
}

#if DUMP
#endif