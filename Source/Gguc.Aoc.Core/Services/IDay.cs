namespace Gguc.Aoc.Core.Services;

public interface IDay
{
    string ClassId { get; }

    int Year { get; }

    int Id { get; }

    bool TestExample { get; set; }

    bool ExecuteTest { get; set; }

    bool ExecuteProd { get; set; }

    string ExpectedTest1 { get; set; }

    string ExpectedTest2 { get; set; }

    string ExpectedProd1 { get; set; }

    string ExpectedProd2 { get; set; }

    void DumpInput();

    void InitializeTest();

    void InitializeProd();

    long SolutionPart1();

    long SolutionPart2();
}
