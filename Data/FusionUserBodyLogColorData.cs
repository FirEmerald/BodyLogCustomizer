using System;
using System.Collections.Generic;
using System.Text;
using BodyLogCustomizer.Data;
using BodyLogCustomizer.Utilities;
using Newtonsoft.Json;

namespace BodyLogCustomizer.Data;

public class FusionUserBodyLogColorData
{
    public string SteamId { get; set; }
    public string BodyLogColorDataBase64 { get; set; }

    public FusionUserBodyLogColorData(string steamId, BodyLogColorData bodyLogColorData)
    {
        SteamId = steamId;
        BodyLogColorDataBase64 = ConvertToBase64(bodyLogColorData);
    }

    private string ConvertToBase64(BodyLogColorData bodyLogColorData)
    {
        var json = JsonConvert.SerializeObject(bodyLogColorData, Formatting.Indented, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Converters = new List<JsonConverter> { new ColorJsonConverter() }
        });
        var bytes = Encoding.UTF8.GetBytes(json);
        return Convert.ToBase64String(bytes);
    }

    public BodyLogColorData DecodeBodyLogColorData()
    {
        var bytes = Convert.FromBase64String(BodyLogColorDataBase64);
        var json = Encoding.UTF8.GetString(bytes);
        return JsonConvert.DeserializeObject<BodyLogColorData>(json);
    }
}