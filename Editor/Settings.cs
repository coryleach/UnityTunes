using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Gameframe.Tunes
{
    [InitializeOnLoad]
    public static class Settings
    {
        private static UtilityPreferences _prefs;

        private const string PrefsKey = "com.gameframe.tunes-prefs";

        static Settings()
        {
            Load();
        }

        public static void Save()
        {
            var json = JsonUtility.ToJson(_prefs);
            EditorPrefs.SetString(PrefsKey,json);
        }

        public static void Load()
        {
            var json = EditorPrefs.GetString(PrefsKey);
            if (string.IsNullOrEmpty(json))
            {
                //Disabling this because I guess people get confused when prompted
                /*var enabled = EditorUtility.DisplayDialog("Enable AutoPause",
                    "Would you like to enable auto-pause for iTunes/Spotify when in play mode?", "Yes", "No");*/
                _prefs = new UtilityPreferences()
                {
                    enabled = false, //=enabled,
                    pauseOnPlay = true,
                    resumeOnExit = true,
                    controlledApps = new List<string>() { "Music", "Spotify" }
                };
                //We need to make sure we save immediately
                //So people who don't interact with with the plugin get prompted every time they open unity
                Save();
                return;
            }
            _prefs = JsonUtility.FromJson<UtilityPreferences>(json);
        }

        public static List<string> ControlledApps => _prefs.controlledApps ?? (_prefs.controlledApps = new List<string>());

        public static bool Enabled
        {
            get => _prefs.enabled;
            set
            {
                _prefs.enabled = value;
                Save();
            }
        }

        public static bool ResumeOnExit
        {
            get => _prefs.resumeOnExit;
            set
            {
                _prefs.resumeOnExit = value;
                Save();
            }
        }

        public static bool PauseOnPlay
        {
            get => _prefs.pauseOnPlay;
            set
            {
                _prefs.pauseOnPlay = value;
                Save();
            }
        }
    }
}
