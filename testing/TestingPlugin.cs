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

    public const string TITLE = "Testing";
    public const string NAME = "testing";
    public const string SHORT_DESCRIPTION = "For testing only";
	public const string EXTRA_DETAILS = "This mod does not make any permanent changes to the game files.  It simply modifies the strings in memory for the duration of the game.  Removing the mod and restarting the game will revert everything to its default state.";

	public const string VERSION = "0.0.0";

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

	[HarmonyPatch(typeof(HealthManager), "Die", new Type[] { typeof(float), typeof(AttackTypes), typeof(NailElements), typeof(GameObject), typeof(bool), typeof(float), typeof(bool), typeof(bool) })]
	class HarmonyPatch_HealthManager_Die {
		private static bool Prefix() {
			try {
				_info_log("You killed something!!");
				return true;
			} catch (Exception e) {
				_error_log("** HarmonyPatch_HealthManager_Die.Prefix ERROR - " + e);
			}
			return true;
		}
	}

	/*
	[HarmonyPatch(typeof(EnemyDeathEffects), "ReceiveDeathEvent", new Type[] { typeof(float), typeof(AttackTypes), typeof(NailElements), typeof(GameObject), typeof(float), typeof(bool), typeof(Action<Transform>), typeof(bool), typeof(GameObject) })]
	class HarmonyPatch_EnembyDeathEffects_ReceiveDeathEvent {
		private static bool Prefix(EnemyDeathEffects __instance) {
			try {
				_info_log("You died, loser!!!");
				return true;
			} catch (Exception e) {
				_error_log("** HarmonyPatch_EnembyDeathEffects_ReceiveDeathEvent.Prefix ERROR - " + e);
			}
			return true;
		}
	}
	*/

	/*
	[HarmonyPatch(typeof(), "")]
	class HarmonyPatch_ {
		private static bool Prefix() {
			
			return true;
		}
	}

	[HarmonyPatch(typeof(), "")]
	class HarmonyPatch_ {
		private static void Postfix() {
			
		}
	}

	[HarmonyPatch(typeof(), "")]
	class HarmonyPatch_ {
		private static bool Prefix() {
			try {

				return false;
			} catch (Exception e) {
				_error_log("** XXXXX.Prefix ERROR - " + e);
			}
			return true;
		}
	}

	[HarmonyPatch(typeof(), "")]
	class HarmonyPatch_ {
		private static void Postfix() {
			try {
				
			} catch (Exception e) {
				_error_log("** XXXXX.Postfix ERROR - " + e);
			}
		}
	}
	*/
}