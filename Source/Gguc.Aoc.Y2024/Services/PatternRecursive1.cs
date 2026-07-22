#define LOG

namespace Gguc.Aoc.Y2024.Services;

using System.Collections.Generic;
using System.Linq;

public class PatternRecursive1
{
    // private readonly Dictionary<string, long> _cache;
    private readonly HashSet<string> _cache1;
    private readonly HashSet<string> _cache2;

    private PatternStep _start;

    public HashSet<string> Patterns { get; set; }
    
    public string Request { get; set; }

    public bool Result { get; private set; }

    public PatternRecursive1()
    {
        _cache1 = new();
        _cache2 = new();
    }

    public void Find()
    {
        _start = new PatternStep
        {
            Pattern = "",
            Value = 0,
            Remain = Request,
        };

        Result = ProcessItem(_start);
    }

    private bool ProcessItem(PatternStep step)
    {
        step.Path = $"{step.Path}{step.Pattern}";
        step.Path2 = $"{step.Path2},{step.Pattern}";
        
        // step.Dump("step");
        
        if (step.Path == Request)
        {
            // step.Path.Dump("step");
            // _active.Clear();
            return true;
        }

        if (_cache1.Contains(step.Path)) return false;
        if (_cache2.Contains(step.Path2)) return false;

        var candidates = GetCandidates(step);
        if(candidates.IsNullOrEmpty()) return false;

        var validated = ValidateCandidates(candidates, step);

        var found = new List<bool>();
        foreach (var candidate in validated)
        {
            found.Add(ProcessItem(candidate));
        }

        _cache1.Add(step.Path);
        _cache2.Add(step.Path2);

        return found.Any(x => x);
    }

    private List<PatternStep> GetCandidates(PatternStep step)
    {
        var request = step.Remain;
        request = request.Remove(0, step.Pattern.Length);
        var patterns = Patterns.Where(x => request.StartsWith(x)).ToList();

        var candidates = new List<PatternStep>();
        patterns.ForEach(x => candidates.Add(step with { Remain = request, Pattern = x, Value = step.Value + 1 }));

        return candidates;
    }

    private List<PatternStep> ValidateCandidates(List<PatternStep> candidates, PatternStep step)
    {
        var validated = new List<PatternStep>();

        foreach (var candidate in candidates)
        {
            if(ValidateCandidate(candidate)) validated.Add(candidate);
        }

        return validated;
    }

    private bool ValidateCandidate(PatternStep step)
    {
        if (_cache1.Contains(step.Path)) return false;
        if (_cache2.Contains(step.Path2)) return false;

        return true;
    }
}