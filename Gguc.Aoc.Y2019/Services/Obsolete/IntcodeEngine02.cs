namespace Gguc.Aoc.Y2019.Services
{
    using System.Collections.Generic;

    public class IntcodeEngine02 : IntcodeEngineBase
    {
        public IntcodeEngine02(List<long> data) : base(data)
        {
        }

        /// <inheritdoc />
        public override long Output => ReadMemory(0);

        public void Reset(int? a = null, int? b = null)
        {
            base.Reset();

            Memory[1] = a ?? Memory[1];
            Memory[2] = b ?? Memory[2];
        }

        /// <inheritdoc />
        public override void SetInput(params long[] values)
        {
        }

        /// <inheritdoc />
        public override void AddInput(long value)
        {
        }

        protected override bool ProcessInstruction()
        {
            var opCode = (int)ReadMemory(Index++);

            if (opCode == 1 || opCode == 2)
            {
                Compute(opCode);
            }
            else
            {
                // TraceLog.Instance.Warn($"Exit! Operation=[{opCode}]");
                return false;
            }

            return true;
        }

        private void Compute(int opCode)
        {
            var x = ReadMemory(Index++);
            var y = ReadMemory(Index++);
            var dest = (int)ReadMemory(Index++);

            // TraceLog.Instance.Debug($"Operation=[{opCode}], x=[{x}], x=[{y}], dest=[{dest}]");

            x = Memory[(int)x];
            y = Memory[(int)y];

            var result = opCode == 1 ? x + y : x * y;

            WriteMemory(dest, result);
        }
    }
}

#if DUMP
#endif