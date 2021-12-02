namespace Gguc.Aoc.Core.Utils;

public class StopwatchUtil
{
    private Stopwatch _stopwatch;
    private long _checkpoint = 0L;

    private StopwatchUtil()
    {
        _stopwatch = new Stopwatch();
    }

    private static readonly StopwatchUtil Instance = new StopwatchUtil();

    [Conditional("STOPWATCH")]
    public static void StartStopwatch()
    {
        Instance._stopwatch = new Stopwatch();
        Instance._stopwatch.Start();

        Trace.WriteLine($" *** Stopwatch started=[{DateTime.Now}]");
    }

    [Conditional("STOPWATCH")]
    public static void StopStopwatch()
    {
        var elapsed = Instance._stopwatch.ElapsedMilliseconds;
        var timestamp = DateTime.Now;

        Instance._stopwatch.Stop();

        Trace.WriteLine($" *** Stopwatch stopped=[{timestamp}]. Elapsed time=[{elapsed}]");
    }

    [Conditional("STOPWATCH")]
    public static void Checkpoint(string title = null)
    {
        var checkpoint = Instance._stopwatch.ElapsedMilliseconds;
        var difference = checkpoint - Instance._checkpoint;

        Instance._checkpoint = checkpoint;

        Trace.WriteLine($" *** Checkpoint=[{checkpoint,7}] *** Difference=[{difference,5}] ***  {title}");
    }
}
