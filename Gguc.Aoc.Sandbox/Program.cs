namespace Gguc.Aoc.Sandbox;

using System;
using Gguc.Aoc.Core.Logging;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello World!");

        var log = new TraceLog();
        log.Debug("Hello!");

        BinaryTree.Run();
    }
}
