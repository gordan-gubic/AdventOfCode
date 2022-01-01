namespace Gguc.Aoc.Y2021.Models;

public class Alu
{
    public long Init { get; set; }

    public Queue<int> Input { get; set; }

    public List<int> History { get; set; } = new List<int>();

    public long W { get; set; }

    public long X { get; set; }
    
    public long Y { get; set; }
    
    public long Z { get; set; }


    public void ProcessInput()
    {
        var data = $"{Init}".Select(x => x.ToInt());
        Input = new Queue<int>(data);
    }

    public long GetValue(string arg)
    {
        return arg switch
        {
            "w" => W,
            "x" => X,
            "y" => Y,
            "z" => Z,
            _ => arg.ToLong(),
        };
    }

    public void SetValue(string arg, long value)
    {
        switch (arg)
        {
            case "w": W = value; break;
            case "x": X = value; break;
            case "y": Y = value; break;
            case "z": Z = value; break;
            default: break;
        };
    }

    public Alu Copy()
    {
        return new Alu
        {
            History = History.ToList(),
            W = W,
            X = X,
            Y = Y,
            Z = Z,
        };
    }
}

