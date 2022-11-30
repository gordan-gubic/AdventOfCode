namespace Gguc.Aoc.Hub.DependencyModules;

using System.Diagnostics.CodeAnalysis;
using Autofac;
using Gguc.Aoc.Core.DependencyModules;
using Gguc.Aoc.Y2018.DependencyModules;
using Gguc.Aoc.Y2019.DependencyModules;
using Gguc.Aoc.Y2020.DependencyModules;
using Gguc.Aoc.Y2021.DependencyModules;
using Gguc.Aoc.Y2022.DependencyModules;
using Module = Autofac.Module;

/// <summary>
/// MainModule for autofac container
/// </summary>
[SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Main module is responsible for registration of all application instances.")]
public class HubModule : Module
{
    /// <inheritdoc />
    protected override void Load(ContainerBuilder builder)
    {
        // Modules
        builder.RegisterModule<CoreModule>();
        builder.RegisterModule<Y2018Module>();
        builder.RegisterModule<Y2019Module>();
        // builder.RegisterModule<Y2020Module>();
        builder.RegisterModule<Y2021Module>();
        builder.RegisterModule<Y2022Module>();
    }
}
