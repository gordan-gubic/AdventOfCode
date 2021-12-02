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

    public class Day05 : Day
    {
        private List<(long, long)> _data;

        public Day05(ILog log, IParser parser) : base(log, parser)
        {
            EnableDebug();
            Initialize();
        }

        /// <inheritdoc />
        protected override void InitParser()
        {
            Parser.Year = 2020;
            Parser.Day = 5;
            Parser.Type = ParserFileType.Test;

            _data = Parser.Parse(ConvertInput);
        }

        /// <inheritdoc />
        protected override void ProcessData()
        {
        }

        /// <inheritdoc />
        public override void DumpInput()
        {
            DumpData();
        }

        protected override void ComputePart1()
        {
            foreach (var (row, col) in _data)
            {
                var r = GetRowId(row, col);
                Max(r);
            }
        }

        protected override void ComputePart2()
        {
            foreach (var (row, col) in _data)
            {
                var r = GetRowId(row, col);
                Segments.Add(r);
            }

            DetectSeat();
        }

        private long GetRowId(long row, long col)
        {
            return row * 8 + col;
        }

        private void DetectSeat()
        {
            var list = Segments.ToList();
            list.Sort();

            var prev = 0L;

            foreach (var seat in list)
            {
                var diff = seat - prev;
                prev = seat;

                if (diff == 2)
                {
                    Result = seat - 1;
                    return;
                }
            }
        }

        private (long, long) ConvertInput(string input)
        {
            var rowString = input.Substring(0, 7).Replace("B", "1").Replace("F", "0");
            var colString = input.Substring(7, 3).Replace("R", "1").Replace("L", "0");

            var row = Convert.ToInt64(rowString, 2);
            var column = Convert.ToInt64(colString, 2);

            return (row, column);
        }

        [Conditional("LOG")]
        private void DumpData()
        {
            Log.DebugLog(ClassId);

            if (_data == null)
            {
                Log.WarnLog(ClassId, "Data is NULL!");
                return;
            }

            if (_data.Count == 0)
            {
                Log.WarnLog(ClassId, "Data is EMPTY!");
                return;
            }

            _data[0].Dump("Item");
            // _data.DumpCollection("List");
        }
    }
}

#if DUMP

#endif