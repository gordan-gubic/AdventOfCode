namespace Gguc.Aoc.Y2018.Models;

public class Node08
{
    public Guid Id { get; } = Guid.NewGuid();

    public Node08 Parent { get; set; }

    public List<Node08> Children { get; set; } = new();

    public List<int> Values { get; set; } = new();

    public long Sum { get; set; }

    public long TotalSum { get; set; }

    public long Value { get; set; } = -1;

    public long GetSum()
    {
        if (Sum > 0) return Sum;

        Values.ForEach(x => Sum += x);

        return Sum;
    }

    public long GetTotalSum()
    {
        if (TotalSum > 0) return TotalSum;

        var total = GetSum();
        Children.ForEach(x => total += x.GetTotalSum());

        TotalSum = total;
        return TotalSum;
    }

    public long GetValue()
    {
        if (Value >= 0) return Value;

        if (Children.Count == 0)
        {
            Value = GetSum();
            return Value;
        }

        Value = 0;
        foreach (var i in Values)
        {
            var index = i - 1;

            if (index < Children.Count)
            {
                Value += Children[index].GetValue();
            }
        }

        return Value;
    }
}