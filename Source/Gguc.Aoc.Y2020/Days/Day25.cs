#define LOGx
#define STOPWATCH

namespace Gguc.Aoc.Y2020.Days;

public class Day25 : Day
{
    private List<string> _source;
    private List<string> _data;
    private long _cardNumber;
    private long _doorNumber;
    private long _cardLoopSize;
    private long _doorLoopSize;

    private const long Subject = 7;
    private const long Salt = 20201227;

    public Day25(ILog log, IParser parser) : base(log, parser)
    {
        EnableDebug();
        Initialize();
    }

    /// <inheritdoc />
    protected override void InitParser()
    {
        Parser.Year = 2020;
        Parser.Day = 25;
        Parser.Type = ParserFileType.Real;

        _source = Parser.Parse();
    }

    /// <inheritdoc />
    protected override void ProcessData()
    {
        _data = _source;

        _cardNumber = _data[0].ToLong();
        _doorNumber = _data[1].ToLong();
    }

    /// <inheritdoc />
    public override void DumpInput()
    {
        DumpData();
    }

    protected override void ComputePart1()
    {
        FindLoopSize();

        Info($"LoopSize: Card: [{_cardNumber}, {_cardLoopSize}], door: [{_doorNumber}, {_doorLoopSize}]");

        Result = FindEncryptionKey();
    }

    protected override void ComputePart2()
    {
    }

    private void FindLoopSize()
    {
        int i = 1;
        var subject = Subject;

        Debug($"Loop: {i,4}, subject: [{subject,10}], card: [{_cardNumber,10}], door: [{_doorNumber,10}]");

        while (true)
        {
            i++;

            subject = Loop(subject, Subject);

            if (subject == _cardNumber) _cardLoopSize = i;
            if (subject == _doorNumber) _doorLoopSize = i;

            Debug($"Loop: {i,4}, subject: [{subject,10}]");

            if (_cardLoopSize > 0 || _doorLoopSize > 0)
            {
                break;
            }
        }
    }

    private long FindEncryptionKey()
    {
        var loop = Math.Max(_cardLoopSize, _doorLoopSize);
        var number = (_cardLoopSize > _doorLoopSize) ? _doorNumber : _cardNumber;
        var encryption = number;
        Info($"loop: {loop}, init number: [{number,10}]");

        for (int i = 2; i <= loop; i++)
        {
            encryption = Loop(encryption, number);
            Debug($"Loop: {i,4}, encryption: [{encryption,10}]");
        }

        return encryption;
    }

    private long Loop(in long id, in long subject)
    {
        var newId = (id * subject) % Salt;
        return newId;
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (!Log.EnableDebug) return;

        Debug();

        _data[0].Dump("Item");
        _cardNumber.DumpJson("_cardNumber");
        _doorNumber.DumpJson("_doorNumber");
    }
}

#if DUMP
#endif