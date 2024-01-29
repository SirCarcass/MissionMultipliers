using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

#if BUILD_UMM
using UnityModManagerNet;
#endif

#if BUILD_BEPINEX
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
#endif

namespace MissionMultipliers
{

#if BUILD_UMM

    public static class Main
    {
        public static bool enabled;
        public static UnityModManager.ModEntry mod;
        public static MissionMultiplierSettings settings;

        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            //new Harmony(modEntry.Info.Id).PatchAll(Assembly.GetExecutingAssembly());

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
            enabled = value;
            return true;
        }

        public static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Mission Pay Multiplier", GUILayout.ExpandWidth(false));
            settings.MissionPayMultiplier = GUILayout.HorizontalSlider(settings.MissionPayMultiplier, 1.0f, 50.0f);
            settings.MissionPayMultiplier = Mathf.Round(settings.MissionPayMultiplier);
            GUILayout.Label(settings.MissionPayMultiplier.ToString("0.0"), GUILayout.ExpandWidth(false));
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
            settings.MissionRepMultiplier = Mathf.Round(settings.MissionRepMultiplier);
            GUILayout.Label(settings.MissionRepMultiplier.ToString("0.0"), GUILayout.ExpandWidth(false));
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
#endif

#if BUILD_BEPINEX
    [BepInPlugin(GUID, NAME, VERSION)]
    public class MissionMultipliersMain : BaseUnityPlugin
    {
        public const string GUID = "com.sircarcass.missionmultipliers";
        public const string NAME = "Mission Multipliers";
        public const string VERSION = "3.0.0";

        internal static ManualLogSource logSource;

        internal ConfigEntry<int> missionPayMultiplier;
        internal ConfigEntry<int> missionRepMultiplier;

        internal static MissionMultipliersMain instance;
        
        private void Awake()
        {
            instance = this;
            logSource = Logger;
            
            missionPayMultiplier = Config.Bind("Multipliers", "Mission Pay Multiplier", 1, new ConfigDescription("Multiplier for generated mission pay.  Doesn't affect accepted missions, only newly generated missions.  Changes to multiplier require restarting the game.", new AcceptableValueRange<int>(1, 100), new ConfigurationManagerAttributes { ShowRangeAsPercent = false}));
            missionRepMultiplier = Config.Bind("Multipliers", "Mission Reputation Multiplier", 1, new ConfigDescription("Multiplier for reputation reward when turning in cargo for a mission.  Changes to multiplier require restarting the game.", new AcceptableValueRange<int>(1,100), new ConfigurationManagerAttributes { ShowRangeAsPercent = false }));

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), GUID);            
        }        
    }

#endif
}