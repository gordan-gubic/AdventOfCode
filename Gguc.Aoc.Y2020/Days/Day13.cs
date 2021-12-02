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

    public class Day13 : Day
    {
        private List<string> _source;
        private List<long> _data;

        private Dictionary<long, long> _buses;
        private long _departure;

        public Day13(ILog log, IParser parser) : base(log, parser)
        {
            EnableDebug();
            Initialize();
        }

        /// <inheritdoc />
        protected override void InitParser()
        {
            Parser.Year = 2020;
            Parser.Day = 13;
            Parser.Type = ParserFileType.Real;

            _source = Parser.Parse();
        }

        /// <inheritdoc />
        protected override void ProcessData()
        {
            _data = new List<long>();
            _departure = _source[0].ToLong();
            _source[1].Split(',', StringSplitOptions.RemoveEmptyEntries).ToList().ForEach(x => _data.Add(x.ToLong()));

            _buses = _data.Where(x => x > 0).ToDictionary(x => x, _ => 0L);
        }

        /// <inheritdoc />
        public override void DumpInput()
        {
            DumpData();
        }

        protected override void ComputePart1()
        {
            Clear();
            FillValuesForDepartureTime();
            
            var min = _buses.OrderBy(k => k.Value).FirstOrDefault();
            Result = min.Key * min.Value;
        }

        protected override void ComputePart2()
        {
            Clear();
            FillValuesForOffsetTime();

            var queue = new Queue<(long Bus, long Offset)>();
            _buses.OrderByDescending(k => k.Key).ForEach(x => queue.Enqueue((x.Key, x.Value)));

            var temp = new Dictionary<long, long>();

            var bus = queue.Dequeue();
            temp[bus.Bus] = bus.Offset;
           
            var step = bus.Bus;
            var offset = bus.Offset;
            var timestamp = 0L;

            while (queue.Count > 0)
            {
                bus = queue.Dequeue();
                temp[bus.Bus] = bus.Offset;

                timestamp = CalculateTimeAndOffset(temp, ref step, ref offset);
            }

            Result = timestamp;
        }

        private long CalculateTimeAndOffset(Dictionary<long, long> temp, ref long step, ref long offset)
        {
            var valid = new List<long>();
            var time = -offset;
           
            do
            {
                time += step;

                var isOk = VerifyTimestamp(temp, time);

                if (isOk)
                {
                    valid.Add(time);
                    if (valid.Count > 2) break;
                }
            } while (true);

            time = valid[0];
            step = valid[1] - time;
            offset = step - time;

            return time;
        }

        private void Clear()
        {
            _buses.SetAll();
        }

        private void FillValuesForDepartureTime()
        {
            _buses.ForEach((k, v) => _buses[k] = k - _departure % k);

            _buses.DumpJson("FillValuesForDepartureTime");
        }

        private void FillValuesForOffsetTime()
        {
            var offset = 0L;

            foreach (var bus in _data)
            {
                if (bus == 0)
                {
                    offset++;
                    continue;
                }

                _buses[bus] = offset++;
            }

            _buses.DumpJson("FillValuesForOffsetTime");
        }

        private bool VerifyTimestamp(IDictionary<long, long> buses, in long time)
        {
            foreach (var bus in buses)
            {
                if ((time + bus.Value) % bus.Key != 0) return false;
            }

            // Log.InfoLog(ClassId, $"Found! Timestamp=[{time}]");
            return true;
        }

        [Conditional("LOG")]
        private void DumpData()
        {
            Log.DebugLog(ClassId);

            _departure.Dump("_departure");
            _data.DumpJson("_data");
            _buses.DumpJson("_buses");
        }
    }
}

#if DUMP

#endif