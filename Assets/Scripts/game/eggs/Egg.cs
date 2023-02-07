using System;
using DG.Tweening;
using UnityEngine;
using Assets.Scripts.statics;
using Assets.Scripts.game.stork;
using Assets.Scripts.game.grabbers;
using Assets.Scripts.game.eggs.data;
using Assets.Scripts.game.eggs.views;
using Assets.Scripts.game.directions.data;
using Assets.Scripts.utils;

namespace Assets.Scripts.game.eggs
{
    public class Egg
    {
        private Action<Egg> arrivedCallback;
        private Tweener tweener;

        public EggData Data { get; private set; }
        public MotherGrabber Mother { get; private set; }
        public bool IsSpawned { get; private set; }
        public bool IsActive { get; private set; }

        public DirectionData DirectionData { get; private set; }
        private Vector3 from;
        private Vector3 to;

        private EggView view;
        public bool IsDelivery { get; private set; }

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
            IsActive = true;
            view.gameObject.SetActive(true);
        }

        public void SetGood()
        {
            view.SortOrder(0);
            view.SetGood(Data.SpriteData.GetGoodSprite());
        }

        public void Break()
        {
            PauseUtils.TryPause(0.3f);
            IsActive = false;

            Mother.EggBroken();
            view.SortOrder(5);
            view.Break(Data.SpriteData.GetBadSprite(), Despawn);
        }

        public void ArrivedToMother()
        {
            IsActive = false;
            Despawn();
        }

        private void Despawn()
        {
            view.gameObject.SetActive(false);
            IsSpawned = false;
        }

        public void MoveTo(DirectionData directionData, Vector3 from, Vector3 to, Action<Egg> arrivedCallback)
        {
            var duration = DurationToPosition(Vector2.Distance(from, to));
            MoveTo(directionData, from, to, duration, arrivedCallback);
        }

        public void Dispense(DirectionData directionData, Vector3 from, Vector3 to, Action<Egg> arrivedCallback)
        {
            IsDelivery = true;
            var duration = DurationToDispose(Vector2.Distance(from, to));
            var stork = Stork.GetStork();
            stork.Deliver(from, to, duration);
            MoveTo(directionData, from, to, duration, OnComplete);

            void OnComplete(Egg egg)
            {
                IsDelivery = false;
                Mother.SetSpriteData(egg.Data);
                arrivedCallback(egg);
            }
        }

        public void MoveTo(DirectionData directionData, Vector3 from, Vector3 to, float duration, Action<Egg> arrivedCallback)
        {
            this.arrivedCallback = arrivedCallback;
            this.from = from;
            this.to = to;

            DirectionData = directionData;

            view.MoveTo(from, to, duration, OnComplete);
            void OnComplete() => this.arrivedCallback(this);
        }

        public void MoveToAndBreak(Vector3 to, float duration)
        {
            KillAnimation();
            arrivedCallback = null;
            view.MoveTo(from, to, duration, Break);
        }

        public void MouthPosition(Vector3 vector3) => view.transform.position = vector3;

        public bool OnCollisionCourse(Egg egg) => egg.to == from;

        public Vector3 GetCurrentPosition() => view.transform.position;

        private void KillAnimation() => tweener?.Kill();

        public static float DurationToPosition(float distance) => distance / GameStatics.EGG_PLAYER_SPEED;
        public static float DurationToDispose(float distance) => distance / GameStatics.EGG_DISPENSER_SPEED;
    }
}
