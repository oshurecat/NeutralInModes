using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using HarmonyLib;
using UnityEngine;
using NeutralInModes.Helpers;
using TMPro;
using System.Collections.Generic;

namespace NeutralInModes.Patches
{
    [HarmonyPatch]
    public static class EndGamePatch
    {
        [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnGameEnd))]
        public class GameEnd
        {
            public static void Postfix()
            {
                StartGamePatch.GameStart.IsStart = false;
            }
        }
        [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.CheckEndCriteria))]
        class CheckGameEndPatch
        {
            public static bool Prefix(ShipStatus __instance)
            {
                return true;
            }
        }
        /*===================================================================-*/

        [HarmonyPatch(typeof(EndGameManager), nameof(EndGameManager.SetEverythingUp))]
        public class EndGameManagerSetUpPatch
        {
            public static bool IsHaison = false;
            public static TMPro.TMP_Text HaisonText;
            [HarmonyPatch(typeof(EndGameNavigation), nameof(EndGameNavigation.ShowProgression))]
            public class ShowProgressionPatch
            {
                public static void Prefix()
                {
                    if (HaisonText != null)
                    {
                        HaisonText.gameObject.SetActive(false);
                    }
                }
            }
            public static void Postfix(EndGameManager __instance)
            {
                GameObject bonusTextObject = UnityEngine.Object.Instantiate(__instance.WinText.gameObject);
                bonusTextObject.transform.position = new Vector3(__instance.WinText.transform.position.x, __instance.WinText.transform.position.y, __instance.WinText.transform.position.z);
                bonusTextObject.transform.localScale = new Vector3(2f, 2f, 1f);
                HaisonText = bonusTextObject.GetComponent<TMPro.TMP_Text>();
                HaisonText.text = "";

                if (IsHaison)
                {
                    HaisonText.text = string.Format("廃村");
                    __instance.BackgroundBar.material.SetColor("_Color", Color.gray);
                    HaisonText.color = Color.gray;
                }
            }
        }
    }
}