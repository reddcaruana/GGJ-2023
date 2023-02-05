using System;
using UnityEngine;
using DG.Tweening;
using Assets.Scripts.utils;
using Assets.Scripts.controllers;

namespace Assets.Scripts.game.eggs.views
{
    public class EggView : SpriteObject
    {
        private Tweener tweener;

        public void SortOrder(int order) => SpriteRenderer.sortingOrder = order;

        public void SetGood(Sprite sprite)
        {
            KillAnimation();
            SpriteRenderer.sprite = sprite;
        }

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

        public void Break(Sprite sprite, Action onComplete)
        {
            KillAnimation();
            SpriteRenderer.sprite = sprite;
            var targetPosition = transform.position;
            targetPosition.y -= ViewController.SizY + SpriteRenderer.bounds.size.y;
            StartBreackAnimation(targetPosition, 0.8f, onComplete);
        }

        private void StartBreackAnimation(Vector3 position, float duration, Action onComplete)
        {
            tweener = transform.
                DOMove(position, duration).
                SetEase(Ease.InOutQuad).
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
