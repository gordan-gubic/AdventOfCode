#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2020.Days;

public class Day02 : Day
{
    private List<Password> _source;
    private List<Password> _data;

    private string _pattern = @"(?<min>\d*)-(?<max>\d*) (?<char>\w): (?<word>\w*)";

    public Day02(ILog log, IParser parser) : base(log, parser)
    {
        EnableDebug();
        Initialize();
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Year = 2020;
        Parser.Day = 2;
        Parser.Type = ParserFileType.Real;

        _source = Parser.Parse(ConvertInput);
    }

    /// <inheritdoc />
    protected override void ProcessData()
    {
        _data = _source;
    }

    /// <inheritdoc />
    public override void DumpInput()
    {
        DumpData();
    }

    protected override void ComputePart1()
    {
        foreach (var password in _data)
        {
            Add(Validate1(password));
        }
    }

    protected override void ComputePart2()
    {
        foreach (var password in _data)
        {
            Add(Validate2(password));
        }
    }

    private bool Validate1(Password password)
    {
        var target = password.Letter;
        var sum = password.Word.Where(x => x == target).Sum(x => (x == target) ? 1 : 0);

        return sum >= password.Min && sum <= password.Max;
    }

    private bool Validate2(Password password)
    {
        var target = password.Letter;
        var word = password.Word.ToLower();

        var letter1 = word[password.Min - 1];
        var letter2 = word[password.Max - 1];

        var isValid1 = word[password.Min - 1] == target && word[password.Max - 1] != target;
        var isValid2 = word[password.Min - 1] != target && word[password.Max - 1] == target;

        // Log.DebugLog(ClassId, $"[{target}]; [{word}]; [{password.Min}, {password.Max}]; [{word[password.Min-1]}]; [{word[password.Max - 1]}]; [{isValid1 || isValid2}]");

        return isValid1 || isValid2;
    }

    private Password ConvertInput(string input)
    {
        var options = RegexOptions.Singleline;
        var password = new Password();

        foreach (Match m in Regex.Matches(input, _pattern, options))
        {
            // Log.DebugLog(ClassId, $"[{m.Value}] found at index [m.Index].");
            password.Min = Int32.Parse(m.Groups["min"]?.Value);
            password.Max = Int32.Parse(m.Groups["max"]?.Value);
            password.Letter = Char.Parse(m.Groups["char"]?.Value.ToLower());
            password.Word = m.Groups["word"]?.Value.ToLower();
        }

        return password;
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        Log.DebugLog(ClassId);

        _data[0].Dump("Item");
        _data.DumpCollection("List");
    }
}

#if DUMP
#endif