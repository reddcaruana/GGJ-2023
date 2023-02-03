using UnityEngine;
using Assets.Scripts.game.eggs;
using Assets.Scripts.game.grabbers;
using Assets.Scripts.game.eggs.data;
using Assets.Scripts.game.grabbers.data;
using Assets.Scripts.game.dispenser.view;
using Assets.Scripts.game.grabbers.views;

namespace Assets.Scripts.game.dispenser
{
    public class Dispenser
    {
        private PassToGrabberData passToGrabber;

        public TargetGrabber Grabber { get; private set; } = new TargetGrabber();

        private bool HasEgg => egg != null;
        private Egg egg;

        private DispensorView view;

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

            CreateGrabberView(parent);
        }

        private void CreateGrabberView(Transform parent)
        {
            var prefab = Resources.Load<GameObject>("Prefabs/Grabbers/GrabberView");
            var view = MonoBehaviour.Instantiate(prefab, parent).AddComponent<GrabberView>();
            Grabber.SetView(view);
        }

        public void GenerateEgg()
        {
            egg = EggManager.Spawn();
            egg.Set(EggData.GetRandom().Id);
            egg.UpdateView();
        }

        public void Pass()
        {
            if (!HasEgg)
            {
                Debug.LogError("[Dispenser] Something went wrong... trying to pass null egg...");
                return;
            }

            var grabber = passToGrabber.Grabber;
            egg.MoveTo(view.transform.position, grabber.GetPosition(), grabber.Receive);
            egg = null;
        }
    }
}
