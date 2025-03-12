using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BodyLogCustomizer.Data;
using Il2CppSLZ.Bonelab;
using LabFusion.Extensions;
using MelonLoader;
using UnityEngine;

namespace BodyLogCustomizer.Utilities
{
    public static class BodyLogExtentions
    {
        public static void ApplyColorsFromData(this PullCordDevice device, BodyLogColorData bodyLogColorData)
        {
            if (device.gameObject.IsPartOfSelf())
            {
                if (!BodyLogCustomizer.ReplicateOnClient.Value)
                    return;
            }
            
            // Change the line color
            device.lineRenderer.material.SetColor("_ColorTint", bodyLogColorData.lineColor);
            device.lineRenderer.startColor = bodyLogColorData.lineColor;
            device.lineRenderer.endColor = bodyLogColorData.lineColor;

            // Change the tick colors
            for (int i = 0; i < device.tickLines.Length; i++)
            {
                var tick = device.tickLines[i];
                tick.GetComponent<Renderer>().material.SetColor("_ColorTint", bodyLogColorData.tickColor[i]);
            }
            
            // Change the dial colors
            for (int i = 0; i < device.dialGlyphs.Length; i++)
            {
                var dial = device.dialGlyphs[i];
                var r = dial.GetComponent<Renderer>();
                r.material.SetColor("_EmissionBase", bodyLogColorData.dialColor[i].emissionBase);
                r.material.SetColor("_EmissionColor", bodyLogColorData.dialColor[i].emissionColor);
            }

            // Change the particle colors
            var particleRenderers = device.GetComponentsInChildren<ParticleSystemRenderer>();
            foreach (var particleRenderer in particleRenderers)
            {
                particleRenderer.material.SetColor("_BaseColor", bodyLogColorData.coreParticleColor);
            }

            // Change the base/ring color
            var trans = device.transform.Find("BodyLog/BodyLog");
            if(trans != null) 
            { 
                var renderer = trans.GetComponent<Renderer>();
                if(renderer != null)
                {

                    // Time for skin types, here we go!
                    switch (bodyLogColorData.baseSkinType)
                    {
                        case BodyLogColorData.BaseSkinType.Default:
                            renderer.material = AssetBundleLoader.DefaultMat;
                            renderer.material.SetColor("_EmissionColor", bodyLogColorData.ringColor);
                            renderer.material.SetColor("_BaseColor", bodyLogColorData.baseLogColor);
                            break;
                        case BodyLogColorData.BaseSkinType.AnimatedFire:
                            renderer.material = AssetBundleLoader.AnimatedFireMat;
                            break;
                        case BodyLogColorData.BaseSkinType.GlintSparkle:
                            renderer.material = AssetBundleLoader.GlintSparkleMat;
                            break;
                        case BodyLogColorData.BaseSkinType.Burnt:
                            renderer.material = AssetBundleLoader.BurntMat;
                            break;
                        case BodyLogColorData.BaseSkinType.ForceShield:
                            renderer.material = AssetBundleLoader.ForceShieldMat;
                            break;
                        case BodyLogColorData.BaseSkinType.AnimatedUVDistortion:
                            renderer.material = AssetBundleLoader.AnimatedUVDistortionMat;
                            break;
                        case BodyLogColorData.BaseSkinType.DissolveBurn:
                            renderer.material = AssetBundleLoader.DissolveBurnMat;
                            break;
                        case BodyLogColorData.BaseSkinType.XRay:
                            renderer.material = AssetBundleLoader.XRayMat;
                            break;
                        case BodyLogColorData.BaseSkinType.GradientGreen:
                            renderer.material = AssetBundleLoader.GradOneMat;
                            renderer.material.SetColor("_EmissionColor", bodyLogColorData.ringColor);
                            renderer.material.SetColor("_BaseColor", bodyLogColorData.baseLogColor);
                            break;
                        case BodyLogColorData.BaseSkinType.GradientTropical:
                            renderer.material = AssetBundleLoader.GradTwoMat;
                            renderer.material.SetColor("_EmissionColor", bodyLogColorData.ringColor);
                            renderer.material.SetColor("_BaseColor", bodyLogColorData.baseLogColor);
                            break;
                        case BodyLogColorData.BaseSkinType.GradientRedAndPurple:
                            renderer.material = AssetBundleLoader.GradThreeMat;
                            renderer.material.SetColor("_EmissionColor", bodyLogColorData.ringColor);
                            renderer.material.SetColor("_BaseColor", bodyLogColorData.baseLogColor);
                            break;
                        case BodyLogColorData.BaseSkinType.GradientGreenAndBlue:
                            renderer.material = AssetBundleLoader.GradFourMat;
                            renderer.material.SetColor("_EmissionColor", bodyLogColorData.ringColor);
                            renderer.material.SetColor("_BaseColor", bodyLogColorData.baseLogColor);
                            break;
                        case BodyLogColorData.BaseSkinType.GradientPinkAndBlue:
                            renderer.material = AssetBundleLoader.GradFiveMat;
                            renderer.material.SetColor("_EmissionColor", bodyLogColorData.ringColor);
                            renderer.material.SetColor("_BaseColor", bodyLogColorData.baseLogColor);
                            break;
                        case BodyLogColorData.BaseSkinType.GradientPurple:
                            renderer.material = AssetBundleLoader.GradSixMat;
                            renderer.material.SetColor("_EmissionColor", bodyLogColorData.ringColor);
                            renderer.material.SetColor("_BaseColor", bodyLogColorData.baseLogColor);
                            break;
                    }
                }
            }
            
            // Change the gizmo ball color
            var gizmoBall = device.ballArt.GetComponentInChildren<Renderer>();
            if (gizmoBall != null)
            {
                gizmoBall.material.SetColor("_Color", bodyLogColorData.gizmoBallColor.gizmoBallColor);
                gizmoBall.material.SetColor("_Emission", bodyLogColorData.gizmoBallColor.gizmoBallEmission);
            }
            
            // Change the preview mesh colors
            var previewMesh = device.previewMeshRenderer;
            if (previewMesh != null)
            {
                previewMesh.material.SetColor("_HologramEdgeColor", bodyLogColorData.previewMeshColor.hologramEdgeColor);
                previewMesh.material.SetColor("_EmissionColor", bodyLogColorData.previewMeshColor.hologramEmissionColor);
                previewMesh.material.SetColor("_Color", bodyLogColorData.baseLogColor);
            }
        }
    }
}
