namespace Gguc.Aoc.Y2021.Models;

public record struct DiracGame
{
    public int Player1Position { get; set; }

    public int Player2Position { get; set; }

    public int Player1Score { get; set; }

    public int Player2Score { get; set; }

    public int Dice { get; internal set; }

    public int DiceCount { get; internal set; }

    public bool IsPlayer1Turn { get; internal set; }
}
