namespace Gguc.Aoc.Y2018.Models;

internal class Worker
{
    public char Processing { get; set; }

    public int Time { get; set; }

    public void Work(char ch, int time)
    {
        var value = Day07.CharValue(ch);

        Processing = ch;
        Time = value + time;
    }
}