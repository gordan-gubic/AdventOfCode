namespace Gguc.Aoc.Y2023.Models;

internal class CamelCards
{
    public string Hand { get; set; }

    public long Bet { get; set; }

    public Dictionary<string, int> Set { get; set; }

    public int Value { get; set; }

    public string High { get; set; }

    public string Raw { get; set; }


    public void CalculateHand(bool useJoker = false)
    {
        var dict = new Dictionary<string, int>();
        var list = new List<string>();

        Func<char, string> convert = useJoker ? ConvertKeyJoker : ConvertKey;

        foreach (var h in this.Hand)
        {
            var hh = convert(h);

            if (!dict.ContainsKey(hh)) dict[hh] = 0;
            dict[hh]++;

            list.Add(hh);
        }

        if (useJoker && dict.Count > 1 && dict.ContainsKey("01"))
        {
            var joker = dict["01"];
            dict.Remove("01");

            var key = dict.OrderByDescending(x => x.Value).ThenBy(x => x.Key).Select(x => x.Key).First();
            dict[key] += joker;
        }

        this.Set = dict;

        // evaluate set
        var ordered = dict.Values.OrderByDescending(x => x).Take(3);
        var value = string.Join("", ordered).PadRight(3, '0').ToInt();

        this.Value = value;

        /*
        var highest = dict.OrderByDescending(x => x.Value).ThenByDescending(x => x.Key).Select(x => x.Key);
        var high = string.Join("", highest);
        hand.High = high;
        */

        var raw = string.Join("", list);
        this.Raw = raw;
    }

    private string ConvertKey(char c)
    {
        // A, K, Q, J, T, 9, 8, 7, 6, 5, 4, 3, or 2

        return c switch
        {
            'A' => "14",
            'K' => "13",
            'Q' => "12",
            'J' => "11",
            'T' => "10",
            _ => $"{c}".PadLeft(2, '0'),
        };
    }

    private string ConvertKeyJoker(char c)
    {
        return c switch
        {
            'A' => "14",
            'K' => "13",
            'Q' => "12",
            'J' => "01",
            'T' => "10",
            _ => $"{c}".PadLeft(2, '0'),
        };
    }
}