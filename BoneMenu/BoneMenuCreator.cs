using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BodyLogCustomizer.Data;
using BodyLogCustomizer.Patches;
using BodyLogCustomizer.Utilities;
using BoneLib;
using BoneLib.BoneMenu;
using BoneLib.BoneMenu.Elements;
using LabFusion.Network;
using MelonLoader;
using SLZ.Rig;
using UnityEngine;
using static BodyLogCustomizer.Data.BodyLogColorData;

namespace BodyLogCustomizer.UI
{
    public static class BoneMenuCreator
    {
        // Color Selector variables
        private static Color mixedColor = Color.white;
        // Categories
        public static MenuCategory mainCategory { get; private set;}
        public static MenuCategory gizmoBallCategory { get; private set; }
        public static MenuCategory dialColorCategory { get; private set; }
        public static MenuCategory previewMeshCategory { get; private set; }
        // Sub Categories
        public static SubPanelElement colorSelectorSubCategory { get; private set; }
        public static MenuElement rElement { get; private set; }
        public static MenuElement gElement { get; private set; }
        public static MenuElement bElement { get; private set; }

        // Elements
        // Preview Mesh Color
        public static MenuElement hologramEdgeColorElement { get; private set; }
        public static MenuElement hologramEmissionColorElement { get; private set; }
        // Line Color
        public static MenuElement lineColorElement { get; private set; }
        // Dial Color
        public static MenuElement dialEmissionBaseElement { get; private set; }
        public static MenuElement dialEmissionColorElement { get; private set; }
        // Ring Color
        public static MenuElement ringColorElement { get; private set; }
        // Core Particle Color
        public static MenuElement coreParticleColorElement { get; private set; }
        // Base Log Color
        public static MenuElement baseLogColorElement { get; private set; }
        // Gizmo Ball Color
        public static MenuElement gizmoBallColorElement { get; private set; }
        public static MenuElement gizmoBallEmissionElement { get; private set; }
        // Tick Color
        public static MenuElement tickColorElement { get; private set; }
        // Base Skin
        public static EnumElement<BaseSkinType> baseSkinElement { get; private set; }
        
        // Debug Stuff
#if DEBUG
        public static MenuCategory debugElement { get; private set; }
        public static MenuElement sendDebugData { get; private set; }

        public static Color globalDialColor { get; private set; }
#endif

