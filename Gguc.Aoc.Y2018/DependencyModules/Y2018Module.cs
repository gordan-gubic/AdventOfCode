namespace Gguc.Aoc.Y2018.DependencyModules;

using Module = Autofac.Module;

/// <summary>
/// MainModule for autofac container
/// </summary>
[SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Main module is responsible for registration of all application instances.")]
public class Y2018Module : Module
{
    /// <inheritdoc />
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<Day01>().Named<IDay>(Registar.Y2018D01);
        builder.RegisterType<Day01>().Keyed<IDay>(201801);
        //builder.RegisterType<Day02>().Named<IDay>(Registar.Y2021D02);

        // Misc
        // ...
    }
}
