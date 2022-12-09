namespace Gguc.Aoc.Core.Services;

public abstract class Day : IDay
{
    protected Day(ILog log, IParser parser)
    {
        Log = log;
        Parser = parser;

        Log.Info($"Instantiate class [{ClassId}]. Id=[{GetHashCode()}]");
    }

    protected Day(ILog log, IParser parser, int year, int id)
    {
        Log = log;
        Parser = parser;
        Year = year;
        Id = id;

        Parser.Year = year;
        Parser.Day = id;

        Log.Info($"Instantiate class [{ClassId}]. Id=[{GetHashCode()}]");
    }

    public string ClassId => GetType().Name;

    public int Year { get; }

    public int Id { get; }

    public string Expected1 { get; set; }

    public string Expected2 { get; set; }

    protected ILog Log { get; }

    protected IParser Parser { get; }

    protected List<long> Segments { get; set; } = new List<long>();

    protected long Result { get; set; } = 0L;

    /// <inheritdoc />
    public abstract void DumpInput();

    /// <inheritdoc />
    public long SolutionPart1()
    {
        Log.DebugLog(ClassId);

        Reset();

        ComputePart1();

        return Result;
    }

    /// <inheritdoc />
    public long SolutionPart2()
    {
        Log.DebugLog(ClassId);

        Reset();

        ComputePart2();

        return Result;
    }

    protected void Initialize()
    {
        InitParser();

        ProcessData();
    }

    protected abstract void InitParser();

    protected virtual void ProcessData() { }

    protected abstract void ComputePart1();

    protected abstract void ComputePart2();

    protected void Add(long value, bool log = false)
    {
        if (log) Log.DebugLog(ClassId, $"Value=[{value}]");

        Result += value;
    }

    protected void Add(bool value, bool log = false)
    {
        if (log) Log.DebugLog(ClassId, $"Value=[{value}]");

        Result += value ? 1 : 0;
    }

    protected void Multiply(long value, bool log = false)
    {
        if (Result == 0) Result = 1;

        if (log) Log.DebugLog(ClassId, $"Value=[{value}]");

        Result *= value;
    }

    protected void Min(long value, bool log = false)
    {
        if (Result == 0) Result = value;

        if (value > Result) return;

        if (log) Log.DebugLog(ClassId, $"Value=[{value}]");

        Result = value;
    }

    protected void Max(long value, bool log = false)
    {
        if (value < Result) return;

        if (log) Log.DebugLog(ClassId, $"Value=[{value}]");

        Result = value;
    }

    protected void Reset()
    {
        Result = 0;
        Segments.Clear();
    }

    [Conditional("LOG")]
    protected void EnableDebug()
    {
        Log.EnableDebug = true;
        Log.Info($"Debug log is enabled=[{Log.EnableDebug}]");
    }

    [Conditional("LOG")]
    protected void Debug(string message = null, [CallerMemberName] string method = null)
    {
        if (!Log.EnableDebug) return;

        Log.DebugLog(ClassId, message, method);
    }

    protected void Info(string message = null, [CallerMemberName] string method = null) => Log.InfoLog(ClassId, message, method);
}
