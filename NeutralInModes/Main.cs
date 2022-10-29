using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using HarmonyLib;

namespace NeutralInModes
{
    [BepInPlugin(Id, "NeutralInModes", VersionString)]
    [BepInProcess("Among Us.exe")]
    public class NeutralInModesPlugin : BasePlugin
    {
        public const string Id = "jp.oshurecat.NeutralInModes";
        // Versionに数字以外は含めないこと
        public const string VersionString = "0.0.2";

        public static System.Version Version = System.Version.Parse(VersionString);
        internal static BepInEx.Logging.ManualLogSource Logger;

        public Harmony Harmony { get; } = new(Id);
        public static NeutralInModesPlugin Instance;
        
        public static int optionsPage = 0;

        public override void Load()
        {
            Logger = Log;
            Instance = this;
            Harmony.PatchAll();
        }
    }
}