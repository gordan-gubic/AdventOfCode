namespace Gguc.Aoc.Y2021.Memory;

public class Day19Memory
{
    public Day19Memory()
    {
        Scanners = new Dictionary<int, List3d>();
        
        ScannerPositions = new Dictionary<int, Point3d>();

        BeaconsX = new HashSet<Point3d>();

        Beacons = new HashSet<(int, int, int)>();

        Distances = new Dictionary<int, DistancesCollection>();
    }

    public Dictionary<int, List3d> Scanners { get; internal set; }

    public Dictionary<int, Point3d> ScannerPositions { get; internal set; }

    public HashSet<Point3d> BeaconsX { get; internal set; }
    
    public HashSet<(int, int, int)> Beacons { get; internal set; }

    public Dictionary<int, DistancesCollection> Distances { get; internal set; }
}
