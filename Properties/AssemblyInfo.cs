using System.Resources;
using System.Reflection;
using System.Runtime.InteropServices;
using BodyLogCustomizer;
using MelonLoader;

[assembly: AssemblyTitle(BodyLogCustomizer.BuildInfo.Name)]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany(BodyLogCustomizer.BuildInfo.Company)]
[assembly: AssemblyProduct(BodyLogCustomizer.BuildInfo.Name)]
[assembly: AssemblyCopyright("Created by " + BodyLogCustomizer.BuildInfo.Author)]
[assembly: AssemblyTrademark(BodyLogCustomizer.BuildInfo.Company)]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
//[assembly: Guid("")]
[assembly: AssemblyVersion(BodyLogCustomizer.BuildInfo.Version)]
[assembly: AssemblyFileVersion(BodyLogCustomizer.BuildInfo.Version)]
[assembly: NeutralResourcesLanguage("en")]
[assembly: MelonInfo(typeof(BodyLogCustomizer.BodyLogCustomizer), BodyLogCustomizer.BuildInfo.Name, BodyLogCustomizer.BuildInfo.Version, BodyLogCustomizer.BuildInfo.Author, BodyLogCustomizer.BuildInfo.DownloadLink)]
[assembly: LabFusion.SDK.Modules.ModuleInfo(typeof(FusionModule), ModuleInfo.Name, ModuleInfo.Version, ModuleInfo.Author, ModuleInfo.Abbreviation, ModuleInfo.AutoRegister, ModuleInfo.Color)]


// Create and Setup a MelonModGame to mark a Mod as Universal or Compatible with specific Games.
// If no MelonModGameAttribute is found or any of the Values for any MelonModGame on the Mod is null or empty it will be assumed the Mod is Universal.
// Values for MelonModGame can be found in the Game's app.info file or printed at the top of every log directly beneath the Unity version.
[assembly: MelonGame("Stress Level Zero", "BONELAB")]