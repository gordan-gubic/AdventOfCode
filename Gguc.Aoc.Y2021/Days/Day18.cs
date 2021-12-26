#define LOGx
#define STOPWATCH

namespace Gguc.Aoc.Y2021.Days;

/*
 * 
 * https://www.geeksforgeeks.org/tree-traversals-inorder-preorder-and-postorder/
 * 
 */

public class Day18 : Day
{
    private const int YEAR = 2021;
    private const int DAY = 18;

    private const string RegexPattern = @"(\[[\w]+,[\w]+\])";
    
    private List<string> _source;
    private List<Snail> _data;

    public Day18(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();
    }

    #region Parse
    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Real;

        _source = Parser.Parse();
    }

    protected override void ProcessData()
    {
        _data = ParseAllText();
    }

    private List<Snail> ParseAllText()
    {
        var snails = new List<Snail>();

        foreach (var line in _source)
        {
            snails.Add(ParseLine(line));
        }

        return snails;
    }

    private Snail ParseLine(string line)
    {
        var dict = new Dictionary<string, Snail>();
        var regex = new Regex(RegexPattern, RegexOptions.Singleline);

        var snail = default(Snail);

        while (true)
        {
            var rm = regex.Match(line);
            if (!rm.Success) break;

            var r = rm.Groups[1].Value;
            snail = Snail.Create(dict, r);
            line = regex.Replace(line, snail.Id, 1);
        }

        return snail;
    }
    #endregion Parse

    protected override void ComputePart1()
    {
        var snails = ParseAllText();
        var mem = new Day18Memory();

        ProcessSnails(mem, snails);

        mem.Grandparent.Dump("Grandparent");

        Result = mem.Grandparent.CalculateMagnitude();
    }

    protected override void ComputePart2()
    {
        Result = 0;

        var count = _data.Count;

        for (int i = 0; i < count; i++)
        {
            for (int j = 0; j < count; j++)
            {
                if (i == j) continue;

                var mem = new Day18Memory();
                var temp = new List<Snail>();
                temp.Add(ParseLine(_source[i]));
                temp.Add(ParseLine(_source[j]));

                ProcessSnails(mem, temp);

                var r = mem.Grandparent.CalculateMagnitude();
                Max(r);
            }
        }
    }

    private void ProcessSnails(Day18Memory mem, List<Snail> snails)
    {
        var queue = new Queue<Snail>(snails);

        while (queue.Count > 0)
        {
            var snail = queue.Dequeue();

            AddSnails(mem, snail);

            ProcessSnails(mem);
        }
    }

    private void AddSnails(Day18Memory mem, Snail snail)
    {
        if (mem.Grandparent == null)
        {
            mem.Grandparent = snail;
            return;
        }

        mem.Grandparent = Snail.Create(mem.Grandparent, snail);
    }

    private void ProcessSnails(Day18Memory mem)
    {
        var valid = false;

        while (!valid)
        {
            mem.Recreate();

            valid = ExplodeOne(mem) && SplitOne(mem);
        }
    }

    private bool ExplodeOne(Day18Memory mem)
    {
        var tree = mem.Tree;

        for (int i = 0; i < tree.Count; i++)
        {
            var snail = tree[i] as Snail;
            if (snail == null) continue;

            if (snail.Level > 4)
            {
                // $"{_grandparent} * Before-Explode * ".Dump();
                Explode(mem, snail);
                // $"{_grandparent} * After--Explode * ".Dump();
                return false;
            }
        }

        return true;
    }

    private bool SplitOne(Day18Memory mem)
    {
        var tree = mem.Tree;

        for (int i = 0; i < tree.Count; i++)
        {
            var snail = tree[i] as Snail;
            if (snail == null)
            {
                continue;
            }

            if (snail.IsLarge())
            {
                // $"{_grandparent} * Before-Split * ".Dump();
                Split(snail);
                // $"{_grandparent} * After--Split * ".Dump();
                return false;
            }
        }

        return true;
    }

    private void Explode(Day18Memory mem, Snail snail)
    {
        var parent = snail.Parent;
        var x = (BoxInt)snail.X;
        var y = (BoxInt)snail.Y;
        // $"Explode: [{x},{y}]".Dump();

        PassLeft(mem, x);
        PassRight(mem, y);

        if (parent.X as Snail == snail)
        {
            parent.X = BoxInt.Create(0);
        }
        else if (parent.Y as Snail == snail)
        {
            parent.Y = BoxInt.Create(0);
        }
    }

    private void Split(Snail snail)
    {
        var x = (snail.X is BoxInt b1 && b1.Value > 9) ? b1 : default;
        var y = (snail.Y is BoxInt b2 && b2.Value > 9) ? b2 : default;

        if (x != null)
        {
            var value = x.Value;

            var v1 = (int)Math.Floor(value / 2.0);
            var v2 = (int)Math.Ceiling(value / 2.0);

            //$"Split-01! Value=[{value}] from X=[{x}] to [{v1},{v2}]".Dump();

            var newSnail = new Snail { X = BoxInt.Create(v1), Y = BoxInt.Create(v2), Parent = snail, Level = snail.Level + 1 };

            snail.X = newSnail;
        }
        else if (y != null)
        {
            var value = y.Value;

            var v1 = (int)Math.Floor(value / 2.0);
            var v2 = (int)Math.Ceiling(value / 2.0);

            //$"Split-01! Value=[{value}] from X=[{y}] to [{v1},{v2}]".Dump();

            var newSnail = new Snail { X = BoxInt.Create(v1), Y = BoxInt.Create(v2), Parent = snail, Level = snail.Level + 1 };

            snail.Y = newSnail;
        }
    }

    private void PassLeft(Day18Memory mem, BoxInt value)
    {
        var tree = mem.Tree;

        var i = tree.IndexOf(value);
        
        if (i == 0) return;

        for (int j = i - 1; j >= 0; j--)
        {
            if (tree[j] is BoxInt b1)
            {
                b1.Add(value);
                break;
            }
        }
    }

    private void PassRight(Day18Memory mem, BoxInt value)
    {
        var tree = mem.Tree;

        var i = tree.IndexOf(value);
        var c = tree.Count;

        if (i >= c - 1) return;

        for (int j = i + 1; j < c; j++)
        {
            if (tree[j] is BoxInt b1)
            {
                b1.Add(value);
                break;
            }
        }
    }

    #region Dump
    public override void DumpInput()
    {
        DumpData();
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (!Log.EnableDebug) return;

        Debug();

        _data[0].Dump("Item");
        // _data.DumpCollection("List");
    }
    #endregion Dump
}