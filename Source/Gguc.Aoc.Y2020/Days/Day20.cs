#define LOGx
#define STOPWATCH

namespace Gguc.Aoc.Y2020.Days;

public class Day20 : Day
{
    private List<string> _source;

    private Dictionary<int, Tile20> _tiles;
    private Dictionary<int, int> _sides;
    private Map<bool> _image;
    private List<Point> _dragonPoints;

    private int _width;
    private int _height;
    private int _rawImageSize;

    public Day20(ILog log, IParser parser) : base(log, parser)
    {
        EnableDebug();
        Initialize();
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Year = 2020;
        Parser.Day = 20;
        Parser.Type = ParserFileType.Real;

        _source = Parser.Parse();
    }

    /// <inheritdoc />
    protected override void ProcessData()
    {
        _tiles = new Dictionary<int, Tile20>();
        _sides = new Dictionary<int, int>();

        _ProcessSource();

        _ProcessTarget();

        _ProcessTiles();
    }

    /// <inheritdoc />
    public override void DumpInput()
    {
        DumpData();
    }

    protected override void ComputePart1()
    {
        var tiles = _tiles.Where(x => x.Value.MatchSides == 2).Select(x => x.Value.Index).ToList();

        Info($"matching tiles=[{tiles.ToJson()}]");

        tiles.ForEach(x => Multiply(x));
    }

    protected override void ComputePart2()
    {
        ComposeImage();

        var countImageX = _image.CountValues(true);
        var countDragonX = _dragonPoints.Count;

        Info($"Image #  = [{countImageX}]");
        Info($"Dragon # = [{countDragonX}]");

        var countDragonN = FindAllDragons(_image);

        Info($"Dragon N = [{countDragonN}]");

        Result = countImageX - (countDragonN * countDragonX);
    }

    private void ComposeImage()
    {
        var imageRaw = AssambeTiles();

        RotateTiles(imageRaw);

        // _image = DrawImageFull(imageRaw);
        _image = DrawImageInner(imageRaw);
        PrintMap(_image);
    }

    private int FindAllDragons(Map<bool> image)
    {
        var listCountDragonN = new List<int>();
        int steps = 0;

        do
        {
            listCountDragonN.Add(FindDragon(image));

            image = image.FlipHorizontally();

            listCountDragonN.Add(FindDragon(image));

            image = image.FlipHorizontally();
            image = image.Rotate();

            steps++;
        } while (steps < 4);

        return listCountDragonN.Max();
    }

