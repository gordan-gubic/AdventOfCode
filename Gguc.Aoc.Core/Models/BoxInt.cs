namespace Gguc.Aoc.Core.Models;

public class BoxInt : StrongBox<int>
{
    public BoxInt()
    {
    }

    public BoxInt(int value)
    {
        Value = value;
    }

    public BoxInt Add(int value)
    {
        this.Value = this.Value + value;
        return this;
    }

    public BoxInt Add(BoxInt value)
    {
        this.Value = this.Value + value.Value;
        return this;
    }

    public override string ToString() => Value.ToString();

    public static BoxInt Create(int value)
    {
        return new BoxInt(value);
    }
}
