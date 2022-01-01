namespace Gguc.Aoc.Core.Extensions;

public static class BitArrayExtensions
{
    public static BitArray ToBitArray(this string input, char trueValue = '1')
    {
        var bits = new BitArray(input.Length);

        for (int i = 0; i < input.Length; i++) bits[i] = input[i] == trueValue;

        return bits;
    }

    public static bool IsEqual(this BitArray ba1, BitArray ba2)
    {
        if (ba1.Count != ba2.Count) return false;

        for (var i = 0; i < ba1.Count; i++)
        {
            if (ba1[i] != ba2[i]) return false;
        }

        return true;
    }
}
