namespace Gguc.Aoc.Core.Logging;

using NLog;

/// <summary>
/// A logger that logs to a nlog trace.
/// </summary>
public class TraceLog : ILog
{
    private static readonly Logger Log = LogManager.GetLogger("Tdx.Phoenix.Wpf");

    /// <summary>
    /// Initializes a new instance of the <see cref="TraceLog"/> class.
    /// </summary>
    public TraceLog()
    {
        System.Diagnostics.Trace.Listeners.Add(new NLogTraceListener());
        var entry = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
        var classId = entry.GetName().Name;
        this.Info("".PadLeft(120, '_'));
        this.InfoLog(classId, $"Version: [{entry.GetName().Version}]");
        this.InfoLog(classId, $"Location: [{entry.Location}]");
#if DEBUG
        this.InfoLog(classId, $"DEBUG BUILD!");
#endif
        Instance = this;
    }

    public static ILog Instance { get; private set; }

    /// <inheritdoc />
    public bool EnableDebug { get; set; }

    /// <inheritdoc />
    public void Trace(string message)
    {
        if (!EnableDebug) return;

        Log.Trace(message);
    }

    /// <inheritdoc />
    public void Debug(string message)
    {
        if (!EnableDebug) return;

        Log.Debug(message);
    }

    /// <inheritdoc />
    public void Info(string message)
    {
        Log.Info(message);
    }

    /// <inheritdoc />
    public void Warn(string message, Exception ex = null)
    {
        message = ex == null ? $"{message}!" : $"{message} - Exception={ex.Message}!\n{ex}";
        Log.Warn(message);
    }

    /// <inheritdoc />
    public void Error(string message, Exception ex = null)
    {
        message = ex == null ? $"{message}!" : $"{message} - Exception={ex.Message}!\n{ex}";
        Log.Error(message);
    }

    /// <inheritdoc />
    public void Fatal(string message, Exception ex = null)
    {
        message = ex == null ? $"{message}!" : $"{message} - Exception={ex.Message}!\n{ex}";
        Log.Fatal(message);
    }
}
