#define LOGx
#define STOPWATCH

namespace Gguc.Aoc.Y2021.Days;

public class Day25 : Day
{
    private const int YEAR = 2021;
    private const int DAY = 25;

    private List<string> _source;
    private Map<char> _map;

    public Day25(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
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
        _map = new Map<char>(_source, x => x);
    }
    #endregion Parse

    protected override void ComputePart2()
    {
    }

    protected override void ComputePart1()
    {
        Result = ProcessMap(_map.Clone());
    }

    private long ProcessMap(Map<char> map)
    {
        var count = 1L;

        var isMoved = false;

        do
        {
            (isMoved, map) = Move(map);

            if (isMoved) count++;
        } while (isMoved);

        DumpMap(map);

        return count;
    }

    private (bool, Map<char>) Move(Map<char> map)
    {
        var width = map.Width;
        var height = map.Height;

        var tempMap = new Map<char>(width, height, '.');
        var newMap = new Map<char>(width, height, '.');
        var isMoved = false;

        map.ForEach((x, y) =>
        {
            if (map[x, y] == '>')
            {
                if (x >= width - 1)
                {
                    if (map[0, y] == '.')
                    {
                        isMoved = true;
                        tempMap[0, y] = '>';
                    }
                    else
                    {
                        tempMap[x, y] = '>';
                    }
                }
                else if (map[x + 1, y] == '.')
                {
                    isMoved = true;
                    tempMap[x + 1, y] = '>';
                }
                else
                {
                    tempMap[x, y] = '>';
                }
            }
            else if (map[x, y] == 'v')
            {
                tempMap[x, y] = 'v';
            }
        });

        tempMap.ForEach((x, y) =>
        {
            if (tempMap[x, y] == 'v')
            {
                if (y >= height - 1)
                {
                    if (tempMap[x, 0] == '.')
                    {
                        isMoved = true;
                        newMap[x, 0] = 'v';
                    }
                    else
                    {
                        newMap[x, y] = 'v';
                    }
                }
                else if (tempMap[x, y + 1] == '.')
                {
                    isMoved = true;
                    newMap[x, y + 1] = 'v';
                }
                else
                {
                    newMap[x, y] = 'v';
                }
            }
            else if (tempMap[x, y] == '>')
            {
                newMap[x, y] = '>';
            }
        });

        return (isMoved, newMap);
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

        DumpMap(_map);
    }

    [Conditional("LOG")]
    private void DumpMap<T>(Map<T> map)
    {
        if (!Log.EnableDebug) return;

        map.MapValueToString().Dump("Map", true);
    }
    #endregion Dump
}