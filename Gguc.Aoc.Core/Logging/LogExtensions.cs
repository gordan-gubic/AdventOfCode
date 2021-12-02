namespace Gguc.Aoc.Core.Logging;

/// <summary>
/// Extensions to the <see cref="ILog"/> implementation
/// </summary>
public static class LogExtensions
{
    /// <summary>
    /// Logs a message with the verbose trace level.
    /// </summary>
    /// <param name="log">The logger.</param>
    /// <param name="type">Class name of the caller.</param>
    /// <param name="message">The message to log.</param>
    /// <param name="method">Method name of the caller.</param>
    public static void TraceLog(
        this ILog log,
        string type,
        string message = "",
        [CallerMemberName] string method = "")
    {
        if (log == null || !log.EnableDebug)
        {
            return;
        }

        message = string.IsNullOrWhiteSpace(message) ? $"{type}.{method}()" : $"{type}.{method}() - {message}";
        log.Trace(message);
    }

    /// <summary>
    /// Logs a message with the debug level.
    /// </summary>
    /// <param name="log">The logger.</param>
    /// <param name="type">Class name of the caller.</param>
    /// <param name="message">The message to log.</param>
    /// <param name="method">Method name of the caller.</param>
    public static void DebugLog(
        this ILog log,
        string type,
        string message = "",
        [CallerMemberName] string method = "")
    {
        if (log == null || !log.EnableDebug)
        {
            return;
        }

        message = string.IsNullOrWhiteSpace(message) ? $"{type}.{method}()" : $"{type}.{method}() - {message}";
        log.Debug(message);
    }

    /// <summary>
    /// Logs a message with the info level.
    /// </summary>
    /// <param name="log">The logger.</param>
    /// <param name="type">Class name of the caller.</param>
    /// <param name="message">The message to log.</param>
    /// <param name="method">Method name of the caller.</param>
    public static void InfoLog(
        this ILog log,
        string type,
        string message = "",
        [CallerMemberName] string method = "")
    {
        if (log == null)
        {
            return;
        }

        message = string.IsNullOrWhiteSpace(message) ? $"{type}.{method}()" : $"{type}.{method}() - {message}";
        log.Info(message);
    }

    /// <summary>
    /// Logs a message with the warning level.
    /// </summary>
    /// <param name="log">The logger.</param>
    /// <param name="type">Class name of the caller.</param>
    /// <param name="message">The message to log.</param>
    /// <param name="ex">The exception to log.</param>
    /// <param name="method">Method name of the caller.</param>
    public static void WarnLog(
        this ILog log,
        string type,
        string message = "",
        Exception ex = null,
        [CallerMemberName] string method = "")
    {
        if (log == null)
        {
            return;
        }

        message = string.IsNullOrWhiteSpace(message) ? $"{type}.{method}()" : $"{type}.{method}() - {message}";
        log.Warn(message, ex);
    }

    /// <summary>
    /// Logs a message with the error level.
    /// </summary>
    /// <param name="log">The logger.</param>
    /// <param name="type">Class name of the caller.</param>
    /// <param name="message">The message to log.</param>
    /// <param name="ex">The exception to log.</param>
    /// <param name="method">Method name of the caller.</param>
    public static void ErrorLog(
        this ILog log,
        string type,
        string message = "",
        Exception ex = null,
        [CallerMemberName] string method = "")
    {
        if (log == null)
        {
            return;
        }

        message = string.IsNullOrWhiteSpace(message) ? $"{type}.{method}()" : $"{type}.{method}() - {message}";
        log.Error(message, ex);
    }

    /// <summary>
    /// Logs a message with the fatal/critical level.
    /// </summary>
    /// <param name="log">The logger.</param>
    /// <param name="type">Class name of the caller.</param>
    /// <param name="message">The message to log.</param>
    /// <param name="ex">The exception to log.</param>
    /// <param name="method">Method name of the caller.</param>
    public static void FatalLog(
        this ILog log,
        string type,
        string message = "",
        Exception ex = null,
        [CallerMemberName] string method = "")
    {
        if (log == null)
        {
            return;
        }

        message = string.IsNullOrWhiteSpace(message) ? $"{type}.{method}()" : $"{type}.{method}() - {message}";
        log.Fatal(message, ex);
    }
}
