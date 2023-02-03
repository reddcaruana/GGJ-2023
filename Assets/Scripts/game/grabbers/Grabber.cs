using UnityEngine;
using Assets.Scripts.game.eggs;
using Assets.Scripts.game.grabbers.views;

namespace Assets.Scripts.game.grabbers
{
    public enum DirectionType { Left, Right, Up, Down }  

    public abstract class Grabber
    {
        protected Egg egg;
        public bool HasEgg => egg != null;
        private GrabberView view;

        public void SetView(GrabberView grabberView) => view = grabberView;

        public void Receive(Egg egg)
        {
            this.egg = egg;
            OnRevevied();
        }

        protected abstract void OnRevevied();

        public Vector3 GetPosition() => view.transform.position;
    }
}
