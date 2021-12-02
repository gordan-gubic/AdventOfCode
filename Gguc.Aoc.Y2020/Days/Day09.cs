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
    using Gguc.Aoc.Core.Services;
    using Gguc.Aoc.Core.Utils;

    public class Day09 : Day
    {
        private List<long> _source;
        private List<long> _data;

        private int PreambleSize = 25;
        private long _target;

        public Day09(ILog log, IParser parser) : base(log, parser)
        {
            EnableDebug();
            Initialize();
        }

        /// <inheritdoc />
        protected override void InitParser()
        {
            Parser.Year = 2020;
            Parser.Day = 9;
            Parser.Type = ParserFileType.Real;

            _source = Parser.Parse(Converters.ToLong);
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
            var preamble = _data.Take(PreambleSize).ToList();
            var queue = new Queue<long>(_data.TakeLast(_data.Count - PreambleSize));

            do
            {
                var target = queue.Dequeue();

                var match = FindMatchInList(preamble, target);

                if (!match)
                {
                    Log.InfoLog(ClassId, $"Break: {target}");
                    Result = target;
                    break;
                }

                preamble.RemoveAt(0);
                preamble.Add(target);
            } while (queue.Count > 0);

            _target = Result;
        }

        protected override void ComputePart2()
        {
            var target = _target;

            var list = _data.ToList();

            do
            {
                var (match, x, y) = FindSum(list, target);

                if (match)
                {
                    Result = x + y;
                    Log.DebugLog(ClassId, $"Match: {x}, {y}, Sum=[{Result}]");
                    break;
                }

                list.RemoveAt(0);
            } while (list.Count > 0);
        }

        private (bool, long, long) FindSum(List<long> input, in long target)
        {
            var list = new List<long>();

            foreach (var i in input)
            {
                list.Add(i);

                var sum = list.Sum();

                if (sum == target)
                {
                    return (true, list.Min(), list.Max());
                }
                
                if (sum > target)
                {
                    return (false, -1, -1);
                }
            }

            return (false, -1, -1);
        }

        private bool FindMatchInList(List<long> input, long target)
        {
            var match = false;

            foreach (var number in input)
            {
                (match, _, _) = FindMatch(input, target, number);

                if (match) break;
            }

            return match;
        }

        private (bool, long, long) FindMatch(List<long> data, long target, long number)
        {
            var diff = target - number;

            if (data.Contains(diff))
            {
                // Log.DebugLog(ClassId, $"{number}, {diff}, {number + diff}, {number * diff}");
                return (true, number, diff);
            }

            return (false, 0, 0);
        }

        [Conditional("LOG")]
        private void DumpData()
        {
            Log.DebugLog(ClassId);

            _data[0].Dump("Item");
            _data.DumpJson("List");
        }
    }
}

#if DUMP

#endif