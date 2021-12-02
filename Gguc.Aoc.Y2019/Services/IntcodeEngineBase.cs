#define LOGx

namespace Gguc.Aoc.Y2019.Services;

public abstract class IntcodeEngineBase : IIntcodeEngine
{
    protected readonly List<long> Data;
    protected readonly int Size;

    public List<long> Memory;
    protected int Index;
    protected long Relative;

    private bool _stopSignal;

    protected IntcodeEngineBase(List<long> data)
    {
        Id = GetHashCode();

        Data = data;
        Size = data.Count;
        Reset();

        // Debug($"[{Id}]: Ctor");
    }

    protected string ClassId => GetType().Name;

    public int Id { get; }

    public bool IsRunning { get; private set; }

    /// <inheritdoc />
    public abstract long Output { get; }

    public virtual void Reset()
    {
        Index = 0;
        Memory = Data.ToList();
    }

    public long Run()
    {
        IsRunning = true;

        while (Index < Size)
        {
            var success = ProcessInstruction();

            if (!success) break;
            if (ShouldStop()) break;
        }

        IsRunning = false;
        return Output;
    }

    /// <inheritdoc />
    public abstract void SetInput(params long[] values);

    /// <inheritdoc />
    public abstract void AddInput(long value);

    /// <inheritdoc />
    public long ReadMemory(int index)
    {
        if (index >= Memory.Count)
        {
            Memory.AddRange(Enumerable.Repeat(0L, index));
        }

        var response = Memory.GetValueSafe(index);
        Debug($"... ReadMemory: index: {index}, response: {response}");

        return response;
    }

    /// <inheritdoc />
    public void WriteMemory(int index, long value)
    {
        if (index >= Memory.Count)
        {
            Memory.AddRange(Enumerable.Repeat(0L, index));
        }

        Memory[index] = value;
    }

    protected long ReadNextMemory(ArgType type = ArgType.Immediate)
    {
        var value = ReadMemory(Index++);

        var x = (int)value;
        var y = (int)value + (int)Relative;

        var response = type switch
        {
            ArgType.Positional => ReadMemory((int)value),
            ArgType.Relative => ReadMemory((int)value + (int)Relative),
            _ => value
        };

        Debug($"... ReadNextMemory: value: {value}, x: {x}, y: {y}, type: {type}, response: {response}");

        return response;
    }

    protected int ReadNextDestination(ArgType argType3)
    {
        var dest = (int)ReadNextMemory();
        if (argType3 == ArgType.Relative) dest += (int)Relative;
        return dest;
    }

    protected abstract bool ProcessInstruction();

    protected void Stop()
    {
        _stopSignal = true;
    }

    protected ArgType GetParameterType(char op)
    {
        return (ArgType)op.ToString().ToInt();
    }

    private bool ShouldStop()
    {
        if (!_stopSignal) return false;

        _stopSignal = !_stopSignal;
        return true;
    }

    /// <inheritdoc />
    public override string ToString() => Memory.ToJson();

    [Conditional("LOG")]
    protected void Debug(string message = null, [CallerMemberName] string method = null)
    {
        if (!TraceLog.Instance.EnableDebug) return;

        TraceLog.Instance.DebugLog(ClassId, message, method);
    }

    protected void Info(string message = null, [CallerMemberName] string method = null) => TraceLog.Instance.InfoLog(ClassId, message, method);

}
