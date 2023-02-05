using Assets.Scripts.game.dispenser;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.controllers
{
    public static class DispensorController
    {
        private static readonly List<Dispenser> dispensors = new List<Dispenser>();

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

        public static void Add(Dispenser dispenser) => dispensors.Add(dispenser);

        public static Dispenser[] GetAvailableRandom(int count)
        {
            var result = new List<Dispenser>();

            const int FAIL_SAFE = 100;
            int f = 0;
            int i = 0;
            do
            {
                var d = dispensors[Random.Range(0, dispensors.Count)];
                if (!result.Contains(d))
                {
                    result.Add(d);
                    ++i;
                }
                ++f;
            } while (i < count && f < FAIL_SAFE);

            if (f == FAIL_SAFE)
                Debug.LogError("[DispensorController] Was unable to Fetch Random Dispenser Data... FAIL SAFE");


            return result.ToArray();
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
