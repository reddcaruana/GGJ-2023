using UnityEngine;
using Assets.Scripts.game.eggs;
using Assets.Scripts.game.grabbers.data;
using Assets.Scripts.game.grabbers.views;

namespace Assets.Scripts.game.grabbers
{
    public abstract class Grabber
    {
        protected GrabberSpriteData spriteData;

        protected Egg egg;
        public bool HasEgg => egg != null;
        protected GrabberView View { get; private set; }

        public void SetView(GrabberView grabberView, int order)
        {
            View = grabberView;
            View.SetOrder(order);
        }

        public abstract void Receive(Egg egg);

        public Vector3 GetPosition() => View.transform.position;
    }
}
