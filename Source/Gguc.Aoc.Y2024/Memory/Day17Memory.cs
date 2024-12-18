namespace Gguc.Aoc.Y2024.Memory;

public class Day17Memory
{
    public Day17Memory()
    {
    }

    public long RegisterA { get; set; }
    public long RegisterB { get; set; }
    public long RegisterC { get; set; }
    public List<long> Input { get; set; }
    public List<long> Output { get; set; } = new();
    public int Index { get; set; }
    public int Size { get; set; }


    public void Clear()
    {
    }

    public void Recreate()
    {
    }

    // public override string ToString() => $"{new {RegisterA, RegisterB, RegisterC, Input=Input.ToJson()}}";
    public override string ToString() => $"{new { RegisterA, RegisterB, RegisterC }}";
}

#if DROP
#endif