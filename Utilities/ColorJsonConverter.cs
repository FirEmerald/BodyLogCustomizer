using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace BodyLogCustomizer.Utilities;

public class ColorJsonConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        Color color = (Color)value;
        writer.WriteStartObject();
        writer.WritePropertyName("r");
        writer.WriteValue(color.r);
        writer.WritePropertyName("g");
        writer.WriteValue(color.g);
        writer.WritePropertyName("b");
        writer.WriteValue(color.b);
        writer.WritePropertyName("a");
        writer.WriteValue(color.a);
        writer.WriteEndObject();
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        JObject item = JObject.Load(reader);
        return new Color((float)item["r"], (float)item["g"], (float)item["b"], (float)item["a"]);
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(Color);
    }
}