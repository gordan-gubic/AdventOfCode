#define LOGx
#define STOPWATCH

namespace Gguc.Aoc.Y2019.Days;

public class Day06 : Day
{
    private List<(string, string)> _source;
    private List<(string, string)> _data;

    private Dictionary<string, Planet> _planets;

    public Day06(ILog log, IParser parser) : base(log, parser)
    {
        EnableDebug();
        Initialize();
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Year = 2019;
        Parser.Day = 6;
        Parser.Type = ParserFileType.Real;

        _source = Parser.Parse(ConvertInput);
    }

    /// <inheritdoc />
    protected override void ProcessData()
    {
        _data = _source;
        _ProcessData();
    }

    /// <inheritdoc />
    public override void DumpInput()
    {
        DumpData();
    }

    protected override void ComputePart1()
    {
        ComputeValues();
        Result = _planets.Sum(x => x.Value.Value);
    }

    protected override void ComputePart2()
    {
        var listYou = GetOrbitList("YOU");
        var listSan = GetOrbitList("SAN");

        var restYou = listYou.Except(listSan).ToList();
        var restSan = listSan.Except(listYou).ToList();
        var union = restYou.Union(restSan).ToList();

        Result = union.Count;
    }

    private void ComputeValues()
    {
        foreach (var name in _planets.Keys.ToList())
        {
            ComputePlanet(name);
        }
    }

    private int ComputePlanet(string name)
    {
        var planet = GetPlanet(name);
        if (planet.Value != -1) return planet.Value;

        planet.Value = ComputePlanet(planet.Orbits) + 1;
        return planet.Value;
    }

    private List<string> GetOrbitList(string name)
    {
        var list = new List<string>();

        var planet = GetPlanet(name);
        name = planet.Orbits;

        while (true)
        {
            planet = GetPlanet(name);
            list.Add(name);

            name = planet.Orbits;

            if (name.IsWhitespace()) break;
        }

        return list;
    }

    private Planet GetPlanet(string name)
    {
        if (!_planets.ContainsKey(name))
        {
            _planets[name] = new Planet { Name = name, Value = 0 };
        }

        var planet = _planets[name];
        return planet;
    }

    private (string, string) ConvertInput(string input)
    {
        var planets = input.Split(')');
        return (planets[0], planets[1]);
    }

    private void _ProcessData()
    {
        _planets = new Dictionary<string, Planet>();

        foreach ((string p1, string p2) in _data)
        {
            _planets[p2] = new Planet { Name = p2, Orbits = p1 };
        }
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (!Log.EnableDebug) return;

        Debug();

        _data[0].Dump("Item");
        // _planets.DumpJson("_planets");
        // _planets.DumpJsonIndented("_planets");
    }
}

#if DUMP

#endif