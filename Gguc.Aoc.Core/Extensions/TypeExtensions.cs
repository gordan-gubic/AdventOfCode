namespace Gguc.Aoc.Core.Extensions;

public static class TypeExtensions
{
    public static Dictionary<string, T> GetAllConstantValues<T>(this Type type)
    {
        var dict = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(T))
            .Select(x => (x.Name, (T)x.GetRawConstantValue()))
            .ToDictionary(x => x.Name, x => x.Item2);

        return dict;
    }

    public static List<Type> GetAllTypesInAssembly(this Type targettype, Assembly assembly)
    {
        var types = assembly.GetTypes()
             .Where(type => type.IsClass && targettype.IsAssignableFrom(type))
             .ToList();

        return types;
    }
}
