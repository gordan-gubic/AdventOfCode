namespace Gguc.Aoc.ConsoleApp;

public class App
{
    private const string ClassId = nameof(App);

    private const int DayKey = 202508;

    private static TraceLog _log;

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

        ExecuteTest(day);

        ExecuteProd(day);
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

    private void ExecuteTest(IDay day)
    {
        if (!day.ExecuteTest) return;

        try
        {
            _log.Info("-------------------------------------------");
            _log.InfoLog(ClassId, "TEST");

            day.InitializeTest();
            DumpInput(day);

            ExecutePart1(day, day.ExpectedTest1);
            ExecutePart2(day, day.ExpectedTest2);
        }
        catch (Exception ex)
        {
            _log.WarnLog(ClassId, $"Test Failed. Error=[{ex.Message}]", ex);
        }
    }

    private void ExecuteProd(IDay day)
    {
        if (!day.ExecuteProd) return;

        try
        {
            _log.Info("-------------------------------------------");
            _log.InfoLog(ClassId, "PROD");

            day.InitializeProd();
            DumpInput(day);

            ExecutePart1(day, day.ExpectedProd1);
            ExecutePart2(day, day.ExpectedProd2);
        }
        catch (Exception ex)
        {
            _log.WarnLog(ClassId, $"Prod Failed. Error=[{ex.Message}]", ex);
        }
    }

    private void DumpInput(IDay day)
    {
        _log.Info("-------------------------------------------");
        day.DumpInput();
        _log.Info("-------------------------------------------");
    }

    private void ExecutePart1(IDay day, string expected = null)
    {
        _log.Info("");
        _log.InfoLog(ClassId, "Part 01");

        var stopwatch = Stopwatch.StartNew();
        var result = day.SolutionPart1();
        stopwatch.Stop();

        if (day.ExpectedProd1.IsNotWhitespace()) _log.InfoLog(ClassId, $" *** Day [{DayKey}] - Part 01 *** Expect: [{expected}]");

        _log.WarnLog(ClassId, $" *** Day [{DayKey}] - Part 01 *** Result: [{result}] *** Time: [{stopwatch.Elapsed}]");
        SetClipboard(result);
    }

    private void ExecutePart2(IDay day, string expected = null)
    {
        _log.Info("");
        _log.InfoLog(ClassId, "Part 02");

        var stopwatch = Stopwatch.StartNew();
        var result = day.SolutionPart2();
        stopwatch.Stop();

        if (day.ExpectedProd2.IsNotWhitespace()) _log.InfoLog(ClassId, $" *** Day [{DayKey}] - Part 02 *** Expect: [{expected}]");

        _log.WarnLog(ClassId, $" *** Day [{DayKey}] - Part 02 *** Result: [{result}] *** Time: [{stopwatch.Elapsed}]");
        SetClipboard(result);
    }

    private void SetClipboard(in long result)
    {
        if (result == 0) return;

        WindowsClipboard.SetText($"{result}");
    }
}
