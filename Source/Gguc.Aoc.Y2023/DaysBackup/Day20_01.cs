#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2023.DaysBackup;

using Module = Models.Module;

public class Day20_01 : Day
{
    private const int YEAR = 2023;
    private const int DAY = 20_01;

    private List<string> _data;
    private Dictionary<string, Module> _modules;
    private long _lows;
    private long _highs;

    public Day20_01(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "883726240";
        Expected2 = "";
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
        _lows = 0L;
        _highs = 0L;

        var result = CountSignals(1000);

        Result = result;
    }

    protected override void ComputePart2()
    {
        _lows = 0L;
        _highs = 0L;

        var result = 0L;

        Result = result;
    }

    private long CountSignals(int count)
    {
        for (var i = 0; i < count; i++)
        {
            var init = new Signal { From = "button", To = "broadcaster", Value = false };
            var queue = new Queue<Signal>(new[] { init });

            while (queue.Count > 0)
            {
                var next = queue.Dequeue();
                SendSignal(queue, next);
            }

            // SendSignal("broadcaster", false, "button");
        }

        Log.Debug($"Lows=[{_lows}]... Highs=[{_highs}]");
        return _lows * _highs;
    }

    private void SendSignal(Queue<Signal> queue, Signal signal)
    {
        var id = signal.To;
        var from = signal.From;
        var value = signal.Value;

        // Log.Debug($"SendSignal: [{from}] -[{value}]- [{id}]");

        if (value) _highs++; else _lows++;

        var module = _modules[id];

        switch (module.Type)
        {
            case ModuleType.Broadcaster:
                {
                    module.Output.ForEach(m => EnqueueSignal(queue, id, m, value));
                    break;
                }

            case ModuleType.FlipFlop:
                {
                    if (value) return;

                    module.State = !module.State;
                    module.Output.ForEach(m => EnqueueSignal(queue, id, m, module.State));

                    break;
                }

            case ModuleType.Conjunction:
                {
                    module.States[from] = value;

                    var output = module.States.Values.All(x => x);
                    module.Output.ForEach(m => EnqueueSignal(queue, id, m, !output));

                    break;
                }
        }
    }

    private void EnqueueSignal(Queue<Signal> queue, string from, string to, bool value)
    {
        var signal = new Signal { From = from, To = to, Value = value };
        queue.Enqueue(signal);
    }

    protected override void ProcessData()
    {
        base.ProcessData();

        var modules = new Dictionary<string, Module>();

        // Gromit do something!
        foreach (var line in _data)
        {
            var x = line.Replace("->", "=").Replace(" ", "");
            var parts = x.Split('=');

            var name = parts[0];
            var type = name[0] switch
            {
                '%' => ModuleType.FlipFlop,
                '&' => ModuleType.Conjunction,
                _ => ModuleType.Output
            };

            if (name == "broadcaster") type = ModuleType.Broadcaster;

            name = type == ModuleType.Broadcaster ? name : name[1..];

            var output = parts[1].Split(',').ToList();

            var m = new Module { Id = name, Type = type, Output = output, State = false };

            modules[name] = m;
        }

        ProcessConjunctions(modules);
        ProcessOutputs(modules);

        _modules = modules;
    }

    private void ProcessConjunctions(Dictionary<string, Module> modules)
    {
        var conj = modules.Where(kv => kv.Value.Type == ModuleType.Conjunction).Select(kv => kv.Key).ToList();
        foreach (var ckey in conj)
        {
            var ms = modules.Where(kv => kv.Value.Output.Contains(ckey)).Select(kv => kv.Key).ToList();
            ms.ForEach(m => modules[ckey].States[m] = false);
        }
    }

    private void ProcessOutputs(Dictionary<string, Module> modules)
    {
        foreach (var m in modules.Keys.ToList())
        {
            modules[m].Output.ForEach(x =>
            {
                if (modules.ContainsKey(x)) return;

                modules[x] = new Module { Id = x, Type = ModuleType.Output };
            });
        }
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

        // _modules.DumpJson();
    }
}

#if DUMP
#endif