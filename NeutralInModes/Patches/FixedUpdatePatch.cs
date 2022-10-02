using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using HarmonyLib;
using UnityEngine;
using NeutralInModes.Helpers;
using System.Collections.Generic;
using Il2CppSystem;
using NeutralInModes.Patches;
using UnityEngine.UI;
using Sentry.Internal;

namespace NeutralInModes.Patches
{
    public static class FixedUpdate
    {
        
        [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
        public static class Update
        {
            public static void Postfix(PlayerControl __instance)
            {
                //SetNamePatch.PlayerSetName();
            }
        }
        
    }
}