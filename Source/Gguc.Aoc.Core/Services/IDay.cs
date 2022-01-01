namespace Gguc.Aoc.Core.Services;

public interface IDay
{
    string ClassId { get; }

    int Year { get; }

    int Id { get; }

    void DumpInput();

    long SolutionPart1();

    long SolutionPart2();
}
