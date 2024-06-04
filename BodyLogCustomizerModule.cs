using System;
using BodyLogCustomizer.Data;
using BodyLogCustomizer.Data.Messages;
using LabFusion.Network;
using LabFusion.SDK.Modules;
using MelonLoader;
using UnityEngine;

namespace BodyLogCustomizer;

public static class ModuleInfo
{
    public const string Name = "BodyLogCustomizer"; 
    public const string Version = "1.0.0"; 
    public const string Author = "BuggedChecker"; 
    public const string Abbreviation = null; 
    public const bool AutoRegister = true; 
    public const ConsoleColor Color = ConsoleColor.Cyan;
}

public class FusionModule : Module
{
    public static FusionModule Instance { get; private set; }
        
    public override void OnModuleLoaded()
    {
        Instance = this;
    }

    public void SendBodyLogColorData(BodyLogColorData bodyLogColorData)
    {
        if (!BodyLogCustomizer.ReplicateOnServer.Value)
            return;
        if (!NetworkInfo.HasServer)
            return;
        var lineColor = bodyLogColorData.lineColor;
        var dialColor = bodyLogColorData.dialColor;
        var tickColor = bodyLogColorData.tickColor;
        var gizmoBallColor = bodyLogColorData.gizmoBallColor;
        var ringColor = bodyLogColorData.ringColor;
        var coreParticleColor = bodyLogColorData.coreParticleColor;
        var baseLogColor = bodyLogColorData.baseLogColor;
        var previewMeshColor = bodyLogColorData.previewMeshColor;
        var baseSkinTypeId = (int)bodyLogColorData.baseSkinType;
        
        using var writer = FusionWriter.Create();
        using var data = ColorDataFusion.ColorData.Create(lineColor, dialColor, tickColor, gizmoBallColor, ringColor, coreParticleColor, baseLogColor, previewMeshColor, baseSkinTypeId);
        writer.Write(data);

        using var message = FusionMessage.ModuleCreate<ColorDataFusion.ColorHandler>(writer);
        MessageSender.SendToServer(NetworkChannel.Reliable, message);
    }
}