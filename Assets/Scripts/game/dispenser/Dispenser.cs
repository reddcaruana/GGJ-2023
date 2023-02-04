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

        public Egg GenerateEgg()
        {
            egg = EggManager.Spawn();
            egg.Set(EggData.GetRandom());
            egg.UpdateView();
            return egg;
        }

        public void Pass()
        {
            if (!HasEgg)
            {
                Debug.LogError("[Dispenser] Something went wrong... Trying to pass null egg...");
                return;
            }

            var grabber = passToGrabber.Grabber;
            egg.MoveTo(passToGrabber.Direction, view.transform.position, grabber.GetPosition(), grabber.Receive);
            egg = null;
        }

        public static Dispenser GetAvailableRandom() => dispensors[Random.Range(0, dispensors.Count)];
    }
}
