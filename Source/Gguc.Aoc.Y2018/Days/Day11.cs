#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2018.Days;

public class Day11 : Day
{
    private const int YEAR = 2018;
    private const int DAY = 11;

    private List<string> _data;
    private List<int> _grids;
    private int _grid;
    private int _width;
    private int _height;

    public Day11(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "19,17"; 
        Expected2 = "233,288,12";
    }

    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Test;
        Parser.Type = ParserFileType.Real;

        _data = Parser.Parse();
    }

    #region Parse
    protected override void ProcessData()
    {
        _grids = new();
        _width = 300;
        _height = 300;

        // Gromit do something!
        foreach (var line in _data)
        {
            _grids.Add(line.ToInt());
        }

        _grid = _grids.FirstOrDefault();
    }
    #endregion

    #region Head
    protected override void ComputePart1()
    {
        // TestValues();
        var map = FillGrid(_grid);

        Result = FindMaxValue1(map);
    }

    protected override void ComputePart2()
    {
        var map = FillGrid(_grid);

        Result = FindMaxValue2(map);
    }
    #endregion

    #region Body
    private long FindMaxValue1(Map<long> map)
    {
        var values = FillValuesFixed(map);
        var (sum, x, y) = MaxRegion(values);
        Log.Info($"Result: [{x},{y}]  grid={_grid}   sum={sum}  x={x}  y={y}");

        return sum;
    }
    private long FindMaxValue2(Map<long> map)
    {
        var values = FillValues(map);
        var (sum, x, y, s) = MaxRegion(values);
        Log.Info($"Result: [{x},{y},{s}]  grid={_grid}   sum={sum}  x={x}  y={y}  step={s}");

        return sum;
    }

    private Map<long> FillGrid(int grid)
    {
        var map = new Map<long>(_width, _height);
        map.ForEach((x, y) => map[x, y] = FuelCellValue(grid, x, y));

        return map;
    }

    private Map<long> FillValuesFixed(Map<long> map)
    {
        var values = new Map<long>(_width, _height);
        values.ForEach((x, y) => values[x, y] = RegionValueFixed(map, x, y));
        return values;
    }

    private Map<(long, int)> FillValues(Map<long> map)
    {
        var values = new Map<(long, int)>(_width, _height);
        values.ForEach((x, y) => values[x, y] = RegionValue(map, x, y));
        return values;
    }

    private (long, int, int) MaxRegion(Map<long> values)
    {
        var max = values.MaxValue();
        var mx = 0;
        var my = 0;

        values.ForEach((x, y) =>
        {
            if (values.GetValue(x, y) == max)
            {
                mx = x;
                my = y;
            }
        });

        return (max, mx, my);
    }

    private (long, int, int, int) MaxRegion(Map<(long, int)> values)
    {
        var max = 0L;
        var mx = 0;
        var my = 0;
        var step = 0;

        values.ForEach((x, y) =>
        {
            var (value, s) = values.GetValue(x, y);
            if (value > max)
            {
                max = value;
                step = s;
                mx = x;
                my = y;
            }
        });

        return (max, mx, my, step);
    }

    private long FuelCellValue(int grid, int x, int y)
    {
        var id = x + 10L;

        var value = id * y;
        value += grid;
        value *= id;

        var power = (value / 100) % 10;
        power -= 5;

        return power;
    }

    private long RegionValueFixed(Map<long> map, int x, int y)
    {
        var sum = 0L;

        for (var j = 0; j < 3; j++)
        {
            for (var i = 0; i < 3; i++)
            {
                sum += map.GetValue(i + x, j + y);
            }
        }

        return sum;
    }

    private (long, int) RegionValue(Map<long> map, int x, int y)
    {
        var limit = _width - Math.Min(x, y);
        limit = 50;

        var max = map.GetValue(x, y);
        var sum = max;
        var step = 0;

        for (var i = 1; i <= limit; i++)
        {
            for (var nx = x; nx < x + i; nx++)
            {
                sum += map.GetValue(nx, y + i);
            }
            for (var ny = y; ny < y + i; ny++)
            {
                sum += map.GetValue(x + i, ny);
            }

            sum += map.GetValue(x + i, y + i);

            if (sum > max)
            {
                max = sum;
                step = i;
            }
        }

        return (max, step + 1);
    }

    private void TestValues()
    {
        var value = FuelCellValue(8, 3, 5);
        Log.Debug($"grid=8   value={value}");

        // Fuel cell at  122,79, grid serial number 57: power level -5
        value = FuelCellValue(57, 122, 79);
        Log.Debug($"grid=57  value={value}");

        value = FuelCellValue(39, 217, 196);
        Log.Debug($"grid=39  value={value}");

        value = FuelCellValue(71, 101, 153);
        Log.Debug($"grid=71  value={value}");
    }
    #endregion

    #region Dump
    /// <inheritdoc />
    public override void DumpInput()
    {
        DumpData();
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (!Log.EnableDebug) return;

        Debug();

        // _data.DumpCollection();
        _grids.DumpCollection();
    }
    #endregion
}

#if DUMP
#endif