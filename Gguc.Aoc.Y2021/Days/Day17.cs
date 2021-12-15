#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Core.Templates;

public class Day17 : Day
{
    private const int YEAR = 2021;
    private const int DAY = 17;

    private List<string> _source;
    private (int x1, int x2, int y1, int y2) _target;
    private HashSet<(int, int)> _hits;
    private int _maxHeight;

    public Day17(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
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
        var regex = @"target area: x=([-\d]+)..([-\d]+), y=([-\d]+)..([-\d]+)";
        var data = _source[0].MatchAll(regex).ToList();
        var x1 = data[1].ToInt();
        var x2 = data[2].ToInt();
        var y1 = data[3].ToInt();
        var y2 = data[4].ToInt();

        _target = (x1, x2, y1, y2);
        _hits = new HashSet<(int, int)>();
        _maxHeight = 0;
    }
    #endregion Parse

    protected override void ComputePart1()
    {
        var target = _target;

        var maxVelocityX = target.x2;
        var minVelocityX = MaxReach(target.x1, maxVelocityX);
        var minVelocityY = target.y1;
        var maxVelocityY = 500;

        $"ComputePart1: x={minVelocityX}-{maxVelocityX}. y={minVelocityY}-{maxVelocityY}.".Dump();

        for (var x = minVelocityX; x <= maxVelocityX; x++)
        {
            ProbeAll(x, minVelocityY, maxVelocityY);
        }

        Result = _maxHeight;
    }

    protected override void ComputePart2()
    {
        Result = _hits.Count;
    }

    private void ProbeAll(int x, int minY, int maxY)
    {
        var y = minY;

        while(true)
        {
            Probe(x, y);

            if (++y > maxY) break;
        }
    }

    private void Probe(int initX, int initY)
    {
        var x = initX;
        var y = initY;
        var maxHeight = 0;

        (int x, int y) position = (0, 0);

        while (true)
        {
            position.x += x;
            position.y += y;
            var isHit = Check(position.x, position.y);

            //$"Probe: initial={(initX, initY)} Position={position}. Hit={isHit}.".Dump();
            
            maxHeight = Math.Max(position.y, maxHeight);

            if (isHit)
            {
                //$"Probe: initial={(initX, initY)}. Position={position}. MaxHeight={maxHeight}. HIT!".Dump();
                AddHit(initX, initY, maxHeight);
                break;
            }

            if (position.x > _target.x2 || position.y < _target.y1) break;

            if (x > 0) x--;
            y--;
        }
    }

    private bool Check(int x, int y)
    {
        return (x >= _target.x1 && x <= _target.x2 && y >= _target.y1 && y <= _target.y2);
    }

    private int MaxReach(int minX, int maxX)
    {
        for (var x = 1; x <= maxX; x++)
        {
            var mr = (x * x) - x + 1;
            if (mr > minX) return x;
        }
        return -1;
    }

    private void AddHit(int x, int y, int height)
    {
        _hits.Add((x, y));
        _maxHeight = Math.Max(_maxHeight, height);
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

        _target.Dump("_target");
    }
    #endregion Dump
}

#if DUMP

#endif