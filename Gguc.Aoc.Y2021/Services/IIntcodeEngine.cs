namespace Gguc.Aoc.Y2021.Services;

public interface IIntcodeEngine
{
    long Output { get; }

    void Reset();

    long Run();

    void SetInput(params long[] values);

    void AddInput(long value);

    long ReadMemory(int index);

    void WriteMemory(int index, long value);
}
