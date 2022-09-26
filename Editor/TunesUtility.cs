using System.IO;
using UnityEngine;

namespace Gameframe.Tunes
{
    public static class TunesUtility
    {
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
