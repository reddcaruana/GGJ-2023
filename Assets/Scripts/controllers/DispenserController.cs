using Assets.Scripts.game.dispenser;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.controllers
{
    public static class DispenserController
    {
        private static readonly List<Dispenser> dispensers = new List<Dispenser>();

        private static DispenseCountData Level1 = new DispenseCountData(3, 1);
        private static DispenseCountData Level2 = new DispenseCountData(7, 2);
        private static DispenseCountData Level3 = new DispenseCountData(11, 3);
        
        public static int GetDispenseCount()
        {
            var score = ScoreController.TotalScore;
            Debug.Log("*-* SCORE: " + score);

            if (score <= Level1.TargetScore)
                return Level1.DispenseCount;
            
            if (score <= Level2.TargetScore)
                return Level2.DispenseCount;
            
            return Level3.DispenseCount;
        }

        public static void Add(Dispenser dispenser) => dispensers.Add(dispenser);

        public static Dispenser[] GetAvailableRandom(int count)
        {
            var result = dispensers.Where(d => !d.HasEgg).ToList();
            result.Sort((a, b) => Random.Range(-1, 2));

            return result.Take(count).ToArray();
        }

        public static void Reset()
        {
            dispensers.Clear();
        }
    }

    public class DispenseCountData
    {
        public readonly int TargetScore;
        public readonly int DispenseCount;

        public DispenseCountData(int targetScore, int dispenseCount)
        {
            TargetScore = targetScore;
            DispenseCount = dispenseCount;
        }
    }
}
