namespace Gguc.Aoc.Y2019.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using Gguc.Aoc.Core.Extensions;
    using Gguc.Aoc.Core.Logging;
    using Gguc.Aoc.Y2019.Models;
    using NLog.Fluent;

    public class IntcodeEngine : IntcodeEngineBase
    {
        private const string ClassId = nameof(IntcodeEngine);

        private long _input;
        private long _output;
        private Queue<long> _inputQueue;
        private IIntcodeEngine _outputEngine;

        public IntcodeEngine(List<long> data) : base(data)
        {
        }

        public override long Output => _output;

        public override void SetInput(params long[] values)
        {
            _inputQueue = new Queue<long>(values);
        }

        public override void AddInput(long value)
        {
            // TraceLog.Instance.Debug($"[{Id}]: AddInput: [{value}]");
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

            var arg1Type = GetParameterType(opcodeText[2]);
            var arg2Type = GetParameterType(opcodeText[1]);
            var arg3Type = GetParameterType(opcodeText[0]);

            // TraceLog.Instance.Debug($"Opcode=[{opcodeText}], Operation=[{opCode}], arg1type=[{arg1Type}], arg2type=[{arg2Type}], arg3type=[{arg3Type}]");

            if (opCode == 1 || opCode == 2)
            {
                Compute(opCode, arg1Type, arg2Type);
            }
            else if (opCode == 3)
            {
                ReadInput(arg1Type);
            }
            else if (opCode == 4)
            {
                SetOutput(arg1Type);
            }
            else if (opCode == 5 || opCode == 6)
            {
                Jump(opCode, arg1Type, arg2Type);
            }
            else if (opCode == 7 || opCode == 8)
            {
                Compare(opCode, arg1Type, arg2Type);
            }
            else
            {
                if (opCode != 99) TraceLog.Instance.Warn($"Exit! Operation=[{opCode}]");
                return false;
            }

            return true;
        }

        private void Compute(int opCode, ArgType arg1Type, ArgType arg2Type)
        {
            var x = ReadNextMemory(arg1Type);
            var y = ReadNextMemory(arg2Type);
            var dest = (int)ReadNextMemory();

            var result = opCode == 1 ? x + y : x * y;

            WriteMemory(dest, result);
        }

        private void ReadInput(ArgType arg1Type)
        {
            if (_inputQueue.Count == 0)
            {
                Index--;
                Stop();
                return;
            }

            var dest = (int)ReadNextMemory();

            // TODO: WriteMemory(dest, GetInput(), arg1Type);
            WriteMemory(dest, GetInput());
        }

        private void SetOutput(ArgType arg1Type)
        {
            // TraceLog.Instance.Debug($"[{Id}]: SetOutput");
            var value = ReadNextMemory(arg1Type);

            _output = value;

            _outputEngine?.AddInput(_output);
        }

        private void Jump(int opCode, ArgType arg1Type, ArgType arg2Type)
        {
            var x = ReadNextMemory(arg1Type);
            var y = ReadNextMemory(arg2Type);

            var result = (opCode == 5 && x != 0) || (opCode == 6 && x == 0);

            if (result) Index = (int)y;
        }

        private void Compare(in int opCode, ArgType arg1Type, ArgType arg2Type)
        {
            var x = ReadNextMemory(arg1Type);
            var y = ReadNextMemory(arg2Type);
            var dest = (int)ReadNextMemory();

            var result = (opCode == 7 && x < y) || (opCode == 8 && x == y);

            WriteMemory(dest, result.ToInt());
        }

        private long GetInput()
        {
            _input = _inputQueue.Dequeue();
            return _input;
        }

        private ArgType GetParameterType(char op)
        {
            return (ArgType)op.ToString().ToInt();
        }
    }
}
