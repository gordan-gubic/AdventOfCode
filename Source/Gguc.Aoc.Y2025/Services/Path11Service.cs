#define LOG

namespace Gguc.Aoc.Y2025.Services;

using Gguc.Aoc.Y2025.Models;

internal class Path11Service
{
    public Path11Service(Dictionary<string, HashSet<string>> data)
    {
        Data = data;
    }

    public Dictionary<string, HashSet<string>> Data { get; set; }

    public List<PathBranch> Solved { get; set; } = [];

    public Dictionary<string, long> Cache { get; set; } = [];

    public string Start { get; set; }

    public string Final { get; set; }

    public long CountAllPaths(params string[] steps)
    {
        Start = steps[0];
        Final = steps[^1];

        var result = 1L;

        for (var i = 0; i < steps.Length - 1; i++)
        {
            result *= CountPathRecursive(steps[i], steps[i + 1]);
        }

        return result;
    }

    public List<PathBranch> CountAllPathsBruteForce(string start, string final)
    {
        Start = start;
        Final = final;

        var startCandidates = Data[start];
        var pathBranches = new List<PathBranch>();
        foreach (var candidate in startCandidates)
        {
            var pb = new PathBranch();
            pb.Nodes.Add(start);
            pb.Next = candidate;

            pathBranches.Add(pb);
        }

        var queue = new Queue<PathBranch>(pathBranches);

        while (queue.Any())
        {
            var pb = queue.Dequeue();

            var candidates = ProcessBranch(pb);

            foreach (var candidate in candidates)
            {
                queue.Enqueue(candidate);
            }
        }

        // Solved.DumpJson();
        return Solved;
    }

    public long CountAllPathsDebug(string start, string stop1, string stop2, string final)
    {
        Start = start;
        Final = final;

        Data.Count.Dump("Data count");

        // stop2 -> final
        /*
        var paths1 = CountAllPaths(stop2, final);
        var l1 = (long)paths1.Count;
        paths1.Count.Dump("paths1");
        */

        var r1 = CountPathRecursive(stop2, final);
        r1.Dump("r1");

        var r2 = CountPathRecursive(stop1, stop2);
        r2.Dump("r2");

        var r3 = CountPathRecursive(start, stop1);
        r3.Dump("r3");

        return r1 * r2 * r3;
    }

    private long CountPathRecursive(string start, string final)
    {
        var id = $"{start}-{final}";
        if (Cache.ContainsKey(id)) return Cache[id];

        if (start == Final)
        {
            Cache[id] = 0L;
            return 0L;
        }
        else if (start == final)
        {
            Cache[id] = 1L;
            return 1L;
        }

        var result = 0L;
        var candidates = Data[start];
        
        if (candidates.IsNullOrEmpty())
        {
            result = 0L;
        }
        else if (candidates.Contains(final))
        {
            result = 1L;
        }
        else
        {
            foreach (var candidate in candidates)
            {
                result += CountPathRecursive(candidate, final);
            }
        }

        Cache[id] = result;
        return result;
    }

    private List<PathBranch> ProcessBranch(PathBranch branch)
    {
        var candidates = new List<PathBranch>();

        var next = branch.Next;
        branch.Nodes.Add(next);

        if (next == Final)
        {
            Solved.Add(branch);
            return [];
        }

        if (!Data.ContainsKey(next)) return [];

        var destinations = Data[next];

        foreach (var dest in destinations)
        {
            if (branch.Nodes.Contains(dest)) continue;

            var candidate = new PathBranch
            {
                Nodes = branch.Nodes.ToHashSet(),
                Next = dest
            };

            candidates.Add(candidate);
        }

        return candidates;
    }
}

internal class PathBranch
{
    public PathBranch()
    {
        Nodes = [];
    }

    public HashSet<string> Nodes { get; set; }

    public string Next { get; set; }
}