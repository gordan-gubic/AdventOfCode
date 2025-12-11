namespace Gguc.Aoc.Y2025.Models;

internal record Factory
{
    public Guid Id { get; } = Guid.NewGuid();

    public BitArray Current { get; set; }

    public BitArray Target { get; set; }

    public List<HashSet<int>> Switches { get; set; }

    public List<int> Joltages { get; set; }


    public void SetTarget(string target)
    {
        target = target.Trim('[', ']');

        Target = target.ToBitArray('#');
        Current = new BitArray(target.Length);
    }

    public void SetSwitches(List<string> parts)
    {
        Switches = [];

        foreach (var part in parts)
        {
            var numbers = part.Trim('(', ')').Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => x.ToInt()).ToHashSet();
            Switches.Add(numbers);
        }
    }

    public void SetJoltages(string joltages)
    {
        Joltages = joltages.Trim('{', '}').Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => x.ToInt()).ToList();
    }
}