#define LOGx
#define STOPWATCH

namespace Gguc.Aoc.Y2020.Days
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Gguc.Aoc.Core.Enums;
    using Gguc.Aoc.Core.Extensions;
    using Gguc.Aoc.Core.Logging;
    using Gguc.Aoc.Core.Services;

    public class Day18 : Day
    {
        private List<string> _source;
        private List<string> _data;

        private string _pattern = @".*(\([0-9 +*]+\)).*";
        private Regex _regex;

        public Day18(ILog log, IParser parser) : base(log, parser)
        {
            EnableDebug();
            Initialize();
        }

        /// <inheritdoc />
        protected override void InitParser()
        {
            Parser.Year = 2020;
            Parser.Day = 18;
            Parser.Type = ParserFileType.Real;

            _source = Parser.Parse(ConvertInput);
        }

        /// <inheritdoc />
        protected override void ProcessData()
        {
            _data = _source;

            _regex = new Regex(_pattern, RegexOptions.Compiled);
        }

        /// <inheritdoc />
        public override void DumpInput()
        {
            DumpData();
        }

        protected override void ComputePart1()
        {
            ComputeAll(Compute1);
        }

        protected override void ComputePart2()
        {
            ComputeAll(Compute2);
        }

        private void ComputeAll(Func<string, long> compute)
        {
            foreach (var line in _data)
            {
                var input = line;

                var isOk = false;
                do
                {
                    isOk = FindMatch(ref input, compute);
                } while (isOk);

                var r = compute(input);
                Add(r);

                Debug($"r=[{r}], input=[{input}]");
            }
        }

        private bool FindMatch(ref string line, Func<string, long> compute)
        {
            var m = _regex.Match(line);
            if (!m.Success) return false;

            var g = m.Groups[1].Value;
            var r = compute(g);

            // Debug($"line=[{line}], val=[{val}], gc=[{gc}, {g}]");

            line = line.Replace(g, r.ToString());

            return true;
        }

        private long Compute1(string input)
        {
            input = input.Replace("+", "%+%").Replace("*", "%*%").Trim('(', ')');
            var tokens = input.Split('%');

            var sum = tokens[0].ToLong();
            for (int i = 1; i < tokens.Length; i = i + 2)
            {
                var op = tokens[i];
                var a2 = tokens[i + 1].ToLong();

                if (op == "+") sum += a2;
                if (op == "*") sum *= a2;
            }

            // Debug($"sum=[{sum}], tokens=[{tokens.ToJson()}]");
            return sum;
        }

        private long Compute2(string input)
        {
            input = input.Replace("+", "%+%").Replace("*", "%*%").Trim('(', ')');
            var tokens = input.Split('%').ToList();

            var sum = 0L;
            var index = -1;

            do
            {
                index = tokens.IndexOf("+");
                if (index == -1) break;

                sum = tokens[index - 1].ToLong() + tokens[index + 1].ToLong();
                tokens.Insert(index - 1, sum.ToString());
                tokens.RemoveRange(index, 3);
            } while (true);

            do
            {
                index = tokens.IndexOf("*");
                if (index == -1) break;

                sum = tokens[index - 1].ToLong() * tokens[index + 1].ToLong();
                tokens.Insert(index - 1, sum.ToString());
                tokens.RemoveRange(index, 3);
            } while (true);

            Debug($"sum=[{sum}], tokens=[{tokens.ToJson()}]");
            return sum;
        }

        private string ConvertInput(string input)
        {
            return input.Replace(" ", "");
        }

        [Conditional("LOG")]
        private void DumpData()
        {
            if (!Log.EnableDebug) return;

            Debug();

            _data[0].Dump("Item");
            _data.DumpCollection("List");
        }
    }
}

#if DUMP

#endif