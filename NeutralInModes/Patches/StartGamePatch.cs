using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using HarmonyLib;
using UnityEngine;
using NeutralInModes.Helpers;
using TMPro;

namespace NeutralInModes.Patches
{
    [HarmonyPatch]
    public static class StartGamePatch
    {
        [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.Start))]
        public class GameStart
        {
            public static bool IsStart;
            public static void Postfix()
            {
                IsStart = true;
            }
        }
    }
}