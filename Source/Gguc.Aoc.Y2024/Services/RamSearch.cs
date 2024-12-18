#define LOG

namespace Gguc.Aoc.Y2024.Services;

using System.Collections.Generic;
using System.Linq;

public class RamSearch
{
    private readonly Dictionary<(int, int), long> _cache;

    private RamStep _start;
    private Queue<RamStep> _active;
    private Point _end;

    public Map<bool> Walls { get; set; }

    public List<RamStep> Result { get; } = new();

    public RamSearch()
    {
        _cache = new();
    }

    public void Find()
    {
        _start = new RamStep
        {
            Point = new Point(0, 0),
        };

        _end = new Point(Walls.Width - 1, Walls.Height - 1);

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

    private void ProcessItem(RamStep step)
    {
        var x = step.Point.X;
        var y = step.Point.Y;

        step.Path.Add(step.Point);
        step.Points.Add(step.Point);
        
        if (step.Point == _end)
        {
            Result.Add(step);
            return;
        }

        if (_cache.ContainsKey((x, y)) && _cache[(x, y)] <= step.Value) return;
        _cache[(x, y)] = step.Value;

        var candidates = GetCandidates(step);
        var validated = ValidateCandidates(candidates, step);
        validated.ForEach(v => _active.Enqueue(v));
    }

    private List<RamStep> GetCandidates(RamStep step)
    {
        return new List<RamStep>
        {
            GetStep(step, 0),
            GetStep(step, 90),
            GetStep(step, 180),
            GetStep(step, 270),
        };
    }

    private List<RamStep> ValidateCandidates(List<RamStep> candidates, RamStep step)
    {
        var validated = new List<RamStep>();

        foreach (var candidate in candidates)
        {
            if(ValidateCandidate(candidate)) validated.Add(candidate);
        }

        return validated;
    }

    private bool ValidateCandidate(RamStep step)
    {
        var x = step.Point.X;
        var y = step.Point.Y;

        if (!Walls.Contains(x, y)) return false;
        if (Walls.GetValue(x, y)) return false;
        if (step.Points.Contains(new Point(x, y))) return false;
        if (_cache.ContainsKey((x, y)) && _cache[(x, y)] < step.Value) return false;

        return true;
    }

    private RamStep GetStep(RamStep step, int dir)
    {
        return new RamStep
        {
            Point = step.Point.DegreeToPoint(dir),
            Value = step.Value + 1,
            Path = step.Path.ToList(),
            Points = step.Points.ToHashSet(),
        };
    }
}