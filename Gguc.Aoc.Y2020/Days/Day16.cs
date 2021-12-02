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
    using Gguc.Aoc.Y2020.Models;

    public class Day16 : Day
    {
        private List<string> _source;
        private List<List<string>> _data;

        private Dictionary<string, TicketRule> _ticketRules;
        private List<long> _ticket;
        private List<List<long>> _nearbyTickets;
        private List<List<long>> _validTickets;

        public Day16(ILog log, IParser parser) : base(log, parser)
        {
            EnableDebug();
            Initialize();
        }

        /// <inheritdoc />
        protected override void InitParser()
        {
            Parser.Year = 2020;
            Parser.Day = 16;
            Parser.Type = ParserFileType.Real;

            _source = Parser.Parse();
        }

        /// <inheritdoc />
        protected override void ProcessData()
        {
            _ProcessData();
        }

        /// <inheritdoc />
        public override void DumpInput()
        {
            DumpData();
        }

        protected override void ComputePart1()
        {
            foreach (var tickets in _nearbyTickets)
            {
                foreach (var ticket in tickets)
                {
                    var isValid = ValidateTicket(ticket);
                    if (!isValid) Add(ticket);
                }
            }
        }

        protected override void ComputePart2()
        {
            ValidateTickets2();

            ComputeFields();

            _ticketRules.DumpJson("_ticketRules");

            var target = "departure";
            var rules = _ticketRules.Where(x => x.Value.Name.StartsWith(target)).Select(x => x.Value.Index);
            rules.DumpJson();

            rules.ForEach(x => Multiply(_ticket[x]));

        }

        private void ValidateTickets2()
        {
            _validTickets = new List<List<long>>();

            foreach (var tickets in _nearbyTickets)
            {
                var isValidList = new List<bool>();

                foreach (var ticket in tickets)
                {
                    var isValid = ValidateTicket(ticket);
                    isValidList.Add(isValid);
                }

                if (isValidList.All(x => x))
                {
                    _validTickets.Add(tickets);
                }
            }

            _validTickets.DumpJson("_validTickets");
        }

        private bool ValidateTicket(in long ticket)
        {
            var isValidList = new List<bool>();

            foreach (var rule in _ticketRules.Values)
            {
                var valid = ticket.IsBetween(rule.ValueA1, rule.ValueA2)
                           || ticket.IsBetween(rule.ValueB1, rule.ValueB2);
                isValidList.Add(valid);
            }

            var isValid = isValidList.Any(x => x);
            // Debug($"Ticket=[{ticket}]. IsValid=[{isValid}]");
            return isValid;
        }

        private void ComputeFields()
        {
            var allfields = new Dictionary<long, List<string>>();

            void AddField(int index, List<string> fields)
            {
                if (allfields.ContainsKey(index))
                {
                    fields = fields.Intersect(allfields[index]).ToList();
                }

                allfields[index] = fields;
            }

            foreach (var ticket in _validTickets)
            {
                for (int i = 0; i < ticket.Count; i++)
                {
                    var fields = DetectField(ticket[i]);
                    // Debug($"I=[{i}] Ticket=[{ticket[i]}]. fields=[{fields.ToJson()}]");
                    AddField(i, fields);
                }
            }

            allfields.DumpJson("allfields");

            ReduceFields(allfields);
        }

        private void ReduceFields(Dictionary<long, List<string>> fields)
        {
            var allkeys = new HashSet<long>(fields.Keys);

            while (allkeys.Count > 0)
            {
                var f = fields.Where(x => x.Value.Count == 1).Select(x => (x.Key, Value: x.Value[0])).FirstOrDefault();

                _ticketRules[f.Value].Index = (int)f.Key;

                allkeys.Remove(f.Key);
                fields.Remove(f.Key);
                fields.ForEach((k, v) => v.Remove(f.Value));
            }
        }

        private List<string> DetectField(in long ticket)
        {
            var validFields = new List<string>();

            foreach (var rule in _ticketRules.Values)
            {
                var valid = ticket.IsBetween(rule.ValueA1, rule.ValueA2)
                            || ticket.IsBetween(rule.ValueB1, rule.ValueB2);

                if (valid)
                {
                    validFields.Add(rule.Name);
                }
            }

            return validFields;
        }

        private void _ProcessData()
        {
            _data = new List<List<string>>();

            var block = new List<string>();

            void AddBlock()
            {
                _data.Add(block);
                block = new List<string>();
            }

            foreach (var s in _source)
            {
                if (s.IsWhitespace())
                {
                    AddBlock();
                    continue;
                }

                block.Add(s);
            }

            AddBlock();

            _data[1].RemoveAt(0);
            _data[2].RemoveAt(0);

            _ticketRules = new Dictionary<string, TicketRule>();
            _data[0].ForEach(x => _ticketRules.AddToDictionary(ToTicketRule(x)));

            _ticket = _data[1].FirstOrDefault().ToSequence();

            _nearbyTickets = new List<List<long>>();
            _data[2].ForEach(x => _nearbyTickets.Add(x.ToSequence()));
        }

        private string _pattern = @"^(?<name>[\w ]+): (?<a1>\d+)-(?<a2>\d+) or (?<b1>\d+)-(?<b2>\d+)$";
        private (string, TicketRule) ToTicketRule(string input)
        {
            var value = new TicketRule();

            foreach (Match m in Regex.Matches(input, _pattern))
            {
                value.Name = m.GroupValue("name");
                value.ValueA1 = m.GroupValue("a1").ToLong();
                value.ValueA2 = m.GroupValue("a2").ToLong();
                value.ValueB1 = m.GroupValue("b1").ToLong();
                value.ValueB2 = m.GroupValue("b2").ToLong();
            }

            return (value.Name, value);
        }

        [Conditional("LOG")]
        private void DumpData()
        {
            if (!Log.EnableDebug) return;

            Debug();

            _data.DumpJson("List");
            _ticketRules.DumpJson("_ticketRules");
            _ticket.DumpJson("_ticket");
            _nearbyTickets.DumpJson("_nearbyTickets");
        }
    }
}

#if DUMP
#endif