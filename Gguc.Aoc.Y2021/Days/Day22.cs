#define LOGx
#define STOPWATCH

namespace Gguc.Aoc.Y2021.Days;

public class Day22 : Day
{
    private const int YEAR = 2021;
    private const int DAY = 22;

    private List<string> _source;
    private List<Cube> _data;

    private HashSet<Point3d> _points;

    public Day22(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
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

    private string _pattern = @"([-\d]+)";

    protected override void ProcessData()
    {
        _data = new List<Cube>();

        foreach (string line in _source)
        {
            var step = new Cube();
            var parts = line.Split(' ');

            step.On = parts[0].ToBool("on");

            var input = parts[1];
            List<string> matches = new List<string>();
            foreach (Match m in Regex.Matches(input, _pattern))
            {
                matches.Add(m.Groups[1].Value);
            }

            step.X1 = matches[0].ToInt();
            step.X2 = matches[1].ToInt();
            step.Y1 = matches[2].ToInt();
            step.Y2 = matches[3].ToInt();
            step.Z1 = matches[4].ToInt();
            step.Z2 = matches[5].ToInt();

            _data.Add(step);
        }
    }
    #endregion Parse

    protected override void ComputePart1()
    {
        /*
         * SKIP - It's slow...
         * 
        */
        var data = _data.ToList();

        _points = new HashSet<Point3d>();

        foreach (var step in data)
        {
            ProcessStep(step, _points);
        }

        Result = _points.Count();
    }

    protected override void ComputePart2()
    {
        var lights = _data.ToList();

        lights = Compesate(lights);

        Result = CalcSteps(lights);
    }

    private void ProcessStep(Cube cube, HashSet<Point3d> points)
    {
        var list = new List<int>
        {
            Math.Abs(cube.X1),
            Math.Abs(cube.X2),
            Math.Abs(cube.Y1),
            Math.Abs(cube.Y2),
            Math.Abs(cube.Z1),
            Math.Abs(cube.Z2),
        };

        if (list.Max() > 50) return;

        for (int z = cube.Z1; z <= cube.Z2; z++)
        {
            for (int y = cube.Y1; y <= cube.Y2; y++)
            {
                for (int x = cube.X1; x <= cube.X2; x++)
                {
                    if (cube.On)
                        points.Add(new Point3d(x, y, z));
                    else
                        points.Remove(new Point3d(x, y, z));
                }
            }
        }
    }

    private long CalcSteps(List<Cube> lights)
    {
        var volume = 0L;
        var count = 0;

        var queue = new Queue<Cube>(lights);
        var list = new List<Cube>();

        while (queue.Count > 0)
        {
            var space = queue.Dequeue();
            // $"Next--{++count}-{space.ToJson()}...".Dump();

            if (space.On)
            {
                list = SplitOverlaps(list, space);
            }
            else
            {
                list = SplitOverlapsOff(list, space);
            }
        }

        foreach (var item in list)
        {
            volume += CalcVolume(item);
        }

        return volume;
    }

    private List<Cube> SplitOverlaps(List<Cube> lightsOn, Cube newlights)
    {
        var uniqueSpaces = new HashSet<Cube>();

        var undefinedSet = new HashSet<Cube>();
        undefinedSet.Add(newlights);

        for (int i = lightsOn.Count - 1; i >= 0; i--)
        {
            if (IsInside(lightsOn[i], newlights))
            {
                lightsOn.RemoveAt(i);
                continue;
            }
        }

        foreach (var space in lightsOn)
        {
            if (!IsOverlap(space, newlights))
            {
                uniqueSpaces.Add(space);
            }
            else
            {
                undefinedSet.Add(space);
            }
        }

        var undefined = undefinedSet.ToList();

        while (undefined.Count > 0)
        {
            var isOverlap = false;

            undefinedSet = new HashSet<Cube>(undefined);
            undefined = undefinedSet.ToList();

            var undefined0 = undefined[0];

            for (int j = 1; j < undefined.Count; j++)
            {
                isOverlap = IsOverlap(undefined0, undefined[j]);

                if (isOverlap)
                {
                    var spaces1 = SplitOn(undefined0, undefined[j]);
                    var spaces2 = SplitOn(undefined[j], undefined0);
                    undefined.RemoveAt(j);
                    undefined.RemoveAt(0);
                    undefined.AddRange(spaces1);
                    undefined.AddRange(spaces2);
                    break;
                }
            }

            if (isOverlap) continue;

            uniqueSpaces.Add(undefined0);
            undefined.RemoveAt(0);
        }

        return uniqueSpaces.ToList();
    }

    private List<Cube> SplitOverlapsOff(List<Cube> lightsOn, Cube lightsOff)
    {
        var uniqueSpaces = new HashSet<Cube>();

        var undefinedSet = new HashSet<Cube>();

        for (int i = lightsOn.Count - 1; i >= 0; i--)
        {
            if (IsInside(lightsOn[i], lightsOff))
            {
                lightsOn.RemoveAt(i);
                continue;
            }
        }

        foreach (var space in lightsOn)
        {
            if (!IsOverlap(space, lightsOff))
            {
                uniqueSpaces.Add(space);
            }
            else
            {
                undefinedSet.Add(space);
            }
        }

        var undefined = undefinedSet.ToList();

        while (undefined.Count > 0)
        {
            undefinedSet = new HashSet<Cube>(undefined);
            undefined = undefinedSet.ToList();

            var undefined0 = undefined[0];

            var isOverlap = IsInside(undefined0, lightsOff);
            if (isOverlap)
            {
                undefined.RemoveAt(0);
                continue;
            }

            isOverlap = IsOverlap(lightsOff, undefined0);
            if (isOverlap)
            {
                var spaces1 = SplitOn(undefined0, lightsOff);
                undefined.RemoveAt(0);

                undefined.AddRange(spaces1);

                continue;
            }

            uniqueSpaces.Add(undefined0);
            undefined.RemoveAt(0);
        }

        return uniqueSpaces.ToList();
    }

    private List<Cube> SplitOn(Cube cube1, Cube cube2)
    {
        if (cube2 == null) return new List<Cube>() { cube1 };

        var spaces = new List<Cube>();

        var xSorted = new SortedSet<int> { cube1.X1, cube1.X2 };
        var ySorted = new SortedSet<int> { cube1.Y1, cube1.Y2 };
        var zSorted = new SortedSet<int> { cube1.Z1, cube1.Z2 };

        if (cube2.X1 > cube1.X1 && cube2.X1 < cube1.X2) xSorted.Add(cube2.X1);
        if (cube2.X2 > cube1.X1 && cube2.X2 < cube1.X2) xSorted.Add(cube2.X2);
        if (cube2.Y1 > cube1.Y1 && cube2.Y1 < cube1.Y2) ySorted.Add(cube2.Y1);
        if (cube2.Y2 > cube1.Y1 && cube2.Y2 < cube1.Y2) ySorted.Add(cube2.Y2);
        if (cube2.Z1 > cube1.Z1 && cube2.Z1 < cube1.Z2) zSorted.Add(cube2.Z1);
        if (cube2.Z2 > cube1.Z1 && cube2.Z2 < cube1.Z2) zSorted.Add(cube2.Z2);

        var xList = xSorted.ToList();
        var yList = ySorted.ToList();
        var zList = zSorted.ToList();

        for (int z = 0; z < zList.Count - 1; z++)
        {
            for (int y = 0; y < yList.Count - 1; y++)
            {
                for (int x = 0; x < xList.Count - 1; x++)
                {
                    var x1 = xList[x];
                    var y1 = yList[y];
                    var z1 = zList[z];

                    var x2 = xList[x + 1];
                    var y2 = yList[y + 1];
                    var z2 = zList[z + 1];

                    spaces.Add(new Cube { On = true, X1 = x1, X2 = x2, Y1 = y1, Y2 = y2, Z1 = z1, Z2 = z2, });
                }
            }
        }

        return spaces;
    }

    private bool IsInside(Cube cube1, Cube cube2)
    {
        if (cube1 == null || cube2 == null) return false;

        if ((cube1.X1 >= cube2.X1)
          && (cube1.X2 <= cube2.X2)
          && (cube1.Y1 >= cube2.Y1)
          && (cube1.Y2 <= cube2.Y2)
          && (cube1.Z1 >= cube2.Z1)
          && (cube1.Z2 <= cube2.Z2)
         ) return true;

        return false;
    }

    private bool IsOverlap(Cube cube1, Cube cube2)
    {
        if (cube1 == null || cube2 == null) return false;

        if (cube1.X1 > (cube2.X2 - 1)
         || cube2.X1 > (cube1.X2 - 1)
         || cube1.Y1 > (cube2.Y2 - 1)
         || cube2.Y1 > (cube1.Y2 - 1)
         || cube1.Z1 > (cube2.Z2 - 1)
         || cube2.Z1 > (cube1.Z2 - 1)
         ) return false;

        return true;
    }

    private long CalcVolume(Cube cube)
    {
        if (cube == null) return 0;

        long vol = (Math.Abs((long)cube.X2 - cube.X1)) * (Math.Abs((long)cube.Y2 - cube.Y1)) * (Math.Abs((long)cube.Z2 - cube.Z1));
        return vol;
    }

    private List<Cube> Compesate(List<Cube> cubes)
    {
        var newList = new List<Cube>();
        foreach (var s in cubes)
        {
            newList.Add(new Cube
            {
                On = s.On,
                X1 = s.X1,
                X2 = s.X2 + 1,
                Y1 = s.Y1,
                Y2 = s.Y2 + 1,
                Z1 = s.Z1,
                Z2 = s.Z2 + 1,
            });
        }
        return newList;
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

        //_data[0].Dump("Item");
        // _data.DumpCollection("List");
    }
    #endregion Dump
}