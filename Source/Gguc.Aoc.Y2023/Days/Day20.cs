#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2023.Days;

using Module = Gguc.Aoc.Y2023.Models.Module;

public class Day20 : Day
{
    private const int YEAR = 2023;
    private const int DAY = 20;

    private List<string> _data;
    private Dictionary<string, Module> _modules;
    private long _lows;
    private long _highs;
    private long _buttons;
    private HashSet<string> _breakIds = new();
    private Dictionary<string, long> _breakModules = new ();

    public Day20(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "883726240";
        Expected2 = "211712400442661";
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
        ProcessData();

        _lows = 0L;
        _highs = 0L;
        _buttons = 0L;

        var result = CountSignals(1000);

        Result = result;
    }

    protected override void ComputePart2()
    {
        ProcessData();

        _lows = 0L;
        _highs = 0L;
        _buttons = 0L;

        var result = CountRx();

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
        }

        Log.Debug($"Lows=[{_lows}]... Highs=[{_highs}]");
        return _lows * _highs;
    }

    private long CountRx()
    {
        /*
         * SO:
         * &ct, &kp, &ks, &xc -> &bb -> rx (all of them have to be TRUE for 'rx' to be FALSE)
         * &gt -> ..., ct ('ct' is TRUE when 'gt' is FALSE)
         * &xd -> ..., kp ('kp' is TRUE when 'xd' is FALSE)
         * &ms -> ..., ks ('ks' is TRUE when 'ms' is FALSE)
         * &zt -> ..., xc ('xc' is TRUE when 'zt' is FALSE)
         *
         * 'gt', 'xd', 'ks', 'zt' are FALSE every so much signals
         *
         * Result: LCM('gt', 'xd', 'ks', 'zt');
         */

        _breakIds = new() { "gt", "xd", "ms", "zt" };
        _breakModules = new();
        _breakIds.ForEach(m => _breakModules[m] = 0L);

        _buttons = 0L;
        var found = false;

        while (true)
        {
            _buttons++;

            var init = new Signal { From = "button", To = "broadcaster", Value = false };
            var queue = new Queue<Signal>(new[] { init });

            while (queue.Count > 0)
            {
                var next = queue.Dequeue();
                SendSignal(queue, next);

                found = !_breakIds.Any();
                if (found) break;
            }

            if (found) break;

            if (_buttons > 10000000) break;
        }

        var result = _breakModules.Values.LeastCommonMultiple();
        Log.Debug($"break=[{_breakModules.ToJson()}]");
        Log.Debug($"result=[{result}]");
        return result;
    }

    private bool CheckBreak(Dictionary<string, long> breakModules)
    {
        throw new NotImplementedException();
    }

    private void SendSignal(Queue<Signal> queue, Signal signal)
    {
        var id = signal.To;
        var from = signal.From;
        var value = signal.Value;

        if (value) _highs++; else _lows++;

        var module = _modules[id];

        switch (module.Type)
        {
            case ModuleType.Broadcaster:
                {
                    module.Sent = value;
                    module.Output.ForEach(m => EnqueueSignal(queue, id, m, module.Sent));
                    break;
                }

            case ModuleType.FlipFlop:
                {
                    if (value) return;

                    module.State = !module.State;
                    module.Sent = module.State;
                    module.Output.ForEach(m => EnqueueSignal(queue, id, m, module.Sent));

                    break;
                }

            case ModuleType.Conjunction:
                {
                    module.States[from] = value;

                    var high = module.States.Values.Any(x => !x);
                    module.Sent = high;
                    module.Output.ForEach(m => EnqueueSignal(queue, id, m, module.Sent));

                    break;
                }
        }

        if (_breakIds.Contains(id) && !module.Sent)
        {
            _highs++;
            _breakModules[id] = _buttons;
            _breakIds.Remove(id);
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

            name = (type == ModuleType.Broadcaster) ? name : name[1..];

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
    private void ShowModule(Module module)
    {
        Log.Debug($"button[{_buttons}]. module: [{module.Id}] [gt:{_modules["gt"].Sent}, xd:{_modules["xd"].Sent}, ms:{_modules["ms"].Sent}, zt:{_modules["zt"].Sent}]");
    }

    private void ShowModule(string id)
    {
        if (_modules["gt"].Sent && _modules["xd"].Sent && _modules["ms"].Sent && _modules["zt"].Sent) return;

        var module = _modules[id];
        Log.Debug($"button[{_buttons}]. module: [{module.Id}] [gt:{_modules["gt"].Sent}, xd:{_modules["xd"].Sent}, ms:{_modules["ms"].Sent}, zt:{_modules["zt"].Sent}]");
    }
#endif