#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2020.Days
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Gguc.Aoc.Core.Enums;
    using Gguc.Aoc.Core.Extensions;
    using Gguc.Aoc.Core.Logging;
    using Gguc.Aoc.Core.Models;
    using Gguc.Aoc.Core.Services;
    using Gguc.Aoc.Y2020.Enums;

    public class Day08 : Day
    {
        private List<Instruction<Operation08>> _source;
        private List<Instruction<Operation08>> _data;

        public Day08(ILog log, IParser parser) : base(log, parser)
        {
            EnableDebug();
            Initialize();
        }

        /// <inheritdoc />
        protected override void InitParser()
        {
            Parser.Year = 2020;
            Parser.Day = 8;
            Parser.Type = ParserFileType.Real;

            _source = Parser.Parse(ConvertInput);
        }

        /// <inheritdoc />
        protected override void ProcessData()
        {
            _data = _source;
        }

        /// <inheritdoc />
        public override void DumpInput()
        {
            DumpData();
        }

        protected override void ComputePart1()
        {
            var data = _data.ToList();
            var accumulator = 0L;
            var executed = new HashSet<int>(Enumerable.Range(0, _data.Count));
            var index = 0;

            do
            {
                var instruction = data[index];
                if (!executed.Contains(index))
                {
                    Log.InfoLog(ClassId, $"Break! Index=[{index}]. Accumulator=[{accumulator}]");
                    break;
                }

                executed.Remove(index);

                ProcessInstruction(in instruction, ref accumulator, ref index);

                if (index >= data.Count)
                {
                    Log.InfoLog(ClassId, $"Correct result! Index=[{index}]. Accumulator=[{accumulator}]");
                    break;
                }
            } while (true);

            Log.DebugLog(ClassId, $"Index=[{index}]. Accumulator=[{accumulator}]");

            Result = accumulator;
        }

        protected override void ComputePart2()
        {
            var correction = new Queue<int>(Enumerable.Range(0, _data.Count));

            var data = _data.ToList();
            var accumulator = 0L;
            var executed = new HashSet<int>(Enumerable.Range(0, _data.Count));
            var index = 0;

            void Reset()
            {
                data = _data.ToList();
                accumulator = 0L;
                executed = new HashSet<int>(Enumerable.Range(0, _data.Count));
                index = 0;
                CorrectNext();
            }

            void CorrectNext()
            {
                if (correction.Count <= 0) return;

                var ix = correction.Dequeue();
                var ins = data[ix];

                if (ins.Operation == Operation08.Acc)
                {
                    CorrectNext();
                    return;
                }

                ins.Operation = (ins.Operation == Operation08.Jmp) ? Operation08.Nop : Operation08.Jmp;
                data[ix] = ins;
            }

            do
            {
                var instruction = data[index];
                if (!executed.Contains(index))
                {
                    // Log.InfoLog(ClassId, $"Break! Index=[{index}]. Accumulator=[{accumulator}]");
                    Reset();
                    continue;
                }

                executed.Remove(index);

                ProcessInstruction(in instruction, ref accumulator, ref index);

                if (index >= data.Count)
                {
                    Log.InfoLog(ClassId, $"Correct result! Index=[{index}]. Accumulator=[{accumulator}]");
                    break;
                }
            } while (true);

            Log.DebugLog(ClassId, $"Index=[{index}]. Accumulator=[{accumulator}]");

            Result = accumulator;
        }

        private void ProcessInstruction(in Instruction<Operation08> instruction, ref long accumulator, ref int index)
        {
            switch (instruction.Operation)
            {
                case Operation08.Acc:
                    accumulator += instruction.Argument;
                    index++;
                    break;

                case Operation08.Jmp:
                    index += instruction.Argument;
                    break;

                case Operation08.Nop:
                    index++;
                    break;
            }
        }

        private Instruction<Operation08> ConvertInput(string input)
        {
            var parts = input.Split();

            return new Instruction<Operation08>
            {
                Operation = Enum.Parse<Operation08>(parts[0], true),
                Argument = parts[1].ToInt()
            };
        }

        [Conditional("LOG")]
        private void DumpData()
        {
            Log.DebugLog(ClassId);

            _data[0].Dump("Item");
            _data.DumpCollection("List");
        }
    }
}

#if DUMP

#endif