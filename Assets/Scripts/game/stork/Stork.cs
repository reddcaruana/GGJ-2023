using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.game.stork.views;

namespace Assets.Scripts.game.stork
{
    public class Stork
    {
        private static readonly List<Stork> storks = new List<Stork>();

        public bool IsBusy { get; private set; }
        private StorkView view;

        public void CreateView()
        {
            var prefab = Resources.Load<GameObject>("Prefabs/Storks/StorkView");
            view = MonoBehaviour.Instantiate(prefab, GameController.ME.LevelManager.transform).AddComponent<StorkView>();
        }

        public void Deliver(Vector3 from, Vector3 to, float duration)
        {
            IsBusy = true;

            var scale = Vector3.one;
            scale.x = from.x < to.x ? -1f : 1f;

            view.transform.localScale = scale;
            view.MoveTo(from, to, duration, OnDelivered);
            void OnDelivered() => Return(to, from, 0.5f);
        }

        private void Return(Vector3 from, Vector3 to, float duration)
        {
            view.MoveTo(from, to, duration, OnComplete);
            void OnComplete() => IsBusy = false;
        }

        public static Stork GetStork()
        {
            Stork stork = null;

            for (int i = 0; i < storks.Count; i++)
            {
                if (!storks[i].IsBusy)
                {
                    stork = storks[i];
                    break;
                }
            }

            if (stork == null)
            {
                stork = new Stork();
                stork.CreateView();
                storks.Add(stork);
            }

            return stork;
        }
    }
}
