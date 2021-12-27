namespace Gguc.Aoc.Y2021.Memory;

public class Day21Memory
{
    public long Player1Wins { get; set; }

    public long Player2Wins { get; set; }

    public long DiceCount { get; internal set; }

    public Stack<DiracGame> Games { get; internal set; }
}
