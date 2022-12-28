#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2022.Days;

public class Day23 : Day
{
    private const int YEAR = 2022;
    private const int DAY = 23;

    private List<string> _data;
    private Map<bool> _orig;
    private Map<bool> _map;
    private List<Func<Map<bool>, Map<int>, ElfNode, bool>> _considerations;


    public Day23(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "4172";
        Expected2 = "942";
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
        Result = ElvesDance(10);
    }

    protected override void ComputePart2()
    {
        Result = ElvesDanceTillStop();
    }

    private long ElvesDance(int target)
    {
        var map = _orig.Clone();
        _considerations = CreateConsiderations();

        for (var i = 0; i < target; i++)
        {
            (_, map) = PlanMoves(map);
        }

        // map.MapBoolToString().Dump("End", true);
        return CountEmptySpace(map);
    }

    private long ElvesDanceTillStop()
    {
        var cont = true;
        var map = _orig.Clone();
        _considerations = CreateConsiderations();

        var i = 1;

        while (cont)
        {
            (cont, map) = PlanMoves(map);
            if (!cont) break;
            i++;
        }

        return i;
    }

    private long CountEmptySpace(Map<bool> map)
    {
        var listCount = ListElves(map).Count;
        var size = map.Width * map.Height;

        return size - listCount;
    }

    private List<ElfNode> ListElves(Map<bool> map)
    {
        var list = new List<ElfNode>();
        map.ForEach((x, y) =>
        {
            if (map.GetValue(x, y)) list.Add(new ElfNode(x, y));
        });
        return list;
    }

    private (bool, Map<bool>) PlanMoves(Map<bool> map)
    {
        map = map.Expand();
        var elves = ListElves(map);
        var plans = new Map<int>(map.Width, map.Height);
        var moves = new Map<int>(map.Width, map.Height);

        ConsiderStaying(map, elves);
        if (elves.Count(x => !x.PlanToStay) == 0) return (false, map);

        RotateConsiderations();
        ConsiderMoves(map, plans, elves);
        // plans.MapValueToString().Dump("Plans", true);

        TryToMove(map, plans, moves, elves);
        // moves.MapValueToString().Dump("Moves", true);

        Move(map, elves);

        map = TryReduce(map);

        return (true, map);
    }

    private Map<bool> TryReduce(Map<bool> map)
    {
        var (minX, maxX, minY, maxY, _) = MinMaxMap(map);

        return map.Sub(minX, minY, maxX, maxY);
    }

    private void ConsiderStaying(Map<bool> map, List<ElfNode> elves)
    {
        foreach (var elf in elves)
        {
            var isFree = FreeAround(map, elf.Location);
            elf.PlanToStay = isFree;
        }
    }

    private bool FreeAround(Map<bool> map, Point point)
    {
        var sum = 0;
        for (var y = -1; y < 2; y++)
        {
            for (var x = -1; x < 2; x++)
            {
                sum += map.GetValue(x + point.X, y + point.Y) ? 1 : 0;
            }
        }

        return sum == 1;
    }

    private void ConsiderMoves(Map<bool> map, Map<int> plans, List<ElfNode> elves)
    {
        elves = elves.Where(x => !x.PlanToStay).ToList();

        foreach (var elf in elves)
        {
            foreach (var plan in _considerations)
            {
                if (plan(map, plans, elf)) break;
            }
        }
    }

    private void TryToMove(Map<bool> map, Map<int> plans, Map<int> moves, List<ElfNode> elves)
    {
        elves = elves.Where(x => x.PlanToMove).ToList();

        foreach (var elf in elves)
        {
            var dest = elf.Destination;
            var isFree1 = !map.GetValue(dest.X, dest.Y);
            var isFree2 = plans.GetValue(dest.X, dest.Y) == 1;

            if (isFree1 && isFree2) moves[dest.X, dest.Y]++;
        }

        foreach (var elf in elves)
        {
            var dest = elf.Destination;
            var isFree = moves.GetValue(dest.X, dest.Y) == 1;
            if (isFree) elf.TryToMove = true;
        }
    }

    private void Move(Map<bool> map, List<ElfNode> elves)
    {
        elves = elves.Where(x => x.TryToMove).ToList();

        foreach (var elf in elves)
        {
            map[elf.Location.X, elf.Location.Y] = false;
            map[elf.Destination.X, elf.Destination.Y] = true;
        }
    }

    private List<Func<Map<bool>, Map<int>, ElfNode, bool>> CreateConsiderations()
    {
        return new List<Func<Map<bool>, Map<int>, ElfNode, bool>>
        {
            ConsiderEast,
            ConsiderNorth,
            ConsiderSouth,
            ConsiderWest,
        };
    }

    private void RotateConsiderations()
    {
        var temp = _considerations[0];
        _considerations.RemoveAt(0);
        _considerations.Add(temp);
    }

    private bool ConsiderNorth(Map<bool> map, Map<int> plans, ElfNode node)
    {
        var x = node.Location.X;
        var y = node.Location.Y - 1;

        var t1 = map[x - 1, y];
        var t2 = map[x, y];
        var t3 = map[x + 1, y];

        return IsFree(plans, node, x, y, t1, t2, t3);
    }

    private bool ConsiderSouth(Map<bool> map, Map<int> plans, ElfNode node)
    {
        var x = node.Location.X;
        var y = node.Location.Y + 1;

        var t1 = map[x - 1, y];
        var t2 = map[x, y];
        var t3 = map[x + 1, y];

        return IsFree(plans, node, x, y, t1, t2, t3);
    }

    private bool ConsiderWest(Map<bool> map, Map<int> plans, ElfNode node)
    {
        var x = node.Location.X - 1;
        var y = node.Location.Y;

        var t1 = map[x, y - 1];
        var t2 = map[x, y];
        var t3 = map[x, y + 1];

        return IsFree(plans, node, x, y, t1, t2, t3);
    }

    private bool ConsiderEast(Map<bool> map, Map<int> plans, ElfNode node)
    {
        var x = node.Location.X + 1;
        var y = node.Location.Y;

        var t1 = map[x, y - 1];
        var t2 = map[x, y];
        var t3 = map[x, y + 1];

        return IsFree(plans, node, x, y, t1, t2, t3);
    }

    private bool IsFree(Map<int> plans, ElfNode node, int x, int y, params bool[] test)
    {
        var isFree = !test.Any(t => t);
        if (isFree)
        {
            plans[x, y]++;
            node.Destination = new Point(x, y);
            node.PlanToMove = true;
        }

        return isFree;
    }

    private (int, int, int, int, List<Point>) MinMaxMap(Map<bool> map)
    {
        var list = new List<Point>();
        map.ForEach((x, y) =>
        {
            if (map.GetValue(x, y)) list.Add(new Point(x, y));
        });

        var orderX = list.Select(p => p.X).OrderBy(p => p).ToList();
        var orderY = list.Select(p => p.Y).OrderBy(p => p).ToList();

        var minX = orderX.FirstOrDefault();
        var maxX = orderX.LastOrDefault();
        var minY = orderY.FirstOrDefault();
        var maxY = orderY.LastOrDefault();

        return (minX, maxX, minY, maxY, list);
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        _orig = new Map<bool>(_data, x => x == '#');
        _map = _orig.Clone();

        // _map.MapBoolToString().Dump("Map", true);
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (!Log.EnableDebug) return;

        Debug();

        //_data.DumpCollection();
    }
}

#if DUMP
#endif