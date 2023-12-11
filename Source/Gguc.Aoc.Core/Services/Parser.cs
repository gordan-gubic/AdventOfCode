namespace Gguc.Aoc.Core.Services;

public class Parser : IParser
{
    public const char DefaultMapCharacter = '#';

    private static readonly string ClassId = nameof(Parser);

    private readonly ILog _log;
    private readonly IFileReader _file;

    private List<string> _lines;

    public Parser(ILog log, IFileReader file)
    {
        _log = log;
        _file = file;
    }

    public int Year { get; set; }

    public int Day { get; set; }

    public ParserFileType Type { get; set; }

    public List<string> Parse()
    {
        if (!ValidateFile()) return default;

        _lines = _file.ReadFile().ToList();

        return ParseLines(_lines, Converters.Copy);
    }

    public List<T> Parse<T>(Func<string, T> convert)
    {
        if (!ValidateFile()) return default;

        _lines = _file.ReadFile().ToList();

        return ParseLines(_lines, convert);
    }

    public List<List<string>> ParseBlock()
    {
        if (!ValidateFile()) return default;

        _lines = _file.ReadFile().ToList();

        return ParseBlockLines(_lines, Converters.Copy);
    }

    public List<List<T>> ParseBlock<T>(Func<string, T> convert)
    {
        if (!ValidateFile()) return default;

        _lines = _file.ReadFile().ToList();

        return ParseBlockLines(_lines, convert);
    }

    public List<int> ParseSequence()
    {
        if (!ValidateFile()) return default;

        _lines = _file.ReadFile().ToList();

        return ParseSequenceLine(_lines[0], Converters.ToInt);
    }

    public List<T> ParseSequence<T>(Func<string, T> convert)
    {
        if (!ValidateFile()) return default;

        _lines = _file.ReadFile().ToList();

        return ParseSequenceLine(_lines[0], convert);
    }

    public bool[,] ParseMap(char mapChar = DefaultMapCharacter)
    {
        if (!ValidateFile()) return default;

        _lines = _file.ReadFile().ToList();

        var map = new bool[_lines[0].Length, _lines.Count];

        for (var y = 0; y < _lines.Count; y++)
        {
            for (var x = 0; x < _lines[y].Length; x++)
            {
                map[x, y] = _lines[y][x] == mapChar;
            }
        }

        return map;
    }

    public Map<bool> ParseMapBool(char mapChar = DefaultMapCharacter)
    {
        if (!ValidateFile()) return default;

        _lines = _file.ReadFile().ToList();

        var map = new Map<bool>(_lines[0].Length, _lines.Count);

        for (var y = 0; y < _lines.Count; y++)
        {
            for (var x = 0; x < _lines[y].Length; x++)
            {
                map.Values[x, y] = _lines[y][x] == mapChar;
            }
        }

        return map;
    }

    public Map<int> ParseMapInt()
    {
        if (!ValidateFile()) return default;

        _lines = _file.ReadFile().ToList();

        var map = new Map<int>(_lines[0].Length, _lines.Count);

        for (var y = 0; y < _lines.Count; y++)
        {
            for (var x = 0; x < _lines[y].Length; x++)
            {
                map.Values[x, y] = _lines[y][x].ToInt();
            }
        }

        return map;
    }

    public Map<int> ParseMapInt(Dictionary<char, int> mapper)
    {
        if (!ValidateFile()) return default;

        _lines = _file.ReadFile().ToList();

        var map = new Map<int>(_lines[0].Length, _lines.Count);

        for (var y = 0; y < _lines.Count; y++)
        {
            for (var x = 0; x < _lines[y].Length; x++)
            {
                map.Values[x, y] = mapper[_lines[y][x]];
            }
        }

        return map;
    }

    public Map<char> ParseMapChar()
    {
        if (!ValidateFile()) return default;

        _lines = _file.ReadFile().ToList();

        var map = new Map<char>(_lines[0].Length, _lines.Count);

        for (var y = 0; y < _lines.Count; y++)
        {
            for (var x = 0; x < _lines[y].Length; x++)
            {
                map.Values[x, y] = _lines[y][x];
            }
        }

        return map;
    }

    public void Log(int line = 0)
    {
        if (_lines.Count <= line) return;

        _log.DebugLog(ClassId, _lines[line]);
    }

    private List<T> ParseLines<T>(List<string> lines, Func<string, T> convert)
    {
        var collection = new List<T>();

        foreach (var line in lines)
        {
            var r = ParseLine<T>(line, convert);
            collection.Add(r);
        }

        return collection;
    }

    private List<List<T>> ParseBlockLines<T>(List<string> lines, Func<string, T> convert)
    {
        var collection = new List<List<T>>();

        var buffer = new List<T>();

        void AddToCollection()
        {
            if (buffer.Count == 0) return;

            collection.Add(buffer.ToList());
            buffer.Clear();
        }

        foreach (var line in lines)
        {
            if (line.IsWhitespace())
            {
                AddToCollection();
                continue;
            }

            var r = ParseLine(line, convert);
            buffer.Add(r);
        }

        AddToCollection();

        return collection;
    }

    private List<T> ParseSequenceLine<T>(string input, Func<string, T> convert)
    {
        var result = new List<T>();
        var sequence = input.Split(',').ToList();

        sequence.ForEach(x => result.Add(convert(x)));

        return result;
    }

    private T ParseLine<T>(string line, Func<string, T> convert)
    {
        var record = convert(line);

        // _log.DebugLog(ClassId, $"Record={record}");

        return record;
    }

    private void Configure()
    {
        switch (Type)
        {
            case ParserFileType.Real:
                _file.FileType = FileType.Zip;
                _file.FileName = Paths.ZipFilePath(Year);
                _file.FileEntry = Paths.DayFileEntry(Day);
                return;

            case ParserFileType.Example:
                _file.FileType = FileType.Zip;
                _file.FileName = Paths.ZipFilePath(Year);
                _file.FileEntry = Paths.DayFileEntry(Day, true);
                return;

            default:
                _file.FileType = FileType.Default;
                _file.FileName = Paths.InputSourceTest;
                return;
        }
    }

    private bool ValidateFile()
    {
        Configure();

        return _file.ValidateFile();
    }
}

#if DUMP

#endif