using UnityEngine;
using Assets.Scripts.game.grabbers.data;

namespace Assets.Scripts.game.grabbers
{
    public class PlayerGrabbor : Grabber
    {
        private PassToGrabberData[] passToGrabbers;

        public void Set(PassToGrabberData[] passToGrabbers) => this.passToGrabbers = passToGrabbers;

        public void PassTo(DirectionType directionType)
        {
            if (!HasEgg) return;

            if (!TryFind(directionType, out Grabber grabber))
            {
                Debug.LogError("[Grabber] Did not find Grabber for Direction: " + directionType);
                return;
            }

            egg.MoveTo(GetPosition(), grabber.GetPosition(), grabber.Receive);
            egg = null;
        }

        protected override void OnRevevied()
        {
            var passTo = passToGrabbers[Random.Range(0, passToGrabbers.Length)];
            Debug.Log("*-* passing To: " + passTo.Direction);
            PassTo(passTo.Direction);
        }

        private bool TryFind(DirectionType directionType, out Grabber grabber)
        {
            grabber = Find(directionType);
            return grabber != null;
        }

        private Grabber Find(DirectionType directionType)
        {
            for (int i = 0; i < passToGrabbers.Length; i++)
                if (passToGrabbers[i].Direction == directionType)
                    return passToGrabbers[i].Grabber;
            return null;
        }
    }
}
