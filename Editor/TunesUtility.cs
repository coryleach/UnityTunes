using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Gameframe.Tunes
{
    [Serializable]
    public struct MusicScripts
    {
        public string checkPlaying;
        public string play;
        public string pause;
    }

    [Serializable]
    public struct MusicAppData
    {
        public string name;
        public MusicScripts scripts;
    }

    [Serializable]
    public struct UtilityPreferences
    {
        public bool enabled;
        public bool pauseOnPlay;
        public bool resumeOnExit;
        public List<string> controlledApps;
    }

    public static class TunesUtility
    {
        public static Dictionary<string, MusicAppData> Apps =
        new Dictionary<string, MusicAppData>() {
            {
                "Music",
                new MusicAppData
                {
                    name = "Music",
                    scripts = new MusicScripts
                    {
                        checkPlaying = "music_checkIfPlaying.scpt",
                        play = "music_play.scpt",
                        pause = "music_pause.scpt",
                    }
                }
            },
            {
                "Spotify",
                new MusicAppData()
                {
                    name = "Spotify",
                    scripts = new MusicScripts()
                    {
                        checkPlaying = "spot_checkIfPlaying.scpt",
                        play = "spot_play.scpt",
                        pause = "spot_pause.scpt",
                    }
                }
            },
        };

        public static bool IsPlaying()
        {
#if UNITY_EDITOR_OSX
            var output = GetScriptOutput("checkIfPlaying.scpt");
            return output.Contains("true");
#endif
            //Only implemented for OSX currently
            return false;
        }

        public static void PauseMusic()
        {
            var result = GetScriptOutput("pause.scpt");
            if (result != "false")
            {
                Debug.LogError("Failed to pause music");
            }
        }

        public static void UnpauseMusic()
        {
            var result = GetScriptOutput("play.scpt");
            if (result != "true")
            {
                Debug.LogError("Failed to play music");
            }
        }

        public static bool IsPlaying(string name)
        {
            var app = GetAppData(name);
#if UNITY_EDITOR_OSX
            var output = GetScriptOutput(app.scripts.checkPlaying);
            return output.Contains("true");
#endif
            //Only implemented for OSX currently
            return false;
        }

        public static void PauseMusic(string name)
        {
            //Debug.Log($"Pause {name}");
            var app = GetAppData(name);
            GetScriptOutput(app.scripts.pause);
        }

        public static void UnpauseMusic(string name)
        {
            //Debug.Log($"Unpause {name}");
            var app = GetAppData(name);
            GetScriptOutput(app.scripts.play);
        }

        private static MusicAppData GetAppData(string name)
        {
            return Apps[name];
        }

        private static string GetScriptOutput(string scriptName)
        {
#if UNITY_EDITOR_OSX
            var assetPath = $"Packages/com.gameframe.tunes/Shell/osx/{scriptName}";
            if (!File.Exists(assetPath))
            {
                Debug.LogError($"{assetPath} does not exist in {Directory.GetCurrentDirectory()}");
                return string.Empty;
            }

            var fullPath = Path.GetFullPath(assetPath).Replace(" ", "\\ ");
            var cmd = $"osascript {fullPath}";
            return ShellUtility.GetCommandResult(cmd).ToLower();
#endif
            //Not implemented for platform
            return string.Empty;
        }
    }
}
