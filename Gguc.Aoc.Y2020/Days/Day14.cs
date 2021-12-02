#define LOGx
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

    public class Day14 : Day
    {
        private List<string> _source;
        private List<(long Key, string Value)> _data;

        private Dictionary<long, long> _memory;
        private Dictionary<int, bool?> _mask;
        private List<Dictionary<int, bool?>> _maskList;

        public Day14(ILog log, IParser parser) : base(log, parser)
        {
            EnableDebug();
            Initialize();
        }

        /// <inheritdoc />
        protected override void InitParser()
        {
            Parser.Year = 2020;
            Parser.Day = 14;
            Parser.Type = ParserFileType.Real;

            _source = Parser.Parse();
        }

        /// <inheritdoc />
        protected override void ProcessData()
        {
            _data = new List<(long, string)>();

            _source.ForEach(ProcessDataValue);

            _memory = new Dictionary<long, long>();
            _mask = new Dictionary<int, bool?>();
            _maskList = new List<Dictionary<int, bool?>>();
        }

        /// <inheritdoc />
        public override void DumpInput()
        {
            DumpData();
        }

        protected override void ComputePart1()
        {
            _memory.Clear();

            foreach (var d in _data)
            {
                if (d.Key == -1)
                {
                    WriteMask(d.Value, 'X');
                    continue;
                }

                WriteMemory1(d.Key, d.Value);
            }

            _memory.ForEach((k, v) => Add(v));

            Debug($"_memory=[{_memory.ToJson()}]");
        }

        protected override void ComputePart2()
        {
            _memory.Clear();

            StopwatchUtil.StartStopwatch();
            foreach (var d in _data)
            {
                if (d.Key == -1)
                {
                    WriteMask(d.Value, '0');
                    WriteMaskLists();
                    continue;
                }

                WriteMemory2(d.Key, d.Value);
            }
            StopwatchUtil.StopStopwatch();

            _memory.ForEach((k, v) => Add(v));

            Debug($"_memory=[{_memory.ToJson()}]");
        }

        private void WriteMask(string value, params char[] ignore)
        {
            _mask.Clear();

            var maskValue = value.ReverseString();
            for (int i = 0; i < maskValue.Length; i++)
            {
                if (ignore.Contains(maskValue[i])) continue;

                switch (maskValue[i])
                {
                    case 'X':
                        _mask[i] = null;
                        break;

                    default:
                        _mask[i] = maskValue[i].ToBool();
                        break;
                }
            }

            Debug($"MaskValue=[{maskValue}]. Mask=[{_mask.ToJson()}]");
        }

        private void WriteMaskLists()
        {
            _maskList.Clear();
            var xcount = _mask.Count(k => !(k.Value ?? false));
            var xcombos = Math.Pow(2, xcount);
            Debug($"mask=[{_mask.ToJson()}]. xcount=[{xcount}]. xcombos=[{xcombos}]");

            for (int i = 0; i < xcombos; i++)
            {
                var xcombo = Convert.ToString(i, 2).PadLeft(xcount, '0');
                _maskList.Add(WriteMaskList(xcombo));
            }
        }

        private Dictionary<int, bool?> WriteMaskList(string xcombo)
        {
            var newMask = _mask.Copy();

            int xindex = 0;
            foreach (var k in _mask.Keys)
            {
                if (newMask[k] ?? false) continue;

                newMask[k] = xcombo[xindex++].ToBool();
            }

            Debug($"xcombo=[{xcombo}]. mask=[{_mask.ToJson()}]. newMask=[{newMask.ToJson()}]");

            return newMask;
        }

        private void WriteMemory1(long address, string input)
        {
            var value = input.ToLong();

            _memory[address] = ApplyMask(value, _mask);

            Debug($"Memory1: Address=[{address}], Value=[{_memory[address]}]");
        }

        private void WriteMemory2(long address, string input)
        {
            var value = input.ToLong();

            foreach (var mask in _maskList)
            {
                var newAddress = ApplyMask(address, mask);
                _memory[newAddress] = value;
                Debug($"Memory2: Address=[{newAddress}], Value=[{_memory[newAddress]}]");
            }
        }

        private long ApplyMask(long input, Dictionary<int, bool?> mask)
        {
            var binary = input.ToBinaryString().PadLeft(36, '0').Reverse().ToList();
            Debug($"[{input}]=>[{binary.ToJson()}] mask={mask.ToJson()}");

            mask.ForEach((k, v) => binary[k] = v ?? false ? '1' : '0');

            binary.Reverse();
            var value = binary.JoinToString().FromBinaryStringToLong();

            Debug($"[{binary.ToJson()}] => [{value}]");
            return value;
        }

        private void ProcessDataValue(string input)
        {
            var parts = input.Split(new[] { ' ', '=' }, StringSplitOptions.RemoveEmptyEntries);

            var key = parts[0].MatchGroup(@"mem\[(\d+)\]").ToInt(-1);

            _data.Add((key, parts[1]));
        }

        [Conditional("LOG")]
        private void DumpData()
        {
            if (!Log.EnableDebug) return;

            Debug();

            _data[0].Dump("mask");
            _data.DumpCollection("List");
        }
    }
}

#if DUMP
#endif