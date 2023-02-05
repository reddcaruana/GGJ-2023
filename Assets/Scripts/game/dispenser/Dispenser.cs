using UnityEngine;
using Assets.Scripts.game.eggs;
using Assets.Scripts.controllers;
using Assets.Scripts.game.grabbers.data;
using Assets.Scripts.game.dispenser.view;
using Assets.Scripts.game.stork;

namespace Assets.Scripts.game.dispenser
{
    public class Dispenser
    {

        private PassToGrabberData passToGrabber;
        private DispensorView view;

        private bool HasEgg => egg != null;
        private Egg egg;

        public Dispenser() => DispensorController.Add(this);

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
            egg.Dispense(passToGrabber.DirectionData, view.transform.position, grabber.GetPosition(), grabber.Receive);
            egg = null;
        }
    }
}
