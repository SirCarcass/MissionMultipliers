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
            new Harmony(modEntry.Info.Id).PatchAll(Assembly.GetExecutingAssembly());

            mod = modEntry;

            settings = MissionMultiplierSettings.Load<MissionMultiplierSettings>(modEntry);

            modEntry.OnToggle = OnToggle;
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;            

            return true;
        }

        public static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            enabled = value;
            return true;
        }

        public static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Mission Pay Multiplier", GUILayout.ExpandWidth(false));
            settings.MissionPayMultiplier = GUILayout.HorizontalSlider(settings.MissionPayMultiplier, 1.0f, 50.0f);
            GUILayout.Label(settings.MissionPayMultiplier.ToString("0"), GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Reset", GUILayout.ExpandWidth(false)))
            {
                settings.MissionPayMultiplier = 1.0f;
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Mission Reputation Multiplier", GUILayout.ExpandWidth(false));
            settings.MissionRepMultiplier = GUILayout.HorizontalSlider(settings.MissionRepMultiplier, 1.0f, 50.0f);
            GUILayout.Label(settings.MissionRepMultiplier.ToString("0"), GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Reset", GUILayout.ExpandWidth(false)))
            {
                settings.MissionRepMultiplier = 1.0f;
            }
            GUILayout.EndHorizontal();
        }

        public static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Save(modEntry);
        }        
    }
}