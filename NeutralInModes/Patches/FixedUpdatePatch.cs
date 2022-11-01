using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using HarmonyLib;
using UnityEngine;
using NeutralInModes;
using System.Collections.Generic;
using Il2CppSystem;
using NeutralInModes.Patches;
using UnityEngine.UI;
using Sentry.Internal;
using UnityEngine.EventSystems;
using UnityEngine.Networking.Types;

namespace NeutralInModes.Patches
{
    public static class FixedUpdate
    {

        [HarmonyPatch(typeof(HudManager), nameof(HudManager.Start))]
        public static class Update
        {
            public static void Postfix(PlayerControl __instance)
            {

            }
        }
        [HarmonyPatch(typeof(ControllerManager), nameof(ControllerManager.Update))]
        public class Controls
        {
            public static void Postfix(ControllerManager __instance)
            {
                if (AmongUsClient.Instance.GameState == AmongUsClient.GameStates.Started)
                {
                    if (Input.GetKeyDown(KeyCode.RightShift) && Input.GetKey(KeyCode.H))
                    {
                        EndGamePatch.EndGameManagerSetUpPatch.IsHaison = true;
                        ShipStatus.RpcEndGame(GameOverReason.HumansByTask, false);
                    }
                }
            }
        }

    }
}