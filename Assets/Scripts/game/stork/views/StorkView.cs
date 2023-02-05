using System;
using DG.Tweening;
using UnityEngine;
using Assets.Scripts.utils;

namespace Assets.Scripts.game.stork.views
{
    public class StorkView : SpriteObject
    {
        private Tweener tweener;
        public void MoveTo(Vector3 from, Vector3 to, float duration, Action onComplete)
        {
            KillAnimation();

            tweener = transform.
                DOMove(to, duration).
                From(from).
                SetEase(Ease.Linear).
                OnComplete(OnComplete);

            void OnComplete()
            {
                tweener = null;
                onComplete?.Invoke();
            }
        }

        private void KillAnimation() => tweener?.Kill();
    }
}
