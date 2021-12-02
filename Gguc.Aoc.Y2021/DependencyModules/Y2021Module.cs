namespace Gguc.Aoc.Y2021.DependencyModules;

using Module = Autofac.Module;

/// <summary>
/// MainModule for autofac container
/// </summary>
[SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Main module is responsible for registration of all application instances.")]
public class Y2021Module : Module
{
    /// <inheritdoc />
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<Day01>().Named<IDay>(Registar.Y2021D01);
        builder.RegisterType<Day02>().Named<IDay>(Registar.Y2021D02);

        // Misc
        // ...
    }
}
