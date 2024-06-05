using System.IO;
using System.Reflection;
using BodyLogCustomizer.Data;
using BodyLogCustomizer.Utilities;
using LabFusion.Representation;
using MelonLoader;
using Newtonsoft.Json;
using SLZ.Props;

public static class FusionUserCacheManager
{
    private static string cacheDirectoryPath = "UserData/BodyLogCustomizer/FusionUserCache/";

    public static void AddOrUpdate(string steamId, BodyLogColorData bodyLogColorData)
    {
        var userBodyLogColorData = new FusionUserBodyLogColorData(steamId, bodyLogColorData);
        var json = JsonConvert.SerializeObject(userBodyLogColorData, Formatting.Indented);
        File.WriteAllText(Path.Combine(cacheDirectoryPath, steamId + ".json"), json);
    }

    public static BodyLogColorData GetBodyLogColorData(string steamId)
    {
        var filePath = Path.Combine(cacheDirectoryPath, steamId + ".json");
        if (File.Exists(filePath))
        {
            var json = File.ReadAllText(filePath);
            var userBodyLogColorData = JsonConvert.DeserializeObject<FusionUserBodyLogColorData>(json);
            return userBodyLogColorData.DecodeBodyLogColorData();
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
            
#if DEBUG
            MelonLogger.Msg($"Applied cache to {steamId}");
#endif
        }
    }
    
    public static void LoadCache()
    {
        if (!Directory.Exists(cacheDirectoryPath))
        {
            Directory.CreateDirectory(cacheDirectoryPath);
        }

        var currentAssemblyHash = MelonUtils.ComputeSimpleSHA256Hash(Assembly.GetExecutingAssembly().Location);
        if (MelonPreferences.GetEntryValue<string>("BodyLogCustomizer", "AssemblyHash") != currentAssemblyHash)
        {
            foreach (var file in Directory.GetFiles(cacheDirectoryPath))
            {
                File.Delete(file);
            }
            MelonPreferences.SetEntryValue("BodyLogCustomizer", "AssemblyHash", currentAssemblyHash);
        }

        foreach (var file in Directory.GetFiles(cacheDirectoryPath))
        {
            var json = File.ReadAllText(file);
            var userBodyLogColorData = JsonConvert.DeserializeObject<FusionUserBodyLogColorData>(json);
            var steamId = Path.GetFileNameWithoutExtension(file);
            AddOrUpdate(steamId, userBodyLogColorData.DecodeBodyLogColorData());
        }
    }
}