//SuperNewRolesより
using AmongUs.GameOptions;
using HarmonyLib;
using NeutralInModes.Patches;
using UnityEngine;
using static NeutralInModes.Patches.EndGamePatch;

namespace NeutralInModes.Mode;

public enum ModeId
{
    No,
    Default,
    VanillaHns
}

public static class ModeHandler
{
    public static ModeId thisMode;
    public static void ClearAndReload()
    {
        if (IsMode(ModeId.Default, false))
        {
            thisMode = ModeId.Default;
        }
    }

        public static bool IsMode(ModeId mode, bool IsChache = true)
    {
        if (mode is ModeId.VanillaHns)
            return GameOptionsManager.Instance.currentGameMode == GameModes.HideNSeek;
        if (AmongUsClient.Instance.NetworkMode == NetworkModes.FreePlay || !PlayerControlHepler.IsMod(AmongUsClient.Instance.HostId))
            return mode is ModeId.Default;
        if (IsChache)
            return mode == thisMode;
        if (mode is ModeId.Default)
            return !ModeSetting.GetBool();
    }
}