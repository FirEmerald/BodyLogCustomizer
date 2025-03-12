using BodyLogCustomizer.Data;
using BodyLogCustomizer.Patches;
using BodyLogCustomizer.Utilities;
using BoneLib.BoneMenu;
using LabFusion.Network;
using UnityEngine;
using static BodyLogCustomizer.Data.BodyLogColorData;

namespace BodyLogCustomizer.UI
{
    public static class BoneMenuCreator
    {
        // Color Selector variables
        private static Color mixedColor = Color.white;
        // Categories
        public static Page mainCategory { get; private set;}
        public static Page gizmoBallCategory { get; private set; }
        public static Page dialColorCategory { get; private set; }
        public static Page previewMeshCategory { get; private set; }
        // Sub Categories TODO bonelib no longer has subpanels sadge
        public static Element rElement { get; private set; }
        public static Element gElement { get; private set; }
        public static Element bElement { get; private set; }

        // Elements
        // Preview Mesh Color
        public static Element hologramEdgeColorElement { get; private set; }
        public static Element hologramEmissionColorElement { get; private set; }
        // Line Color
        public static Element lineColorElement { get; private set; }
        // Dial Color
        public static Element dialEmissionBaseElement { get; private set; }
        public static Element dialEmissionColorElement { get; private set; }
        // Ring Color
        public static Element ringColorElement { get; private set; }
        // Core Particle Color
        public static Element coreParticleColorElement { get; private set; }
        // Base Log Color
        public static Element baseLogColorElement { get; private set; }
        // Gizmo Ball Color
        public static Element gizmoBallColorElement { get; private set; }
        public static Element gizmoBallEmissionElement { get; private set; }
        // Tick Color
        public static Element tickColorElement { get; private set; }
        // Base Skin
        public static EnumElement baseSkinElement { get; private set; }
        
        // Debug Stuff
#if DEBUG
        public static Page debugElement { get; private set; }
        public static Element sendDebugData { get; private set; }

        public static Color globalDialColor { get; private set; }
#endif

