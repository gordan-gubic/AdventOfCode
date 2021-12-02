#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2019.Days;

public class Day08 : Day
{
    private List<string> _source;
    private List<int> _data;
    private List<int[,]> _layers;
    private List<(int, int, int)> _layersCount;

    public Day08(ILog log, IParser parser) : base(log, parser)
    {
        EnableDebug();
        Initialize();
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Year = 2019;
        Parser.Day = 8;
        Parser.Type = ParserFileType.Real;

        _source = Parser.Parse();
    }

    /// <inheritdoc />
    protected override void ProcessData()
    {
        _data = new List<int>();
        _layers = new List<int[,]>();

        _ProcessData();
        _ProcessLayers();

    }

    /// <inheritdoc />
    public override void DumpInput()
    {
        DumpData();
    }

    protected override void ComputePart1()
    {
        CountLayers();
        FindLayer();
    }

    protected override void ComputePart2()
    {
        FlatLayers();
    }

    private void CountLayers()
    {
        _layersCount = new List<(int, int, int)>();

        foreach (var layer in _layers)
        {
            int c0, c1, c2;
            c0 = c1 = c2 = 0;

            foreach (var i in layer)
            {
                switch (i)
                {
                    case 0: c0++; break;
                    case 1: c1++; break;
                    case 2: c2++; break;
                }
            }

            _layersCount.Add((c0, c1, c2));
        }
    }

    private void FindLayer()
    {
        var min0 = Int32.MaxValue;
        var m1 = 0;
        var m2 = 0;

        foreach ((int c0, int c1, int c2) in _layersCount)
        {
            if (c0 < min0)
            {
                min0 = c0;
                m1 = c1;
                m2 = c2;
            }
        }

        Debug($"Min: {min0}, 1: {m1}, 2: {m2}");
        Result = m1 * m2;
    }

    private void FlatLayers()
    {
        var targetX = 25;
        var targetY = 6;
        var flatLayer = new int[targetX, targetY];

        for (int i = _layers.Count - 1; i >= 0; i--)
        {
            var layer = _layers[i];

            for (int y = 0; y < targetY; y++)
            {
                for (int x = 0; x < targetX; x++)
                {
                    if (layer[x, y] == 2) continue;

                    flatLayer[x, y] = layer[x, y];
                }
            }
        }

        Print(flatLayer);
    }

    private void _ProcessData()
    {
        _source[0].ForEach(x => _data.Add(x.ToString().ToInt()));
    }

    private void _ProcessLayers()
    {
        var targetX = 25;
        var targetY = 6;
        var layerSize = targetX * targetY;
        var layer = new int[targetX, targetY];
        var i = 0;

        while (i < _data.Count)
        {
            if (i % layerSize == 0)
            {
                layer = new int[targetX, targetY];
                _layers.Add(layer);
            }

            for (int y = 0; y < targetY; y++)
            {
                for (int x = 0; x < targetX; x++)
                {
                    layer[x, y] = _data[i++];
                }
            }
        }
    }

    private void Print(int[,] layer)
    {
        var sb = new StringBuilder();
        var width = layer.GetLength(0);
        var height = layer.GetLength(1);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                sb.Append(layer[x, y] == 1 ? $"{(char)2} " : "  ");
            }

            sb.AppendLine();
        }

        Console.WriteLine();
        Console.WriteLine(sb);
        Console.WriteLine();
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (!Log.EnableDebug) return;

        Debug();

        // _layers.DumpJson("_layers");
    }
}

#if DUMP

#endif