using UnityEngine;
using System.Collections;

namespace Assets.Scripts.utils
{
    public static class PauseUtils
    {
        private static bool isPaused;

        public static void TryPause(float time)
        {
            Debug.Log("IsPaused: " + isPaused);
            if (isPaused) return;
            CoroutineRunner.ME.Pause(time, null);
        }
    }
}
