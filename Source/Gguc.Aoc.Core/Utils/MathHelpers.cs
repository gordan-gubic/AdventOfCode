namespace Gguc.Aoc.Core.Utils;

using System.Numerics;

public static class MathHelpers
{
    public static T GreatestCommonDivisor<T>(T a, T b) where T : INumber<T>
    {
        while (b != T.Zero)
        {
            var temp = b;
            b = a % b;
            a = temp;
        }

        return a;
    }

    public static T LeastCommonMultiple<T>(T a, T b) where T : INumber<T>
        => (a / GreatestCommonDivisor(a, b)) * b;

    public static T LeastCommonMultiple<T>(this IEnumerable<T> values) where T : INumber<T>
        => values.Aggregate(LeastCommonMultiple);

    public static T TriangularSequence<T>(this T value) where T : INumber<T>
        => value * (value + T.One) / (T.One + T.One);
}