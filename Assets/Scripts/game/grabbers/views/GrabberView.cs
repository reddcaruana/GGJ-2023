using System;
using UnityEngine;
using DG.Tweening;
using Assets.Scripts.utils;
using Assets.Scripts.controllers;
using Assets.Scripts.game.directions.data;
using static Assets.Scripts.game.grabbers.MotherGrabber;

namespace Assets.Scripts.game.grabbers.views
{
    public class GrabberView : SpriteObject
    {
        public static readonly Vector3 VECTOR3_ONE = Vector3.one;
        public static readonly Vector3 SCALE_FLIP_RIGHT = new Vector3(-1f, 1, 1f);

        private Vector2 localPos;
        private PositionAlignmentType positionType;
        private Tweener tweener;
        public void CenterSprite()
        {
            localPos = SpriteRenderer.transform.localPosition;
            localPos.y -= SpriteRenderer.bounds.size.y / 2f;

            SpriteRenderer.transform.localPosition = localPos;
        }

        public void SetPositionType(PositionAlignmentType positionType) => this.positionType = positionType;

        public void SetOrder(int order) => SpriteRenderer.sortingOrder = order;

        public Vector3 GetBounds() => SpriteRenderer.bounds.size;

        public void SetIdle(Sprite sprite)
        {
            SpriteRenderer.sprite = sprite;
            KillAnimation();
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

        public void SetReceive(Sprite sprite, DirectionData directionData, Action onComplete = null)
        {
            SpriteRenderer.sprite = sprite;
            KillAnimation();
            //StartReciveAnimation(directionData, onComplete);
            onComplete?.Invoke();
        }

        private void StartReciveAnimation(DirectionData directionData, Action onComplete)
        {
            const float endValue = 0.5f;
            const float duration = 0.8f;

            var axis = directionData.Axis;
            var direction = directionData.DirectionMultiplier;
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

        public void SetPass(Sprite sprite, DirectionData directionData, Action onComplete)
        {
            SpriteRenderer.sprite = sprite;
            StartPassAnimation(directionData, onComplete);
        }

        private void StartPassAnimation(DirectionData directionData, Action onComplete)
        {
            const float endValue = 1.1f;
            const float duration = 0.2f;

            if (directionData.Compare(DirectionData.Right))
                transform.localScale = SCALE_FLIP_RIGHT;
            else
                transform.rotation = directionData.Quaternion;

            tweener = SpriteRenderer.transform.
                DOScaleX(endValue, duration).
                From(1).
                SetEase(Ease.InOutQuad).
                OnComplete(OnComplete);

            void OnComplete()
            {
                ResetTransforms();
                tweener = null;
                onComplete?.Invoke();
            }
        }

        public void SetHit(Sprite sprite, Action onComplete)
        {
            SpriteRenderer.sprite = sprite;
            CoroutineRunner.ME.Wait(0.5f, onComplete);
        }

        public void Enter(Sprite sprite, Action onComlete)
        {
            gameObject.SetActive(true);
            SpriteRenderer.sprite = sprite;

            KillAnimation();
            const float duration = 0.3f;

            var targetPos = transform.parent.position;
            var startPos = targetPos;
            startPos.y += ViewController.SizY + SpriteRenderer.bounds.size.y;
            tweener = transform.
                DOMove(targetPos, duration).
                From(startPos).
                SetEase(Ease.OutQuad).
                OnComplete(OnComplete);

            void OnComplete()
            {
                tweener = null;
                onComlete?.Invoke();
            }
        }

        public void ExitNoAnim() => gameObject.SetActive(false);

        public void Exit(Sprite sprite, Action onComlete)
        {
            SpriteRenderer.sprite = sprite;

            const float duration = 0.3f;

            var startPos = transform.position;
            var targetPos = startPos;
            targetPos.y -= ViewController.SizY + SpriteRenderer.bounds.size.y;
            tweener = transform.
                DOMove(targetPos, duration).
                From(startPos).
                SetEase(Ease.InBack).
                OnComplete(OnComplete);

            void OnComplete()
            {
                gameObject.SetActive(false);
                FixPosition();
                tweener = null;
                onComlete?.Invoke();
            }
        }

        public void FixPosition()
        {
            var position = transform.parent.position;

            switch (positionType)
            {
                case PositionAlignmentType.BottomRightRight: position.x = (ViewController.SizX / 2f) - (GetBounds().x / 2f); break;
                case PositionAlignmentType.BottomRight: position.y = -(ViewController.SizY / 2f) + (GetBounds().y / 2f); break;
                case PositionAlignmentType.BottomLeft: position.y = -(ViewController.SizY / 2f) + (GetBounds().y / 2f); break;
                case PositionAlignmentType.BottomLeftLeft: position.x = -(ViewController.SizX / 2f) + (GetBounds().x / 2f); break;
                case PositionAlignmentType.UpperLeftLeft: position.x = -(ViewController.SizX / 2f) + (GetBounds().x / 2f); break;
                case PositionAlignmentType.UpperLeft: position.y = (ViewController.SizY / 2f) - (GetBounds().y / 2f); break;
                case PositionAlignmentType.UpperRight: position.y = (ViewController.SizY / 2f) - (GetBounds().y / 2f); break;
                case PositionAlignmentType.UpperRightRight: position.x = (ViewController.SizX / 2f) - (GetBounds().x / 2f); break;
            }

            transform.position = position;
        }

        public void Happy(Sprite sprite, Action onComlpete)
        {
            SpriteRenderer.sprite = sprite;
            CoroutineRunner.ME.Wait(1f, onComlpete);
        }

        public void Sad(Sprite sprite, Action onComlpete)
        {
            SpriteRenderer.sprite = sprite;
            CoroutineRunner.ME.Wait(1f, onComlpete);
        }

        public void Woble(DirectionData directionData)
        {
            KillAnimation();

            const float duration = 0.1f;
            Vector3 target = transform.position;
            target[directionData.Axis] = 1f * directionData.DirectionMultiplier;
            tweener = transform.
                DOMove(target, duration).
                SetEase(Ease.InOutQuad).
                SetLoops(2, LoopType.Yoyo).
                OnComplete(OnComplete);

            void OnComplete() => tweener = null;
        }

        public Vector3 GetEggAttachmentPosition() => transform.Find("EggAttachment").position;


        private void ResetTransforms()
        {
            transform.localPosition = localPos;
            transform.localScale = VECTOR3_ONE;
            transform.rotation = Quaternion.identity;
        }

        private void KillAnimation() 
        { 
            tweener?.Kill();
            tweener =  null;
            ResetTransforms();
        }

    }
}
