namespace Gguc.Aoc.ConsoleApp;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine(" ***** ");

        try
        {
            new App().Run();
        }
        catch (Exception ex)
        {
            Trace.TraceError($"Unhandled exception in App! Exception: {ex.Message}\n{ex}");
        }
    }
}
