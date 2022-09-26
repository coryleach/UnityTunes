using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Gameframe.Tunes
{
    [Serializable]
    public struct PlatformScripts
    {
        public string checkPlaying;
        public string play;
        public string pause;
    }

    [Serializable]
    public struct MusicScripts
    {
        public PlatformScripts Win;
        public PlatformScripts Mac;

#if UNITY_EDITOR_OSX
        public string checkPlaying => Mac.checkPlaying;
        public string play => Mac.play;
        public string pause => Mac.pause;
#else
        public string checkPlaying => Win.checkPlaying;
        public string play => Win.play;
        public string pause => Win.pause;
#endif
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
                        Mac = new PlatformScripts
                        {
                            checkPlaying = "music_checkIfPlaying.scpt",
                            play = "music_play.scpt",
                            pause = "music_pause.scpt",
                        },
                        Win = new PlatformScripts
                        {
                            checkPlaying = "music_checkIfPlaying.scpt",
                            play = "music_play.scpt",
                            pause = "music_pause.scpt",
                        }
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
                        Mac = new PlatformScripts()
                        {
                            checkPlaying = "spot_checkIfPlaying.scpt",
                            play = "spot_play.scpt",
                            pause = "spot_pause.scpt",
                        },
                        Win = new PlatformScripts
                        {
                            checkPlaying = "checkIfPlaying.ps1",
                            play = "play.ps1",
                            pause = "pause.ps1",
                        }
                    }
                }
            },
        };

        public static bool IsPlaying()
        {
#if UNITY_EDITOR_OSX
            var output = GetScriptOutput("checkIfPlaying.scpt");
            return output.Contains("true");
#elif UNITY_EDITOR_WIN
#endif
            //Only implemented for this platform
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
#if UNITY_EDITOR_OSX || UNITY_EDITOR_WIN
            var app = GetAppData(name);
            var output = GetScriptOutput(app.scripts.checkPlaying);
            return output.Contains("true");
#else
            //Not implemented on this platform
            return false;
#endif      
        }

        public static void PauseMusic(string name)
        {
            var app = GetAppData(name);
            GetScriptOutput(app.scripts.pause);
        }

        public static void UnpauseMusic(string name)
        {
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
#elif UNITY_EDITOR_WIN
            var assetPath = $"Packages/com.gameframe.tunes/Shell/win/{scriptName}";
            if (!File.Exists(assetPath))
            {
                return string.Empty;
            }

            var fullPath = Path.GetFullPath(assetPath).Replace(" ", "\\ ");
            var result = ShellUtility.GetCommandResult(fullPath);
            return result != null ? result.ToLower() : string.Empty;
#else
            //Not implemented for platform
            return string.Empty;
#endif  
        }

        public static void CheckCompatibility()
        {
            if ( !Settings.Enabled )
            {
                return;
            }

#if UNITY_EDITOR_WIN
            var executionPolicy = ShellUtility.GetCommandResult("Get-ExecutionPolicy");
            if (executionPolicy == "Restricted")
            {
                UnityEditor.EditorUtility.DisplayDialog("Script Error", "Scripts are restricted on this machine. Tunes utility will be disabled. Please run Powershell in Administrator mode and run the command 'Set-ExecutionPolicy RemoteSigned' or 'Set-ExecutionPolicy Unrestricted'. ","Ok");
                Settings.Enabled = false;
            }
#endif
        }

    }
}
