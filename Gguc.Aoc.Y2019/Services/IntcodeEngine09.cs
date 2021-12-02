#define LOGx

namespace Gguc.Aoc.Y2019.Services;

public class IntcodeEngine09 : IntcodeEngineBase
{
    private long _input;
    private long _output;
    private Queue<long> _inputQueue;
    private IIntcodeEngine _outputEngine;

    public IntcodeEngine09(List<long> data) : base(data)
    {
    }

    public override long Output => _output;

    public override void SetInput(params long[] values)
    {
        _inputQueue = new Queue<long>(values);
    }

    public override void AddInput(long value)
    {
        // Debug($"[{Id}]: AddInput: [{value}]");
        _inputQueue.Enqueue(value);

        if (!IsRunning) Run();
    }

    public void SetOutputEngine(IIntcodeEngine engine)
    {
        _outputEngine = engine;
    }

    protected override bool ProcessInstruction()
    {
        var opFull = ReadNextMemory();
        var opcodeText = $"{opFull}".PadLeft(5, '0');

        var opCode = opcodeText.Substring(3, 2).ToInt();

        var argType1 = GetParameterType(opcodeText[2]);
        var argType2 = GetParameterType(opcodeText[1]);
        var argType3 = GetParameterType(opcodeText[0]);

        Debug($"Opcode=[{opcodeText}], Operation=[{opCode}], a1=[{argType1}], a2=[{argType2}], a3=[{argType3}], Index=[{Index}], Relative=[{Relative}]");
        // Debug($"...Memory=[{Memory.ToJson()}]");

        if (opCode == 1 || opCode == 2)
        {
            Compute(opCode, argType1, argType2, argType3);
        }
        else if (opCode == 3)
        {
            ReadInput(argType1);
        }
        else if (opCode == 4)
        {
            SetOutput(argType1);
        }
        else if (opCode == 5 || opCode == 6)
        {
            Jump(opCode, argType1, argType2);
        }
        else if (opCode == 7 || opCode == 8)
        {
            Compare(opCode, argType1, argType2, argType3);
        }
        else if (opCode == 9)
        {
            SetRelative(argType1);
        }
        else
        {
            if (opCode != 99) TraceLog.Instance.Warn($"Exit! Operation=[{opCode}]");
            return false;
        }

        return true;
    }

    private void Compute(int opCode, ArgType argType1, ArgType argType2, ArgType argType3)
    {
        var x = ReadNextMemory(argType1);
        var y = ReadNextMemory(argType2);
        var dest = ReadNextDestination(argType3);

        var result = opCode == 1 ? x + y : x * y;

        Debug($"...Compute: x={x} y={y} d={dest} r={result}");
        WriteMemory(dest, result);
    }

    private void ReadInput(ArgType argType1)
    {
        if (_inputQueue.Count == 0)
        {
            Index--;
            Stop();
            return;
        }

        var input = GetInput();
        var dest = (int)ReadNextMemory();

        if (argType1 == ArgType.Relative) dest += (int)Relative;

        Debug($"...ReadInput: i={input} d={dest}");
        WriteMemory(dest, input);
    }

    private void SetOutput(ArgType argType1)
    {
        // Debug($"[{Id}]: SetOutput");
        var value = ReadNextMemory(argType1);

        _output = value;
        Info($"Output=[{_output}]");

        _outputEngine?.AddInput(_output);
    }

    private void Jump(int opCode, ArgType argType1, ArgType argType2)
    {
        var x = ReadNextMemory(argType1);
        var y = ReadNextMemory(argType2);

        var result = (opCode == 5 && x != 0) || (opCode == 6 && x == 0);

        Debug($"...Jump: x={x} y={y} r={result}");
        if (result) Index = (int)y;
    }

    private void Compare(in int opCode, ArgType argType1, ArgType argType2, ArgType argType3)
    {
        var x = ReadNextMemory(argType1);
        var y = ReadNextMemory(argType2);
        var dest = ReadNextDestination(argType3);

        var result = (opCode == 7 && x < y) || (opCode == 8 && x == y);

        Debug($"...Compare: x={x} y={y} r={result}");
        WriteMemory(dest, result.ToInt());
    }

    private void SetRelative(ArgType argType1)
    {
        var x = ReadNextMemory(argType1);

        Relative += x;

        Debug($"...SetRelative: x={x} r={Relative}");
    }

    private long GetInput()
    {
        _input = _inputQueue.Dequeue();
        return _input;
    }
}
