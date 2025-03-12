using System;
using BodyLogCustomizer.Data;
using BodyLogCustomizer.Data.Messages;
using LabFusion.Network;
using LabFusion.SDK.Modules;

namespace BodyLogCustomizer;

public class FusionModule : Module
{
    public static FusionModule Instance { get; private set; }

    public override string Name => "BodyLogCustomizer";

    public override string Author => "BuggedChecker";

    public override Version Version => new(1, 1, 0);

    public override ConsoleColor Color => ConsoleColor.Cyan;

    protected override void OnModuleRegistered()
    {
        Instance = this;
    }

    protected override void OnModuleUnregistered()
    {
        Instance = null;
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