namespace Gguc.Aoc.Y2019.Services
{
    using System;
    using System.Collections.Generic;
    using Gguc.Aoc.Core.Extensions;
    using Gguc.Aoc.Core.Logging;
    using Gguc.Aoc.Y2019.Models;

    public class IntcodeEngine05 : IntcodeEngineBase
    {
        private long _input;
        private long _output;
        private Queue<long> _inputQueue;

        public IntcodeEngine05(List<long> data) : base(data)
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
        }

        protected override bool ProcessInstruction()
        {
            var opFull = ReadMemory(Index++);
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
                ReadInput();
            }
            else if (opCode == 4)
            {
                SetOutput();
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

        private void Compute(int opCode, ParameterType arg1Type, ParameterType arg2Type)
        {
            var x = ReadMemory(Index++);
            var y = ReadMemory(Index++);
            var dest = (int)ReadMemory(Index++);

            x = arg1Type == ParameterType.Positional ? Memory[(int)x] : x;
            y = arg2Type == ParameterType.Positional ? Memory[(int)y] : y;

            var result = opCode == 1 ? x + y : x * y;

            WriteMemory(dest, result);
        }

        private void ReadInput()
        {
            var dest = (int)ReadMemory(Index++);

            WriteMemory(dest, GetInput());
        }

        private void SetOutput()
        {
            // TraceLog.Instance.Debug($"[{Id}]: SetOutput");
            var dest = (int)ReadMemory(Index++);

            _output = ReadMemory(dest);
        }

        private void Jump(int opCode, ParameterType arg1Type, ParameterType arg2Type)
        {
            var x = ReadMemory(Index++);
            var y = ReadMemory(Index++);

            x = arg1Type == ParameterType.Positional ? Memory[(int)x] : x;
            y = arg2Type == ParameterType.Positional ? Memory[(int)y] : y;

            var result = (opCode == 5 && x != 0) || (opCode == 6 && x == 0);

            if (result) Index = (int)y;
        }

        private void Compare(in int opCode, ParameterType arg1Type, ParameterType arg2Type)
        {
            var x = ReadMemory(Index++);
            var y = ReadMemory(Index++);
            var dest = (int)ReadMemory(Index++);

            x = arg1Type == ParameterType.Positional ? Memory[(int)x] : x;
            y = arg2Type == ParameterType.Positional ? Memory[(int)y] : y;

            var result = (opCode == 7 && x < y) || (opCode == 8 && x == y);

            WriteMemory(dest, result.ToInt());
        }

        private long GetInput()
        {
            _input = _inputQueue.Dequeue();
            return _input;
        }

        private ParameterType GetParameterType(char op)
        {
            return (ParameterType)op.ToString().ToInt();
        }
    }
}
