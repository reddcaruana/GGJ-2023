using System;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.utils
{
    public static class AnimationUtils
    {
        public static Tweener Update(float duration, Action<float> update, Action onComplete, 
            float from = 0f, float to = 1f, int loop = 1, LoopType loopType = LoopType.Yoyo)
        {
            float i = from;
            var tweener = DOTween.To(() => i, (x) =>
            {
                i = x;
                update?.Invoke(i);
            }, to, duration).
            SetEase(Ease.InOutQuad).
            SetLoops(loop, loopType).
            OnComplete(OnComplete);

            void OnComplete() => onComplete?.Invoke();

            return tweener;
        }

        public static Vector3 VolumePreservation(float value, int axis, float defVal = 1)
        {
            var otherAxis0 = IncimentAndLoop(axis);
            var otherAxis1 = IncimentAndLoop(otherAxis0);

            var normalized = defVal +  defVal - value;

            var scale = new Vector3();
            scale[axis] = value;
            scale[otherAxis0] = defVal * normalized;
            scale[otherAxis1] = defVal * normalized;

            return scale; 
        }

        private static int IncimentAndLoop(int value) => IncimentAndLoop(value, 0, 2);
        private static int IncimentAndLoop(int value, int min, int max) 
        {
            value += 1;
            if (value > max)
                value = min;

            return value;

        }
    }
}
