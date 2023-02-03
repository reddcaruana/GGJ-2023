using UnityEngine;

namespace Assets.Scripts.game.grabbers
{
    public class TargetGrabber : Grabber
    {
        protected override void OnRevevied()
        {
            Debug.Log("*-* Received!");
            egg.Despawn();
            egg = null;
            GameController.ME.Level.Test();
        }
    }
}
