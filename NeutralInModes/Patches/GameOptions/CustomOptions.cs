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
    //TheOtherRolesAUさん本当にありがとうございます！！！！！！！！！！！！！！！！！
    public class CustomOption
    {
        public enum CustomOptionType
        {
            General,
            Impostor,
            Neutral,
            Crewmate,
            Modifier
        }

        public static List<CustomOption> options = new List<CustomOption>();
        public static int preset = 0;

        public int id;
        public string name;
        public System.Object[] selections;

        public int defaultSelection;
        public ConfigEntry<int> entry;
        public int selection;
        public OptionBehaviour optionBehaviour;
        public CustomOption parent;
        public bool isHeader;
        public CustomOptionType type;

        // Option creation

        public CustomOption(int id, CustomOptionType type, string name, System.Object[] selections, System.Object defaultValue, CustomOption parent, bool isHeader)
        {
            this.id = id;
            this.name = parent == null ? name : "-- " + name;
            this.selections = selections;
            int index = Array.IndexOf(selections, defaultValue);
            this.defaultSelection = index >= 0 ? index : 0;
            this.parent = parent;
            this.isHeader = isHeader;
            this.type = type;
            selection = 0;
            if (id != 0)
            {
                entry = NeutralInModesPlugin.Instance.Config.Bind($"プリセット {preset}", id.ToString(), defaultSelection);
                selection = Mathf.Clamp(entry.Value, 0, selections.Length - 1);
            }
            options.Add(this);
        }

        public static CustomOption Create(int id, CustomOptionType type, string name, string[] selections, CustomOption parent = null, bool isHeader = false)
        {
            return new CustomOption(id, type, name, selections, "", parent, isHeader);
        }

        public static CustomOption Create(int id, CustomOptionType type, string name, float defaultValue, float min, float max, float step, CustomOption parent = null, bool isHeader = false)
        {
            List<object> selections = new();
            for (float s = min; s <= max; s += step)
                selections.Add(s);
            return new CustomOption(id, type, name, selections.ToArray(), defaultValue, parent, isHeader);
        }

        public static CustomOption Create(int id, CustomOptionType type, string name, bool defaultValue, CustomOption parent = null, bool isHeader = false)
        {
            return new CustomOption(id, type, name, new string[] { "<color=#ff0000>オフ</color>", "<color=#00ffff>オン</color>" }, defaultValue ? "オン" : "<color=#ff0000>オフ</color>", parent, isHeader);
        }

        // Static behaviour

        public static void switchPreset(int newPreset)
        {
            CustomOption.preset = newPreset;
            foreach (CustomOption option in CustomOption.options)
            {
                if (option.id == 0) continue;

                option.entry = NeutralInModesPlugin.Instance.Config.Bind($"プリセット {preset}", option.id.ToString(), option.defaultSelection);
                option.selection = Mathf.Clamp(option.entry.Value, 0, option.selections.Length - 1);
                if (option.optionBehaviour != null && option.optionBehaviour is StringOption stringOption)
                {
                    stringOption.oldValue = stringOption.Value = option.selection;
                    stringOption.ValueText.text = option.selections[option.selection].ToString();
                }
            }
        }
        /*
        public static void ShareOptionSelections()
        {
            if (CachedPlayer.AllPlayers.Count <= 1 || AmongUsClient.Instance!.AmHost == false && CachedPlayer.LocalPlayer.PlayerControl == null) return;

            var optionsList = new List<CustomOption>(CustomOption.options);
            while (optionsList.Any())
            {
                byte amount = (byte)Math.Min(optionsList.Count, 20);
                var writer = AmongUsClient.Instance!.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.ShareOptions, SendOption.Reliable, -1);
                writer.Write(amount);
                for (int i = 0; i < amount; i++)
                {
                    var option = optionsList[0];
                    optionsList.RemoveAt(0);
                    writer.WritePacked((uint)option.id);
                    writer.WritePacked(Convert.ToUInt32(option.selection));
                }
                AmongUsClient.Instance.FinishRpcImmediately(writer);
            }
        }
        */

        // Getter

        public int GetSelection()
        {
            return selection;
        }

        public bool GetBool()
        {
            return selection > 0;
        }

        public float GetFloat()
        {
            return (float)selections[selection];
        }


        public int GetQuantity()
        {
            return selection + 1;
        }
        public virtual string GetString()
        {
            string text = selections[selection].ToString();
            return text;
        }

        // Option changes

        public void updateSelection(int newSelection)
        {
            selection = Mathf.Clamp((newSelection + selections.Length) % selections.Length, 0, selections.Length - 1);
            if (optionBehaviour != null && optionBehaviour is StringOption stringOption)
            {
                stringOption.oldValue = stringOption.Value = selection;
                stringOption.ValueText.text = selections[selection].ToString();

                if (AmongUsClient.Instance?.AmHost == true && CachedPlayer.LocalPlayer.PlayerControl)
                {
                    if (id == 0) switchPreset(selection); // Switch presets
                    else if (entry != null) entry.Value = selection; // Save selection to config

                    //ShareOptionSelections();// Share all selections
                }
            }
        }
    }

    [HarmonyPatch(typeof(GameOptionsMenu), nameof(GameOptionsMenu.Start))]
    class GameOptionsMenuStartPatch
    {
        public static void Postfix(GameOptionsMenu __instance)
        {
            if (GameObject.Find("NIMSettings") != null)
            { // Settings setup has already been performed, fixing the title of the tab and returning
                GameObject.Find("NIMSettings").transform.FindChild("GameGroup").FindChild("Text").GetComponent<TMPro.TextMeshPro>().SetText("<color=#fcc800>N I M </color>の設定");
                return;
            }
            if (GameObject.Find("ImpostorSettings") != null)
            {
                GameObject.Find("ImpostorSettings").transform.FindChild("GameGroup").FindChild("Text").GetComponent<TMPro.TextMeshPro>().SetText("<color=red>インポスター</color>の設定");
                return;
            }
            if (GameObject.Find("NeutralSettings") != null)
            {
                GameObject.Find("NeutralSettings").transform.FindChild("GameGroup").FindChild("Text").GetComponent<TMPro.TextMeshPro>().SetText("<color=#c0c0c0>第三陣営</color>の設定");
                return;
            }
            if (GameObject.Find("CrewmateSettings") != null)
            {
                GameObject.Find("CrewmateSettings").transform.FindChild("GameGroup").FindChild("Text").GetComponent<TMPro.TextMeshPro>().SetText("<color=#00ffff>クルーメイト</color>の設定");
                return;
            }
            if (GameObject.Find("ModifierSettings") != null)
            {
                GameObject.Find("ModifierSettings").transform.FindChild("GameGroup").FindChild("Text").GetComponent<TMPro.TextMeshPro>().SetText("<color=#47FF9C>両陣営可能役職</color>の設定");
                return;
            }


            var template = UnityEngine.Object.FindObjectsOfType<StringOption>().FirstOrDefault();
            if (template == null) return;
            var gameSettings = GameObject.Find("Game Settings");
            var gameSettingMenu = UnityEngine.Object.FindObjectsOfType<GameSettingMenu>().FirstOrDefault();

            var NIMSettings = UnityEngine.Object.Instantiate(gameSettings, gameSettings.transform.parent);
            var NIMMenu = NIMSettings.transform.FindChild("GameGroup").FindChild("SliderInner").GetComponent<GameOptionsMenu>();
            NIMSettings.name = "NIMSettings";

            var impostorSettings = UnityEngine.Object.Instantiate(gameSettings, gameSettings.transform.parent);
            var impostorMenu = impostorSettings.transform.FindChild("GameGroup").FindChild("SliderInner").GetComponent<GameOptionsMenu>();
            impostorSettings.name = "ImpostorSettings";

            var neutralSettings = UnityEngine.Object.Instantiate(gameSettings, gameSettings.transform.parent);
            var neutralMenu = neutralSettings.transform.FindChild("GameGroup").FindChild("SliderInner").GetComponent<GameOptionsMenu>();
            neutralSettings.name = "NeutralSettings";

            var crewmateSettings = UnityEngine.Object.Instantiate(gameSettings, gameSettings.transform.parent);
            var crewmateMenu = crewmateSettings.transform.FindChild("GameGroup").FindChild("SliderInner").GetComponent<GameOptionsMenu>();
            crewmateSettings.name = "CrewmateSettings";

            var modifierSettings = UnityEngine.Object.Instantiate(gameSettings, gameSettings.transform.parent);
            var modifierMenu = crewmateSettings.transform.FindChild("GameGroup").FindChild("SliderInner").GetComponent<GameOptionsMenu>();
            modifierSettings.name = "ModifierSettings";

            var roleTab = GameObject.Find("RoleTab");
            var gameTab = GameObject.Find("GameTab");

            var NIMTab = UnityEngine.Object.Instantiate(roleTab, roleTab.transform.parent);
            var NIMTabHighlight = NIMTab.transform.FindChild("Hat Button").FindChild("Tab Background").GetComponent<SpriteRenderer>();
            NIMTab.transform.FindChild("Hat Button").FindChild("Icon").GetComponent<SpriteRenderer>().sprite = Helpers.LoadSpriteFromResources("NeutralInModes.Resources.NIMTabIcon.png", 100f);

            var crewmateTab = UnityEngine.Object.Instantiate(roleTab, NIMTab.transform);
            var crewmateTabHighlight = crewmateTab.transform.FindChild("Hat Button").FindChild("Tab Background").GetComponent<SpriteRenderer>();
            crewmateTab.transform.FindChild("Hat Button").FindChild("Icon").GetComponent<SpriteRenderer>().sprite = Helpers.LoadSpriteFromResources("NeutralInModes.Resources.NIMTabIconCrewmate.png", 100f);
            crewmateTab.name = "CrewmateTab";

            var impostorTab = UnityEngine.Object.Instantiate(roleTab, crewmateTab.transform);
            var impostorTabHighlight = impostorTab.transform.FindChild("Hat Button").FindChild("Tab Background").GetComponent<SpriteRenderer>();
            impostorTab.transform.FindChild("Hat Button").FindChild("Icon").GetComponent<SpriteRenderer>().sprite = Helpers.LoadSpriteFromResources("NeutralInModes.Resources.NIMTabIconImpostor.png", 100f);
            impostorTab.name = "ImpostorTab";

            var neutralTab = UnityEngine.Object.Instantiate(roleTab, impostorTab.transform);
            var neutralTabHighlight = neutralTab.transform.FindChild("Hat Button").FindChild("Tab Background").GetComponent<SpriteRenderer>();
            neutralTab.transform.FindChild("Hat Button").FindChild("Icon").GetComponent<SpriteRenderer>().sprite = Helpers.LoadSpriteFromResources("NeutralInModes.Resources.NIMTabIconNeutral.png", 100f);
            neutralTab.name = "NeutralTab";

            var modifierTab = UnityEngine.Object.Instantiate(roleTab, neutralTab.transform);
            var modifierTabHighlight = modifierTab.transform.FindChild("Hat Button").FindChild("Tab Background").GetComponent<SpriteRenderer>();
            modifierTab.transform.FindChild("Hat Button").FindChild("Icon").GetComponent<SpriteRenderer>().sprite = Helpers.LoadSpriteFromResources("NeutralInModes.Resources.NIMTabIconModifier.png", 100f);
            modifierTab.name = "ModifierTab";


            // Position of Tab Icons
            gameTab.transform.position += Vector3.left * 3f;
            roleTab.transform.position += Vector3.left * 3f;
            NIMTab.transform.position += Vector3.left * 2f;
            crewmateTab.transform.localPosition = Vector3.right * 1f;
            impostorTab.transform.localPosition = Vector3.right * 1f;
            neutralTab.transform.localPosition = Vector3.right * 1f;
            modifierTab.transform.localPosition = Vector3.right * 1f;

            var tabs = new GameObject[] { gameTab, roleTab, NIMTab, crewmateTab, impostorTab, neutralTab,modifierTab};
            for (int i = 0; i < tabs.Length; i++)
            {
                var button = tabs[i].GetComponentInChildren<PassiveButton>();
                if (button == null) continue;
                int copiedIndex = i;
                button.OnClick = new UnityEngine.UI.Button.ButtonClickedEvent();
                button.OnClick.AddListener((System.Action)(() => {
                    gameSettingMenu.RegularGameSettings.SetActive(false);
                    gameSettingMenu.RolesSettings.gameObject.SetActive(false);
                    NIMSettings.gameObject.SetActive(false);
                    impostorSettings.gameObject.SetActive(false);
                    neutralSettings.gameObject.SetActive(false);
                    crewmateSettings.gameObject.SetActive(false);
                    modifierSettings.gameObject.SetActive(false);
                    gameSettingMenu.GameSettingsHightlight.enabled = false;
                    gameSettingMenu.RolesSettingsHightlight.enabled = false;
                    NIMTabHighlight.enabled = false;
                    impostorTabHighlight.enabled = false;
                    neutralTabHighlight.enabled = false;
                    crewmateTabHighlight.enabled = false;
                    modifierTabHighlight.enabled = false;
                    if (copiedIndex == 0)
                    {
                        gameSettingMenu.RegularGameSettings.SetActive(true);
                        gameSettingMenu.GameSettingsHightlight.enabled = true;
                    }
                    else if (copiedIndex == 1)
                    {
                        gameSettingMenu.RolesSettings.gameObject.SetActive(true);
                        gameSettingMenu.RolesSettingsHightlight.enabled = true;
                    }
                    else if (copiedIndex == 2)
                    {
                        NIMSettings.gameObject.SetActive(true);
                        NIMTabHighlight.enabled = true;
                    }
                    else if (copiedIndex == 3)
                    {
                        crewmateSettings.gameObject.SetActive(true);
                        crewmateTabHighlight.enabled = true;
                    }
                    else if (copiedIndex == 4)
                    {
                        impostorSettings.gameObject.SetActive(true);
                        impostorTabHighlight.enabled = true;
                    }
                    else if (copiedIndex == 5)
                    {
                        neutralSettings.gameObject.SetActive(true);
                        neutralTabHighlight.enabled = true;
                    }
                    else if (copiedIndex == 6)
                    {
                        modifierSettings.gameObject.SetActive(true);
                        modifierTabHighlight.enabled = true;
                    }



                }));
            }

            foreach (OptionBehaviour option in NIMMenu.GetComponentsInChildren<OptionBehaviour>())
                UnityEngine.Object.Destroy(option.gameObject);
            List<OptionBehaviour> NIMOptions = new List<OptionBehaviour>();

            foreach (OptionBehaviour option in impostorMenu.GetComponentsInChildren<OptionBehaviour>())
                UnityEngine.Object.Destroy(option.gameObject);
            List<OptionBehaviour> impostorOptions = new List<OptionBehaviour>();

            foreach (OptionBehaviour option in neutralMenu.GetComponentsInChildren<OptionBehaviour>())
                UnityEngine.Object.Destroy(option.gameObject);
            List<OptionBehaviour> neutralOptions = new List<OptionBehaviour>();

            foreach (OptionBehaviour option in crewmateMenu.GetComponentsInChildren<OptionBehaviour>())
                UnityEngine.Object.Destroy(option.gameObject);
            List<OptionBehaviour> crewmateOptions = new List<OptionBehaviour>();

             foreach (OptionBehaviour option in modifierMenu.GetComponentsInChildren<OptionBehaviour>())
                UnityEngine.Object.Destroy(option.gameObject);
            List<OptionBehaviour> modifierOptions = new List<OptionBehaviour>();


            List<Transform> menus = new List<Transform>() { NIMMenu.transform, impostorMenu.transform, neutralMenu.transform, crewmateMenu.transform, modifierMenu.transform};
            List<List<OptionBehaviour>> optionBehaviours = new List<List<OptionBehaviour>>() { NIMOptions, impostorOptions, neutralOptions, crewmateOptions, modifierOptions};

            for (int i = 0; i < CustomOption.options.Count; i++)
            {
                CustomOption option = CustomOption.options[i];
                if (option.optionBehaviour == null)
                {
                    StringOption stringOption = UnityEngine.Object.Instantiate(template, menus[(int)option.type]);
                    optionBehaviours[(int)option.type].Add(stringOption);
                    stringOption.OnValueChanged = new Action<OptionBehaviour>((o) => { });
                    stringOption.TitleText.text = option.name;
                    stringOption.Value = stringOption.oldValue = option.selection;
                    stringOption.ValueText.text = option.selections[option.selection].ToString();

                    option.optionBehaviour = stringOption;
                }
                option.optionBehaviour.gameObject.SetActive(true);
            }

            NIMMenu.Children = NIMOptions.ToArray();
            NIMSettings.gameObject.SetActive(false);

            impostorMenu.Children = impostorOptions.ToArray();
            impostorSettings.gameObject.SetActive(false);

            neutralMenu.Children = neutralOptions.ToArray();
            neutralSettings.gameObject.SetActive(false);

            crewmateMenu.Children = crewmateOptions.ToArray();
            crewmateSettings.gameObject.SetActive(false);

            modifierMenu.Children = crewmateOptions.ToArray();
            modifierSettings.gameObject.SetActive(false);


            // Adapt task count for main options

            var commonTasksOption = __instance.Children.FirstOrDefault(x => x.name == "NumCommonTasks").TryCast<NumberOption>();
            if (commonTasksOption != null) commonTasksOption.ValidRange = new FloatRange(0f, 4f);

            var shortTasksOption = __instance.Children.FirstOrDefault(x => x.name == "NumShortTasks").TryCast<NumberOption>();
            if (shortTasksOption != null) shortTasksOption.ValidRange = new FloatRange(0f, 23f);

            var longTasksOption = __instance.Children.FirstOrDefault(x => x.name == "NumLongTasks").TryCast<NumberOption>();
            if (longTasksOption != null) longTasksOption.ValidRange = new FloatRange(0f, 15f);
        }
    }

    [HarmonyPatch(typeof(StringOption), nameof(StringOption.OnEnable))]
    public class StringOptionEnablePatch
    {
        public static bool Prefix(StringOption __instance)
        {
            CustomOption option = CustomOption.options.FirstOrDefault(option => option.optionBehaviour == __instance);
            if (option == null) return true;

            __instance.OnValueChanged = new Action<OptionBehaviour>((o) => { });
            __instance.TitleText.text = option.name;
            __instance.Value = __instance.oldValue = option.selection;
            __instance.ValueText.text = option.selections[option.selection].ToString();

            return false;
        }
    }

    [HarmonyPatch(typeof(StringOption), nameof(StringOption.Increase))]
    public class StringOptionIncreasePatch
    {
        public static bool Prefix(StringOption __instance)
        {
            CustomOption option = CustomOption.options.FirstOrDefault(option => option.optionBehaviour == __instance);
            if (option == null) return true;
            option.updateSelection(option.selection + 1);
            return false;
        }
    }

    [HarmonyPatch(typeof(StringOption), nameof(StringOption.Decrease))]
    public class StringOptionDecreasePatch
    {
        public static bool Prefix(StringOption __instance)
        {
            CustomOption option = CustomOption.options.FirstOrDefault(option => option.optionBehaviour == __instance);
            if (option == null) return true;
            option.updateSelection(option.selection - 1);
            return false;
        }
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.RpcSyncSettings))]
    public class RpcSyncSettingsPatch
    {
        public static void Postfix()
        {
            //CustomOption.ShareOptionSelections();
        }
    }


    [HarmonyPatch(typeof(GameOptionsMenu), nameof(GameOptionsMenu.Update))]
    class GameOptionsMenuUpdatePatch
    {
        private static float timer = 1f;
        public static void Postfix(GameOptionsMenu __instance)
        {
            // Return Menu Update if in normal among us settings 
            var gameSettingMenu = UnityEngine.Object.FindObjectsOfType<GameSettingMenu>().FirstOrDefault();
            if (gameSettingMenu.RegularGameSettings.active || gameSettingMenu.RolesSettings.gameObject.active) return;

            __instance.GetComponentInParent<Scroller>().ContentYBounds.max = -0.5F + __instance.Children.Length * 0.55F;
            timer += Time.deltaTime;
            if (timer < 0.1f) return;
            timer = 0f;

            float offset = 2.75f;
            foreach (CustomOption option in CustomOption.options)
            {
                if (GameObject.Find("NIMSettings") && option.type != CustomOption.CustomOptionType.General)
                    continue;
                if (GameObject.Find("ImpostorSettings") && option.type != CustomOption.CustomOptionType.Impostor)
                    continue;
                if (GameObject.Find("NeutralSettings") && option.type != CustomOption.CustomOptionType.Neutral)
                    continue;
                if (GameObject.Find("CrewmateSettings") && option.type != CustomOption.CustomOptionType.Crewmate)
                    continue;
                if (GameObject.Find("ModifierSettings") && option.type != CustomOption.CustomOptionType.Modifier)
                    continue;
                if (option?.optionBehaviour != null && option.optionBehaviour.gameObject != null)
                {
                    bool enabled = true;
                    var parent = option.parent;
                    while (parent != null && enabled)
                    {
                        enabled = parent.selection != 0;
                        parent = parent.parent;
                    }
                    option.optionBehaviour.gameObject.SetActive(enabled);
                    if (enabled)
                    {
                        offset -= option.isHeader ? 0.75f : 0.5f;
                        option.optionBehaviour.transform.localPosition = new Vector3(option.optionBehaviour.transform.localPosition.x, offset, option.optionBehaviour.transform.localPosition.z);
                    }
                }
            }
        }
    }


    [HarmonyPatch]
    class GameOptionsDataPatch
    {
        private static IEnumerable<MethodBase> TargetMethods()
        {
            return typeof(GameOptionsData).GetMethods().Where(x => x.ReturnType == typeof(string) && x.GetParameters().Length == 1 && x.GetParameters()[0].ParameterType == typeof(int));
        }

        private static string buildRoleOptions()
        {
            var impRoles = buildOptionsOfType(CustomOption.CustomOptionType.Impostor, true) + "\n";
            var neutralRoles = buildOptionsOfType(CustomOption.CustomOptionType.Neutral, true) + "\n";
            var crewRoles = buildOptionsOfType(CustomOption.CustomOptionType.Crewmate, true);
            return impRoles + neutralRoles + crewRoles;
        }

        private static string buildOptionsOfType(CustomOption.CustomOptionType type, bool headerOnly)
        {
            StringBuilder sb = new StringBuilder("\n");
            var options = CustomOption.options.Where(o => o.type == type);
            foreach (var option in options)
            {
                if (option.parent == null)
                {
                    sb.AppendLine($"{option.name}: {option.selections[option.selection].ToString()}");
                }
            }
            if (headerOnly) return sb.ToString();
            else sb = new StringBuilder();

            foreach (CustomOption option in options)
            {
                if (option.parent != null)
                {
                    bool isIrrelevant = option.parent.GetSelection() == 0 || (option.parent.parent != null && option.parent.parent.GetSelection() == 0);

                    Color c = isIrrelevant ? Color.grey : Color.white;  // No use for now
                    if (isIrrelevant) continue;
                    sb.AppendLine(Helpers.cs(c, $"{option.name}: {option.selections[option.selection].ToString()}"));
                }
                else
                {
                    if (option == CustomOptionHolder.crewmateRolesCountMax)
                    {
                        var optionName = CustomOptionHolder.cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "クルーメイトの数");
                        var max = CustomOptionHolder.crewmateRolesCountMax.GetSelection();
                        var optionValue = $"{max}";
                        sb.AppendLine($"{optionName}: {optionValue}");
                    }
                    else if (option == CustomOptionHolder.neutralRolesCountMax)
                    {
                        var optionName = CustomOptionHolder.cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "第三陣営の数");
                        var max = CustomOptionHolder.neutralRolesCountMax.GetSelection();
                        var optionValue = $"{max}";
                        sb.AppendLine($"{optionName}: {optionValue}");
                    }
                    else if (option == CustomOptionHolder.impostorRolesCountMax)
                    {
                        var optionName = CustomOptionHolder.cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "インポスターの数");
                        var max = CustomOptionHolder.impostorRolesCountMax.GetSelection();
                        var optionValue = $"{max}";
                        sb.AppendLine($"{optionName}: {optionValue}");
                    }
                    else if ((option == CustomOptionHolder.crewmateRolesCountMax) || (option == CustomOptionHolder.neutralRolesCountMax) || (option == CustomOptionHolder.impostorRolesCountMax))
                    {
                        continue;
                    }
                    else
                    {
                        sb.AppendLine($"\n{option.name}: {option.selections[option.selection].ToString()}");
                    }
                }
            }
            return sb.ToString();
        }

        private static void Postfix(ref string __result)
        {
            int counter = NeutralInModesPlugin.optionsPage;
            string hudString = "";

            switch (counter)
            {
                case 0:
                    hudString += "Page 1: バニラの設定 \n\n" + __result;
                    break;
                case 1:
                    hudString += "Page 2: NIMの設定 \n" + buildOptionsOfType(CustomOption.CustomOptionType.General, false);
                    break;
                case 2:
                    hudString += "Page 3: 全ての役職の設定 \n" + buildRoleOptions();
                    break;
                case 3:
                    hudString += "Page 4: クルーの設定 \n" + buildOptionsOfType(CustomOption.CustomOptionType.Crewmate, false);
                    break;
                case 4:
                    hudString += "Page 5: インポスターの設定 \n" + buildOptionsOfType(CustomOption.CustomOptionType.Impostor, false);
                    break;
                case 5:
                    hudString += "Page 6: 第三陣営の設定 \n" + buildOptionsOfType(CustomOption.CustomOptionType.Neutral, false);
                    break;
                case 6:
                    hudString += "Page 7: 両陣営可能役職の設定 \n" + buildOptionsOfType(CustomOption.CustomOptionType.Modifier, false);
                    break;
            }

            hudString += $"\n Tabキーを押してページを切り替えれます ({counter + 1}/7)";
            __result = hudString;
        }
    }

    [HarmonyPatch(typeof(KeyboardJoystick), nameof(KeyboardJoystick.Update))]
    public static class GameOptionsNextPagePatch
    {
        public static void Postfix(KeyboardJoystick __instance)
        {
            int page = NeutralInModesPlugin.optionsPage;
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                NeutralInModesPlugin.optionsPage = (NeutralInModesPlugin.optionsPage + 1) % 7;
            }
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            {
                NeutralInModesPlugin.optionsPage = 0;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            {
                NeutralInModesPlugin.optionsPage = 1;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
            {
                NeutralInModesPlugin.optionsPage = 2;
            }
            if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
            {
                NeutralInModesPlugin.optionsPage = 3;
            }
            if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5))
            {
                NeutralInModesPlugin.optionsPage = 4;
            }
            if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6))
            {
                NeutralInModesPlugin.optionsPage = 5;
            }
            if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad6))
            {
                NeutralInModesPlugin.optionsPage = 6;
            }
            if (page != NeutralInModesPlugin.optionsPage)
            {
                Vector3 position = (Vector3)HudManager.Instance?.GameSettings?.transform.localPosition;
                if (position != null)
                {
                    HudManager.Instance.GameSettings.transform.localPosition = new Vector3(position.x, 2.9f, position.z);
                }
            }
        }
    }


    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public class GameSettingsScalePatch
    {
        public static void Prefix(HudManager __instance)
        {
            if (__instance.GameSettings != null) __instance.GameSettings.fontSize = 1.2f;
        }
    }


    // This class is taken from Town of Us Reactivated, https://github.com/eDonnes124/Town-Of-Us-R/blob/master/source/Patches/CustomOption/Patches.cs, Licensed under GPLv3
    //すっげぇ......
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public class HudManagerUpdate
    {
        public static float
            MinX,/*-5.3F*/
            OriginalY = 2.9F,
            MinY = 2.9F;


        public static Scroller Scroller;
        private static Vector3 LastPosition;
        private static float lastAspect;
        private static bool setLastPosition = false;

        public static void Prefix(HudManager __instance)
        {
            if (__instance.GameSettings?.transform == null) return;

            // Sets the MinX position to the left edge of the screen + 0.1 units
            Rect safeArea = Screen.safeArea;
            float aspect = Mathf.Min((Camera.main).aspect, safeArea.width / safeArea.height);
            float safeOrthographicSize = CameraSafeArea.GetSafeOrthographicSize(Camera.main);
            MinX = 0.1f - safeOrthographicSize * aspect;

            if (!setLastPosition || aspect != lastAspect)
            {
                LastPosition = new Vector3(MinX, MinY);
                lastAspect = aspect;
                setLastPosition = true;
                if (Scroller != null) Scroller.ContentXBounds = new FloatRange(MinX, MinX);
            }

            CreateScroller(__instance);

            Scroller.gameObject.SetActive(__instance.GameSettings.gameObject.activeSelf);

            if (!Scroller.gameObject.active) return;

            var rows = __instance.GameSettings.text.Count(c => c == '\n');
            float LobbyTextRowHeight = 0.06F;
            var maxY = Mathf.Max(MinY, rows * LobbyTextRowHeight + (rows - 38) * LobbyTextRowHeight);

            Scroller.ContentYBounds = new FloatRange(MinY, maxY);

            // Prevent scrolling when the player is interacting with a menu
            if (CachedPlayer.LocalPlayer?.PlayerControl.CanMove != true)
            {
                __instance.GameSettings.transform.localPosition = LastPosition;

                return;
            }

            if (__instance.GameSettings.transform.localPosition.x != MinX ||
                __instance.GameSettings.transform.localPosition.y < MinY) return;

            LastPosition = __instance.GameSettings.transform.localPosition;
        }

        private static void CreateScroller(HudManager __instance)
        {
            if (Scroller != null) return;

            Scroller = new GameObject("SettingsScroller").AddComponent<Scroller>();
            Scroller.transform.SetParent(__instance.GameSettings.transform.parent);
            Scroller.gameObject.layer = 5;

            Scroller.transform.localScale = Vector3.one;
            Scroller.allowX = false;
            Scroller.allowY = true;
            Scroller.active = true;
            Scroller.velocity = new Vector2(0, 0);
            Scroller.ScrollbarYBounds = new FloatRange(0, 0);
            Scroller.ContentXBounds = new FloatRange(MinX, MinX);
            Scroller.enabled = true;

            Scroller.Inner = __instance.GameSettings.transform;
            __instance.GameSettings.transform.SetParent(Scroller.transform);
        }
    }
}