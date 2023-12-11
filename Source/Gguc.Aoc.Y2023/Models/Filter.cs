namespace Gguc.Aoc.Y2023.Models;

public record Filter
{
    public string A1 { get; set; }

    public string A2 { get; set; }

    public string B1 { get; set; }

    public string B2 { get; set; }

    public string C1 { get; set; }

    public string C2 { get; set; }
    
    public bool IsValid { get; set; }

    public void Clear()
    {
        A1 = A2 = B1 = B2 = C1 = C2 = null;
    }

    public void AddA(string item1, string item2)
    {
        A1 = item1;
        A2 = item2;
        B1 = B2 = C1 = C2 = null;
        IsValid = true;
    }

    public void AddB(string item1, string item2)
    {
        B1 = item1;
        B2 = item2;
        C1 = C2 = null;
        IsValid = B1 != A1 && B1 != A2 && B2 != A1 && B2 != A2;
    }

    public void AddC(string item1, string item2)
    {
        C1 = item1;
        C2 = item2;
        IsValid = C1 != A1 && C1 != A2 && C2 != A1 && C2 != A2 && C1 != B1 && C1 != B2 && C2 != B1 && C2 != B2;
    }
}