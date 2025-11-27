#define LOG

namespace Gguc.Aoc.Y2024.Services;

using System.Collections.Generic;
using System.Linq;

public class PatternSearch
{
    // private readonly Dictionary<string, long> _cache1;
    private readonly HashSet<string> _cache1;
    private readonly HashSet<string> _cache2;

    private PatternStep _start;
    private Queue<PatternStep> _active;

    public HashSet<string> Patterns { get; set; }
    
    public string Request { get; set; }

    public List<PatternStep> Result { get; } = new();

    public PatternSearch()
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

        _active = new();
        _active.Enqueue(_start);

        CalculatePath();
    }

    private void CalculatePath()
    {
        while (_active.Count > 0)
        {
            var step = _active.Dequeue();
            ProcessItem(step);
        }
    }

    private void ProcessItem(PatternStep step)
    {
        step.Path = $"{step.Path}{step.Pattern}";
        step.Path2 = $"{step.Path2},{step.Pattern}";
        
        // step.Dump("step");
        
        if (step.Path == Request)
        {
            Result.Add(step);
            _active.Clear();
            return;
        }

        if (_cache1.Contains(step.Path)) return;
        if (_cache2.Contains(step.Path2)) return;

        var candidates = GetCandidates(step);
        var validated = ValidateCandidates(candidates, step);
        validated.ForEach(v => _active.Enqueue(v));

        _cache1.Add(step.Path);
        _cache2.Add(step.Path2);

        // candidates.ForEach(c => _active.Enqueue(c));
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