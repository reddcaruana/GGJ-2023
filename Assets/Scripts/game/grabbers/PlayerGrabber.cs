using UnityEngine;
using Assets.Scripts.utils;
using Assets.Scripts.game.eggs;
using Assets.Scripts.controllers;
using Assets.Scripts.game.grabbers.data;
using Assets.Scripts.game.directions.data;

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

        public void PassTo(DirectionData directionData)
        {
            if (!HasEgg) return;

            if (!TryFindOtherGrabber(directionData, out Grabber grabber))
            {
                Debug.LogError("[Grabber] Did not find GrabberData for Direction: " + directionData);
                return;
            }

            View.SetPass(spriteData.GetPassSprite(), directionData, OnPassed);
            void OnPassed() => View.SetIdle(spriteData.GetIdleSprite());

            DebugController.Remove(this);

            egg.MoveTo(directionData, GetPosition(), grabber.GetPosition(), grabber.Receive);
            var e = egg;
            egg = null;

            if (EggController.CheckForCollision(e))
                GameController.ME.LevelManager.Dispense();

        }

        public override void Receive(Egg egg)
        {
            if (HasEgg)
            {
                egg.Break();
                this.egg.Break();
                this.egg = null;
                View.SetHit(spriteData.GetHitSprite(), OnHitDone);
                void OnHitDone()
                {
                    View.SetIdle(spriteData.GetIdleSprite());
                    GameController.ME.LevelManager.Dispense();
                }
                return;
            }

            this.egg = egg;
           
            egg.MouthPosition(View.GetEggAttachmentPosition());
            View.SetReceive(spriteData.GetRecieveSprite(), egg.DirectionData);

            DebugController.Add(this);

            if (DebugController.Automatic)
            {
                CoroutineRunner.ME.Wait(1f, OnDoneWaiting);
                void OnDoneWaiting()
                {
                    var passTo = PassToData[Random.Range(0, PassToData.Length)];
                    PassTo(passTo.DirectionData);
                }
            }
        }

        private bool TryFindOtherGrabber(DirectionData directionData, out Grabber grabber)
        {
            grabber = FindOtherGrabber(directionData);
            return grabber != null;
        }

        private Grabber FindOtherGrabber(DirectionData directionData)
        {
            for (int i = 0; i < PassToData.Length; i++)
                if (PassToData[i].DirectionData.Compare(directionData))
                    return PassToData[i].Grabber;
            return null;
        }
    }
}
