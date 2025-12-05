namespace Gguc.Aoc.Core.Models;

using System.Numerics;

/// <summary>
/// Simple representation of an inclusive range of numbers.
/// </summary>
/// <param name="Lower">Gets the lower bound of this <see cref="Range" />.</param>
/// <param name="Upper">Gets the upper bound of this <see cref="Range" />.</param>
public record Range<T>(T Lower, T Upper) where T : INumber<T>
{
    /// <summary>
    /// Gets the number of items between the <see cref="Lower" /> and <see cref="Upper" /> bounds.
    /// </summary>
    public T Length => Upper - Lower + T.One;
}