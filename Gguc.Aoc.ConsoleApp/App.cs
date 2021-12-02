namespace Gguc.Aoc.ConsoleApp;

public class App
{
    private static readonly string ClassId = nameof(App);
    private static TraceLog _log;

    private static readonly int DayKey = 202103;

    public App()
    {
        try
        {
            _log = new TraceLog();

            Initialize();
        }
        catch (Exception ex)
        {
            Trace.TraceError($"Unhandled exception in App! Exception: {ex.Message}\n{ex}");
            throw;
        }
    }

    /// <summary>
    /// Gets the main container.
    /// </summary>
    public IContainer MainContainer { get; private set; }

    public void Run()
    {
        _log.InfoLog(ClassId, "Begin");

        var day = MainContainer.ResolveKeyed<IDay>(DayKey);

        Header(day);

        DumpInput(day);

        ExecutePart1(day);

        ExecutePart2(day);
    }

    private void Initialize()
    {
        InitializeMainContainer();
    }

    private void InitializeMainContainer()
    {
        var builder = new ContainerBuilder();
        builder.RegisterModule(new MainModule(_log));

        MainContainer = builder.Build();
    }

    private void Header(IDay day)
    {
        var bar = "".PadLeft(80, '*');
        var message = $"\n{bar}\n* Year..: {day.Year}\n* Day...: {day.Id}\n{bar}";

        _log.InfoLog(ClassId, message);
    }

    private void DumpInput(IDay day)
    {
        day.DumpInput();
    }

    private void ExecutePart1(IDay day)
    {
        var result = day.SolutionPart1();

        _log.WarnLog(ClassId, $" *** Day [{DayKey}] - Part 01 *** Result: [{result}]");
        SetClipboard(result);
    }

    private void ExecutePart2(IDay day)
    {
        var result = day.SolutionPart2();

        _log.WarnLog(ClassId, $" *** Day [{DayKey}] - Part 02 *** Result: [{result}]");
        SetClipboard(result);
    }

    private void SetClipboard(in long result)
    {
        if (result == 0) return;

        WindowsClipboard.SetText($"{result}");
    }
}
