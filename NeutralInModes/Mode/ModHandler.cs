using AmongUs.GameOptions;
using HarmonyLib;
using NeutralInModes.Patches;
using UnityEngine;
using static NeutralInModes.Patches.EndGamePatch;

namespace NeutralInModes.Mode;

//仮設置[HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.CloseDoorsOfType))]

public enum ModeID
{
    No,
    Default,
    VanillaHnS
}
public static class ModeHandler
{
    public static ModeID thismode;
    public static void ClearAndReload()
    {
        
    }
}