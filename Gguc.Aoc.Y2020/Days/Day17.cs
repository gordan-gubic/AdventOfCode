#define LOGx
#define STOPWATCH

namespace Gguc.Aoc.Y2020.Days
{
    using System;
    using System.Diagnostics;
    using Gguc.Aoc.Core.Enums;
    using Gguc.Aoc.Core.Logging;
    using Gguc.Aoc.Core.Services;
    using Gguc.Aoc.Y2020.Models;

    public class Day17 : Day
    {
        private bool[,] _source;
        private int _cycles = 6;

        public Day17(ILog log, IParser parser) : base(log, parser)
        {
            EnableDebug();
            Initialize();
        }

        /// <inheritdoc />
        protected override void InitParser()
        {
            Parser.Year = 2020;
            Parser.Day = 17;
            Parser.Type = ParserFileType.Real;

            _source = Parser.ParseMap();
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
            var space = InitSpace3d();

            space.ActivateSpaceForCycles(_cycles);

            Result = space.CountValues();
        }

        protected override void ComputePart2()
        {
            var space = InitSpace4d();

            space.ActivateSpaceForCycles(_cycles);

            Result = space.CountValues();
        }

        private ILiveSpace InitSpace3d()
        {
            var l0 = _source.GetLength(0) + _cycles * 2;
            var l1 = _source.GetLength(1) + _cycles * 2;
            var l2 = _cycles * 2 + 1;

            var space = new LiveSpace3d(l0, l1, l2);
            space.Init(_source, _cycles);
            return space;
        }

        private ILiveSpace InitSpace4d()
        {
            var l0 = _source.GetLength(0) + _cycles * 2;
            var l1 = _source.GetLength(1) + _cycles * 2;
            var l2 = _cycles * 2 + 1;
            var l3 = _cycles * 2 + 1;

            var space = new LiveSpace4d(l0, l1, l2, l3);
            space.Init(_source, _cycles);
            return space;
        }

        [Conditional("LOGx")]
        private void DumpData()
        {
            if (!Log.EnableDebug) return;

            Debug();

            // _data.DumpSpace("Space");
        }
    }
}

#if DUMP

#endif