#define LOGx
#define STOPWATCH

namespace Gguc.Aoc.Y2020.Days;

public class Day19 : Day
{
    private List<string> _source;

    private List<string> _messages;
    private Dictionary<int, Rule19> _rules;

    public Day19(ILog log, IParser parser) : base(log, parser)
    {
        EnableDebug();
        Initialize();
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Year = 2020;
        Parser.Day = 19;
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
        var rules42 = _rules[42].Values;
        var rules31 = _rules[31].Values;

        foreach (var message in _messages)
        {
            var a = message;
            var count1 = 0;
            var count2 = 0;

            (a, count1) = CutRules(a, rules42);
            (a, count2) = CutRules(a, rules31);

            var isValid = a.IsWhitespace() && count1 == 2 && count2 == 1;
            Add(isValid);
        }
    }

    protected override void ComputePart2()
    {
        var rules42 = _rules[42].Values;
        var rules31 = _rules[31].Values;

        foreach (var message in _messages)
        {
            var a = message;
            var count1 = 0;
            var count2 = 0;

            (a, count1) = CutRules(a, rules42);
            (a, count2) = CutRules(a, rules31);

            var isValid = a.IsWhitespace() && count1 > 1 && count2 > 0 && (count1 > count2);
            Add(isValid);
        }
    }

    private (string message, int count) CutRules(string message, HashSet<string> rules)
    {
        var count = 0;

        while (true)
        {
            var isValid = false;
            foreach (var rule in rules)
            {
                if (message.StartsWith(rule))
                {
                    message = message.Remove(0, rule.Length);
                    isValid = true;
                    count++;
                    break;
                }
            }

            if (!isValid) break;
        }

        return (message, count);
    }

    protected void ComputePart1BruteForce()
    {
        var target = 0;
        var rules = _rules[target].Values;

        foreach (var message in _messages)
        {
            if (rules.Contains(message)) Add(1);
        }
    }

    private void _ProcessData()
    {
        var rules = new List<string>();
        _messages = new List<string>();

        var segment = 0;
        foreach (var line in _source)
        {
            if (line.IsWhitespace()) { segment++; continue; }

            if (segment == 0) rules.Add(line);
            else _messages.Add(line);
        }

        // rules.DumpCollection("rules");
        // _messages.DumpCollection("messages");

        _rules = new Dictionary<int, Rule19>();
        foreach (var rule in rules)
        {
            var parts = rule.Split(':');
            var index = parts[0].ToInt();
            var raw = parts[1].Trim(' ', '"');

            _rules[index] = new Rule19 { Index = index, Raw = raw };
        }

        _rules.Where(x => x.Value.Raw == "a").ForEach(x => x.Value.Values = new HashSet<string> { "a" });
        _rules.Where(x => x.Value.Raw == "b").ForEach(x => x.Value.Values = new HashSet<string> { "b" });

        _rules.Remove(0);
        _rules.Remove(8);
        _rules.Remove(11);

        foreach (var rule in _rules)
        {
            rule.Value.Values = GetRuleValue(rule.Key);
        }
    }

    private HashSet<string> GetRuleValue(int index)
    {
        var rule = _rules[index];
        if (rule.Values != null) return rule.Values;

        CompileRule(rule);

        return rule.Values;
    }

    private void CompileRule(in Rule19 rule)
    {
        var list = new HashSet<string>();
        var alternatives = rule.Raw.Split('|');

        foreach (var alt in alternatives)
        {
            var rules = alt.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => x.ToInt());
            Combine(rules).ForEach(x => list.Add(x));
        }

        rule.Values = list;
    }

    private HashSet<string> Combine(IEnumerable<int> rules)
    {
        var multilist = new List<HashSet<string>>();

        foreach (var ruleIndex in rules)
        {
            var list = GetRuleValue(ruleIndex);
            multilist.Add(list);
        }

        var list0 = multilist.GetValueSafe(0);
        var list1 = multilist.GetValueSafe(1);

        if (list1 == null) return list0;

        var listAll = new HashSet<string>();
        foreach (var str0 in list0)
        {
            foreach (var str1 in list1)
            {
                listAll.Add($"{str0}{str1}");
            }
        }

        return listAll;
    }

    [Conditional("LOGx")]
    private void DumpData()
    {
        if (!Log.EnableDebug) return;

        Debug();

        _rules.DumpJson("_rules");
        _messages.DumpJson("_messages");
    }
}

#if DUMP
#endif