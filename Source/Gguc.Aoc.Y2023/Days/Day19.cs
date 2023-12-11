#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2023.Days;

using System.Numerics;

public class Day19 : Day
{
    private const int YEAR = 2023;
    private const int DAY = 19;

    private List<string> _data;
    private Dictionary<string, List<string>> _workflows;
    private List<Machine> _machines;

    public Day19(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "406934";
        Expected2 = "131192538505367";
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Test;
        Parser.Type = ParserFileType.Real;

        _data = Parser.Parse();
    }

    /// <inheritdoc />
    public override void DumpInput()
    {
        DumpData();
    }

    protected override void ComputePart1()
    {
        var result = SumAccepted();

        Result = result;
    }

    protected override void ComputePart2()
    {
        var result = SumAllAccepted();
        Log.Warn($"Result: [{result}]");

        Result = (long)result;
    }

    private long SumAccepted()
    {
        var sum = 0L;

        foreach (var machine in _machines)
        {
            var isAccepted = IsAccepted(machine);
            if (isAccepted) sum += machine.Value;
        }

        return sum;
    }

    private ulong SumAllAccepted()
    {
        var init = new Machine2 { X1 = 1, X2 = 4000, M1 = 1, M2 = 4000, A1 = 1, A2 = 4000, S1 = 1, S2 = 4000, Result = "in" };
        // Log.Debug($"Total.: [{init.Value}]");

        var queue = new Queue<Machine2>(new[] { init });

        var accepted = new List<Machine2>();
        var rejected = new List<Machine2>();

        while (queue.Count > 0)
        {
            var item = queue.Dequeue();

            if (item.Result == "A")
            {
                accepted.Add(item);
                continue;
            }

            if (item.Result == "R")
            {
                rejected.Add(item);
                continue;
            }

            var list = ProcessMachine2(item);
            list.ForEach(m => queue.Enqueue(m));
        }

        _ = rejected;

        var sum = 0UL;
        accepted.ForEach(m => sum += m.Value);

        return sum;
    }

    private List<Machine2> ProcessMachine2(Machine2 item)
    {
        var id = item.Result;
        var workflow = _workflows[id];

        var list = ProcessWorkflow2(workflow, item);
        return list;
    }

    private bool IsAccepted(Machine machine)
    {
        var current = _workflows["in"];

        while (true)
        {
            var result = ProcessWorkflow(current, machine);
            // Log.Debug($"Result=[{result}]");

            if (result == "A") return true;
            if (result == "R") return false;

            if (!_workflows.ContainsKey(result))
            {
                Log.Warn($"No key! Machine=[{machine.ToJson()}] Key=[{result}]");
                break;
            }
            current = _workflows[result];
        }

        // var result = ProcessWorkflow(start, machine);
        // Log.Debug($"Result=[{result}]");

        return false;
    }

    private string ProcessWorkflow(List<string> steps, Machine machine)
    {
        var i = 0;
        while (true)
        {
            var value = steps[i].Trim();

            if (value.Contains('>') || value.Contains('<'))
            {
                var isValid = Evaluate(machine, value);
                i += (isValid) ? 1 : 2;
            }
            else
            {
                return value;
            }
        }
    }

    private List<Machine2> ProcessWorkflow2(List<string> steps, Machine2 machine)
    {
        var list = new List<Machine2>();

        var queue = new Queue<Machine2>(new[] { machine });

        foreach (var step in steps)
        {
            var value = step.Trim();

            if (value.Contains('>') || value.Contains('<'))
            {
                var item = queue.Dequeue();
                var (m1, m2) = Split(item, value);
                queue.Enqueue(m1);
                queue.Enqueue(m2);
            }
            else
            {
                var item = queue.Dequeue();
                item.Result = value;
                list.Add(item);
            }
        }

        return list;
    }

    private bool Evaluate(Machine machine, string input)
    {
        var prop = input[0];
        var op = input[1];
        var value = input[2..].ToInt();

        var machineValue = prop switch
        {
            'x' => machine.X,
            'm' => machine.M,
            'a' => machine.A,
            's' => machine.S,
            _ => 0,
        };

        if (op == '>') return machineValue > value;
        return machineValue < value;
    }

    private (Machine2, Machine2) Split(Machine2 machine, string input)
    {
        var prop = input[0];
        var op = input[1];
        var value = (uint)input[2..].ToInt();

        var (a, d) = prop switch
        {
            'x' => (machine.X1, machine.X2),
            'm' => (machine.M1, machine.M2),
            'a' => (machine.A1, machine.A2),
            's' => (machine.S1, machine.S2),
            _ => (default, default),
        };

        var (b, c) = (op == '<') ? (value - 1, value) : (value, value + 1);

        var m1 = Update(machine, prop, a, b);
        var m2 = Update(machine, prop, c, d);

        return (op == '<') ? (m1, m2) : (m2, m1);
    }

    private Machine2 Update(Machine2 orig, char prop, uint i1, uint i2)
    {
        var machine = orig.Copy();

        switch (prop)
        {
            case 'x':
                {
                    machine.X1 = i1;
                    machine.X2 = i2;
                    break;
                }
            case 'm':
                {
                    machine.M1 = i1;
                    machine.M2 = i2;
                    break;
                }
            case 'a':
                {
                    machine.A1 = i1;
                    machine.A2 = i2;
                    break;
                }
            case 's':
                {
                    machine.S1 = i1;
                    machine.S2 = i2;
                    break;
                }
        }

        return machine;
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        var workflows = new Dictionary<string, List<string>>();
        var machines = new List<Machine>();

        var queue = new Queue<string>(_data);

        while (queue.Count > 0)
        {
            var line = queue.Dequeue();
            if (line.IsNullOrEmpty()) break;

            var parts = line.Split('{');
            var id = parts[0];
            var steps = parts[1].Trim('{', '}').Split(':', ',').ToList();
            workflows[id] = steps;
        }

        while (queue.Count > 0)
        {
            var line = queue.Dequeue();
            if (line.IsNullOrEmpty()) break;

            line = line.Trim('{', '}').Replace("x=", "").Replace("m=", "").Replace("a=", "").Replace("s=", "");
            var parts = line.Split(',');
            var machine = new Machine
            {
                X = parts[0].ToInt(),
                M = parts[1].ToInt(),
                A = parts[2].ToInt(),
                S = parts[3].ToInt(),
            };
            machines.Add(machine);
        }

        _workflows = workflows;
        _machines = machines;
    }

    private int Convert(string input)
    {
        return input.ToInt();
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (!Log.EnableDebug) return;

        Debug();

        // _data.DumpCollection();

        // _workflows.DumpJsonIndented();
        // _machines.DumpJsonIndented();
    }
}

#if DUMP
#endif