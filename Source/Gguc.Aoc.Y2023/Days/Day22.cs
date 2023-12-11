#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2023.Days;

public class Day22 : Day
{
    private const int YEAR = 2023;
    private const int DAY = 22;

    private List<string> _data;
    private List<Brick> _bricks;
    private Space3d<int> _space;
    private Space3d<int> _finalSpace;
    private List<Brick> _finalBricks;

    private Dictionary<int, HashSet<int>> _supports = new();
    private Dictionary<int, HashSet<int>> _supported = new();
    private Dictionary<int, int> _heights = new();
    private Dictionary<int, int> _notsafe = new();

    public Day22(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "375";
        Expected2 = "72352";
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
        var bricks = _bricks.ToList();
        var space = _space.Copy();

        var result = CountBricks(bricks, space);

        Result = result;
    }

    protected override void ComputePart2()
    {
        var result = CountDifference();

        Result = result;
    }

    private long CountBricks(List<Brick> bricks, Space3d<int> space)
    {
        bricks = OrderBricks(bricks);

        FillSpace(bricks, space);

        DropBricks(bricks, space);

        _finalBricks = bricks.ToList();
        _finalSpace = space.Copy();
        foreach (var brick in bricks)
        {
            _heights[brick.Id] = brick.Z1;
        }

        var result = CountSupport();
        return result;
    }
    private long CountDifference()
    {
        var sum = 0L;

        foreach (var brickId in _notsafe.Keys)
        {
            var space = _finalSpace.Copy();
            var bricks = _finalBricks.CloneDeep();

            DisintegrateBrick(brickId, bricks, space);
            
            DropBricks(bricks, space, brickId + 1);
            
            var heights = new Dictionary<int, int>();
            foreach (var brick in bricks)
            {
                heights[brick.Id] = brick.Z1;
            }

            sum += HeightsDifferences(heights, _heights);
        }

        return sum;
    }

    private long HeightsDifferences(Dictionary<int, int> heights, Dictionary<int, int> dictionary)
    {
        return heights.Count(x => x.Value != _heights[x.Key]);
    }

    private void DisintegrateBrick(int brickId, List<Brick> bricks, Space3d<int> space)
    {
        var brick = bricks.FirstOrDefault(x => x.Id == brickId);
        // Log.Debug($"--Disintegrate--[{brick}]");
        bricks.Remove(brick);

        for (var x = brick.X1; x <= brick.X2; x++)
        {
            for (var y = brick.Y1; y <= brick.Y2; y++)
            {
                for (var z = brick.Z1; z <= brick.Z2; z++)
                {
                    space[x, y, z] = 0;
                }
            }
        }
    }

    private List<Brick> OrderBricks(List<Brick> bricks)
    {
        // var list = bricks.OrderBy(x => x.Z1).ThenBy(x => x.Z2).ThenBy(x => x.X1).ThenBy(x => x.X2).ThenBy(x => x.Y1).ThenBy(x => x.Y2).ToList();
        var list = bricks.OrderBy(x => x.Z1).ThenBy(x => x.X1).ThenBy(x => x.Y1).ToList();
        for (var i = 0; i < list.Count; i++)
        {
            list[i].Id = i + 1;
            _supports[i + 1] = new();
            _supported[i + 1] = new();
        }
        return list;
    }

    private void DropBricks(List<Brick> bricks, Space3d<int> space, int id = 0)
    {
        foreach (var brick in bricks)
        {
            if(brick.Id < id) continue;
            DropBrick(brick, space);
        }
    }

    private void DropBrick(Brick brick, Space3d<int> space)
    {
        while (true)
        {
            if (brick.Z1 == 1) break;

            var zdown = brick.Z1 - 1;
            var down = new HashSet<int>();

            for (var i = brick.X1; i <= brick.X2; i++)
            {
                for (var j = brick.Y1; j <= brick.Y2; j++)
                {
                    var value = space[i, j, zdown];

                    if (value > 0) down.Add(value);
                }
            }

            if (down.Any())
            {
                down.ForEach(x => Support(x, brick.Id));
                break;
            }

            LowerBrick(brick, space);
        }
    }

    private void LowerBrick(Brick brick, Space3d<int> space)
    {
        var z1 = brick.Z1 - 1;
        var z2 = brick.Z2;

        brick.Z1--;
        brick.Z2--;

        for (var x = brick.X1; x <= brick.X2; x++)
        {
            for (var y = brick.Y1; y <= brick.Y2; y++)
            {
                space[x, y, z1] = brick.Id;
                space[x, y, z2] = 0;
            }
        }
    }

    private void Support(int down, int up)
    {
        _supports[down].Add(up);
        _supported[up].Add(down);
    }

    private long CountSupport()
    {
        var all = new HashSet<int>();
        var notsafe = new HashSet<int>();
        var safe1 = new HashSet<int>();
        var safe2 = new HashSet<int>();

        foreach (var brick in _bricks)
        {
            all.Add(brick.Id);

            if (_supports[brick.Id].Count == 0)
            {
                safe1.Add(brick.Id);
                continue;
            }

            var supports = _supports[brick.Id].Count(x => _supported[x].Count <= 1);
            if (supports == 0)
            {
                safe2.Add(brick.Id);
                continue;
            }

            notsafe.Add(brick.Id);
        }

        notsafe.ForEach(b => _notsafe[b] = 0);

        return safe1.Count + safe2.Count;
    }

    private void FillSpace(List<Brick> bricks, Space3d<int> space)
    {
        foreach (var brick in bricks)
        {
            var points = GetPoints(brick);
            points.ForEach(p => space[p.X, p.Y, p.Z] = brick.Id);
        }
    }

    private List<Point3d> GetPoints(Brick brick)
    {
        var list = new List<Point3d>();

        if (brick.X1 != brick.X2)
        {
            for (var i = brick.X1; i <= brick.X2; i++)
            {
                list.Add(new Point3d(i, brick.Y1, brick.Z1));
            }
        }
        else if (brick.Y1 != brick.Y2)
        {
            for (var i = brick.Y1; i <= brick.Y2; i++)
            {
                list.Add(new Point3d(brick.X1, i, brick.Z1));
            }
        }
        else if (brick.Z1 != brick.Z2)
        {
            for (var i = brick.Z1; i <= brick.Z2; i++)
            {
                list.Add(new Point3d(brick.X1, brick.Y1, i));
            }
        }
        else
        {
            list.Add(new Point3d(brick.X1, brick.Y1, brick.Z1));
        }

        return list;
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        _bricks = new List<Brick>();

        foreach (var line in _data)
        {
            var parts = line.Split(',', '~').Select(x => x.ToInt()).ToList();
            var brick = new Brick { X1 = parts[0], X2 = parts[3], Y1 = parts[1], Y2 = parts[4], Z1 = parts[2], Z2 = parts[5] };
            _bricks.Add(brick);
        }

        var x = 0;
        var y = 0;
        var z = 0;
        foreach (var brick in _bricks)
        {
            x = Math.Max(x, brick.X2);
            y = Math.Max(y, brick.Y2);
            z = Math.Max(z, brick.Z2);
        }

        _space = new Space3d<int>(x + 1, y + 1, z + 1);
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
        // _bricks.DumpCollection();
    }
}

#if DUMP

        //space.ToString().Dump("space", true);
#endif