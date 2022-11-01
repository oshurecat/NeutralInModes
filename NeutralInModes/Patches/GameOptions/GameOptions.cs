using System.Collections.Generic;
using UnityEngine;
using BepInEx.Configuration;
using System;
using System.Linq;
using HarmonyLib;
using Hazel;
using System.Reflection;
using System.Text;
using NeutralInModes.Patches;
using NeutralInModes;

namespace NeutralInModes
{
    public class GameOptions
    {
        [HarmonyPatch(typeof(GameOptionsMenu), nameof(GameOptionsMenu.Start))]
        class GameOptionsMenuStartPatch
        {
            [HarmonyPatch(typeof(GameOptionsData), nameof(GameOptionsData.SetRecommendations))]
            public static class ChangeRecommendedSettingPatch
            {
                public static bool Prefix(GameOptionsData __instance, int numPlayers, GameModes modes)
                {
                    numPlayers = Mathf.Clamp(numPlayers, 4, 15);
                    __instance.NumImpostors = GameOptionsData.RecommendedImpostors[numPlayers];
                    __instance.ConfirmImpostor = false;
                    __instance.EmergencyCooldown = 30;
                    __instance.PlayerSpeedMod = 1.25f;
                    __instance.CrewLightMod = 1f;
                    __instance.ImpostorLightMod = 2;
                    __instance.KillCooldown = 35;
                    __instance.VisualTasks = false;
                    __instance.NumEmergencyMeetings = 1;
                    __instance.TaskBarMode = TaskBarMode.MeetingOnly;
                    __instance.KillDistance = 0;
                    __instance.DiscussionTime = 0;
                    __instance.VotingTime = 180;
                    __instance.isDefaults = true;
                    __instance.AnonymousVotes = true;
                    __instance.NumCommonTasks = 4;
                    __instance.NumLongTasks = 3;
                    __instance.NumShortTasks = 5;


                    __instance.RoleOptions.ShapeshifterCooldown = 10f;
                    __instance.RoleOptions.ShapeshifterDuration = 20f;
                    __instance.RoleOptions.ShapeshifterLeaveSkin = true;
                    __instance.RoleOptions.ScientistCooldown = 10;
                    __instance.RoleOptions.ScientistBatteryCharge = 3f;
                    __instance.RoleOptions.GuardianAngelCooldown = 60f;
                    __instance.RoleOptions.ProtectionDurationSeconds = 10f;
                    __instance.RoleOptions.ImpostorsCanSeeProtect = true;
                    __instance.RoleOptions.EngineerCooldown = 10f;
                    __instance.RoleOptions.EngineerInVentMaxTime = 8f;

                    return false;
                }
            }
            public static void Postfix(GameOptionsMenu __instance)
            { 
                var PlayerSpeedModOption = __instance.Children.FirstOrDefault(x => x.name == "PlayerSpeed").TryCast<NumberOption>();//上限解放
                if (PlayerSpeedModOption != null) PlayerSpeedModOption.ValidRange = new FloatRange(-20f, 20f);

                
                var killCoolOption = __instance.Children.FirstOrDefault(x => x.name == "KillCooldown").TryCast<NumberOption>();
                if (killCoolOption != null) killCoolOption.ValidRange = new FloatRange(0.1f, 100f);

                var commonTasksOption = __instance.Children.FirstOrDefault(x => x.name == "NumCommonTasks").TryCast<NumberOption>();
                if (commonTasksOption != null) commonTasksOption.ValidRange = new FloatRange(0f, 100f);

                var shortTasksOption = __instance.Children.FirstOrDefault(x => x.name == "NumShortTasks").TryCast<NumberOption>();
                if (shortTasksOption != null) shortTasksOption.ValidRange = new FloatRange(0f, 100f);

                var longTasksOption = __instance.Children.FirstOrDefault(x => x.name == "NumLongTasks").TryCast<NumberOption>();
                if (longTasksOption != null) longTasksOption.ValidRange = new FloatRange(0f, 100f);

                var CrewLightModOption = __instance.Children.FirstOrDefault(x => x.name == "CrewmateVision").TryCast<NumberOption>();
                if (CrewLightModOption != null) CrewLightModOption.ValidRange = new FloatRange(-10f, 10f);

                var ImpostorLightModOption = __instance.Children.FirstOrDefault(x => x.name == "ImpostorVision").TryCast<NumberOption>();
                if (ImpostorLightModOption != null) ImpostorLightModOption.ValidRange = new FloatRange(-10f, 10f);

                var MeetingButtonCoolDownOption = __instance.Children.FirstOrDefault(x => x.name == "EmergencyCooldown").TryCast<NumberOption>();
                if (MeetingButtonCoolDownOption != null) MeetingButtonCoolDownOption.ValidRange = new FloatRange(-2000f, 1000f);

                var MeetingButtonCountOption = __instance.Children.FirstOrDefault(x => x.name == "EmergencyMeetings").TryCast<NumberOption>();
                if (MeetingButtonCountOption != null) MeetingButtonCountOption.ValidRange = new FloatRange(-200f, 100f);

                var VotingTimeOption = __instance.Children.FirstOrDefault(x => x.name == "VotingTime").TryCast<NumberOption>();
                if (VotingTimeOption != null) VotingTimeOption.ValidRange = new FloatRange(-2000f, 1000f);

                var DiscussionTimeOption = __instance.Children.FirstOrDefault(x => x.name == "DiscussionTime").TryCast<NumberOption>();
                if (DiscussionTimeOption != null) DiscussionTimeOption.ValidRange = new FloatRange(-2000f, 1000f);

            }
        }
    }
}