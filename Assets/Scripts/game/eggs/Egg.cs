﻿using System;
using DG.Tweening;
using UnityEngine;
using Assets.Scripts.statics;
using Assets.Scripts.game.grabbers;
using Assets.Scripts.game.eggs.data;
using Assets.Scripts.game.eggs.views;

namespace Assets.Scripts.game.eggs
{
    public class Egg
    {
        public EggData Data { get; private set; }
        public MotherGrabber Mother { get; private set; }
        public bool IsSpawned { get; private set; }

        public DirectionType DirectionType { get; private set; }
        private Vector3 from;
        private Vector3 to;

        private EggView view;

        public void Set(EggData data) => Data = data;
        public void SetMother(MotherGrabber mother) => Mother = mother;

        public void CreateView(Transform parent)
        {
            if (view != null)
            {
                Debug.LogError("[Egg] Already Has View");
                return;
            }

            var prefab = Resources.Load<GameObject>("Prefabs/Eggs/EggView");
            view = MonoBehaviour.Instantiate(prefab, parent).AddComponent<EggView>();
        }

        public void Spawn()
        {
            IsSpawned = true;
            view.gameObject.SetActive(true);
        }

        public void Break()
        {
            Mother.Leave();
            view.Break(() => Despawn());
        }

        public void Despawn()
        {
            IsSpawned = false;
            view.gameObject.SetActive(false);
        }

        public void UpdateView() => view.Set(Data);

        public void MoveTo(DirectionType directionType, Vector3 from, Vector3 to, Action<Egg> arrivedCallback)
        {
            var duration = DurationToPosition(Vector2.Distance(from, to));
            MoveTo(directionType, from, to, duration, arrivedCallback);
        }

        public void Dispense(DirectionType directionType, Vector3 from, Vector3 to, Action<Egg> arrivedCallback)
        {
            var duration = DurationToDispose(Vector2.Distance(from, to));
            MoveTo(directionType, from, to, duration, arrivedCallback);
        }

        public void MoveTo(DirectionType directionType, Vector3 from, Vector3 to, float duration, Action<Egg> arrivedCallback)
        {
            this.from = from;
            this.to = to;

            DirectionType = directionType;

            view.transform.
                DOMove(to, duration).
                From(from).
                SetEase(Ease.Linear).
                OnComplete(OnComplete);

            void OnComplete() => arrivedCallback(this);
        }

        public void MouthPosition(Vector3 vector3) => view.transform.position = vector3;

        public bool OnCollisiosnCourse(Egg egg) => egg.to == from;

        public void MoveToAndBreack(Vector3 to, float duration)
        {
            MoveTo(DirectionType, view.transform.position, to, duration, OnArrived);
            void OnArrived(Egg _) => Break();
        }

        public Vector3 GetCurrentPosition() => view.transform.position;

        public static float DurationToPosition(float distance) => distance / GameStatics.EGG_PLAYER_SPEED;
        public static float DurationToDispose(float distance) => distance / GameStatics.EGG_DISPENSER_SPEED;
    }
}
