using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AmongUs.GameOptions;
using BepInEx.Configuration;
using HarmonyLib;
using Hazel;
using NeutralInModes.Mode;
using NeutralInModes.Patches;
using UnityEngine;
using UnityEngine.Events;

namespace NeutralInModes.Modules;

public enum CustomOptionType
{
    Generic,
    Impostor,
    Neutral,
    Crewmate,
    Empty // 使用されない
}

public class CustomOption
{
    public static List<CustomOption> options = new();
    public static int preset = 0;

    public int id;
    public bool isSHROn;
    public CustomOptionType type;
    public string name;
    public string format;
    public System.Object[] selections;

    public int defaultSelection;
    public ConfigEntry<int> entry;
    public int HostSelection;
    public int ClientSelection;
    public int ClientSelectedSelection;
    public int selection
    {
        set
        {
            if (AmongUsClient.Instance == null || AmongUsClient.Instance.AmHost)
            {
                    ClientSelectedSelection = value;
                
            }
            else
            {
                HostSelection = value;
            }
        }
    }
}