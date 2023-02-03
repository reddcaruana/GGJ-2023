using System;
using DG.Tweening;
using UnityEngine;
using Assets.Scripts.statics;
using Assets.Scripts.game.eggs.views;

namespace Assets.Scripts.game.eggs
{
    public class Egg
    {
        public int Id { get; private set; }
        public bool IsSpawned { get; private set; }
        private EggView view;

        public void Set(int id) => Id = id;

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

        public void Despawn()
        {
            IsSpawned = false;
            view.gameObject.SetActive(false);
        }

        public void UpdateView() => view.Set(Id);

        public void MoveTo(Vector3 from, Vector3 to, Action<Egg> arrivedCallback)
        {
            var duration = Vector2.Distance(from, to) / GameStatics.EGG_SPEED;
            MoveTo(from, to, duration, arrivedCallback);
        }

        public void MoveTo(Vector3 from, Vector3 position, float duration, Action<Egg> arrivedCallback)
        {
            view.transform.
                DOMove(position, duration).
                From(from).
                SetEase(Ease.Linear).
                OnComplete(OnComplete);

            void OnComplete() => arrivedCallback(this);
        }
    }
}
