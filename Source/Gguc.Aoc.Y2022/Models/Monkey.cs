namespace Gguc.Aoc.Y2022.Models;

using Newtonsoft.Json;

internal class Monkey
{
    public int Id { get; set; }

    public Queue<long> Items { get; set; } = new();

    [JsonIgnore]
    public Func<long, long> Operation { get; set; }

    public int Divisor { get; set; }

    public int True { get; set; }

    public int False { get; set; }
}