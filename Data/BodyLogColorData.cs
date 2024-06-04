using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BodyLogCustomizer.Data
{
    [System.Serializable]
    public class BodyLogColorData
    {
        public struct DialColor
        {
            public Color emissionBase;
            public Color emissionColor;
        }
        public struct GizmoColor
        {
            public Color gizmoBallColor;
            public Color gizmoBallEmission;
        }
        public struct PreviewMeshColor
        {
            public Color hologramEdgeColor;
            public Color hologramEmissionColor;
        }
        public enum BaseSkinType
        {
            Default,
            AnimatedFire,
            GlintSparkle,
            Burnt,
            ForceShield,
            AnimatedUVDistortion,
            DissolveBurn,
            XRay,
            GradientGreen,
            GradientTropical,
            GradientRedAndPurple,
            GradientGreenAndBlue,
            GradientPinkAndBlue,
            GradientPurple,
        }
        
        public Color ringColor;
        public Color coreParticleColor;
        public Color baseLogColor;

        public Color lineColor;
        public Color[] tickColor;
        public PreviewMeshColor previewMeshColor;
        public GizmoColor gizmoBallColor;
        public DialColor[] dialColor;
        public BaseSkinType baseSkinType;

        public BodyLogColorData(Color lineColor, DialColor[] dialColor, Color[] tickColor, GizmoColor gizmoBallColor, Color ringColor, Color coreParticleColor, Color baseLogColor, PreviewMeshColor previewMeshColor, int baseSkinTypeId)
        {
            this.lineColor = lineColor;
            this.tickColor = new Color[7];
            this.gizmoBallColor = gizmoBallColor;
            this.ringColor = ringColor;
            this.coreParticleColor = coreParticleColor;
            this.dialColor = new DialColor[6];
            for (int i = 0; i < 6; i++)
            {
                this.dialColor[i] = dialColor[i];
            }
            for (int i = 0; i < 7; i++)
            {
                this.tickColor[i] = tickColor[i];
            }
            this.baseLogColor = baseLogColor;
            this.previewMeshColor = previewMeshColor;
            this.baseSkinType = (BaseSkinType)baseSkinTypeId;
        }
    }
}

