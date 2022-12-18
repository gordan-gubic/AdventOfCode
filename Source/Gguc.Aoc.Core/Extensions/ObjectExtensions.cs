namespace Gguc.Aoc.Core.Extensions;

/// <summary>
/// Extension methods for object.
/// </summary>
public static class ObjectExtensions
{
    /// <summary>
    /// Deep clone object.
    /// </summary>
    /// <typeparam name="T">The type of the object to clone.</typeparam>
    /// <param name="source">The source object.</param>
    /// <returns>The object cloned from the source.</returns>
    public static T CloneDeep<T>(this T source)
    {
        try
        {
            return source.ToJson().FromJson<T>();
        }
        catch (Exception ex)
        {
            Trace.TraceWarning($"Error occurred during cloning of the object of the type '{typeof(T)}'! Error=[{ex.Message}]");
            return default;
        }
    }
}