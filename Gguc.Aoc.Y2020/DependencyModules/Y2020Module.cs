namespace Gguc.Aoc.Y2020.DependencyModules
{
    using System.Diagnostics.CodeAnalysis;
    using Autofac;
    using Gguc.Aoc.Core.Services;
    using Gguc.Aoc.Core.Utils;
    using Gguc.Aoc.Y2020.Days;
    using Module = Autofac.Module;

    /// <summary>
    /// MainModule for autofac container
    /// </summary>
    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Main module is responsible for registration of all application instances.")]
    public class Y2020Module : Module
    {
        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Day01>().Named<IDay>(Registar.Y2020D01);
            builder.RegisterType<Day02>().Named<IDay>(Registar.Y2020D02);
            builder.RegisterType<Day03>().Named<IDay>(Registar.Y2020D03);
            builder.RegisterType<Day04>().Named<IDay>(Registar.Y2020D04);
            builder.RegisterType<Day05>().Named<IDay>(Registar.Y2020D05);
            builder.RegisterType<Day06>().Named<IDay>(Registar.Y2020D06);
            builder.RegisterType<Day07>().Named<IDay>(Registar.Y2020D07);
            builder.RegisterType<Day08>().Named<IDay>(Registar.Y2020D08);
            builder.RegisterType<Day09>().Named<IDay>(Registar.Y2020D09);
            builder.RegisterType<Day10>().Named<IDay>(Registar.Y2020D10);
            builder.RegisterType<Day11>().Named<IDay>(Registar.Y2020D11);
            builder.RegisterType<Day12>().Named<IDay>(Registar.Y2020D12);
            builder.RegisterType<Day13>().Named<IDay>(Registar.Y2020D13);
            builder.RegisterType<Day14>().Named<IDay>(Registar.Y2020D14);
            builder.RegisterType<Day15>().Named<IDay>(Registar.Y2020D15);
            builder.RegisterType<Day16>().Named<IDay>(Registar.Y2020D16);
            builder.RegisterType<Day17>().Named<IDay>(Registar.Y2020D17);
            builder.RegisterType<Day18>().Named<IDay>(Registar.Y2020D18);
            builder.RegisterType<Day19>().Named<IDay>(Registar.Y2020D19);
            builder.RegisterType<Day20>().Named<IDay>(Registar.Y2020D20);
            builder.RegisterType<Day21>().Named<IDay>(Registar.Y2020D21);
            builder.RegisterType<Day22>().Named<IDay>(Registar.Y2020D22);
            builder.RegisterType<Day23>().Named<IDay>(Registar.Y2020D23);
            builder.RegisterType<Day24>().Named<IDay>(Registar.Y2020D24);
            builder.RegisterType<Day25>().Named<IDay>(Registar.Y2020D25);

            // Misc
            // ...
        }
    }
}