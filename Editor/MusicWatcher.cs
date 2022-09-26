using System;
using UnityEditor;

namespace Gameframe.Tunes
{
    [InitializeOnLoad]
    public class MusicWatcher
    {
        static MusicWatcher()
        {
        }

        private static bool isPaused = false;

        [InitializeOnEnterPlayMode]
        private static void InitializeOnEnterPlayMode()
        {
            isPaused = false;
            EditorApplication.playModeStateChanged += ModeChangedOnPlay;
        }

        private static void ModeChangedOnPlay(PlayModeStateChange state)
        {
            switch (state)
            {
                case PlayModeStateChange.EnteredEditMode:
                    EditorApplication.playModeStateChanged -= ModeChangedOnPlay;
                    if (isPaused)
                    {
                        TunesUtility.UnpauseMusic();
                    }
                    break;
                case PlayModeStateChange.EnteredPlayMode:
                    if (TunesUtility.IsPlaying())
                    {
                        isPaused = true;
                        TunesUtility.PauseMusic();
                    }
                    break;
            }
        }
    }
}
