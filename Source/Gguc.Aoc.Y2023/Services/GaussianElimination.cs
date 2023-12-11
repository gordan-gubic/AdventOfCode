#define LOG

namespace Gguc.Aoc.Y2023.Services;

/*
 * Gaussian Elimination
 *
 * [2023 Day 24 (part 2)] a straightforward non-solver solution
 * https://www.reddit.com/r/adventofcode/comments/18q40he/2023_day_24_part_2_a_straightforward_nonsolver/
 *
 * (dy'-dy) X + (dx-dx') Y + (y-y') DX + (x'-x) DY = x' dy' - y' dx' - x dy + y dx
 *
 * Gaussian Elimination With 4 Variables Using Elementary Row Operations With Matrices
 * https://www.youtube.com/watch?v=4VAXv6yRULU&ab_channel=TheOrganicChemistryTutor
 *
 * https://en.wikipedia.org/wiki/Gaussian_elimination
 *
 */

internal class GaussianElimination
{
    private List<List<double>> _rows;
    private List<double> _values = new();

    public void SetMatrix(List<List<long>> rows)
    {
        _rows = new();

        foreach (var row in rows)
        {
            _rows.Add(row.Select(Convert.ToDouble).ToList());
        }
    }

    public List<double> Result => _values;

    public List<long> ResultConverted => ConvertToLong(_values);

    public void Calculate()
    {
        Validate();
        ReduceRows();

        _rows.DumpJson(" -- ReduceRows -- ");

        CollapseRows();

        _values.DumpJson(" -- CollapseRows -- ");
    }

    private void Validate()
    {
        for (var i = 0; i < _rows.Count; i++)
        {
            var value = _rows[i][i];

            if (value == 0L)
            {
                $"Row/Column [{i}] must be non-zero!".Dump();
            }
        }
    }

    private void ReduceRows()
    {
        for (var i = 0; i < _rows.Count - 1; i++)
        {
            for (var j = i + 1; j < _rows.Count; j++)
            {
                var r1 = _rows[i];
                var r2 = _rows[j];

                // " ---- ---- ".Dump();
                // $"{r1[0]}\t{r1[1]}\t{r1[2]}\t{r1[3]}\t{r1[4]}".Dump();
                // $"{r2[0]}\t{r2[1]}\t{r2[2]}\t{r2[3]}\t{r2[4]}".Dump();
                
                ReduceRow(r1, r2, i);
                // $"{r2[0]}\t{r2[1]}\t{r2[2]}\t{r2[3]}\t{r2[4]}".Dump();
            }
        }
    }

    private void ReduceRow(List<double> row1, List<double> row2, int index)
    {
        var coef1 = row1[index];
        var coef2 = row2[index];

        for (var i = 0; i < row1.Count; i++)
        {
            var x = row1[i] * coef2;
            row2[i] = x - row2[i] * coef1;
        }
    }

    private void CollapseRows()
    {
        // (w, x, y, z) or (x, y, dx, dy)

        // -- c4 | z | dy -- 
        var c4 = (double)_rows[3][4] / _rows[3][3];

        // -- c3 | y | dx --
        var c3 = (_rows[2][4] - _rows[2][3] * c4) / _rows[2][2];

        // -- c2 | x | y --
        var c2 = (_rows[1][4] - _rows[1][3] * c4 - _rows[1][2] * c3) / _rows[1][1];

        // -- c1 | w | x --
        var c1 = (_rows[0][4] - _rows[0][3] * c4 - _rows[0][2] * c3 - _rows[0][1] * c2) / _rows[0][0];

        _values.Clear();
        _values.Add(c1);
        _values.Add(c2);
        _values.Add(c3);
        _values.Add(c4);
    }
    private List<long> ConvertToLong(List<double> values)
    {
        // var result = new List<long>(); _values.ForEach(x => result.Add(Convert.ToInt64(x)));

        return _values.Select(Convert.ToInt64).ToList();
    }
}