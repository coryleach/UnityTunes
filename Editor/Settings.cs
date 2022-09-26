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
                _prefs = new UtilityPreferences()
                {
                    enabled = true,
                    pauseOnPlay = true,
                    resumeOnExit = true,
                    controlledApps = new List<string>() { "Music", "Spotify" }
                };
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
