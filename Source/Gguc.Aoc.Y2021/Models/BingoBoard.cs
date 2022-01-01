namespace Gguc.Aoc.Y2021.Models;

public class BingoBoard
{
    public const int Size = 5;

    private List<string> _data;
    private List<int> _hits;
    private Map<int> _board;
    private List<int> _controlRows;
    private List<int> _controlCols;
    private int _sum;
    private int _sumHits;

    public BingoBoard(List<string> data)
    {
        _data = data;

        ProcessData();

        GetHashCode().Dump();
    }

    public int LastNumber { get; private set; }

    public int Sum => CalculateSum();

    private void ProcessData()
    {
        _board = new Map<int>(Size);
        _hits = new List<int>();
        _controlRows = new List<int>();
        _controlCols = new List<int>();

        for (int i = 0; i < Size; i++)
        {
            var temp = _data[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            _controlRows.Add(0);
            _controlCols.Add(0);

            for (int j = 0; j < Size; j++)
            {
                var value = temp[j].ToInt();
                _board.Values[j, i] = value;
                _sum += value;
            }
        }

        // _board.Dump("Board");
    }

    public void Reset()
    {
        _hits.Clear();
        _controlRows.SetAll();
        _controlCols.SetAll();
        _sumHits = 0;
    }

    public bool ExecuteNumber(int number)
    {
        LastNumber = number;

        Check();

        return Validate();
    }

    private void Check()
    {
        for (int y = 0; y < Size; y++)
        {
            for (int x = 0; x < Size; x++)
            {
                if (_board.GetValue(x, y) == LastNumber)
                {
                    _hits.Add(LastNumber);
                    _sumHits += LastNumber;
                    _controlRows[y]++;
                    _controlCols[x]++;
                }
            }
        }
    }

    private bool Validate()
    {
        var fullCol = _controlCols.Any(x => x >= Size);
        var fullRow = _controlRows.Any(x => x >= Size);

        return fullCol || fullRow;
    }

    private int CalculateSum()
    {
        return _sum - _sumHits;
    }

    public override string ToString()
    {
        return _data.ToJson();
    }
}
