namespace Gguc.Aoc.Core.Utils;

public static class Combinator
{
    public static IEnumerable<IEnumerable<T>> Combine<T>(IEnumerable<IEnumerable<T>> sequences)
    {
        IEnumerable<IEnumerable<T>> emptyProduct = new[] { Enumerable.Empty<T>() };

        return sequences.Aggregate(
            emptyProduct,
            (accumulator, sequence) =>
                from accseq in accumulator
                from item in sequence
                select accseq.Concat(new[] { item }));
    }

    public static IEnumerable<T[]> Permutations<T>(IEnumerable<T> source)
    {
        if (null == source)
            throw new ArgumentNullException(nameof(source));

        T[] data = source.ToArray();

        return Enumerable
            .Range(0, 1 << (data.Length))
            .Select(index => data
                .Where((v, i) => (index & (1 << i)) != 0)
                .ToArray());
    }

    public static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length)
    {
        if (length == 1) return list.Select(t => new T[] { t });

        return GetPermutations(list, length - 1)
            .SelectMany(t => list.Where(e => !t.Contains(e)),
                (t1, t2) => t1.Concat(new T[] { t2 }));
    }

    public static void GetCombination<T>(List<T> set, List<List<T>> result)
    {
        for (int i = 0; i < set.Count; i++)
        {
            List<T> temp = new List<T>(set.Where((s, index) => index != i));

            if (temp.Count > 0 && !result.Where(l => l.Count == temp.Count).Any(l => l.SequenceEqual(temp)))
            {
                result.Add(temp);

                GetCombination<T>(temp, result);
            }
        }
    }

    public static List<List<int>> GetCombination(List<int> list)
    {
        var max = list.Max();
        var combos = new List<List<int>>();
        var combo = new List<int>();

        double count = Math.Pow(2, list.Count);
        for (int i = 1; i <= count - 1; i++)
        {
            for (int j = 0; j < list.Count; j++)
            {
                int b = i & (1 << j);
                if (b > 0)
                {
                    var value = list[j];

                    if (combo.Count == 0 && value > 3) break;

                    combo.Add(value);
                }
            }

            combos.Add(combo.ToList());
            combo.Clear();
        }

        return combos;
    }
}

#if DUMP
#endif