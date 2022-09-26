using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace Gameframe.Tunes
{
    [InitializeOnLoad]
    public class MusicWatcher
    {
        static MusicWatcher()
        {
            TunesUtility.CheckCompatibility();
        }

        private static readonly Dictionary<string, bool> IsPaused = new Dictionary<string, bool>();

        private static bool GetIsPaused(string app)
        {
            return IsPaused.TryGetValue(app, out var val) && val;
        }

        private static void SetIsPaused(string app, bool value)
        {
            IsPaused[app] = value;
        }

        [InitializeOnEnterPlayMode]
        private static void InitializeOnEnterPlayMode()
        {
            TunesUtility.CheckCompatibility();
            IsPaused.Clear();
            EditorApplication.playModeStateChanged += ModeChangedOnPlay;
        }

        private static void ModeChangedOnPlay(PlayModeStateChange state)
        {
            if (!Settings.Enabled)
            {
                return;
            }

            switch (state)
            {
                case PlayModeStateChange.EnteredEditMode:
                    EditorApplication.playModeStateChanged -= ModeChangedOnPlay;

                    if (!Settings.ResumeOnExit)
                    {
                        return;
                    }

                    foreach (var app in Settings.ControlledApps.Where(GetIsPaused))
                    {
                        TunesUtility.UnpauseMusic(app);
                    }
                    break;
                case PlayModeStateChange.EnteredPlayMode:
                    foreach (var app in Settings.ControlledApps.Where(TunesUtility.IsPlaying))
                    {
                        SetIsPaused(app,true);
                        if (Settings.PauseOnPlay)
                        {
                            TunesUtility.PauseMusic(app);
                        }
                    }
                    break;
            }
        }
    }
}
