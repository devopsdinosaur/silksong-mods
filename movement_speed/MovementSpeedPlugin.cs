using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class PluginInfo {

    public const string TITLE = "Movement Speed";
    public const string NAME = "movement_speed";
    public const string SHORT_DESCRIPTION = "Change player movement speed using in-game configurable multipliers.";
	public const string EXTRA_DETAILS = "This mod does not make any permanent changes to the game files.  It simply modifies the strings in memory for the duration of the game.  Removing the mod and restarting the game will revert everything to its default state.";

	public const string VERSION = "0.0.2";

    public const string AUTHOR = "devopsdinosaur";
    public const string GAME_TITLE = "Silksong";
    public const string GAME = "silksong";
    public const string GUID = AUTHOR + "." + GAME + "." + NAME;
    public const string REPO = "silksong-mods";

    public static Dictionary<string, string> to_dict() {
        Dictionary<string, string> info = new Dictionary<string, string>();
        foreach (FieldInfo field in typeof(PluginInfo).GetFields((BindingFlags) 0xFFFFFFF)) {
            info[field.Name.ToLower()] = (string) field.GetValue(null);
        }
        return info;
    }
}

[BepInPlugin(PluginInfo.GUID, PluginInfo.TITLE, PluginInfo.VERSION)]
public class NpcRenamePlugin : DDPlugin {
	private static NpcRenamePlugin m_instance = null;
    private Harmony m_harmony = new Harmony(PluginInfo.GUID);
    private static float m_original_dash_speed = -1;

	private void Awake() {
        logger = this.Logger;
        try {
			m_instance = this;
            this.m_plugin_info = PluginInfo.to_dict();
            Settings.Instance.early_load(m_instance);
            m_instance.create_nexus_page();
            this.m_harmony.PatchAll();
            logger.LogInfo($"{PluginInfo.GUID} v{PluginInfo.VERSION} loaded.");
        } catch (Exception e) {
            _error_log("** Awake FATAL - " + e);
        }
    }

    [HarmonyPatch(typeof(HeroController), "GetRunSpeed")]
    class HarmonyPatch_HeroController_GetRunSpeed {
        private static void Postfix(HeroController __instance, ref float __result) {
            try {
                if (Settings.m_enabled.Value) {
                    __result *= Settings.m_speed_multiplier.Value;
                }
            } catch { }
        }
    }

    [HarmonyPatch(typeof(HeroController), "Update")]
    class HarmonyPatch_HeroController_Update {
        private static bool Prefix(HeroController __instance) {
            try {
                if (m_original_dash_speed == -1) {
                    m_original_dash_speed = __instance.DASH_SPEED;
                }
                if (Settings.m_enabled.Value) {
                    __instance.DASH_SPEED = m_original_dash_speed * Settings.m_dash_speed_multiplier.Value;
                }
                return true;
            } catch { }
            return true;
        }
    }
}