namespace Gguc.Aoc.Core.Services;

public interface IParser
{
    int Year { get; set; }

    int Day { get; set; }

    ParserFileType Type { get; set; }

    List<string> Parse();

    List<T> Parse<T>(Func<string, T> convert);

    List<List<string>> ParseBlock();

    List<List<T>> ParseBlock<T>(Func<string, T> convert);

    List<int> ParseSequence();

    List<T> ParseSequence<T>(Func<string, T> convert);

    bool[,] ParseMap(char mapChar = Parser.DefaultMapCharacter);

    Map<bool> ParseMapBool(char mapChar = Parser.DefaultMapCharacter);

    Map<int> ParseMapInt(Dictionary<char, int> mapper);

    void Log(int line = 0);
}