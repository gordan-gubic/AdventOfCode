#define LOG

namespace Gguc.Aoc.Y2024.Services;

using System.Collections.Generic;
using System.Linq;

public class MazeSearch
{
    private readonly Dictionary<(int, int, int), long> _cache;

    private MazeStep _start;
    private Queue<MazeStep> _active;

    public Map<char> Map { get; set; }
    
    public Map<bool> Walls { get; set; }

    public List<MazeStep> Result { get; } = new();

    public MazeSearch()
    {
        _cache = new();
    }

    public void Find()
    {
        var start = Map.Find('S');
        var end = Map.Find('E');

        _start = new MazeStep
        {
            Point = new Point(start.Item2, start.Item3),
            Dir = 90
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

    private void ProcessItem(MazeStep step)
    {
        var x = step.Point.X;
        var y = step.Point.Y;

        step.Path.Add(step.Point);
        step.Points.Add(step.Point);
        
        if (Map.GetValue(x, y) == 'E')
        {
            Result.Add(step);
            _active.Clear();
            return;
        }

        if (_cache.ContainsKey((x, y, step.Dir)) && _cache[(x, y, step.Dir)] < step.Value) return;
        _cache[(x, y, step.Dir)] = step.Value;

        var candidates = GetCandidates(step);
        var validated = ValidateCandidates(candidates, step);
        validated.ForEach(v => _active.Enqueue(v));
    }

    private List<MazeStep> GetCandidates(MazeStep step)
    {
        return new List<MazeStep>
        {
            GetStraight(step),
            GetLeft(step),
            GetRight(step),
        };
    }

    private List<MazeStep> ValidateCandidates(List<MazeStep> candidates, MazeStep step)
    {
        var validated = new List<MazeStep>();

        foreach (var candidate in candidates)
        {
            if(ValidateCandidate(candidate)) validated.Add(candidate);
        }

        return validated;
    }

    private bool ValidateCandidate(MazeStep step)
    {
        var x = step.Point.X;
        var y = step.Point.Y;

        if (!Map.Contains(x, y)) return false;
        if (Walls.GetValue(x, y)) return false;
        if (step.Points.Contains(new Point(x, y))) return false;
        if (_cache.ContainsKey((x, y, step.Dir)) && _cache[(x, y, step.Dir)] < step.Value) return false;

        return true;
    }

    private MazeStep GetStraight(MazeStep step)
    {
        return new MazeStep
        {
            Point = step.Point.DegreeToPoint(step.Dir),
            Dir = step.Dir,
            Value = step.Value + 1,
            Path = step.Path.ToList(),
            Points = step.Points.ToHashSet(),
        };
    }

    private MazeStep GetLeft(MazeStep step)
    {
        var dir = step.Dir.DegreeToLeft();

        return new MazeStep
        {
            Point = step.Point.DegreeToPoint(dir),
            Dir = dir,
            Value = step.Value + 1001,
            Path = step.Path.ToList(),
            Points = step.Points.ToHashSet(),
        };
    }

    private MazeStep GetRight(MazeStep step)
    {
        var dir = step.Dir.DegreeToRight();

        return new MazeStep
        {
            Point = step.Point.DegreeToPoint(dir),
            Dir = dir,
            Value = step.Value + 1001,
            Path = step.Path.ToList(),
            Points = step.Points.ToHashSet(),
        };
    }
}