using UnityEngine;
using Assets.Scripts.game.eggs;
using System.Collections.Generic;
using Assets.Scripts.game.eggs.data;
using Assets.Scripts.game.grabbers.data;
using Assets.Scripts.game.dispenser.view;

namespace Assets.Scripts.game.dispenser
{
    public class Dispenser
    {
        private static readonly List<Dispenser> dispensors = new List<Dispenser>();

        private PassToGrabberData passToGrabber;
        private DispensorView view;

        private bool HasEgg => egg != null;
        private Egg egg;

        public Dispenser() => dispensors.Add(this);

        public void Set(PassToGrabberData passToGrabber) => this.passToGrabber = passToGrabber;

        public void CreateView(Transform parent)
        {
            if (view != null)
            {
                Debug.LogError("[Dispenser] Already has view");
                return;
            }

            var prefab = Resources.Load<GameObject>("Prefabs/Dispenser/DispenserView");
            view = MonoBehaviour.Instantiate(prefab, parent).AddComponent<DispensorView>();
        }

        public void SetEgg(Egg egg) => this.egg = egg;

        public void Pass()
        {
            if (!HasEgg)
            {
                Debug.LogError("[Dispenser] Something went wrong... Trying to pass null egg...");
                return;
            }

            var grabber = passToGrabber.Grabber;
            egg.Dispense(passToGrabber.Direction, view.transform.position, grabber.GetPosition(), grabber.Receive);
            egg = null;
        }

        public static Dispenser[] GetAvailableRandom(int count)
        {
            var result = new List<Dispenser>();

            const int FAIL_SAFE = 100;
            int i = 0;
            do
            {
                var d = dispensors[Random.Range(0, dispensors.Count)];
                if (!result.Contains(d))
                {
                    result.Add(d);
                    i++;
                }
            } while (i < count && i < FAIL_SAFE);

            Debug.Log($"*-* Count: {count}, I: {i}");
            return result.ToArray();
        }
    }
}
