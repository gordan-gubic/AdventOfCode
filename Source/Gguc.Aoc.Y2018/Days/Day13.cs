#define LOG
#define STOPWATCH

namespace Gguc.Aoc.Y2018.Days;

public class Day13 : Day
{
    private const int YEAR = 2018;
    private const int DAY = 13;

    private List<string> _data;
    private List<Cart> _carts;
    private Map<char> _map;

    public Day13(ILog log, IParser parser) : base(log, parser, YEAR, DAY)
    {
        EnableDebug();
        Initialize();

        Expected1 = "53,133";
        Expected2 = "111,68";
    }

    protected override void InitParser()
    {
        Parser.Type = ParserFileType.Test;
        Parser.Type = ParserFileType.Real;

        _data = Parser.Parse();
    }

    #region Parse
    protected override void ProcessData()
    {
        _carts = new();

        var width = _data[0].Length;
        var height = _data.Count;
        
        _map = new Map<char>(_data, x => x);

        // Gromit do something!
        foreach (var line in _data)
        {
        }

        var carts = new [] { '<', '>', '^', 'v' };
        _map.ForEach((x, y, v) =>
        {
            if (carts.Contains(v))
            {
                var r = CreateCart(x, y, v);
                _carts.Add(r.Item1);
                _map[x, y] = r.Item2;
            }
        });
    }

    private (Cart, char) CreateCart(int x, int y, char ch)
    {
        var temp = ch switch
        {
            '<' => (Orientation.Left, '-'),
            '>' => (Orientation.Right, '-'),
            '^' => (Orientation.Top, '|'),
            'v' => (Orientation.Bottom, '|'),
            _ => (Orientation.Top, '-'),
        };

        var cart = new Cart { Orientation = temp.Item1, X = x, Y = y, Crossroad = Orientation.Left };

        return (cart, temp.Item2);
    }
    #endregion

    #region Head
    protected override void ComputePart1()
    {
        ProcessData();
        Result = MoveCarts1();
    }

    protected override void ComputePart2()
    {
        ProcessData();
        Result = MoveCarts2();
    }
    #endregion

    #region Body
    private long MoveCarts1()
    {
        var crash = false;
        var tick = 0;
        var x = 0;
        var y = 0;

        while (!crash)
        {
            foreach (var cart in _carts.OrderBy(c => c.Y).ThenBy(c => c.X))
            {
                MoveCart(cart);
                crash = CheckCrash(cart);
                if (crash)
                {
                    x = cart.X;
                    y = cart.Y;
                    Log.Debug($"Crash...{cart}  {tick}");
                    break;
                }
            }

            tick++;
        }

        Log.Info($"Ticks...{tick}");
        Log.Info($"Crash={x},{y}");
        return 0L;
    }

    private long MoveCarts2()
    {
        var tick = 0;

        while (true)
        {
            foreach (var cart in _carts.OrderBy(c => c.Y).ThenBy(c => c.X).ToList())
            {
                if(!_carts.Contains(cart)) continue;

                MoveCart(cart);
                if (CheckCrash(cart))
                {
                    RemoveCarts(cart.X, cart.Y);
                }
            }

            tick++;
            if(_carts.Count <= 1) break;
        }

        var left = _carts.FirstOrDefault();
        Log.Info($"Ticks...{tick}");
        Log.Info($"Carts={left.X},{left.Y}");
        return 0L;
    }

    private void MoveCart(Cart cart)
    {
        var ori = cart.Orientation;
        var x = cart.X;
        var y = cart.Y;

        switch (ori)
        {
            case Orientation.Top: y--; break;
            case Orientation.Bottom: y++; break;
            case Orientation.Left: x--; break;
            case Orientation.Right: x++; break;
        }

        var next = _map.GetValue(x, y);
        UpdateCart(cart, x, y, next);
    }

    private bool CheckCrash(Cart cart)
    {
        var count = _carts.Count(c => c.X == cart.X && c.Y == cart.Y);
        return count > 1;
    }

    private void RemoveCarts(int x, int y)
    {
        _carts.RemoveAll(c => c.X == x && c.Y == y);
    }

    private void UpdateCart(Cart cart, int x, int y, char next)
    {
        cart.X = x;
        cart.Y = y;

        var ori = cart.Orientation;

        if (next == '+')
        {
            if (cart.Crossroad == Orientation.Left)
            {
                ori = (Orientation)((int)(ori + 3) % 4);
            }
            else if (cart.Crossroad == Orientation.Right)
            {
                ori = (Orientation)((int)(ori + 1) % 4);
            }

            cart.Crossroad = cart.Crossroad switch
            {
                Orientation.Left => Orientation.Top,
                Orientation.Top => Orientation.Right,
                Orientation.Right => Orientation.Left,
                _ => Orientation.Top,
            };
        }
        else if (next is '\\')
        {
            ori = cart.Orientation switch
            {
                Orientation.Top => Orientation.Left,
                Orientation.Left => Orientation.Top,
                Orientation.Bottom => Orientation.Right,
                Orientation.Right => Orientation.Bottom,
                _ => Orientation.Top,
            };
        }
        else if (next is '/')
        {
            ori = cart.Orientation switch
            {
                Orientation.Top => Orientation.Right,
                Orientation.Right => Orientation.Top,
                Orientation.Bottom => Orientation.Left,
                Orientation.Left => Orientation.Bottom,
                _ => Orientation.Top,
            };
        }

        cart.Orientation = ori;
    }

    #endregion

    #region Dump
    /// <inheritdoc />
    public override void DumpInput()
    {
        DumpData();
    }

    [Conditional("LOG")]
    private void DumpData()
    {
        if (!Log.EnableDebug) return;

        Debug();

        // Log.Debug($"Map:\n{_map.MapValueToString()}");
        Log.Debug($"Carts:\n{_carts.ToJson()}");
    }
    #endregion
}

#if DUMP
#endif