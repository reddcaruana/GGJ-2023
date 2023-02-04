using System;
using UnityEngine;
using DG.Tweening;
using Assets.Scripts.utils;

namespace Assets.Scripts.game.grabbers.views
{
    public class GrabberView : SpriteObject
    {
        private Tweener tweener;

        public void CenterSprite()
        {
            var loaclPos = SpriteRenderer.transform.localPosition;
            loaclPos.y -= SpriteRenderer.bounds.size.y / 2f;
            SpriteRenderer.transform.localPosition = loaclPos;
        }

        public Vector3 GetBounds() => SpriteRenderer.bounds.size;

        public void SetIdle(Sprite sprite)
        {
            SpriteRenderer.sprite = sprite;
            killAnimation();
            StartIdelAnimation();
        }

        private void StartIdelAnimation()
        {
            const float endValue = 0.95f;
            const float duration = 1.8f;

            tweener = AnimationUtils.Update(duration, OnUpdate, onComplete: null, loop: -1);

            void OnUpdate(float f)
            {
                var scale = SpriteRenderer.transform.localScale;
                scale.y  = Mathf.Lerp(1, endValue, f);
                SpriteRenderer.transform.localScale = AnimationUtils.VolumePreservation(scale.y, 1);
            }
        }

        public void SetReceive(Sprite sprite, DirectionType directionType, Action onComplete = null)
        {
            SpriteRenderer.sprite = sprite;
            killAnimation();
            StartReciveAnimation(directionType, onComplete);
        }

        private void StartReciveAnimation(DirectionType directionType, Action onComplete)
        {
            const float endValue = 0.5f;
            const float duration = 0.8f;

            (int axis, int direction) = GetAxisAndDirection(directionType);

            tweener = AnimationUtils.Update(duration, OnUpdate, onComplete: OnComplete, loop: 2);

            void OnUpdate(float f)
            {
                var scale = SpriteRenderer.transform.localScale;
                scale[axis] = Mathf.Lerp(1, endValue, f * direction);
                SpriteRenderer.transform.localScale = AnimationUtils.VolumePreservation(scale[axis], axis);
            }

            void OnComplete()
            {
                tweener = null;
                onComplete?.Invoke();
            }
        }

        public void SetPass(Sprite sprite, DirectionType directionType, Action onComplete)
        {
            SpriteRenderer.sprite = sprite;
            StartPassAnimation(directionType, onComplete);
        }

        private void StartPassAnimation(DirectionType directionType, Action onComplete)
        {
            const float endValue = 0.5f;
            const float duration = 0.8f;

            tweener = SpriteRenderer.transform.
                DOScaleX(endValue, duration).
                From(1).
                SetEase(Ease.InOutQuad).
                OnComplete(OnComplete);

            void OnComplete()
            {
                tweener = null;
                onComplete?.Invoke();
            }
        }

        public void SetHit(Sprite sprite, Action onComplete)
        {
            SpriteRenderer.sprite = sprite;
            CoroutineRunner.ME.Wait(0.5f, onComplete);
        }

        public Vector3 GetEggAttachmentPosition() => transform.Find("EggAttachment").position;

        public static (int, int) GetAxisAndDirection(DirectionType directionType)
        {
            switch (directionType)
            {
                case DirectionType.Left: return (0, -1);
                case DirectionType.Right: return (0, 1);
                case DirectionType.Up: return (1, 1);
                case DirectionType.Down: return (1, -1);
                default: Debug.LogError("[GrabberView] Unable to find Direction: " + directionType);
                    return (0, 0);
            }
        }

        private void killAnimation() 
        { 
            tweener?.Kill();
            tweener =  null;
            SpriteRenderer.transform.localScale = Vector3.one;
        }

        public void Exit()
        {
            gameObject.SetActive(false);
        }

        public void Enter()
        {
            gameObject.SetActive(true);
        }
    }
}
