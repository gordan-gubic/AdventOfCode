namespace Gguc.Aoc.Core.Extensions;

public static class ListExtensions
{
    public static T GetValueSafe<T>(this List<T> input, int index, T defaultValue = default)
    {
        if (index >= input.Count) return defaultValue;

        return input[index];
    }

    public static void SetAll<T>(this IList<T> list, T defaultValue = default)
    {
        for(int i = 0; i < list.Count(); i++) list[i] = defaultValue;
    }

    public static void SetAll<TKey, TValue>(this IDictionary<TKey, TValue> dict, TValue defaultValue = default)
    {
        foreach (var key in dict.Keys.ToList()) dict[key] = defaultValue;
    }

    public static void ForEach<TKey, TValue>(this IDictionary<TKey, TValue> dict, Action<TKey, TValue> action)
    {
        foreach (var key in dict.Keys.ToList()) action(key, dict[key]);
    }

    public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
    {
        foreach (var item in list) action(item);
    }

    public static void ForEach<T>(this T[] list, Action<T> action)
    {
        foreach (var item in list) action(item);
    }

    public static (T, T) MinMax<T>(this IEnumerable<T> list)
    {
        return (list.Min(), list.Max());
    }

    public static Dictionary<TKey, TValue> Copy<TKey, TValue>(this IDictionary<TKey, TValue> dict)
    {
        return dict.ToDictionary(k => k.Key, k => k.Value);
    }

    public static void AddToDictionary<TKey, TValue>(this IDictionary<TKey, TValue> dict, (TKey, TValue) input)
    {
        dict.Add(input.Item1, input.Item2);
    }

    public static bool IsNullOrEmpty<T>(this IEnumerable<T> list)
    {
        return list == null || !list.Any();
    }

    public static void Swap<T>(this IList<T> list, int index1, int index2)
    {
        (list[index1], list[index2]) = (list[index2], list[index1]);
    }
}
