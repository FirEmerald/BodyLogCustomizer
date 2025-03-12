using HarmonyLib;
using BodyLogCustomizer.Utilities;
using BodyLogCustomizer.Data;
using LabFusion.Extensions;
using LabFusion.Network;
using Il2CppSLZ.Bonelab;

namespace BodyLogCustomizer.Patches
{
    [HarmonyPatch(typeof(PullCordDevice), "Awake")]
    internal static class PullCordPatch
    {
        public static PullCordDevice localDevice;
        internal static void Postfix(PullCordDevice __instance)
        {
            if (BodyLogCustomizer.HasFusion && NetworkInfo.HasServer && !__instance.gameObject.IsPartOfSelf())
                return;
            
            localDevice = __instance;
            
            if (!BodyLogCustomizer.isEnabled.Value) 
                return;
            
            if (!BodyLogCustomizer.ReplicateOnClient.Value)
                return;
            
            __instance.ApplyColorsFromData(LogPrefInitializer.bodyLogColorData);
            
            if (BodyLogCustomizer.HasFusion && NetworkInfo.HasServer && BodyLogCustomizer.ReplicateOnServer.Value)
                FusionModule.Instance.SendBodyLogColorData(LogPrefInitializer.bodyLogColorData);
        }
    }
}
