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
    }
}
