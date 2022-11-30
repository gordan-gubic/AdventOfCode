#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2018.Days;

public class Day03 : Day
{
    private const int YEAR = 2018;
    private const int DAY = 3;

    private List<string> _source;
    private List<FabricClaim> _data;
    private Map<int> _map;

    public Day03(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();
    }

    #region Parse
    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Real;

        _source = Parser.Parse();
    }

    protected override void ProcessData()
    {
        _data = new List<FabricClaim>();

        var maxX = 0;
        var maxY = 0;

        foreach (var line in _source)
        {
            var claim = ParseLine(line);
            _data.Add(claim);
            maxX = Math.Max(maxX, claim.X + claim.Width);
            maxY = Math.Max(maxY, claim.Y + claim.Height);
        }

        _map = new Map<int>(maxX + 1, maxY + 1, 0);
    }

    private FabricClaim ParseLine(string line)
    {
        // #2 @ 3,1: 4x4
        var parts = line.Split(new char[] { ' ', '@', ',', ':', 'x' }, StringSplitOptions.RemoveEmptyEntries);

        return new FabricClaim
        {
            Id = parts[0],
            X = parts[1].ToInt(),
            Y = parts[2].ToInt(),
            Width = parts[3].ToInt(),
            Height = parts[4].ToInt(),
        };
    }
    #endregion Parse

    protected override void ComputePart1()
    {
        foreach (var claim in _data)
        {
            ProcessClaim(claim);
        }

        var r = _map.CountValues(x => x > 1);
        Result = r;
    }

    protected override void ComputePart2()
    {
        var r = string.Empty;

        foreach (var claim in _data)
        {
            var isOk = VerifyClaim(claim);

            if (isOk)
            {
                r = claim.Id;
                break;
            }
        }
        
        Result = r.Replace("#", "").ToInt();
    }

    private void ProcessClaim(FabricClaim claim)
    {
        var maxX = claim.X + claim.Width;
        var maxY = claim.Y + claim.Height;

        for (int y = claim.Y; y < maxY; y++)
        {
            for (int x = claim.X; x < maxX; x++)
            {
                _map[x, y]++;
            }
        }
    }

    private bool VerifyClaim(FabricClaim claim)
    {
        var maxX = claim.X + claim.Width;
        var maxY = claim.Y + claim.Height;

        for (int y = claim.Y; y < maxY; y++)
        {
            for (int x = claim.X; x < maxX; x++)
            {
                if (_map[x, y] > 1) return false;
            }
        }

        return true;
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

        _data[0].Dump("Item");
        //_data.DumpJson("List");
        //_map.MapValueToString().Dump("Map", true);
    }
    #endregion Dump
}

#if DROP

#endif