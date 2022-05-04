using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityModManagerNet;

namespace MissionMultipliers
{
    public static class Main
    {
        public static bool enabled;
        public static UnityModManager.ModEntry mod;
        public static MissionMultiplierSettings settings;

        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            mod = modEntry;

            settings = MissionMultiplierSettings.Load<MissionMultiplierSettings>(modEntry);

            modEntry.OnToggle = OnToggle;
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;

            new Harmony(modEntry.Info.Id).PatchAll(Assembly.GetExecutingAssembly());

            return true;
        }

        public static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
#if DEBUG
            FileLog.Log("Value passed to OnToggle is " + value);
#endif
            enabled = value;
#if DEBUG
            if (enabled)
                FileLog.Log("Enabled");
            else FileLog.Log("Not enabled");
#endif
            return true;
        }

        public static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Mission Pay Multiplier", GUILayout.ExpandWidth(false));
            settings.MissionPayMultiplier = GUILayout.HorizontalSlider(settings.MissionPayMultiplier, 1.0f, 50.0f);
            GUILayout.Label(settings.MissionPayMultiplier.ToString("0.00"), GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Reset", GUILayout.ExpandWidth(false)))
            {
                settings.MissionPayMultiplier = 1.0f;
            }
            GUILayout.EndHorizontal();
        }

        public static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Save(modEntry);
        }        
    }
}