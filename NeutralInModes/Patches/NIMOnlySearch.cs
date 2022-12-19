using AmongUs.Data;
using AmongUs.GameOptions;
using HarmonyLib;
using Hazel;
using InnerNet;
using NeutralInModes;
using UnityEngine;
using UnityEngine.Events;

namespace NeutralInModes.Patches;

public static class NIMOnlySearch
{
    public const string FilterText = "NIM";
    [HarmonyPatch(typeof(FilterTagsMenu), nameof(FilterTagsMenu.ChooseOption))]
    public static class FilterTagsMenuChooseOptionPatch
    {
        public static void Postfix(FilterTagsMenu __instance, ChatLanguageButton button, string filter)
        {
            if (__instance.targetOpts.FilterTags.Contains(FilterText))
            {
                if (filter == FilterText)
                {
                    __instance.targetOpts.FilterTags = new();
                    __instance.targetOpts.FilterTags.Add(FilterText);
                    foreach (var btn in __instance.controllerSelectable)
                    {
                        btn.GetComponent<ChatLanguageButton>().SetSelected(false);
                    }
                    button.SetSelected(true);
                }
                else
                {
                    __instance.targetOpts.FilterTags.Remove(FilterText);
                    foreach (var btn in __instance.controllerSelectable)
                    {
                        ChatLanguageButton LangBtn = btn.GetComponent<ChatLanguageButton>();
                        if (LangBtn.Text.text == FilterText)
                            LangBtn.SetSelected(false);
                    }
                }
                __instance.UpdateButtonText();
            }
        }
    }
}