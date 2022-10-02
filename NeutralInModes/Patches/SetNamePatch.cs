using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using HarmonyLib;
using UnityEngine;
using NeutralInModes.Helpers;
using System.Collections.Generic;
using Il2CppSystem;
using System.Drawing;

namespace NeutralInModes.Patches
{
    public static class SetNamePatch
    {
        /*
        public static Dictionary<int, string> PlayerName = new();
        public static string GetPlayerName(this PlayerControl p)
        {
            //return p.Data.PlayerName;
            var playerid = p.PlayerId;
            if (PlayerName.ContainsKey(playerid))//もしPlayerNameを定義してる人のIdだった場合そのままPlayerNameを返す
            {
                return PlayerName[playerid];
            }
            else//もしPlayerNameを定義してない人のIdだった場合Playerの名前を定義してそれを返す
            {
                PlayerName[playerid] = p.Data.PlayerName;
                return PlayerName[playerid];
            }
        }
        
        public static void PlayerSetName()
        {
            string RoleName = "";
            foreach (PlayerControl p in CachedPlayer.AllPlayers)
            {
                if (p.IsCrew())
                {
                    RoleName = "<size=75%><color=#00ffff>クルーメイト</size>\n";
                    p.SetName(RoleName + p.GetPlayerName() + "</color>");
                }
                else
                {
                    RoleName = "<size=75%><color=#ff0000>インポスター</color></size>\n";
                    p.SetName(RoleName + p.GetPlayerName());
                }
            }
        }
        */
        //いる？
    }
}