namespace Gguc.Aoc.Y2022.Models;

using Newtonsoft.Json;

internal class Gibbon
{
    public string Name { get; set; }

    public long OriginalValue { get; set; }

    public long Value { get; set; }

    public string GibbonId1 { get; set; }

    public string GibbonId2 { get; set; }

    public Gibbon Gibbon1 { get; set; }

    public Gibbon Gibbon2 { get; set; }

    public string OperationId { get; set; }

    [JsonIgnore]
    public Func<long> Operation { get; set; }

    public bool? IsHuman { get; set; }

    public string GetFormula()
    {
        if (Name == "humn") return $"{Name}";
        if (Value > 0) return $"{Value}";

        return $"{Gibbon1.GetFormula()}{OperationId}{Gibbon2.GetFormula()}";
    }

    public bool GetHuman()
    {
        if (Name == "humn")
        {
            IsHuman = true;
            return IsHuman.Value;
        }

        if (IsHuman.HasValue) return IsHuman.Value;

        if (Value > 0)
        {
            IsHuman = false;
            return IsHuman.Value;
        }

        IsHuman = Gibbon1.GetHuman() || Gibbon2.GetHuman();
        return IsHuman.Value;
    }

    public long GetValue()
    {
        if (Value > 0) return Value;

        Value = Operation();

        return Value;
    }

    public bool IsEqual()
    {
        var value1 = Gibbon1.GetValue();
        var value2 = Gibbon2.GetValue();

        return value1 == value2;
    }

    public void Reset()
    {
        Value = OriginalValue;
    }
}