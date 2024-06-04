using MelonLoader;
using UnityEngine;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using BoneLib.BoneMenu.Elements;
using BoneLib.BoneMenu;
using BodyLogCustomizer.Data;
using BodyLogCustomizer.UI;
using BoneLib;
using LabFusion.SDK.Modules;
using SLZ.Rig;
using LabFusion.Utilities;
using Il2CppSystem;
using LabFusion.Representation;

namespace BodyLogCustomizer
{
    public static class BuildInfo
    {
        public const string Name = "BodyLogCustomizer"; // Name of the Mod.  (MUST BE SET)
        public const string Author = "BuggedChecker"; // Author of the Mod.  (Set as null if none)
        public const string Company = null; // Company that made the Mod.  (Set as null if none)
        public const string Version = "1.0.0"; // Version of the Mod.  (MUST BE SET)
        public const string DownloadLink = null; // Download Link for the Mod.  (Set as null if none)
    }

    public class BodyLogCustomizer : MelonMod
    {
        public static MelonPreferences_Category BodyLogPrefs;
        public static MelonPreferences_Entry<string> AssemblyHash;
        public static MelonPreferences_Entry<bool> isEnabled;
        public static MelonPreferences_Entry<bool> ReplicateOnServer;
        public static MelonPreferences_Entry<bool> ReplicateOnClient;
        
        public static bool HasFusion = false;

        public override void OnInitializeMelon()
        {
            // Check if Fusion is installed
            HasFusion = HelperMethods.CheckIfAssemblyLoaded("labfusion");
            
            // Preferences
            BodyLogPrefs = MelonPreferences.CreateCategory("BodyLogCustomizer", "BodyLog Customizer");
            AssemblyHash = BodyLogPrefs.CreateEntry("AssemblyHash", "", "Assembly Hash (DO NOT CHANGE)");
            isEnabled = BodyLogPrefs.CreateEntry("isEnabled", false, "Enable BodyLog Customizer");
            ReplicateOnServer = BodyLogPrefs.CreateEntry("ReplicateOnServer", true, "Replicate BodyLog Customizer on Server");
            ReplicateOnClient = BodyLogPrefs.CreateEntry("ReplicateOnClient", true, "Replicate BodyLog Customizer on Client");
            
            BodyLogPrefs.SetFilePath(MelonUtils.UserDataDirectory + "/BodyLogCustomizer.cfg");
            BodyLogPrefs.SaveToFile(true);

            // Data
            LogPrefInitializer.Initialize();

            // UI
            BoneMenuCreator.CreateMenu();

            if (HasFusion)
            {
                // Load Module
                ModuleHandler.LoadModule(Assembly.GetExecutingAssembly());

                // Hooks
                MultiplayerHooking.OnPlayerRepCreated += OnPlayerRepCreated;
            }

            // Load Assets
            AssetBundleLoader.LoadAssets();
        }
        private void OnPlayerRepCreated(RigManager rm)
        {
            var playerRep = PlayerRepManager.TryGetPlayerRep(rm, out var rep);
            
            FusionUserCacheManager.LoadCache();
            FusionUserCacheManager.ApplyCacheToDevice(rep.pullCord);
            
            FusionModule.Instance.SendBodyLogColorData(LogPrefInitializer.bodyLogColorData);
        }
    }
}
