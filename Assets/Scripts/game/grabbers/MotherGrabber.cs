using Assets.Scripts.game.eggs;
using Assets.Scripts.controllers;
using Assets.Scripts.game.eggs.data;

namespace Assets.Scripts.game.grabbers
{
    public class MotherGrabber : Grabber
    {
        public enum PositionAlignmentType { BottomRightRight, BottomRight, BottomLeft, BottomLeftLeft, UpperLeftLeft, UpperLeft, UpperRight, UpperRightRight }

        public bool IsActive => !EggData.NoEgg.Compare(expectedEggId);
        private int expectedEggId;

        public MotherGrabber() => MotherController.Add(this);

        public void SetSpriteData(EggData eggData)
        {
            expectedEggId = eggData.Id;

            if (IsActive)
            {
                spriteData = eggData.GetMotherSprite();
                View.Enter(spriteData.GetFallingSprite(), OnEnetered);
                void OnEnetered() => View.SetIdle(spriteData.GetIdleSprite());
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

            if (egg.Data.Id == expectedEggId) 
            {
                ScoreController.IncrimentScore(egg.Data.Id);
                this.egg.ArrivedToMother();
                this.egg = null;
                Leave();
            }
            else
                this.egg.Break();

            this.egg = null;
            GameController.ME.LevelManager.Dispense();
        }

        public void Leave()
        {
            if (spriteData == null)
            {
                View.ExitNoAnim();
                Continue();
            }
            else
                View.Exit(spriteData.GetFallingSprite(), Continue);

            void Continue() => expectedEggId = EggData.NoEgg.Id;
        }
    }
}
