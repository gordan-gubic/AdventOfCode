#define LOG

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
        // Days
        var dayTypes = typeof(IDay).GetAllTypesInAssembly(Assembly.GetExecutingAssembly());

        foreach (var dayType in dayTypes)
        {
            var constants = dayType.GetAllConstantValues<int>();
            var key = $"{constants["YEAR"]:D4}{constants["DAY"]:D2}".ToInt();

            builder.RegisterType(dayType).Keyed<IDay>(key);
        }

        // Misc
        // ...
    }
}