    private int FindDragon(Map<bool> image)
    {
        var count = 0;
        var width = image.Width;
        var height = image.Height;

        var isValid = false;
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                foreach (var point in _dragonPoints)
                {
                    isValid = image.GetValue(x + point.X, y + point.Y);

                    if (!isValid)
                    {
                        break;
                    }
                }

                if (isValid)
                {
                    Debug($"Found! at [{x}, {y}]");
                    count++;
                }
            }
        }

        return count;
    }

    private Tile20[,] AssambeTiles()
    {
        int width = _rawImageSize;
        int height = _rawImageSize;

        var imageRaw = new Tile20[width, height];

        var cornerTiles = _tiles.Where(x => x.Value.MatchSides == 2).Select(x => x.Value.Index).ToList();
        var sideTiles = _tiles.Where(x => x.Value.MatchSides == 3).Select(x => x.Value.Index).ToList();
        var bodyTiles = _tiles.Where(x => x.Value.MatchSides == 4).Select(x => x.Value.Index).ToList();

        void Add(int imageX, int imageY, int tileIx, List<int> removeFromList)
        {
            imageRaw[imageX, imageY] = _tiles[tileIx];
            removeFromList.Remove(tileIx);
        }

        var tileindex = 0;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // first corner
                if (x == 0 && y == 0)
                {
                    tileindex = cornerTiles.FirstOrDefault();
                    Add(x, y, tileindex, cornerTiles);
                    continue;
                }

                // corners
                if ((x == 0 || x == width - 1) && (y == 0 || y == height - 1))
                {
                    if (x == 0)
                    {
                        var upSides = imageRaw[x, y - 1].Sides;
                        tileindex = FindTileByOneSides(cornerTiles, upSides);
                        Add(x, y, tileindex, cornerTiles);
                    }
                    else
                    {
                        var leftSides = imageRaw[x - 1, y].Sides;
                        tileindex = FindTileByOneSides(cornerTiles, leftSides);
                        Add(x, y, tileindex, cornerTiles);
                    }

                    continue;
                }

                // sides
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {

                    if (x == 0 || x == width - 1)
                    {
                        var upSides = imageRaw[x, y - 1].Sides;
                        tileindex = FindTileByOneSides(sideTiles, upSides);
                        Add(x, y, tileindex, sideTiles);
                    }
                    else
                    {
                        var leftSides = imageRaw[x - 1, y].Sides;
                        tileindex = FindTileByOneSides(sideTiles, leftSides);
                        Add(x, y, tileindex, sideTiles);
                    }

                    continue;
                }

                // body
                {
                    var leftSides = imageRaw[x - 1, y].Sides;
                    var upSides = imageRaw[x, y - 1].Sides;
                    tileindex = FindTileByBothSides(bodyTiles, leftSides, upSides);
                    Add(x, y, tileindex, bodyTiles);
                }
            }
        }

        Debug($"corners: {cornerTiles.ToJson()}");
        Debug($"sides..: {sideTiles.ToJson()}");
        Debug($"blocks.: {bodyTiles.ToJson()}");

        Print(imageRaw);

        return imageRaw;
    }

    private void RotateTiles(Tile20[,] imageRaw)
    {
        var width = imageRaw.GetLength(0);
        var height = imageRaw.GetLength(1);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var tile = imageRaw[x, y];

                var leftTile = (x > 0) ? imageRaw[x - 1, y] : default;
                var upTile = (y > 0) ? imageRaw[x, y - 1] : default;

                if (leftTile != null)
                {
                    CalculateSides(leftTile);
                    var r = CompareSides(leftTile.Sides, tile.Sides);
                    if (r.isValid)
                    {
                        tile.MatchLeft = r.rotateIndex;
                    }
                }
                else if (upTile != null)
                {
                    CalculateSides(upTile);
                    var r = CompareSides(upTile.Sides, tile.Sides);
                    if (r.isValid)
                    {
                        tile.MatchUp = r.rotateIndex;
                    }
                }

                Debug($"Tile={tile}");

                RotateTile(tile);
                CalculateSides(tile);
            }
        }
    }

    private void RotateTile(Tile20 tile)
    {
        var map = tile.Map;

        if (tile.MatchLeft != -1)
        {
            switch (tile.MatchLeft)
            {
                case 0:
                    map = map.Rotate();
                    map = map.FlipHorizontally();
                    break;
                case 1:
                    map = map.Rotate();
                    map = map.Rotate();
                    map = map.Rotate();
                    break;
                case 2:
                    map = map.Rotate();
                    break;
                case 3:
                    map = map.FlipHorizontally();
                    map = map.Rotate();
                    break;
                case 4:
                    break;
                case 5:
                    map = map.FlipVertically();
                    break;
                case 6:
                    map = map.FlipHorizontally();
                    break;
                case 7:
                    map = map.Rotate();
                    map = map.Rotate();
                    break;
            }
        }
        else if (tile.MatchUp != -1)
        {
            switch (tile.MatchUp)
            {
                case 0:
                    break;
                case 1:
                    map = map.FlipHorizontally();
                    break;
                case 2:
                    map = map.FlipVertically();
                    break;
                case 3:
                    map = map.Rotate();
                    map = map.Rotate();
                    break;
                case 4:
                    map = map.Rotate();
                    map = map.FlipHorizontally();
                    break;
                case 5:
                    map = map.Rotate();
                    break;
                case 6:
                    map = map.Rotate();
                    map = map.Rotate();
                    map = map.Rotate();
                    break;
                case 7:
                    map = map.FlipHorizontally();
                    map = map.Rotate();
                    break;
            }
        }

        if (tile.FlipHor) map = map.FlipHorizontally();
        if (tile.FlipVer) map = map.FlipVertically();

        tile.Map = map;
    }

    private Map<bool> DrawImageFull(Tile20[,] imageRaw)
    {
        var widthImageRaw = imageRaw.GetLength(0);
        var heightImageRaw = imageRaw.GetLength(1);
        var widthTile = imageRaw[0, 0].Map.Width;
        var heightTile = imageRaw[0, 0].Map.Height;
        var width = widthImageRaw * widthTile;
        var height = heightImageRaw * heightTile;

        var image = new Map<bool>(width, height);

        for (int rawy = 0; rawy < heightImageRaw; rawy++)
        {
            for (int rawx = 0; rawx < widthImageRaw; rawx++)
            {
                for (int y = 0; y < heightTile; y++)
                {
                    for (int x = 0; x < widthTile; x++)
                    {
                        var ix = x + (widthTile * rawx);
                        var iy = y + (heightTile * rawy);

                        image.Values[ix, iy] = imageRaw[rawx, rawy].Map.Values[x, y];
                    }
                }
            }
        }

        return image;
    }

    private Map<bool> DrawImageInner(Tile20[,] imageRaw)
    {
        var widthImageRaw = imageRaw.GetLength(0);
        var heightImageRaw = imageRaw.GetLength(1);
        var widthTile = imageRaw[0, 0].Map.Width - 2;
        var heightTile = imageRaw[0, 0].Map.Height - 2;
        var width = widthImageRaw * widthTile;
        var height = heightImageRaw * heightTile;

        var image = new Map<bool>(width, height);

        for (int rawy = 0; rawy < heightImageRaw; rawy++)
        {
            for (int rawx = 0; rawx < widthImageRaw; rawx++)
            {
                for (int y = 0; y < heightTile; y++)
                {
                    for (int x = 0; x < widthTile; x++)
                    {
                        var ix = x + (widthTile * rawx);
                        var iy = y + (heightTile * rawy);

                        image.Values[ix, iy] = imageRaw[rawx, rawy].Map.Values[x + 1, y + 1];
                    }
                }
            }
        }

        return image;
    }

    private int FindTileByOneSides(List<int> tiles, List<int> leftSides)
    {
        foreach (var tileIndex in tiles)
        {
            var sides = _tiles[tileIndex].Sides;
            var result = CompareSides(leftSides, sides).isValid;

            if (result) return tileIndex;
        }

        return -1;
    }

    private int FindTileByBothSides(List<int> tiles, List<int> leftSides, List<int> upSides)
    {
        foreach (var tileIndex in tiles)
        {
            var sides = _tiles[tileIndex].Sides;
            var result1 = CompareSides(leftSides, sides).isValid;
            var result2 = CompareSides(upSides, sides).isValid;

            if (result1 && result2) return tileIndex;
        }

        return -1;
    }

    private (bool isValid, int rotateIndex) CompareSides(List<int> prevTileSides, List<int> sides)
    {
        var modSides = new HashSet<int>
            {
                prevTileSides[0],
                prevTileSides[2],
                prevTileSides[4],
                prevTileSides[6],
            };

        for (int i = 0; i < sides.Count; i++)
        {
            if (modSides.Contains(sides[i])) return (true, i);
        }

        return (false, -1);
    }

    private void _ProcessSource()
    {
        var index = 0;
        var tileIndex = 0;
        var mapRaw = new List<string>();

        void AddMap()
        {
            var map = new Map<bool>(mapRaw, x => x == '#');
            var tile = new Tile20 { Index = tileIndex, Map = map };
            _tiles[tileIndex] = tile;

            index = 0;
            mapRaw.Clear();
        }

        foreach (var line in _source)
        {
            if (line.IsWhitespace()) { AddMap(); continue; }

            index++;

            if (index == 1)
            {
                tileIndex = line.RegexValue(@"Tile (\d+):").ToInt();
                continue;
            }

            mapRaw.Add(line);
        }

        AddMap();
    }

    private void _ProcessTarget()
    {
        Debug();

        var target = new List<string>
            {
                "                  # ",
                "#    ##    ##    ###",
                " #  #  #  #  #  #   ",
            };

        var dragon = new Map<bool>(target, x => x == '#');

        // FindDragonPoints();
        var points = new List<Point>();
        var width = dragon.Width;
        var height = dragon.Height;

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                if (dragon.GetValue(x, y)) points.Add(new Point(x, y));
            }
        }

        _dragonPoints = points;

        PrintMap(dragon);

        _dragonPoints.DumpCollection("_dragonPoints");

        Debug();
    }

    private void _ProcessTiles()
    {
        _width = _tiles.FirstOrDefault().Value.Map.Width;
        _height = _tiles.FirstOrDefault().Value.Map.Height;
        _rawImageSize = (int)Math.Sqrt(_tiles.Count);

        foreach (var tile in _tiles)
        {
            CalculateSides(tile.Value);
            CountSides(tile.Value);
        }

        foreach (var tile in _tiles)
        {
            MatchSides(tile.Value);
        }
    }

    private void CalculateSides(Tile20 tile)
    {
        var map = tile.Map;

        var sbUp = new StringBuilder();
        var sbDn = new StringBuilder();
        var sbLf = new StringBuilder();
        var sbRg = new StringBuilder();
        var sbUpI = new StringBuilder();
        var sbDnI = new StringBuilder();
        var sbLfI = new StringBuilder();
        var sbRgI = new StringBuilder();

        for (int i = 0; i < _width; i++)
        {
            sbUp.Append(map.GetValue(i, 0) ? "1" : "0");
            sbDn.Append(map.GetValue(i, _width - 1) ? "1" : "0");
            sbLf.Append(map.GetValue(0, i) ? "1" : "0");
            sbRg.Append(map.GetValue(_width - 1, i) ? "1" : "0");
        }

        for (int i = _width - 1; i >= 0; i--)
        {
            sbUpI.Append(map.GetValue(i, 0) ? "1" : "0");
            sbDnI.Append(map.GetValue(i, _width - 1) ? "1" : "0");
            sbLfI.Append(map.GetValue(0, i) ? "1" : "0");
            sbRgI.Append(map.GetValue(_width - 1, i) ? "1" : "0");
        }
        tile.Sides.Clear();

        tile.Sides.Add(sbUp.ToString().FromBinaryStringToInt());
        tile.Sides.Add(sbUpI.ToString().FromBinaryStringToInt());

        tile.Sides.Add(sbDn.ToString().FromBinaryStringToInt());
        tile.Sides.Add(sbDnI.ToString().FromBinaryStringToInt());

        tile.Sides.Add(sbLf.ToString().FromBinaryStringToInt());
        tile.Sides.Add(sbLfI.ToString().FromBinaryStringToInt());

        tile.Sides.Add(sbRg.ToString().FromBinaryStringToInt());
        tile.Sides.Add(sbRgI.ToString().FromBinaryStringToInt());
    }

    private void CountSides(Tile20 tile)
    {
        foreach (var tileSide in tile.Sides)
        {
            if (!_sides.ContainsKey(tileSide)) _sides[tileSide] = 0;
            _sides[tileSide]++;
        }
    }

    private void MatchSides(Tile20 tile)
    {
        var count = 0;

        for (int i = 0; i < tile.Sides.Count - 1; i = i + 2)
        {
            var target = Math.Max(_sides[tile.Sides[i]], _sides[tile.Sides[i + 1]]);
            if (target > 1) count++;
        }

        tile.MatchSides = count;
    }

    [Conditional("LOGxx")]
    private void DumpData()
    {
        if (!Log.EnableDebug) return;

        Debug();

        foreach (var tile in _tiles.Values)
        {
            tile.Print();
        }

        // _tiles.Values.DumpCollection("_tiles");
        _sides.DumpJson("_sides");
    }

    [Conditional("LOG")]
    private void Print(Tile20[,] imageRaw)
    {
        var width = imageRaw.GetLength(0);
        var height = imageRaw.GetLength(1);

        for (int y = 0; y < height; y++)
        {
            Console.WriteLine(y);

            for (int x = 0; x < width; x++)
            {
                Console.WriteLine(imageRaw[x, y]);
            }

            Console.WriteLine();
        }
    }

    [Conditional("LOG")]
    public void PrintMap(Map<bool> map)
    {
        var width = map.Width;
        var height = map.Height;

        var sb = new StringBuilder();

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                sb.Append(map.GetValue(x, y) ? "#" : ".");
            }
            sb.AppendLine();
        }

        Console.WriteLine(sb.ToString());
    }
}

