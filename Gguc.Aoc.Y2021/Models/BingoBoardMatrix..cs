namespace Gguc.Aoc.Y2021.Models;

public class BingoBoardMatrix
{
    private List<string> _data;
    private Map<int> _map;
    private Map<int> _temp;
    private bool[,] _controlRows;
    private bool[,] _controlCols;

    public BingoBoardMatrix(List<string> data)
    {
        _data = data;

        ProcessData();

        GetHashCode().Dump();
    }

    public int LastNumber { get; private set; }

    public int Sum => CalculateSum();

    private void ProcessData()
    {
        var map = new Map<int>(5, 5);

        for (int i = 0; i < map.Height; i++)
        {
            var temp = _data[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);

            for (int j = 0; j < map.Width; j++)
            {
                map.Values[j, i] = temp[j].ToInt();
            }
        }

        // map.ToString().Dump();
        // map.Dump("Board");
        _map = map;
        _temp = map.Clone();

        _controlRows = new bool[5, 5];
        _controlCols = new bool[5, 5];
    }

    public bool ExecuteNumber(int number)
    {
        LastNumber = number;

        Check();

        return Validate();
    }

    private void Check()
    {
        for (int y = 0; y < _temp.Height; y++)
        {
            for (int x = 0; x < _temp.Width; x++)
            {
                if (_temp.GetValue(x, y) == LastNumber)
                {
                    _temp.Values[x, y] = -1;

                    _controlRows[x, y] = true;

                    _controlCols[y, x] = true;
                }
            }
        }
    }

    private bool Validate()
    {
        var l1 = new List<bool>();
        var l2 = new List<bool>();

        for (int y = 0; y < _temp.Height; y++)
        {
            l1.Clear();
            l2.Clear();

            for (int x = 0; x < _temp.Width; x++)
            {
                l1.Add(_controlRows[x, y]);
                l2.Add(_controlCols[x, y]);
            }

            if (l1.All(x => x) || l2.All(x => x))
            {
                return true;
            }
        }

        return false;
    }

    private int CalculateSum()
    {
        var sum = 0;

        for (int y = 0; y < _temp.Height; y++)
        {
            for (int x = 0; x < _temp.Width; x++)
            {
                var v = _temp.GetValue(x, y);
                if (v != -1)
                {
                    sum += v;
                }
            }
        }

        return sum;
    }

    public override string ToString()
    {
        return _data.ToJson();
    }
}