        public static void CreateMenu()
        {
            mainCategory = MenuManager.CreateCategory("BodyLog Customizer", Color.yellow);
            mainCategory.CreateBoolElement("Auto-apply Enabled", Color.cyan, BodyLogCustomizer.isEnabled.Value, (bool value) =>
            {
                BodyLogCustomizer.isEnabled.Value = value;
            });
            mainCategory.CreateFunctionElement("Apply Colors", Color.cyan, () =>
            {
                PullCordPatch.localDevice.ApplyColorsFromData(LogPrefInitializer.bodyLogColorData); 
                LogPrefInitializer.DeserializeConfig(); 
                if (BodyLogCustomizer.HasFusion && NetworkInfo.HasServer && BodyLogCustomizer.ReplicateOnServer.Value)
                    FusionModule.Instance.SendBodyLogColorData(LogPrefInitializer.bodyLogColorData);
            });
            colorSelectorSubCategory = mainCategory.CreateSubPanel("Color Selector", Color.cyan);
            var currColor = colorSelectorSubCategory.CreateFunctionElement("Current Color", Color.white, () => { });
            rElement = colorSelectorSubCategory.CreateFloatElement("R", Color.red, mixedColor.r, .1f, 0f, 1f, (value) =>
            {
                mixedColor.r = value;
                rElement.SetColor(new Color(value, 0, 0));
                currColor.SetColor(mixedColor);
            });
            gElement = colorSelectorSubCategory.CreateFloatElement("G", Color.green, mixedColor.g, .1f, 0f, 1f, (value) =>
            {
                mixedColor.g = value;
                gElement.SetColor(new Color(0, value, 0));
                currColor.SetColor(mixedColor);
            });
            bElement = colorSelectorSubCategory.CreateFloatElement("B", Color.blue, mixedColor.b, .1f, 0f, 1f, (value) =>
            {
                mixedColor.b = value;
                bElement.SetColor(new Color(0, 0, value));
                currColor.SetColor(mixedColor);
            });
            if (!HelperMethods.IsAndroid())
            {
                var clipboardElement = colorSelectorSubCategory.CreateFunctionElement("Paste from Clipboard", Color.magenta, () =>
                {
                    var clipboardText = Clipboard.GetText();
                    var result = ColorUtility.TryParseHtmlString(clipboardText, out Color color);
                    if (result)
                    {
                        mixedColor = color;
                        currColor.SetColor(mixedColor);
                    }
                });
            }
            
            baseSkinElement = mainCategory.CreateEnumElement<BaseSkinType>("Body Log Skin", Color.yellow, (value) => { LogPrefInitializer.bodyLogColorData.baseSkinType = value; });
            baseSkinElement.SetValue(LogPrefInitializer.bodyLogColorData.baseSkinType);

            previewMeshCategory = mainCategory.CreateCategory("Preview Mesh", Color.cyan);
            hologramEdgeColorElement = previewMeshCategory.CreateFunctionElement("Set Hologram Edge Color (CB)", LogPrefInitializer.bodyLogColorData.previewMeshColor.hologramEdgeColor, () =>
            {
                var x = MenuColorButton(hologramEdgeColorElement); 
                LogPrefInitializer.bodyLogColorData.previewMeshColor.hologramEdgeColor = x;
            });
            hologramEmissionColorElement = previewMeshCategory.CreateFunctionElement("Set Hologram Emission Color (CB)", LogPrefInitializer.bodyLogColorData.previewMeshColor.hologramEmissionColor, () =>
            {
                var x = MenuColorButton(hologramEmissionColorElement); 
                LogPrefInitializer.bodyLogColorData.previewMeshColor.hologramEmissionColor = x;
            });
            
            dialColorCategory = mainCategory.CreateCategory("Dial Color", Color.cyan);
            dialEmissionBaseElement = dialColorCategory.CreateFunctionElement("Set Emission Base (CB)", LogPrefInitializer.bodyLogColorData.dialColor[0].emissionBase, () => 
            { 
                var x = MenuColorButton(dialEmissionBaseElement);
                for (int i = 0; i < LogPrefInitializer.bodyLogColorData.dialColor.Length; i++)
                {
                    LogPrefInitializer.bodyLogColorData.dialColor[i].emissionBase = x;
                }

            });
            dialEmissionColorElement = dialColorCategory.CreateFunctionElement("Set Emission Color (CB)", LogPrefInitializer.bodyLogColorData.dialColor[0].emissionColor, () =>
            {
                var x = MenuColorButton(dialEmissionColorElement);
                for (int i = 0; i < LogPrefInitializer.bodyLogColorData.dialColor.Length; i++)
                {
                    LogPrefInitializer.bodyLogColorData.dialColor[i].emissionColor = x;
                }
            });
            
            gizmoBallCategory = mainCategory.CreateCategory("Gizmo Ball", Color.cyan);
            gizmoBallColorElement = gizmoBallCategory.CreateFunctionElement("Set Gizmo Ball Color (CB)", LogPrefInitializer.bodyLogColorData.gizmoBallColor.gizmoBallColor, () =>
            {
                var x = MenuColorButton(gizmoBallColorElement); 
                LogPrefInitializer.bodyLogColorData.gizmoBallColor.gizmoBallColor = x;
            });
            gizmoBallEmissionElement = gizmoBallCategory.CreateFunctionElement("Set Gizmo Ball Emission (CB)", LogPrefInitializer.bodyLogColorData.gizmoBallColor.gizmoBallEmission, () =>
            { 
                var x = MenuColorButton(gizmoBallEmissionElement); 
                LogPrefInitializer.bodyLogColorData.gizmoBallColor.gizmoBallEmission = x;
            });
            
            baseLogColorElement = mainCategory.CreateFunctionElement("Set Base Log Color (CB)", LogPrefInitializer.bodyLogColorData.baseLogColor, () =>
            {
                var x = MenuColorButton(baseLogColorElement); 
                LogPrefInitializer.bodyLogColorData.baseLogColor = x;
            });
            ringColorElement = mainCategory.CreateFunctionElement("Set Ring Color (CB)", LogPrefInitializer.bodyLogColorData.ringColor, () =>
            {
                var x = MenuColorButton(ringColorElement); 
                LogPrefInitializer.bodyLogColorData.ringColor = x;
            });
            coreParticleColorElement = mainCategory.CreateFunctionElement("Set Core Particle Color (CB)", LogPrefInitializer.bodyLogColorData.coreParticleColor, () =>
            {
                var x = MenuColorButton(coreParticleColorElement); 
                LogPrefInitializer.bodyLogColorData.coreParticleColor = x;
            });


            lineColorElement = mainCategory.CreateFunctionElement("Set Line Color (CB)", LogPrefInitializer.bodyLogColorData.lineColor, () => 
            {
                var x = MenuColorButton(lineColorElement); 
                LogPrefInitializer.bodyLogColorData.lineColor = x;
            });
            tickColorElement = mainCategory.CreateFunctionElement("Set Tick Color (CB)", LogPrefInitializer.bodyLogColorData.tickColor[0], () =>
            {
                var x = MenuColorButton(tickColorElement);
                for (int i = 0; i < LogPrefInitializer.bodyLogColorData.tickColor.Length; i++)
                {
                    LogPrefInitializer.bodyLogColorData.tickColor[i] = x;
                }
            });

#if DEBUG
            debugElement = mainCategory.CreateCategory("Debug", Color.red);
            sendDebugData = debugElement.CreateFunctionElement("Send Debug Data", Color.red, () =>
            {
                FusionModule.Instance.SendBodyLogColorData(LogPrefInitializer.bodyLogColorData);
            });
#endif
        }

        public static Color MenuColorButton(MenuElement element)
        {
            Color color = mixedColor;
            
            element.SetColor(color);
            
            return color;
        }
    }
}