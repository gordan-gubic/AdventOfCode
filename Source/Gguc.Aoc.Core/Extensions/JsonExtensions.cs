namespace Gguc.Aoc.Core.Extensions;

/// <summary>
/// Extension methods for JSON
/// </summary>
public static class JsonExtensions
{
    private static readonly JsonSerializerSettings DefaultJsonSerializerSettings = new JsonSerializerSettings
    {
        Converters = new List<JsonConverter> { new StringEnumConverter() }
    };

    private static readonly JsonSerializerSettings IndentedJsonSerializerSettings = new JsonSerializerSettings
    {
        Converters = new List<JsonConverter> { new StringEnumConverter() },
        Formatting = Formatting.Indented,
    };

    /// <summary>
    /// Deserializes the JSON to the specified .NET type.
    /// </summary>
    /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
    /// <param name="json">The JSON to deserialize.</param>
    /// <returns>The deserialized object from the JSON string.</returns>
    public static T FromJson<T>(this string json)
    {
        try
        {
            return JsonConvert.DeserializeObject<T>(json, DefaultJsonSerializerSettings);
        }
        catch (Exception ex)
        {
            Trace.TraceWarning($"Error occurred during deserialization from json. JSON=[{json}]. Exception=[{ex.Message}]!");
            return default;
        }
    }

    /// <summary>
    /// Serializes the specified object to a JSON string.
    /// </summary>
    /// <param name="value">The object to serialize.</param>
    /// <returns>A JSON string representation of the object.</returns>
    public static string ToJson(this object value)
    {
        try
        {
            return JsonConvert.SerializeObject(value, DefaultJsonSerializerSettings);
        }
        catch (Exception ex)
        {
            Trace.TraceWarning($"Error occurred during serialization to json. Value=[{value}]. Exception=[{ex.Message}]!");
            return default;
        }
    }

    /// <summary>
    /// Serializes the specified object to a JSON string with indented formatting.
    /// </summary>
    /// <param name="value">The object to serialize.</param>
    /// <returns>A JSON string representation of the object.</returns>
    public static string ToJsonIndented(this object value)
    {
        try
        {
            return JsonConvert.SerializeObject(value, IndentedJsonSerializerSettings);
        }
        catch (Exception ex)
        {
            Trace.TraceWarning($"Error occurred during serialization to json. Value=[{value}]. Exception=[{ex.Message}]!");
            return default;
        }
    }
}