        public static void CreateMenu()
        {
            mainCategory = Page.Root.CreatePage("BodyLog Customizer", Color.yellow);
            mainCategory.CreateBool("Auto-apply Enabled", Color.cyan, BodyLogCustomizer.isEnabled.Value, (bool value) =>
            {
                BodyLogCustomizer.isEnabled.Value = value;
            });
            mainCategory.CreateFunction("Apply Colors", Color.cyan, () =>
            {
                PullCordPatch.localDevice.ApplyColorsFromData(LogPrefInitializer.bodyLogColorData); 
                LogPrefInitializer.DeserializeConfig(); 
                if (BodyLogCustomizer.HasFusion && NetworkInfo.HasServer && BodyLogCustomizer.ReplicateOnServer.Value)
                    FusionModule.Instance.SendBodyLogColorData(LogPrefInitializer.bodyLogColorData);
            });
            var currColor = mainCategory.CreateFunction("Current Color", Color.white, () => { });
            rElement = mainCategory.CreateFloat("R", Color.red, mixedColor.r, .1f, 0f, 1f, (value) =>
            {
                mixedColor.r = value;
                rElement.ElementColor = new Color(value, 0, 0);
                currColor.ElementColor = mixedColor;
            });
            gElement = mainCategory.CreateFloat("G", Color.green, mixedColor.g, .1f, 0f, 1f, (value) =>
            {
                mixedColor.g = value;
                gElement.ElementColor = new Color(0, value, 0);
                currColor.ElementColor = mixedColor;
            });
            bElement = mainCategory.CreateFloat("B", Color.blue, mixedColor.b, .1f, 0f, 1f, (value) =>
            {
                mixedColor.b = value;
                bElement.ElementColor = new Color(0, 0, value);
                currColor.ElementColor = mixedColor;
            });
            /* TODO forms is not available anymore, is there a suitable replacement?
            if (!HelperMethods.IsAndroid())
            {
                var clipboardElement = colorSelectorSubCategory.CreateFunction("Paste from Clipboard", Color.white, () =>
                {
                    var clipboardText = Clipboard.GetText();
                    var result = ColorUtility.TryParseHtmlString(clipboardText, out Color color);
                    if (result)
                    {
                        mixedColor = color;
                        currColor.ElementColor = mixedColor;
                    }
                });
            }
            */
            
            baseSkinElement = mainCategory.CreateEnum("Body Log Skin", Color.yellow, LogPrefInitializer.bodyLogColorData.baseSkinType, (value) => { LogPrefInitializer.bodyLogColorData.baseSkinType = (BaseSkinType)value; });

            previewMeshCategory = mainCategory.CreatePage("Preview Mesh", Color.cyan);
            hologramEdgeColorElement = previewMeshCategory.CreateFunction("Set Hologram Edge Color", LogPrefInitializer.bodyLogColorData.previewMeshColor.hologramEdgeColor, () =>
            {
                var x = MenuColorButton(hologramEdgeColorElement); 
                LogPrefInitializer.bodyLogColorData.previewMeshColor.hologramEdgeColor = x;
            });
            hologramEmissionColorElement = previewMeshCategory.CreateFunction("Set Hologram Emission Color", LogPrefInitializer.bodyLogColorData.previewMeshColor.hologramEmissionColor, () =>
            {
                var x = MenuColorButton(hologramEmissionColorElement); 
                LogPrefInitializer.bodyLogColorData.previewMeshColor.hologramEmissionColor = x;
            });
            
            dialColorCategory = mainCategory.CreatePage("Dial Color", Color.cyan);
            dialEmissionBaseElement = dialColorCategory.CreateFunction("Set Emission Base", LogPrefInitializer.bodyLogColorData.dialColor[0].emissionBase, () => 
            { 
                var x = MenuColorButton(dialEmissionBaseElement);
                for (int i = 0; i < LogPrefInitializer.bodyLogColorData.dialColor.Length; i++)
                {
                    LogPrefInitializer.bodyLogColorData.dialColor[i].emissionBase = x;
                }

            });
            dialEmissionColorElement = dialColorCategory.CreateFunction("Set Emission Color", LogPrefInitializer.bodyLogColorData.dialColor[0].emissionColor, () =>
            {
                var x = MenuColorButton(dialEmissionColorElement);
                for (int i = 0; i < LogPrefInitializer.bodyLogColorData.dialColor.Length; i++)
                {
                    LogPrefInitializer.bodyLogColorData.dialColor[i].emissionColor = x;
                }
            });
            
            gizmoBallCategory = mainCategory.CreatePage("Gizmo Ball", Color.cyan);
            gizmoBallColorElement = gizmoBallCategory.CreateFunction("Set Gizmo Ball Color", LogPrefInitializer.bodyLogColorData.gizmoBallColor.gizmoBallColor, () =>
            {
                var x = MenuColorButton(gizmoBallColorElement); 
                LogPrefInitializer.bodyLogColorData.gizmoBallColor.gizmoBallColor = x;
            });
            gizmoBallEmissionElement = gizmoBallCategory.CreateFunction("Set Gizmo Ball Emission", LogPrefInitializer.bodyLogColorData.gizmoBallColor.gizmoBallEmission, () =>
            { 
                var x = MenuColorButton(gizmoBallEmissionElement); 
                LogPrefInitializer.bodyLogColorData.gizmoBallColor.gizmoBallEmission = x;
            });
            
            baseLogColorElement = mainCategory.CreateFunction("Set Base Log Color", LogPrefInitializer.bodyLogColorData.baseLogColor, () =>
            {
                var x = MenuColorButton(baseLogColorElement); 
                LogPrefInitializer.bodyLogColorData.baseLogColor = x;
            });
            ringColorElement = mainCategory.CreateFunction("Set Ring Color", LogPrefInitializer.bodyLogColorData.ringColor, () =>
            {
                var x = MenuColorButton(ringColorElement); 
                LogPrefInitializer.bodyLogColorData.ringColor = x;
            });
            coreParticleColorElement = mainCategory.CreateFunction("Set Core Particle Color", LogPrefInitializer.bodyLogColorData.coreParticleColor, () =>
            {
                var x = MenuColorButton(coreParticleColorElement); 
                LogPrefInitializer.bodyLogColorData.coreParticleColor = x;
            });


            lineColorElement = mainCategory.CreateFunction("Set Line Color", LogPrefInitializer.bodyLogColorData.lineColor, () => 
            {
                var x = MenuColorButton(lineColorElement); 
                LogPrefInitializer.bodyLogColorData.lineColor = x;
            });
            tickColorElement = mainCategory.CreateFunction("Set Tick Color", LogPrefInitializer.bodyLogColorData.tickColor[0], () =>
            {
                var x = MenuColorButton(tickColorElement);
                for (int i = 0; i < LogPrefInitializer.bodyLogColorData.tickColor.Length; i++)
                {
                    LogPrefInitializer.bodyLogColorData.tickColor[i] = x;
                }
            });

#if DEBUG
            debugElement = mainCategory.CreatePage("Debug", Color.red);
            sendDebugData = debugElement.CreateFunction("Send Debug Data", Color.red, () =>
            {
                FusionModule.Instance.SendBodyLogColorData(LogPrefInitializer.bodyLogColorData);
            });
#endif
        }

        public static Color MenuColorButton(Element element)
        {
            Color color = mixedColor;

            element.ElementColor = color;
            
            return color;
        }
    }
}