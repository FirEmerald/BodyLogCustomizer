using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static BodyLogCustomizer.Data.BodyLogColorData;

namespace BodyLogCustomizer.Data
{
    public class DefaultValues
    {
        public static readonly GizmoColor gizmoBallColor = new GizmoColor
        {
            gizmoBallColor = Color.white,
            gizmoBallEmission = Color.white
        };
        public static readonly PreviewMeshColor previewMeshColor = new PreviewMeshColor
        {
            hologramEdgeColor = Color.white,
            hologramEmissionColor = Color.white
        };
        public static readonly Color ringColor = Color.white;
        public static readonly Color coreParticleColor = Color.white;
        public static readonly Color baseLogColor = Color.white;

        public static readonly Color lineColor = Color.white;
        public static readonly Color[] tickColor = new Color[]
        {
            Color.white,
            Color.white,
            Color.white,
            Color.white,
            Color.white,
            Color.white,
            Color.white,
            Color.white,
        };
        public static readonly DialColor[] dialColor = new DialColor[]
        {
            new DialColor
            {
                emissionBase = Color.white,
                emissionColor = Color.white,
            },
            new DialColor
            {
                emissionBase = Color.white,
                emissionColor = Color.white,
            },
            new DialColor
            {
                emissionBase = Color.white,
                emissionColor = Color.white,
            },
            new DialColor
            {
                emissionBase = Color.white,
                emissionColor = Color.white,
            },
            new DialColor
            {
                emissionBase = Color.white,
                emissionColor = Color.white,
            },
            new DialColor
            {
                emissionBase = Color.white,
                emissionColor = Color.white,
            }
        };
        public static readonly BaseSkinType baseSkinType = BaseSkinType.Default;
    }
}
