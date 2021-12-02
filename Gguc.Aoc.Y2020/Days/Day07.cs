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
    using Gguc.Aoc.Y2020.Models;

    public class Day07 : Day
    {
        private List<string> _source;
        private List<Bag> _data = new List<Bag>();
        private Dictionary<string, Bag> _bags = new Dictionary<string, Bag>();

        public Day07(ILog log, IParser parser) : base(log, parser)
        {
            EnableDebug();
            Initialize();
        }

        /// <inheritdoc />
        protected override void InitParser()
        {
            Parser.Year = 2020;
            Parser.Day = 7;
            Parser.Type = ParserFileType.Real;

            _source = Parser.Parse(ConvertInput);
        }

        /// <inheritdoc />
        protected override void ProcessData()
        {
            foreach (var record in _source)
            {
                var parts = record.Split(new char[]{' ', ':', ',' }, StringSplitOptions.RemoveEmptyEntries);
                // parts.DumpJson();
                
                var bag = ProcessParts(parts);
                _data.Add(bag);
                _bags[bag.Id] = bag;
            }

            foreach (var bag in _data)
            {
                ProcessBags(bag);
            }
        }

        /// <inheritdoc />
        public override void DumpInput()
        {
            // DumpData();
        }

        protected override void ComputePart1()
        {
            ResetValues();

            var id = "shiny-gold";
            // var id = "faded-violet";
            // var id = "dark-violet";

            ValueContain(id);

            // DumpData();
        }

        protected override void ComputePart2()
        {
            ResetValues();

            var id = "shiny-gold";
            // var id = "faded-violet";
            // var id = "dark-violet";

            ValueContent(id);
            // ValueSum(id);

            // DumpData();
        }

        private void ResetValues()
        {
            foreach (var bag in _data)
            {
                bag.Value = -1;
            }
        }

        private Bag ProcessParts(string[] parts)
        {
            var stack = new Queue<string>(parts);

            var bag = new Bag(stack.Dequeue(), stack.Dequeue());
            bag.Raw = new List<string>(stack);

            return bag;
        }

        private void ProcessBags(Bag bag)
        {
            var stack = new Queue<string>(bag.Raw);

            do
            {
                stack.TryDequeue(out var x);
                stack.TryDequeue(out var y);
                stack.TryDequeue(out var z);
                var count = x.ToInt();

                if (count > 0)
                {
                    var child = FindBag(y, z);
                    bag.Items[child] = count;
                }
            } while (stack.Count > 0);
        }

        private Bag FindBag(string name1, string name2)
        {
            var id = $"{name1}-{name2}";

            if (!_bags.ContainsKey(id))
            {
                Log.WarnLog(ClassId, $"No bag [{id}]");
                return null;
            }

            return _bags[id];
        }

        private void ValueContain(string bagId)
        {
            foreach (var bag in _data)
            {
                if (bag.Value != -1) continue;

                bag.Value = CalculateContain(bag, bagId);
            }

            foreach (var bag in _data)
            {
                Result += bag.Value;
            }
        }

        private void ValueContent(string bagId)
        {
            var bag = _bags[bagId];

            bag.Value = CalculateContent(bag);

            Result = bag.Value;
        }

        private void ValueSum(string bagId)
        {
            foreach (var bag in _data)
            {
                if (bag.Value != -1) continue;

                bag.Value = CalculateSum(bag, bagId);
            }

            foreach (var bag in _data)
            {
                Result += bag.Value;
            }
        }

        private int CalculateContain(Bag bag, string bagId)
        {
            if (bag.Value != -1) return bag.Value;

            var value = 0;
            foreach (var child in bag.Items)
            {
                if (child.Key.Id == bagId)
                {
                    value = 1;
                    break;
                }

                value = CalculateContain(child.Key, bagId);

                if (value > 0)
                {
                    break;
                }
            }

            bag.Value = value;
            return value;
        }

        private int CalculateContent(Bag bag)
        {
            if (bag.Value != -1) return bag.Value;

            var sum = 0;
            foreach (var child in bag.Items)
            {
                sum += child.Value * (1 + CalculateContent(child.Key));
            }
            
            bag.Value = sum;
            return sum;
        }

        private int CalculateSum(Bag bag, string bagId)
        {
            if (bag.Value != -1) return bag.Value;

            var sum = 0;
            foreach (var child in bag.Items)
            {
                if (child.Key.Id == bagId)
                {
                    sum += child.Value;
                    continue;
                }

                sum += child.Value * CalculateSum(child.Key, bagId);
            }

            bag.Value = sum;
            return sum;
        }

        private string ConvertInput(string input)
        {
            return input
                .Replace("no other bags", "0")
                .Replace("bags", "")
                .Replace("bag", "")
                .Replace("contain", ":")
                .Trim('.', ' ');
        }

        [Conditional("LOG")]
        private void DumpData()
        {
            Log.DebugLog(ClassId);

            // _data[0].Dump("Item");
            // _data.DumpCollection("List");
            // _data.DumpJson("List");

            foreach (var bag in _data)
            {
                Log.DebugLog(ClassId, bag.ToString());
            }
        }
    }
}

#if DUMP
#endif