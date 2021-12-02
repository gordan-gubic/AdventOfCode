#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2020.Days
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Gguc.Aoc.Core.Enums;
    using Gguc.Aoc.Core.Extensions;
    using Gguc.Aoc.Core.Logging;
    using Gguc.Aoc.Core.Services;

    public class Day06 : Day
    {
        private List<List<List<char>>> _source;
        private List<List<List<char>>> _data;

        public Day06(ILog log, IParser parser) : base(log, parser)
        {
            EnableDebug();
            Initialize();
        }

        /// <inheritdoc />
        protected override void InitParser()
        {
            Parser.Year = 2020;
            Parser.Day = 6;
            Parser.Type = ParserFileType.Real;

            _source = Parser.ParseBlock(Convert);
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
            foreach (var list in _data)
            {
                var buffer = list[0] as IEnumerable<char>;
                foreach (var record in list)
                {
                    buffer = buffer.Union(record);
                }

                Add(buffer.Count());
            }
        }

        protected override void ComputePart2()
        {
            foreach (var list in _data)
            {
                var buffer = list[0] as IEnumerable<char>;
                foreach (var record in list)
                {
                    buffer = buffer.Intersect(record);
                }

                Add(buffer.Count());
            }
        }

        private List<char> Convert(string input)
        {
            return input.ToList();
        }

        [Conditional("LOG")]
        private void DumpData()
        {
            Log.DebugLog(ClassId);
            
            // _data[0].Dump("Item");
            _data.DumpJson("List");
        }
    }
}

#if DUMP

#endif