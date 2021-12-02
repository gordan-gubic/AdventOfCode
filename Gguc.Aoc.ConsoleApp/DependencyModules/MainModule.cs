namespace Gguc.Aoc.ConsoleApp.DependencyModules;

using Module = Autofac.Module;

/// <summary>
/// MainModule for autofac container
/// </summary>
[SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Main module is responsible for registration of all application instances.")]
public class MainModule : Module
{
    private readonly ILog _log;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainModule"/> class.
    /// </summary>
    /// <param name="log">The logger.</param>
    public MainModule(ILog log)
    {
        _log = log;
    }

    /// <inheritdoc />
    protected override void Load(ContainerBuilder builder)
    {
        // Logging and classes shared between modules
        if (_log == null)
        {
            builder.RegisterType<TraceLog>().As<ILog>().SingleInstance();
        }
        else
        {
            builder.RegisterInstance(_log).As<ILog>().AsSelf().SingleInstance().ExternallyOwned();
        }

        // Modules
        builder.RegisterModule<HubModule>();

        // Misc
        // ...
    }
}
