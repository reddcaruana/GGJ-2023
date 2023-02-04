using Assets.Scripts.game.eggs;
using Assets.Scripts.controllers;
using System.Collections.Generic;
using Assets.Scripts.game.eggs.data;

namespace Assets.Scripts.game.grabbers
{
    public class MotherGrabber : Grabber
    {
        public enum PositionAlignmentType { BottomRightRight, BottomRight, BottomLeft, BottomLeftLeft, UpperLeftLeft, UpperLeft, UpperRight, UpperRightRight }
        private static List<MotherGrabber> mothers = new List<MotherGrabber>();

        public bool IsActive => !EggData.NoEgg.Compare(eggId);
        private int eggId;

        public MotherGrabber() => mothers.Add(this);

        public void SetSpriteData(EggData eggData)
        {
            eggId = eggData.Id;

            if (IsActive)
            {
                spriteData = eggData.GetMotherSprite();
                View.SetIdle(spriteData.GetIdleSprite());
                View.Enter();
            }
            else
                Leave();
        }

        public void FixPosition(PositionAlignmentType positionType)
        {
            var position = View.transform.parent.position;

            switch (positionType)
            {
                case PositionAlignmentType.BottomRightRight: position.x = (ViewController.SizX / 2f) - (View.GetBounds().x / 2f); break;
                case PositionAlignmentType.BottomRight: position.y = -(ViewController.SizY / 2f) + (View.GetBounds().y / 2f); break;
                case PositionAlignmentType.BottomLeft: position.y = -(ViewController.SizY / 2f) + (View.GetBounds().y / 2f); break;
                case PositionAlignmentType.BottomLeftLeft: position.x = -(ViewController.SizX / 2f) + (View.GetBounds().x / 2f); break;
                case PositionAlignmentType.UpperLeftLeft: position.x = -(ViewController.SizX / 2f) + (View.GetBounds().x / 2f); break;
                case PositionAlignmentType.UpperLeft: position.y = (ViewController.SizY / 2f) - (View.GetBounds().y / 2f); break;
                case PositionAlignmentType.UpperRight: position.y = (ViewController.SizY / 2f) - (View.GetBounds().y / 2f); break;
                case PositionAlignmentType.UpperRightRight: position.x = (ViewController.SizX / 2f) - (View.GetBounds().x / 2f); break;
            }

            View.transform.parent.position = position;
        }

        public override void Receive(Egg egg)
        {
            this.egg = egg;

            if (egg.Data.Id == eggId) 
            {
                ScoreController.IncrimentScore(egg.Data.Id);
                this.egg.Despawn();
                Leave();
            }
            else
                this.egg.Break();

            this.egg = null;
            GameController.ME.LevelManager.Dispense();
        }

        public void Leave()
        {
            eggId = EggData.NoEgg.Id;
            View.Exit();
        }

        public static bool TryGetAvailableMother(out MotherGrabber mother)
        {
            mother = GetAvailableMother();
            return mother != null;
        }

        public static MotherGrabber GetAvailableMother()
        {
            for (int i = 0; i < mothers.Count; i++)
                if (!mothers[i].IsActive)
                    return mothers[i];
            return null;
        }
    }
}
