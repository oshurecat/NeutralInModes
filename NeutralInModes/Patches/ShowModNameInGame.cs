using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using Epic.OnlineServices.TitleStorage;
using HarmonyLib;
using TMPro;
using UnityEngine;
using static Il2CppMono.Security.X509.X520;
using static UnityEngine.UI.Button;
using Object = UnityEngine.Object;

//RHRより
namespace NeutralInModes.Patches
{
    public static class AlwaysPatch
    {
        [HarmonyPatch(typeof(PingTracker), nameof(PingTracker.Update))]
        public static class PingTrackerPatch
        {
            public static void Postfix(PingTracker __instance)
            {
                string CustomText = "";
                __instance.text.alignment = TextAlignmentOptions.TopRight;
                __instance.text.text = $"{TitleTextPatch.NIMNAMEPlay}\n<size=100%>{ __instance.text.text}</size>\n<size=130%>{CustomText}</size>";

                /*
                    var amongUsLogo = GameObject.Find("bannerLogo_AmongUs");
                    /*=============このMODの名前=====================*/
                /*
                var credentials = UnityEngine.Object.Instantiate<TMPro.TextMeshPro>(__instance.text);
                    credentials.transform.position = new Vector3(0, 334f, 0);
                    string credentialsText = "";
                    credentialsText += "";
                    credentials.SetText(credentialsText);

                    credentials.alignment = TMPro.TextAlignmentOptions.Center;
                    credentials.fontSize *= 0.9f;

                    var NIMName = UnityEngine.Object.Instantiate(credentials);
                    NIMName.transform.position = new Vector3(2.9f, 2.8f, -5);
                    NIMName.transform.localScale = new Vector3(1f, 1f, 10);
                    NIMName.SetText(string.Format(NIMNAME));

                    credentials.transform.SetParent(amongUsLogo.transform);
                    NIMName.transform.SetParent(amongUsLogo.transform);
                */                
            }
        }
    }
}