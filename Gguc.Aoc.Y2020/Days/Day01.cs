#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2020.Days
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Gguc.Aoc.Core.Enums;
    using Gguc.Aoc.Core.Extensions;
    using Gguc.Aoc.Core.Logging;
    using Gguc.Aoc.Core.Services;
    using Gguc.Aoc.Core.Utils;

    public class Day01 : Day
    {
        private List<int> _source;
        private HashSet<int> _data;

        public Day01(ILog log, IParser parser) : base(log, parser)
        {
            EnableDebug();
            Initialize();
        }

        /// <inheritdoc />
        protected override void InitParser()
        {
            Parser.Year = 2020;
            Parser.Day = 1;
            Parser.Type = ParserFileType.Test;

            _source = Parser.Parse(Converters.ToInt);
        }

        /// <inheritdoc />
        protected override void ProcessData()
        {
            _data = new HashSet<int>(_source);
        }

        /// <inheritdoc />
        public override void DumpInput()
        {
            DumpData();
        }

        protected override void ComputePart1()
        {
            var tempList = new Stack<int>(_data);
            var target = 2020;

            do
            {
                var number = tempList.Pop();

                var (match, x, y) = FindMatch(target, number); 
                if (match)
                {
                    Add(x * y, true);
                    return;
                }
            } while (tempList.Count > 0);
        }

        protected override void ComputePart2()
        {
            var tempList1 = new Stack<int>(_data);

            do
            {
                var number1 = tempList1.Pop();
                var target = 2020 - number1;

                var tempList2 = new Stack<int>(tempList1);

                do
                {
                    var number = tempList2.Pop();

                    var (match, x, y) = FindMatch(target, number);
                    if (match)
                    {
                        Add(number1 * x * y, true);
                        return;
                    }
                } while (tempList2.Count > 0);
            } while (tempList1.Count > 0);
        }

        private (bool, int, int) FindMatch(int target, int number)
        {
            var diff = target - number;

            if (_data.Contains(diff))
            {
                Console.WriteLine($"{number}, {diff}, {number + diff}, {number * diff}");
                return (true, number, diff);
            }

            return (false, 0, 0);
        }

        [Conditional("LOG")]
        private void DumpData()
        {
            Log.DebugLog(ClassId);

            if (_data == null)
            {
                Log.WarnLog(ClassId, $"Data is NULL!");
                return;
            }

            if (_data.Count == 0)
            {
                Log.WarnLog(ClassId, $"Data is EMPTY!");
                return;
            }

            // _data[0].Dump("Item");
            _data.DumpCollection("List");
        }
    }
}

#if DUMP

#endif