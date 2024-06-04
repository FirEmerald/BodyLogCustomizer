using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BodyLogCustomizer.Utilities;
using LabFusion.Data;
using LabFusion.Representation;
using MelonLoader;
using SLZ.Props;
using UnityEngine;

namespace BodyLogCustomizer.Data;

public static class FusionUserCacheManager
{
    private static FusionDictionary<string, FusionUserBodyLogColorData> cache;
    private static string cacheFilePath = "UserData/BodyLogCustomizer/FusionUserCache.json";

    static FusionUserCacheManager()
    {
        LoadCache();
    }

    public static void LoadCache()
    {
        if (File.Exists(cacheFilePath))
        {
            var json = File.ReadAllText(cacheFilePath);
            cache = JsonConvert.DeserializeObject<FusionDictionary<string, FusionUserBodyLogColorData>>(json);
        }
        else
        {
                cache = new FusionDictionary<string, FusionUserBodyLogColorData>();
        }
    }

    public static void AddOrUpdate(string steamId, BodyLogColorData bodyLogColorData)
    {
        var userBodyLogColorData = new FusionUserBodyLogColorData(steamId, bodyLogColorData);
        cache[steamId] = userBodyLogColorData;
        SaveCache();
    }

    private static void SaveCache()
    {
        var json = JsonConvert.SerializeObject(cache, Formatting.Indented);
        File.WriteAllText(cacheFilePath, json);
    }
        
    public static BodyLogColorData GetBodyLogColorData(string steamId)
    {
        if (cache.ContainsKey(steamId))
        {
            return cache[steamId].DecodeBodyLogColorData();
        }
        return null;
    }
        
    public static void ApplyCacheToDevice(PullCordDevice device)
    {
        foreach (var playerRep in PlayerRepManager.PlayerReps)
        {
            var steamId = playerRep.PlayerId.LongId.ToString();
            var colorData = GetBodyLogColorData(steamId);
            if (colorData == null)
            {
                continue;
            }
            
            device.ApplyColorsFromData(colorData);
        }
    }
}
