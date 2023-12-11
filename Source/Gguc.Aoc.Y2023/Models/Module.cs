namespace Gguc.Aoc.Y2023.Models;

internal record Module
{
    public string Id { get; set; } 

    public ModuleType Type { get; set; }

    public bool Sent { get; set; }

    public bool State { get; set; }

    public Dictionary<string, bool> States { get; set; } = new();

    public List<string> Output { get; set; }
}

internal enum ModuleType
{
    Broadcaster,

    FlipFlop,

    Conjunction,

    Output,
}