#if DUMP

        private int FindTileByLeftSides(List<int> sideTiles, List<int> leftSides)
        {
            foreach (var tileIndex in sideTiles)
            {
                var sides = _tiles[tileIndex].Sides;
                var result = CompareSides(leftSides, sides);

                if (result.isValid)
                {
                    // _tiles[tileIndex].MatchLeft = ModifyMatch(result.pi, result.si);
                    _tiles[tileIndex].MatchLeft = result.si;
                    _tiles[tileIndex].FlipVer = result.pi % 2 == 1;
                    return tileIndex;
                }
            }

            return -1;
        }

        private int FindTileByUpSides(List<int> sideTiles, List<int> upSides)
        {
            foreach (var tileIndex in sideTiles)
            {
                var sides = _tiles[tileIndex].Sides;
                var result = CompareSides(upSides, sides);

                if (result.isValid)
                {
                    // _tiles[tileIndex].MatchUp = ModifyMatch(result.pi, result.si);
                    _tiles[tileIndex].MatchUp = result.si;
                    _tiles[tileIndex].FlipHor = result.pi % 2 == 1;
                    return tileIndex;
                }
            }

            return -1;
        }

        private int FindTileByBothSides(List<int> sideTiles, List<int> leftSides, List<int> upSides)
        {
            foreach (var tileIndex in sideTiles)
            {
                var sides = _tiles[tileIndex].Sides;
                var result1 = CompareSides(leftSides, sides);
                var result2 = CompareSides(upSides, sides);

                if (result1.isValid && result2.isValid)
                {
                    //_tiles[tileIndex].MatchLeft = ModifyMatch(result1.pi, result1.si);
                    //_tiles[tileIndex].MatchUp = ModifyMatch(result2.pi, result2.si);
                    _tiles[tileIndex].MatchLeft = result1.si;
                    _tiles[tileIndex].FlipVer = result1.pi % 2 == 1;
                    _tiles[tileIndex].MatchUp = result2.si;
                    _tiles[tileIndex].FlipHor = result2.pi % 2 == 1;
                    return tileIndex;
                }
            }

            return -1;
        }

        private int ModifyMatch(int pi, int si)
        {
            var mode = pi % 2 == 1;

            switch (si)
            {
                case 0: return mode ? 0 : 1;
                case 1: return mode ? 1 : 0;
                case 2: return mode ? 2 : 3;
                case 3: return mode ? 3 : 2;
                case 4: return mode ? 4 : 5;
                case 5: return mode ? 5 : 4;
                case 6: return mode ? 6 : 7;
                case 7: return mode ? 7 : 6;
            }

            return si;
        }

        private bool CompareSidesX(List<int> prevTileSides, List<int> sides)
        {
            foreach (var side in sides)
            {
                if (prevTileSides.Contains(side))
                {
                    // var pi = prevTileSides.IndexOf(side);
                    // var si = sides.IndexOf(side);
                    // Debug($"Found side match! Side=[{side}] PI=[{pi}] SI=[{si}]");
                    return true;
                }
            }

            return false;
        }

        private (bool isValid, int pi, int si) CompareSides(List<int> prevTileSides, List<int> sides)
        {
            var modSides = prevTileSides.ToList();
            modSides.RemoveAt(7);
            modSides.RemoveAt(5);
            modSides.RemoveAt(3);
            modSides.RemoveAt(1);

            foreach (var side in sides)
            {
                if (modSides.Contains(side))
                {
                    var pi = prevTileSides.IndexOf(side);
                    var si = sides.IndexOf(side);
                    return (true, pi, si);
                }
            }

            return (false, -1, -1);
        }
        

        private bool CompareSides(List<int> prevTileSides, List<int> sides)
        {
            var modSides = new HashSet<int>
            {
                prevTileSides[0],
                prevTileSides[2],
                prevTileSides[4],
                prevTileSides[6],
            };

            foreach (var side in sides)
            {
                if (modSides.Contains(side)) return true;
            }

            return false;
        }
#endif