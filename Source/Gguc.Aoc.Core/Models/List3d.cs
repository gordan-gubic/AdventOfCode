namespace Gguc.Aoc.Core.Models;

public class List3d : List<Point3d>
{
    public List3d()
    {
    }

    public List3d Clone()
    {
        var map = new List3d();

        ForEach((x) => map.Add(x));

        return map;
    }

    public new void ForEach(Action<Point3d> action)
    {
        foreach (var item in this)
        {
            action(item);
        }
    }

    public override string ToString()
    {
        var sb = new StringBuilder();

        foreach (var item in this)
        {
            sb.Append(item.ToString());
        }

        return sb.ToString();
    }
}


#if DUMP

public List3d RotateLeft()
{
    CalcDimensions();

    var map = new List3d();

    ForEach((p1) =>
    {
        //var p2 = new Point3d(p1.Z, p1.Y, _maxZ - p1.X + 1);
        // var p2 = new Point3d(p1.Z, p1.Y, _depth - p1.X + 1);
        // var p2 = new Point3d(p1.Z, p1.Y, p1.X - _depth + 1);
        var p2 = new Point3d(p1.Z, p1.Y, -p1.X);
        // var p2 = new Point3d(p1.Z, p1.Y, _maxZ - p1.X + 1);

        map.Add(p2);
    });

    return map;
}

public List3d RotateUp()
{
    CalcDimensions();

    var map = new List3d();

    ForEach((p1) =>
    {
        //var p2 = new Point3d(p1.X, _maxZ - p1.Z + 1, p1.Y);
        //var p2 = new Point3d(p1.X, _depth - p1.Z + 1, p1.Y);
        //var p2 = new Point3d(p1.X, p1.Z - _depth + 1, p1.Y);
        var p2 = new Point3d(p1.X, -p1.Z, p1.Y);
        map.Add(p2);
    });

    return map;
} 



    public List3d FlipX()
    {
        var map = new List3d();

        ForEach((p1) =>
        {
            var p2 = new Point3d(-p1.X, p1.Y, p1.Z);
            map.Add(p2);
        });

        return map;
    }

    public List3d FlipY()
    {
        var map = new List3d();

        ForEach((p1) =>
        {
            var p2 = new Point3d(p1.X, -p1.Y, p1.Z);
            map.Add(p2);
        });

        return map;
    }

    public List3d FlipZ()
    {
        var map = new List3d();

        ForEach((p1) =>
        {
            var p2 = new Point3d(p1.X, p1.Y, -p1.Z);
            map.Add(p2);
        });

        return map;
    }

    public List3d RotateXYZ()
    {
        var map = new List3d();

        ForEach((p1) =>
        {
            var p2 = new Point3d(p1.X, p1.Y, p1.Z);
            map.Add(p2);
        });

        return map;
    }

    public List3d RotateXZY()
    {
        var map = new List3d();

        ForEach((p1) =>
        {
            var p2 = new Point3d(p1.X, p1.Z, p1.Y);
            map.Add(p2);
        });

        return map;
    }

    public List3d RotateYXZ()
    {
        var map = new List3d();

        ForEach((p1) =>
        {
            var p2 = new Point3d(p1.Y, p1.X, p1.Z);
            map.Add(p2);
        });

        return map;
    }

    public List3d RotateYZX()
    {
        var map = new List3d();

        ForEach((p1) =>
        {
            var p2 = new Point3d(p1.Y, p1.Z, p1.X);
            map.Add(p2);
        });

        return map;
    }

    public List3d RotateZXY()
    {
        var map = new List3d();

        ForEach((p1) =>
        {
            var p2 = new Point3d(p1.Z, p1.X, p1.Y);
            map.Add(p2);
        });

        return map;
    }

    public List3d RotateZYX()
    {
        var map = new List3d();

        ForEach((p1) =>
        {
            var p2 = new Point3d(p1.Z, p1.Y, p1.X);
            map.Add(p2);
        });

        return map;
    }
    public void CalcDimensions()
    {
        int minX, maxX, minY, maxY, minZ, maxZ, width, height, depth;
        minX = maxX = minY = maxY = minZ = maxZ = width = height = depth = 0;

        ForEach((p) =>
        {
            minX = Math.Min(minX, p.X);
            maxX = Math.Max(maxX, p.X);
            minY = Math.Min(minY, p.Y);
            maxY = Math.Max(maxY, p.Y);
            minZ = Math.Min(minZ, p.Z);
            maxZ = Math.Max(maxZ, p.Z);
        });

        width = maxX - minX;
        height = maxX - minX;
        depth = maxX - minX;

        _maxX = maxX;
        _maxY = maxY;
        _maxZ = maxZ;

        _width = width;
        _height = height;
        _depth = depth;
    }

#endif