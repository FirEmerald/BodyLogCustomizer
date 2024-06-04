using System;
using System.Timers;
using BodyLogCustomizer.Utilities;
using LabFusion.Data;
using LabFusion.Network;
using LabFusion.Representation;
using MelonLoader;
using UnityEngine;
using static BodyLogCustomizer.Data.BodyLogColorData;

namespace BodyLogCustomizer.Data.Messages
{
    public class ColorDataFusion
    {
        public class ColorData : IFusionSerializable, IDisposable
        {
            public GizmoColor GizmoColor { get; set; }
            public PreviewMeshColor PreviewMeshColor { get; set; }
            public byte PlayerId { get; set; }
            public Color RingColor { get; set; }
            public Color CoreParticleColor { get; set; }
            public Color BaseLogColor { get; set; }
            public Color LineColor { get; set; }
            public Color[] TickColor { get; set; }
            public DialColor[] DialColor { get; set; }
            public int BaseSkinTypeId { get; set; }


            public void Dispose()
            {
                GC.SuppressFinalize(this);
            }

            public void Serialize(FusionWriter writer)
            {
                writer.Write(PlayerId);
                writer.Write(GizmoColor.gizmoBallColor);
                writer.Write(GizmoColor.gizmoBallEmission);
                writer.Write(PreviewMeshColor.hologramEdgeColor);
                writer.Write(PreviewMeshColor.hologramEmissionColor);
                writer.Write(RingColor);
                writer.Write(CoreParticleColor);
                writer.Write(BaseLogColor);
                writer.Write(LineColor);
                writer.Write(BaseSkinTypeId);
                foreach (var color in TickColor)
                {
                    writer.Write(color);
                }
                foreach (var dialColor in DialColor)
                {
                    writer.Write(dialColor.emissionBase);
                    writer.Write(dialColor.emissionColor);
                }
            }

            public void Deserialize(FusionReader reader)
            {
                PlayerId = reader.ReadByte();
                GizmoColor = new GizmoColor
                {
                    gizmoBallColor = reader.ReadColor(),
                    gizmoBallEmission = reader.ReadColor()
                };
                PreviewMeshColor = new PreviewMeshColor
                {
                    hologramEdgeColor = reader.ReadColor(),
                    hologramEmissionColor = reader.ReadColor()
                };
                RingColor = reader.ReadColor();
                CoreParticleColor = reader.ReadColor();
                BaseLogColor = reader.ReadColor();
                LineColor = reader.ReadColor();
                TickColor = new Color[7];
                DialColor = new DialColor[6];
                BaseSkinTypeId = reader.ReadInt32();
                for (int i = 0; i < 7; i++)
                {
                    TickColor[i] = reader.ReadColor();
                }
                for (int i = 0; i < 6; i++)
                {
                    DialColor[i].emissionBase = reader.ReadColor();
                    DialColor[i].emissionColor = reader.ReadColor();
                }
            }

            public static ColorData Create(Color lineColor, DialColor[] dialColor, Color[] tickColor, GizmoColor gizmoColor, Color ringColor, Color coreParticleColor, Color baseLogColor, PreviewMeshColor previewMeshColor, int baseSkinTypeId)
            {
                return new ColorData
                {
                    PlayerId = PlayerIdManager.LocalSmallId,
                    LineColor = lineColor,
                    TickColor = tickColor,
                    GizmoColor = gizmoColor,
                    DialColor = dialColor,
                    RingColor = ringColor,
                    CoreParticleColor = coreParticleColor,
                    BaseLogColor = baseLogColor,
                    PreviewMeshColor = previewMeshColor,
                    BaseSkinTypeId = baseSkinTypeId
                };
            }
        }

        public class ColorHandler : ModuleMessageHandler
        {
            public override void HandleMessage(byte[] bytes, bool isServerHandled = false)
            {
                using var reader = FusionReader.Create(bytes);
                using var data = reader.ReadFusionSerializable<ColorData>();
                if (NetworkInfo.IsServer && isServerHandled)
                {
                    using var message = FusionMessage.ModuleCreate<ColorHandler>(bytes);
                    MessageSender.BroadcastMessage(NetworkChannel.Reliable, message);
                }
                else
                {
                    var bodyLogColorData = new BodyLogColorData(
                        data.LineColor,
                        data.DialColor,
                        data.TickColor,
                        data.GizmoColor,
                        data.RingColor,
                        data.CoreParticleColor,
                        data.BaseLogColor,
                        data.PreviewMeshColor,
                        data.BaseSkinTypeId
                    );

                #if DEBUG
                    MelonLogger.Msg($"Player with id {data.PlayerId} sent color data:");
                    MelonLogger.Msg($"Base Skin Type id: {data.BaseSkinTypeId}");
                    MelonLogger.Msg($"Gizmo Ball Color: {data.GizmoColor.gizmoBallColor.ToString()}");
                    MelonLogger.Msg($"Gizmo Emission Color: {data.GizmoColor.gizmoBallEmission.ToString()}");
                    MelonLogger.Msg($"Ring Color: {data.RingColor.ToString()}");
                    MelonLogger.Msg($"Core Particle Color: {data.CoreParticleColor.ToString()}");
                    MelonLogger.Msg($"Base Log Color: {data.BaseLogColor.ToString()}");
                    MelonLogger.Msg($"Line Color: {data.LineColor.ToString()}");
                    MelonLogger.Msg($"Tick Color: {data.TickColor[0].ToString()}");
                    MelonLogger.Msg($"Dial Color Emission Base: {data.DialColor[0].emissionBase.ToString()}");
                    MelonLogger.Msg($"Dial Color Emission Color: {data.DialColor[0].emissionColor.ToString()}");
                    MelonLogger.Msg($"Preview Mesh Edge Color: {data.PreviewMeshColor.hologramEdgeColor.ToString()}");
                    MelonLogger.Msg($"Preview Mesh Emission Color: {data.PreviewMeshColor.hologramEmissionColor.ToString()}");
                #endif

                    foreach (var playerRep in PlayerRepManager.PlayerReps)
                    {
                        if (playerRep.PlayerId.SmallId == data.PlayerId)
                        {
                            var device = playerRep.pullCord;
                            
                            // Bullshit solution but it works
                            var timer = new Timer(1000);
                            timer.AutoReset = false;
                            timer.Elapsed += (sender, args) =>
                            {
                                device.ApplyColorsFromData(bodyLogColorData);
                            };

                            FusionUserCacheManager.AddOrUpdate(playerRep.PlayerId.LongId.ToString(), bodyLogColorData);
                        }
                    }
                }
            }
        }
    }
}