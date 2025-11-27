namespace Gguc.Aoc.Core.Extensions;

using System;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

/// <summary>
/// Extension methods for JSON.
/// </summary>
public static class JsonExtensions
{
    private static readonly JsonSerializerOptions DefaultJsonSerializerSettings = new()
    {
        Converters = { new JsonStringEnumConverter() },
        PropertyNameCaseInsensitive = true
    };

    private static readonly JsonSerializerOptions IndentedJsonSerializerSettings = new()
    {
        Converters = { new JsonStringEnumConverter() },
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
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
            return JsonSerializer.Deserialize<T>(json, DefaultJsonSerializerSettings);
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
            return JsonSerializer.Serialize(value, DefaultJsonSerializerSettings);
        }
        catch (Exception ex)
        {
            Trace.TraceWarning($"Error occurred during serialization to json. Value=[{value}]. Exception=[{ex.Message}]!");
            return default;
        }
    }

    /// <summary>
    /// Serializes the specified object to an indented JSON string.
    /// </summary>
    /// <param name="value">The object to serialize.</param>
    /// <returns>An indented JSON string representation of the object.</returns>
    public static string ToJsonIndented(this object value)
    {
        try
        {
            return JsonSerializer.Serialize(value, IndentedJsonSerializerSettings);
        }
        catch (Exception ex)
        {
            Trace.TraceWarning($"Error occurred during serialization to json. Value=[{value}]. Exception=[{ex.Message}]!");
            return default;
        }
    }

    /// <summary>
    /// Gets the value type from a JSON value.
    /// </summary>
    /// <param name="jsonValue">The JSON value.</param>
    /// <returns>The value type.</returns>
    public static Type GetValueType(this JsonNode jsonValue)
    {
        if (jsonValue == null)
        {
            return null;
        }

        var value = jsonValue.GetValue<object>();

        if (value is JsonElement element)
        {
            return element.GetValueType();
        }

        return value.GetType();
    }

    /// <summary>
    /// Gets the value type from a JSON value.
    /// </summary>
    /// <param name="jsonValue">The JSON value.</param>
    /// <returns>The value type.</returns>
    public static Type GetValueType(this JsonElement jsonValue)
    {
        return jsonValue.ValueKind switch
        {
            JsonValueKind.False => typeof(bool),
            JsonValueKind.True => typeof(bool),
            JsonValueKind.Number => typeof(double),
            JsonValueKind.String => typeof(string),
            JsonValueKind.Object => typeof(JsonObject),
            JsonValueKind.Array => typeof(JsonArray),
            JsonValueKind.Undefined => null,
            JsonValueKind.Null => null,
            _ => typeof(JsonElement)
        };
    }
}