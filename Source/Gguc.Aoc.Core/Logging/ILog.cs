namespace Gguc.Aoc.Core.Logging;

/// <summary>
/// Interface for log targets.
/// </summary>
public interface ILog
{
    /// <summary>
    /// Enable logging on debug or lower trace level.
    /// </summary>
    bool EnableDebug { get; set; }

    /// <summary>
    /// Logs a message with the verbose trace level.
    /// </summary>
    /// <param name="message">The message to log.</param>
    void Trace(string message);

    /// <summary>
    /// Logs a message with the debug level.
    /// </summary>
    /// <param name="message">The message to log.</param>
    void Debug(string message);

    /// <summary>
    /// Logs a message with the info level.
    /// </summary>
    /// <param name="message">The message to log.</param>
    void Info(string message);

    /// <summary>
    /// Logs a message with the warning level.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="ex">The associated exception.</param>
    void Warn(string message, Exception ex = null);

    /// <summary>
    /// Logs a message with the error level.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="ex">The associated exception.</param>
    [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", Justification = "Error is the name of the log level.")]
    void Error(string message, Exception ex = null);

    /// <summary>
    /// Logs a message with the fatal level.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="ex">The associated exception.</param>
    void Fatal(string message, Exception ex = null);
}
