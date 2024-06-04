using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Reflection;
using BoneLib;
using MelonLoader;

namespace BodyLogCustomizer.Data
{
    internal static class AssetBundleLoader // Lets agree to never look at this ever, ever, ever again. This is and going to be fucking horrendous
    {
        internal static Material DefaultMat { get; private set; }
        internal static Material AnimatedFireMat { get; private set; }
        internal static Material GlintSparkleMat { get; private set; }
        internal static Material BurntMat { get; private set; }
        internal static Material ForceShieldMat { get; private set; }
        internal static Material AnimatedUVDistortionMat { get; private set; }
        internal static Material DissolveBurnMat { get; private set; }
        internal static Material XRayMat { get; private set; }
        internal static Material GradOneMat { get; private set; }
        internal static Material GradTwoMat { get; private set; }
        internal static Material GradThreeMat { get; private set; }
        internal static Material GradFourMat { get; private set; }
        internal static Material GradFiveMat { get; private set; }
        internal static Material GradSixMat { get; private set; }

        internal static void LoadAssets()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            AssetBundle resourceBundle = HelperMethods.LoadEmbeddedAssetBundle(assembly, HelperMethods.IsAndroid() ? "$safeprojectname$.Resources.Android.bodylogskins.bundle" : "$safeprojectname$.Resources.Windows.bodylogskins.bundle");

            if (resourceBundle == null)
            {
                MelonLogger.Error("Failed to load AssetBundle!");
                return;
            }

            DefaultMat = resourceBundle.LoadPersistentAsset<Material>("Assets/Base/BodyLog/BaseBodyLog.mat");

            AnimatedFireMat = resourceBundle.LoadPersistentAsset<Material>("Assets/AmplifyShaderPack/Samples URP/Samples/Animated Fire/Animated Fire.mat");
            
            GlintSparkleMat = resourceBundle.LoadPersistentAsset<Material>("Assets/AmplifyShaderPack/Samples URP/Samples/Glint Sparkle/Glint Sparkle.mat");
            
            BurntMat = resourceBundle.LoadPersistentAsset<Material>("Assets/AmplifyShaderPack/Samples URP/Samples/Burn/Burn Effect.mat");
            
            ForceShieldMat = resourceBundle.LoadPersistentAsset<Material>("Assets/AmplifyShaderPack/Samples URP/Samples/Community Force Shield/Force Shield.mat");
            
            AnimatedUVDistortionMat = resourceBundle.LoadPersistentAsset<Material>("Assets/AmplifyShaderPack/Samples URP/Samples/Animated UV Distortion/Animated UV Distortion.mat");
            
            DissolveBurnMat = resourceBundle.LoadPersistentAsset<Material>("Assets/AmplifyShaderPack/Samples URP/Samples/Community Dissolve Burn/Dissolve Burn.mat");
            
            XRayMat = resourceBundle.LoadPersistentAsset<Material>("Assets/AmplifyShaderPack/Samples URP/Samples/Xray/XRay.mat");
            
            // Time to add the 6 other gradient skins, sigh
            
            GradOneMat = resourceBundle.LoadPersistentAsset<Material>("Assets/Base/BodyLog/BaseBodyLog Grad One.mat");
            
            GradTwoMat = resourceBundle.LoadPersistentAsset<Material>("Assets/Base/BodyLog/BaseBodyLog Grad Two.mat");
            
            GradThreeMat = resourceBundle.LoadPersistentAsset<Material>("Assets/Base/BodyLog/BaseBodyLog Grad Three.mat");
            
            GradFourMat = resourceBundle.LoadPersistentAsset<Material>("Assets/Base/BodyLog/BaseBodyLog Grad Four.mat");
            
            GradFiveMat = resourceBundle.LoadPersistentAsset<Material>("Assets/Base/BodyLog/BaseBodyLog Grad Five.mat");
            
            GradSixMat = resourceBundle.LoadPersistentAsset<Material>("Assets/Base/BodyLog/BaseBodyLog Grad Six.mat");

            if (AnimatedFireMat == null)
                MelonLogger.Error("Failed to load AnimatedFireMat!");
            if (GlintSparkleMat == null)
                MelonLogger.Error("Failed to load GlintSparkleMat!");
            if (BurntMat == null)
                MelonLogger.Error("Failed to load BurntMat!");
            if (ForceShieldMat == null)
                MelonLogger.Error("Failed to load ForceShieldMat!");
            if (AnimatedUVDistortionMat == null)
                MelonLogger.Error("Failed to load AnimatedUVDistortionMat!");
            if (DissolveBurnMat == null)
                MelonLogger.Error("Failed to load DissolveBurnMat!");
            if (XRayMat == null)
                MelonLogger.Error("Failed to load XRayMat!");
            if (GradOneMat == null)
                MelonLogger.Error("Failed to load GradOneMat!");
            if (GradTwoMat == null)
                MelonLogger.Error("Failed to load GradTwoMat!");
            if (GradThreeMat == null)
                MelonLogger.Error("Failed to load GradThreeMat!");
            if (GradFourMat == null)
                MelonLogger.Error("Failed to load GradFourMat!");
            if (GradFiveMat == null)
                MelonLogger.Error("Failed to load GradFiveMat!");
            if (GradSixMat == null)
                MelonLogger.Error("Failed to load GradSixMat!");
        }
    }
}
