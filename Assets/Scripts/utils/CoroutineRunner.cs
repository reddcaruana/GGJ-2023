using System;
using UnityEngine;
using System.Collections;

namespace Assets.Scripts.utils
{
    public class CoroutineRunner : MonoBehaviour
    {
        private static CoroutineRunner me;
        public static CoroutineRunner ME 
        { 
            get 
            { 
                if (me == null)
                    me = new GameObject("CoroutineRunner").AddComponent<CoroutineRunner>();
                return me;
            }
        }

        public Coroutine Wait(float value, Action onComplete)
        {
            IEnumerator Wait()
            {
                yield return new WaitForSeconds(value);
                onComplete();
            }
            return StartCoroutine(Wait());
        }

        public Coroutine Pause(float value, Action onComplete)
        {
            IEnumerator Pause()
            {
                Time.timeScale = 0;
                yield return new WaitForSecondsRealtime(value);
                Time.timeScale = 1;
                onComplete?.Invoke();
            }
            return StartCoroutine(Pause());
        }
    }
}
