﻿using System;
using UnityEngine;
using DG.Tweening;
using Assets.Scripts.utils;
using Assets.Scripts.controllers;
using Assets.Scripts.game.directions.data;

namespace Assets.Scripts.game.grabbers.views
{
    public class GrabberView : SpriteObject
    {
        public static readonly Vector3 VECTOR3_ONE = Vector3.one;
        public static readonly Vector3 SCALE_FLIP_RIGHT = new Vector3(-1f, 1, 1f);

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
            Debug.Log("*-* Enter");
            gameObject.SetActive(true);
            SpriteRenderer.sprite = sprite;

            KillAnimation();
            const float duration = 0.3f;

            var targetPos = transform.position;
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
            Debug.Log("*-* Exit");

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
                tweener = null;
                onComlete?.Invoke();
            }
        }

        public Vector3 GetEggAttachmentPosition() => transform.Find("EggAttachment").position;

        private void ResetTransforms()
        {
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
