using MelonLoader;
using System.Timers;
using BodyLogCustomizer.Data;
using BodyLogCustomizer.UI;
using BoneLib;
using LabFusion.Entities;
using LabFusion.Player;
using Il2CppSLZ.Marrow;
using LabFusion.Representation;
using Il2CppSLZ.Bonelab;
using System;
using Il2CppSLZ.VRMK;
using LabFusion.Data;
using LabFusion.Network;
using UnityEngine;

namespace BodyLogCustomizer
{
    public static class BuildInfo
    {
        public const string Name = "BodyLogCustomizer"; // Name of the Mod.  (MUST BE SET)
        public const string Author = "BuggedChecker"; // Author of the Mod.  (Set as null if none)
        public const string Company = null; // Company that made the Mod.  (Set as null if none)
        public const string Version = "1.1.0"; // Version of the Mod.  (MUST BE SET)
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
            HasFusion = HelperMethods.CheckIfAssemblyLoaded("LabFusion");
            
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
            FusionUserCacheManager.LoadCache();

            // UI
            BoneMenuCreator.CreateMenu();

            if (HasFusion)
            {
                LoadModule();
            }

            // Load Assets
            AssetBundleLoader.LoadAssets();
        }
        private void LoadModule()
        {
            // Load Module
            LabFusion.SDK.Modules.ModuleManager.RegisterModule<FusionModule>();

            // Hooks
            NetworkPlayer.OnNetworkRigCreated += OnPlayerRepCreated;
        }

        private void OnPlayerRepCreated(NetworkPlayer player, RigManager rm)
        {
            var timer = new Timer(1000);
            timer.AutoReset = false;
            timer.Start();
            timer.Elapsed += (sender, args) =>
            {
                FusionUserCacheManager.LoadCache();
                FusionUserCacheManager.ApplyCacheToDevice(player, rm.physicsRig.GetComponentInChildren<PullCordDevice>(includeInactive: true));
            
                FusionModule.Instance.SendBodyLogColorData(LogPrefInitializer.bodyLogColorData);
            };
        }
    }
}
