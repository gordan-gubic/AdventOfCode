namespace Gguc.Aoc.Y2023.Models;

internal record Machine2
{
    private ulong? _value;

    public uint X1 { get; set; }

    public uint X2 { get; set; }

    public uint M1 { get; set; }

    public uint M2 { get; set; }

    public uint A1 { get; set; }

    public uint A2 { get; set; }

    public uint S1 { get; set; }

    public uint S2 { get; set; }

    public ulong Value => _value ?? GetValue();

    public string Result { get; set; }

    public Machine2 Copy() => new() { X1 = X1, X2 = X2, M1 = M1, M2 = M2, A1 = A1, A2 = A2, S1 = S1, S2 = S2 };

    public ulong GetValue()
    {
        _value = (ulong)(X2 - X1 + 1) * (M2 - M1 + 1) * (A2 - A1 + 1) * (S2 - S1 + 1);
        return _value.Value;
    }
}