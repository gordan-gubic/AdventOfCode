namespace Gguc.Aoc.Y2024.Models;

/* Class containing left and
right child of current
node and key value*/
public class Node
{
    public long Value;

    public int Level;

    public Node Parent;
    public Node Left;
    public Node Right;

    public Node(Node parent, long value, int level)
    {
        Parent = parent;
        Value = value;
        Level = level;
        Left = Right = null;
    }
}

public class BinaryTree
{
    // Root of Binary Tree
    Node Root;

    public BinaryTree(Node root)
    {
        Root = root;
    }

    /*
    // Given a binary tree, print 	its nodes according to the	"bottom-up" postorder traversal.
    void printPostorder(Node node)
    {
        if (node == null)
            return;

        // first recur on left subtree
        printPostorder(node.left);

        // then recur on right subtree
        printPostorder(node.right);

        // now deal with the node
        Console.Write(node.key + " ");
    }

    // Given a binary tree, print	its nodes in inorder
    void printInorder(Node node)
    {
        if (node == null)
            return;

        // first recur on left child
        printInorder(node.left);

        // then print the data of node
        Console.Write(node.key + " ");

        // now recur on right child
        printInorder(node.right);
    }

    // Given a binary tree, print	its nodes in preorder
    void printPreorder(Node node)
    {
        if (node == null)
            return;

        // first print data of node
        Console.Write(node.key + " ");

        // then recur on left subtree
        printPreorder(node.left);

        // now recur on right subtree
        printPreorder(node.right);
    }

    // Wrappers over above recursive functions
    void printPostorder() { printPostorder(root); }
    void printInorder() { printInorder(root); }
    void printPreorder() { printPreorder(root); }
    */
}