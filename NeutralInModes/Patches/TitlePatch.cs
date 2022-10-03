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
    public static class TitleTextPatch
    {
        public static string NIMNAME = $"<size=130%><color=#0000cd>NeutralInModes</color>v{NeutralInModesPlugin.Version}</size>";
        public static string NIMNAMEPlay = $"<size=125%><color=#0000cd>NeutralInModes\n</color>v{NeutralInModesPlugin.Version}</size>";

        [HarmonyPatch(typeof(VersionShower), nameof(VersionShower.Start))]
        private static class VersionShowerPatch
        {
            static void Postfix(VersionShower __instance)
            {

                var amongUsLogo = GameObject.Find("bannerLogo_AmongUs");
                if (amongUsLogo == null) return;
                var credentials = UnityEngine.Object.Instantiate<TMPro.TextMeshPro>(__instance.text);
                credentials.transform.position = new Vector3(0, 334f, 0);
                string credentialsText = "";
                credentialsText += "";
                credentials.SetText(credentialsText);

                credentials.alignment = TMPro.TextAlignmentOptions.Center;
                credentials.fontSize *= 0.9f;

                var NIMName = UnityEngine.Object.Instantiate(credentials);
                NIMName.transform.position = new Vector3(0, -0.2f, 0);
                NIMName.SetText(string.Format("<size=125%><color=#00ffff>Authors : oshurecat</color></size>\n" + NIMNAME));

                credentials.transform.SetParent(amongUsLogo.transform);
                NIMName.transform.SetParent(amongUsLogo.transform);
            }
        }
        public static class TitlePatch
        {

            [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start))]
            public static class TitleLogoPatch
            {
                public static void Postfix(MainMenuManager __instance)
                {

                    var AmongUsLogo = GameObject.Find("bannerLogo_AmongUs");//ロゴ検出
                    AmongUsLogo.transform.localScale = new Vector3(0.7f, 0.7f, 1f);
                    AmongUsLogo.transform.position = new Vector3(0f, 2f, 0f);
                    var TitleLogo = new GameObject("bannerLogo");
                    TitleLogo.transform.position = Vector3.up;
                    var renderer = TitleLogo.AddComponent<SpriteRenderer>();
                    renderer.sprite = Helpers.Helpers.LoadSpriteFromImages("NeutralInModes.Images.NIMimage.png", 250f);
                    DestroyableSingleton<ModManager>.Instance.ShowModStamp();//モッドスタンプ召喚
                }

            }
        }
    }
}