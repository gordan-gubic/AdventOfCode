namespace Gguc.Aoc.Core.Services;

public interface IDay
{
    string ClassId { get; }

    int Year { get; }

    int Id { get; }

    string Expected1 { get; set; }

    string Expected2 { get; set; }

    void DumpInput();

    long SolutionPart1();

    long SolutionPart2();
}
