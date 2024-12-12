#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2024.Days;

public class Day09 : Day
{
    private const int YEAR = 2024;
    private const int DAY = 9;

    private List<string> _data;
    private List<int> _disk;
    private List<int> _files;
    private List<FileMeta> _metas;

    public Day09(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "6448989155953";
        Expected2 = "6476642796832";
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
        Result = DefragFiles1();
    }

    protected override void ComputePart2()
    {
        Result = DefragFiles2();
    }

    private long DefragFiles1()
    {
        var files = _files.ToList();
        var stack = new Stack<int>(files.Where(x => x >= 0));
        var defrag = new List<int>();
        var size = stack.Count;

        for (var i = 0; i < size; i++)
        {
            var x = (files[i] >= 0) ? files[i] : stack.Pop();
            defrag.Add(x);
        }

        return CalculateChecksum(defrag);
    }

    private long DefragFiles2()
    {
        var defrag = new List<int>();
        var stack = new Stack<FileMeta>(_metas.Where(x => x.Value >= 0));
        var holes = _metas.Where(x => x.Value < 0).ToList();

        while (stack.TryPop(out var file))
        {
            var hole = holes.FirstOrDefault(x => x.Size >= file.Size && x.Index < file.Index);
            if (hole == null) continue;

            file.Index = hole.Index;

            hole.Size -= file.Size;
            hole.Index += file.Size;
        }

        var metas = _metas.Where(x => x.Value > 0).OrderBy(x => x.Index).ToList();
        var size = metas.Last().Index + metas.Last().Size;
        for (var i = 0; i < size; i++) defrag.Add(0);
        foreach (var meta in metas)
        {
            var begin = meta.Index;
            for (int i = 0; i < meta.Size; i++)
            {
                defrag[begin + i] = meta.Value;
            }
        }

        return CalculateChecksum(defrag);
    }

    private long CalculateChecksum(List<int> defrag)
    {
        var sum = 0L;
        
        var size = defrag.Count;

        for (var i = 0; i < size; i++)
        {
            if(defrag[i] < 0) continue;
            sum += defrag[i] * i;
        }

        return sum;
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        _disk = _data[0].Select(x => x.ToInt()).ToList();
        _files = new();
        _metas = new();

        var id = 0;
        var file = id;
        var index = 0;
        foreach (var value in _disk)
        {
            var meta = new FileMeta { Value = file, Index = index, Size = value };
            _metas.Add(meta);

            for (var i = 0; i  < value; i++)
            {
                _files.Add(file);
            }
            
            file = (file >= 0) ? -1 : ++id;
            index += value;
        }
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

        // _disk.DumpCollection("disk", true);
        // _files.DumpCollection("files", true);
        // _metas.DumpCollection("metas", true);
    }
}

#if DUMP
#endif