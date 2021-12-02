#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2020.Days
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using Gguc.Aoc.Core.Enums;
    using Gguc.Aoc.Core.Extensions;
    using Gguc.Aoc.Core.Logging;
    using Gguc.Aoc.Core.Services;
    using Gguc.Aoc.Core.Utils;

    public class Day10 : Day
    {
        private List<int> _source;
        private List<int> _data;

        private List<int> _mandatory;
        private List<int> _optionally;
        private List<List<int>> _segments;
        private List<int> _counts;

        public Day10(ILog log, IParser parser) : base(log, parser)
        {
            EnableDebug();
            Initialize();
        }

        /// <inheritdoc />
        protected override void InitParser()
        {
            Parser.Year = 2020;
            Parser.Day = 10;
            Parser.Type = ParserFileType.Real;

            _source = Parser.Parse(Converters.ToInt);
        }

        /// <inheritdoc />
        protected override void ProcessData()
        {
            _data = _source;
            _data.Sort();
        }

        /// <inheritdoc />
        public override void DumpInput()
        {
            DumpData();
        }

        protected override void ComputePart1()
        {
            var diffs = new Dictionary<int, int>();
            var prev = 0;

            diffs[1] = 0;
            diffs[3] = 1;

            foreach (var adapter in _data)
            {
                var diff = adapter - prev;
                diffs[diff]++;
                prev = adapter;
            }

            Log.DebugLog(ClassId, $"Diffs=[{diffs.ToJson()}]");

            Result = diffs[1] * diffs[3];
        }

        protected override void ComputePart2()
        {
            Result = 1;

            SortAdapters();
            SplitAdapters();

            CalculateCombinations();
        }

        private void SortAdapters()
        {
            _mandatory = new List<int>();
            _optionally = new List<int>();

            var control = new HashSet<int>(_data) { 0, _data.Max() + 3 };

            foreach (var adapter in _data)
            {
                if (control.Contains(adapter - 1) && control.Contains(adapter + 1))
                {
                    _optionally.Add(adapter);
                }
                else
                {
                    _mandatory.Add(adapter);
                }
            }

            // _mandatory.DumpJson("_mandatory");
            // _optionally.DumpJson("_optionally");
        }

        private void SplitAdapters()
        {
            _segments = new List<List<int>>();
            _counts = new List<int>();

            var prev = _optionally[0];

            void AddSegment(List<int> seg)
            {
                _segments.Add(seg.ToList());
                _counts.Add(seg.Count);
                seg.Clear();
            }

            var segment = new List<int>();
            foreach (var adapter in _optionally)
            {
                var diff = adapter - prev;

                if (diff > 1) AddSegment(segment);

                segment.Add(adapter);
                prev = adapter;
            }

            AddSegment(segment);

            // _segments.DumpJson("_segments");
            // _counts.DumpJson("_counts");
        }

        private void CalculateCombinations()
        {
            /*
             * For each segment size there is a particular number of combinations of adapters
             * that can fit the chain to keep it valid...
             *
             * size:1 -> combinations:2
             * -------
             * 0
             * 1
             * 
             * size:2 -> combinations:4
             * -------
             * 00
             * 01
             * 10
             * 11
             *
             * 
             * size:3 -> combinations:7
             * -------
             * 000 -> not allowed - bridge would be to far
             * 001
             * 010
             * 100
             * 011
             * 101
             * 110
             * 111
             *
             */

            var combos = new Dictionary<int, int>
            {
                [1] = 2,
                [2] = 4,
                [3] = 7,
            };

            foreach (var count in _counts)
            {
                Multiply(combos[count]);
            }
        }

        private string ConvertInput(string input)
        {
            return input;
        }

        [Conditional("LOG")]
        private void DumpData()
        {
            Log.DebugLog(ClassId);

            _data[0].Dump("Item");
            // _data.DumpCollection("List");
        }
    }
}

#if DUMP

#endif