using UnityEditor;
using UnityEngine;

namespace Gameframe.Tunes
{
    public static class TunesMenu
    {
        private const string EnabledPath = "Window/Tunes/Enabled";
        private const string PauseOnPlayPath = "Window/Tunes/Behaviour/PauseOnPlay";
        private const string ResumeOnExitPath = "Window/Tunes/Behaviour/ResumeOnStop";

        private const string AppMusicPath = "Window/Tunes/Apps/Music";
        private const string AppSpotifyPath = "Window/Tunes/Apps/Spotify";

        [MenuItem(EnabledPath)]
        private static void Enabled()
        {
            Settings.Enabled = !Settings.Enabled;
            Debug.Log($"Tunes Control is now {(Settings.Enabled ? "Enabled" : "Disabled")}");
        }

        [MenuItem(EnabledPath, true)]
        private static bool EnabledValidate()
        {
            Menu.SetChecked(EnabledPath, Settings.Enabled);
            return true;
        }

        [MenuItem(PauseOnPlayPath)]
        private static void Pause()
        {
            Settings.PauseOnPlay = !Settings.PauseOnPlay;
            Debug.Log($"Pause on Play {(Settings.PauseOnPlay ? "Enabled" : "Disabled")}");
        }

        [MenuItem(PauseOnPlayPath, true)]
        private static bool PauseValidate()
        {
            Menu.SetChecked(PauseOnPlayPath, Settings.PauseOnPlay);
            return true;
        }

        [MenuItem(ResumeOnExitPath)]
        private static void Resume()
        {
            Settings.ResumeOnExit = !Settings.ResumeOnExit;
            Debug.Log($"Resume on Exit {(Settings.PauseOnPlay ? "Enabled" : "Disabled")}");
        }

        [MenuItem(ResumeOnExitPath, true)]
        private static bool ResumeValidate()
        {
            Menu.SetChecked(ResumeOnExitPath, Settings.ResumeOnExit);
            return true;
        }

        [MenuItem(AppMusicPath)]
        private static void Music()
        {
            App("Music");
        }

        [MenuItem(AppMusicPath,true)]
        private static bool MusicValidate()
        {
            return AppValidate(AppMusicPath, "Music");
        }

        [MenuItem(AppSpotifyPath)]
        private static void Spotify()
        {
            App("Spotify");
        }

        [MenuItem(AppSpotifyPath,true)]
        private static bool SpotifyValidate()
        {
            return AppValidate(AppSpotifyPath, "Spotify");
        }

        private static void App(string app)
        {
            if (Settings.ControlledApps.Contains(app))
            {
                Settings.ControlledApps.Remove(app);
            }
            else
            {
                Settings.ControlledApps.Add(app);
            }
        }

        private static bool AppValidate(string path, string app)
        {
            Menu.SetChecked(path, Settings.ControlledApps.Contains(app));
            return true;
        }
    }
}
