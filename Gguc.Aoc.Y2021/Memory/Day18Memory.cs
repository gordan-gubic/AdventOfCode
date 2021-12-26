namespace Gguc.Aoc.Y2021.Memory;

public class Day18Memory
{
    public Day18Memory()
    {
        Snails = new List<Snail>();

        Tree = new List<object>();
    }

    public List<Snail> Snails { get; set; }

    public List<object> Tree { get; set; }

    public Snail Grandparent { get; set; }

    public void Clear()
    {
        Snails.Clear();

        Tree.Clear();

        Grandparent = null;
    }

    public void Recreate()
    {
        Snails.Clear();
        Tree.Clear();

        if (Grandparent == null) return;

        SetLevel(Grandparent, 1);
        AddToList(Snails, Grandparent);
        AddToTree(Tree, Grandparent);
    }

    private void AddToList(List<Snail> snails, Snail snail)
    {
        if (snail.X is Snail s1) AddToList(snails, s1);

        snails.Add(snail);

        if (snail.Y is Snail s2) AddToList(snails, s2);
    }

    private void AddToTree(List<object> tree, object value)
    {
        if (value is Snail snail1)
        {
            AddToTree(tree, (object)snail1.X);
        }

        tree.Add((object)value);

        if (value is Snail snail2)
        {
            AddToTree(tree, (object)snail2.Y);
        }
    }

    private void SetLevel(Snail snail, int level)
    {
        snail.Level = level;

        if (snail.X is Snail s1) SetLevel(s1, level + 1);
        if (snail.Y is Snail s2) SetLevel(s2, level + 1);
    }
}

#if DROP

/*
PREORDER
private void AddToList(List<Snail> snails, Snail snail)
{
    snails.Add(snail);

    if (snail.X is Snail s1) AddToList(snails, s1);
    if (snail.Y is Snail s2) AddToList(snails, s2);
}

private void AddToTree(List<object> tree, object value)
{
    tree.Add((object)value);

    if(value is Snail snail)
    {
        AddToTree(tree, (object)snail.X);
        AddToTree(tree, (object)snail.Y);
    }
}
*/

#endif