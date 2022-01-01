namespace Gguc.Aoc.Core.DependencyModules;

using Module = Autofac.Module;

/// <summary>
/// MainModule for autofac container
/// </summary>
[SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Main module is responsible for registration of all application instances.")]
public class CoreModule : Module
{
    /// <inheritdoc />
    protected override void Load(ContainerBuilder builder)
    {
        // Models
        builder.RegisterType<FileReader>().As<IFileReader>();
        builder.RegisterType<Parser>().As<IParser>();
    }
}
