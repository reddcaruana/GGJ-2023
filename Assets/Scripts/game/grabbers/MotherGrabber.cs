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

        public void SetPositionType(PositionAlignmentType positionType) => View.SetPositionType(positionType);

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
            {
                View.ExitNoAnim();
                egg = null;
            }
        }

        public override void Receive(Egg egg)
        {
            this.egg = egg;

            if (egg.Data.Id == expectedEggId) 
            {
                ScoreController.IncrimentScore(egg.Data.Id);
                this.egg.ArrivedToMother();
                Happy();
            }
            else
                this.egg.Break();
        }

        private void Happy() => View.Happy(spriteData.GetGoodSprite(), Leave);

        public void EggBroken() => View.Sad(spriteData.GetBadSprite(), Leave);

        public void Leave()
        {
            GameController.ME.LevelManager.Dispense();
            egg = null;
            View.Exit(spriteData.GetFallingSprite(), Continue);
            void Continue() => expectedEggId = EggData.NoEgg.Id;
        }
    }
}
