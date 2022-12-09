#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2022.Days;

public class Day07 : Day
{
    private const int YEAR = 2022;
    private const int DAY = 7;

    private List<string> _data;
    private Dictionary<Guid, FileDescription> _folders;
    private FileDescription _root;

    public Day07(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "1517599";
        Expected2 = "2481982";
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
        Result = SumTo(100000L);
    }

    protected override void ComputePart2()
    {
        Result = ReduceTo(70000000L, 30000000L);
    }

    private long SumTo(long total)
    {
        var sum = 0L;

        foreach (var (_, v) in _folders)
        {
            if (v.TotalSize < total) sum += v.TotalSize;
        }

        return sum;
    }

    private long ReduceTo(long total, long target)
    {
        var free = total - _root.TotalSize;
        var rest = target - free;

        Log.Debug($"Total=[{total}]. target=[{target}]. free=[{free}]. rest=[{rest}].");

        var min = _folders.Values.Where(x => x.TotalSize > rest).Min(x => x.TotalSize);

        return min;
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        _folders = new();

        _root = CreateFolder("root");
        var current = _root;

        foreach (var line in _data)
        {
            if (line == "$ cd /")
            {
                current = _root;
            }
            else if (line == "$ cd ..")
            {
                current = current.Parent;
            }
            else if (line.StartsWith("$ cd"))
            {
                var name = line.Split(' ').LastOrDefault();
                current = current.Children.FirstOrDefault(x => x.Name == name);
            }
            else if (line.StartsWith("dir "))
            {
                var name = line.Split(' ').LastOrDefault();
                var child = CreateFolder(name, current);
                current.Children.Add(child);
            }
            else if (line.StartsWith("$ ls"))
            {
                continue;
            }
            else
            {
                var size = line.Split(" ", StringSplitOptions.RemoveEmptyEntries).FirstOrDefault().ToInt();
                current.Size += size;
            }
        }

        _root.GetTotalSize();

        // root.DumpJsonIndented();
    }

    private FileDescription CreateFolder(string name, FileDescription parent = null)
    {
        var fd = new FileDescription { Name = name, Parent = parent };
        _folders[fd.Id] = fd;
        return fd;
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (!Log.EnableDebug) return;

        Debug();

        // _data.DumpCollection();
        Log.Info("Your puzzle answer was 1517599.");
        Log.Info("Your puzzle answer was 2481982.");
    }
}

#if DUMP
#endif