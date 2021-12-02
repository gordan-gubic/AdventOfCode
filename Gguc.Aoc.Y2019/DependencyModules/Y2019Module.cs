namespace Gguc.Aoc.Y2019.DependencyModules;

using Module = Autofac.Module;

/// <summary>
/// MainModule for autofac container
/// </summary>
[SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Main module is responsible for registration of all application instances.")]
public class Y2019Module : Module
{
    /// <inheritdoc />
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<Day01>().Named<IDay>(Registar.Y2019D01);
        builder.RegisterType<Day02>().Named<IDay>(Registar.Y2019D02);
        builder.RegisterType<Day03>().Named<IDay>(Registar.Y2019D03);
        builder.RegisterType<Day04>().Named<IDay>(Registar.Y2019D04);
        builder.RegisterType<Day05>().Named<IDay>(Registar.Y2019D05);
        builder.RegisterType<Day06>().Named<IDay>(Registar.Y2019D06);
        builder.RegisterType<Day07>().Named<IDay>(Registar.Y2019D07);
        builder.RegisterType<Day08>().Named<IDay>(Registar.Y2019D08);
        builder.RegisterType<Day09>().Named<IDay>(Registar.Y2019D09);
        builder.RegisterType<Day10>().Named<IDay>(Registar.Y2019D10);
        builder.RegisterType<Day11>().Named<IDay>(Registar.Y2019D11);
        builder.RegisterType<Day12>().Named<IDay>(Registar.Y2019D12);

        // Misc
        // ...
    }
}
