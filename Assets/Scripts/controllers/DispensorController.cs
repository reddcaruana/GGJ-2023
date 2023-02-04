using UnityEngine;

namespace Assets.Scripts.controllers
{
    public static class DispensorController
    {
        private static DispensCountData Level1 = new DispensCountData(2, 1);
        private static DispensCountData Level2 = new DispensCountData(6, 2);
        private static DispensCountData Level3 = new DispensCountData(9, 3);
        public static int GetDispensCount()
        {
            var score = ScoreController.TotalScore();
            Debug.Log("*-* SCORE: " + score);

            if (score <= Level1.TargetScore)
                return Level1.DispenceCount;
            if (score <= Level2.TargetScore)
                return Level2.DispenceCount;
            
            return Level3.DispenceCount;
        }
    }

    public class DispensCountData
    {
        public readonly int TargetScore;
        public readonly int DispenceCount;

        public DispensCountData(int targetScore, int dispenceCount)
        {
            TargetScore = targetScore;
            DispenceCount = dispenceCount;
        }
    }
}
