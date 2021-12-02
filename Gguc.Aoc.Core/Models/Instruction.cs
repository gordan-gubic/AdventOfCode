namespace Gguc.Aoc.Core.Models;

public struct Instruction<T>
{
    public T Operation { get; set; }

    public int Argument { get; set; }

    /// <inheritdoc />
    public override string ToString() => this.ToJson();
}
