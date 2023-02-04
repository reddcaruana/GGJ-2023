using UnityEngine;
using Assets.Scripts.utils;
using Assets.Scripts.game.eggs;
using Assets.Scripts.game.grabbers.data;
using Assets.Scripts.controllers;

namespace Assets.Scripts.game.grabbers
{
    public class PlayerGrabber : Grabber
    {
        protected PassToGrabberData[] PassToData { get; private set; }
        public void Set(PassToGrabberData[] passToData) => PassToData = passToData;

        public void SetSpriteData(GrabberSpriteData data)
        {
            spriteData = data;
            View.SetIdle(data.GetIdleSprite());
            View.CenterSprite();
        }

        public void PassTo(DirectionType directionType)
        {
            if (!HasEgg) return;

            if (!TryFindOtherGrabber(directionType, out Grabber grabber))
            {
                Debug.LogError("[Grabber] Did not find GrabberData for Direction: " + directionType);
                return;
            }

            View.SetPass(spriteData.GetPassSprite(), directionType, OnPassed);
            void OnPassed() => View.SetIdle(spriteData.GetIdleSprite());

            egg.MoveTo(directionType, GetPosition(), grabber.GetPosition(), grabber.Receive);
            egg = null;
        }

        public override void Receive(Egg egg)
        {
            if (HasEgg)
            {
                egg.Break();
                this.egg.Break();
                View.SetHit(spriteData.GetHitSprite(), OnHitDone);
                void OnHitDone() => View.SetIdle(spriteData.GetIdleSprite());
                return;
            }

            this.egg = egg;
           
            egg.MouthPosition(View.GetEggAttachmentPosition());
            View.SetReceive(spriteData.GetRecieveSprite(), egg.DirectionType);

            DebugController.ActiveGrabber = this;

            if (DebugController.Automatic)
            {
                CoroutineRunner.ME.Wait(1f, OnDoneWaiting);
                void OnDoneWaiting()
                {
                    var passTo = PassToData[Random.Range(0, PassToData.Length)];
                    PassTo(passTo.Direction);
                }
            }
        }

        private bool TryFindOtherGrabber(DirectionType directionType, out Grabber grabber)
        {
            grabber = FindOtherGrabber(directionType);
            return grabber != null;
        }

        private Grabber FindOtherGrabber(DirectionType directionType)
        {
            for (int i = 0; i < PassToData.Length; i++)
                if (PassToData[i].Direction == directionType)
                    return PassToData[i].Grabber;
            return null;
        }
    }
}
