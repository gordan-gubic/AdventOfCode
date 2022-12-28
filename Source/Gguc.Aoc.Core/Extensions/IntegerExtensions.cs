namespace Gguc.Aoc.Core.Extensions;

public static class IntegerExtensions
{
    public static int Ceiling(this int input, int divisor)
    {
        // return (input / divisor) + (input % divisor == 0 ? 0 : 1);
        return (int)(input + divisor - 1) / divisor;
    }
}
