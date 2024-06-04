using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using BodyLogCustomizer.Utilities;
using MelonLoader;
using Newtonsoft.Json;
using UnityEngine;
using static BodyLogCustomizer.Data.BodyLogColorData;

namespace BodyLogCustomizer.Data
{
    public static class LogPrefInitializer
    {
        public const string dataFolder = "UserData/BodyLogCustomizer";
        public const string logPrefsFile = "LocalBodyLogPref.json";
        public const string combinedPrefsFile = $"{dataFolder}/{logPrefsFile}";

        public static BodyLogColorData bodyLogColorData { get; set;}
        public static void Initialize()
        {
            System.IO.Directory.CreateDirectory(dataFolder);
            var assemblyLocation = Assembly.GetExecutingAssembly().Location;
            if (System.IO.File.Exists(combinedPrefsFile))
            {
                var hash = MelonUtils.ComputeSimpleSHA256Hash(assemblyLocation);
                MelonLogger.Msg("Hash: " + hash);
                if (MelonPreferences.GetEntryValue<string>("BodyLogCustomizer", "AssemblyHash") == hash)
                {
                    LogPrefInitializer.bodyLogColorData = SerializeConfig();
                    return;
                }
                else
                {
                    System.IO.File.Move(combinedPrefsFile, combinedPrefsFile+"_"+hash);
#if DEBUG
                    System.IO.File.Delete(combinedPrefsFile + "_" + BodyLogCustomizer.AssemblyHash.Value);
#endif
                    BodyLogCustomizer.AssemblyHash.Value = hash;
                }
            }

            Color lineColor = DefaultValues.lineColor;
            GizmoColor gizmoColor = DefaultValues.gizmoBallColor;
            DialColor[] dialColor = DefaultValues.dialColor;
            Color[] tickColor = DefaultValues.tickColor;
            Color ringColor = DefaultValues.ringColor;
            Color coreParticleColor = DefaultValues.coreParticleColor;
            Color baseLogColor = DefaultValues.baseLogColor;
            PreviewMeshColor previewMeshColor = DefaultValues.previewMeshColor;
            BaseSkinType baseSkinType = DefaultValues.baseSkinType;

            var bodyLogColorData = new BodyLogColorData(lineColor, dialColor, tickColor, gizmoColor, ringColor, coreParticleColor, baseLogColor, previewMeshColor, (int)baseSkinType); ;

            string json = JsonConvert.SerializeObject(bodyLogColorData, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Converters = new List<JsonConverter> { new ColorJsonConverter() }
            });
            System.IO.File.WriteAllText(combinedPrefsFile, json);
            LogPrefInitializer.bodyLogColorData = bodyLogColorData;
        }
        public static BodyLogColorData SerializeConfig()
        {
            if (!System.IO.File.Exists(combinedPrefsFile))
            {
                MelonLogger.Error("Failed to find BodyLog preferences file!");
                return null;
            }

            string json = System.IO.File.ReadAllText(combinedPrefsFile);
            return JsonConvert.DeserializeObject<BodyLogColorData>(json);
        }
        
        public static void DeserializeConfig()
        {
            if(bodyLogColorData == null)
            {
                MelonLogger.Error("Failed to deserialize BodyLog preferences file!");
                return;
            }
            string json = JsonConvert.SerializeObject(bodyLogColorData, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Converters = new List<JsonConverter> { new ColorJsonConverter() }
            });
            System.IO.File.WriteAllText(combinedPrefsFile, json);
        }
    }
}

