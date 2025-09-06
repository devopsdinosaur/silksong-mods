using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class Settings {
    private static Settings m_instance = null;
    public static Settings Instance {
        get {
            if (m_instance == null) {
                m_instance = new Settings();
            }
            return m_instance;
        }
    }
    private DDPlugin m_plugin = null;
    
    // General
    public static ConfigEntry<bool> m_enabled;
    public static ConfigEntry<string> m_log_level;
    public static ConfigEntry<float> m_speed_multiplier;
    public static ConfigEntry<float> m_dash_speed_multiplier;

    // Hotkeys
    public static ConfigEntry<string> m_hotkey_modifier;
    public static ConfigEntry<string> m_hotkey_toggle_enabled;


    public ConfigEntry<T> create_entry<T>(string category, string name, T default_value, string description, EventHandler change_callback) {
        ConfigEntry<T> result = this.m_plugin.Config.Bind<T>(category, name, default_value, description);
        if (change_callback != null) {
            result.SettingChanged += change_callback;
        }
        return result;
    }

    public void early_load(DDPlugin plugin) {
        this.m_plugin = plugin;
        
        // General
        m_enabled = this.create_entry("General", "Enabled", true, "Set to false to disable this mod.", on_setting_changed);
        m_log_level = this.create_entry("General", "Log Level", "info", "[Advanced] Logging level, one of: 'none' (no logging), 'error' (only errors), 'warn' (errors and warnings), 'info' (normal logging), 'debug' (extra log messages for debugging issues).  Not case sensitive [string, default info].  Debug level not recommended unless you're noticing issues with the mod.  Changes to this setting require an application restart.", on_setting_changed);
        m_speed_multiplier = this.create_entry("General", "Movement Speed Multiplier", 1f, "Multiplier for player movement speed (1 = default speed [default], Greater than 1 = faster, Less than 1 = slower).", on_setting_changed);
        m_dash_speed_multiplier = this.create_entry("General", "Dash Speed Multiplier", 1f, "Multiplier for dash speed (1 = default speed [default], Greater than 1 = faster, Less than 1 = slower).  * NOTE * - Values higher than ~3 can cause air dashes to result in massive distance changes and unpredictable results.", on_setting_changed);
        DDPlugin.set_log_level(m_log_level.Value);

        // Hotkeys
        m_hotkey_modifier = this.m_plugin.Config.Bind<string>("Hotkeys", "Hotkey - Modifier", "", "Comma-separated list of Unity Keycodes used as the special modifier key (i.e. ctrl,alt,command) one of which is required to be down for hotkeys to work.  Set to '' (blank string) to not require a special key (not recommended).  See this link for valid Unity KeyCode strings (https://docs.unity3d.com/ScriptReference/KeyCode.html)");
        m_hotkey_toggle_enabled = this.m_plugin.Config.Bind<string>("Hotkeys", "Hotkey - Toggle Enabled", "F5", "Comma-separated list of Unity Keycodes, any of which will (when combined with 'modifier' key) enable/disable this mod.");
    }

    public void late_load() {
        
    }

    public static void on_setting_changed(object sender, EventArgs e) {
		
	}
}