using System.Linq;
using Assets.Scripts.game.eggs.data;

namespace Assets.Scripts.controllers
{
    public static class ScoreController
    {
        private static int[] score = new int[EggData.Count];

        public static void IncrementScore(int eggDataId) => score[eggDataId] += 1;

        public static int TotalScore => score.Sum();

        public static void Reset()
        {
            for (int i = 0; i < score.Length; i++)
                score[i] = 0;
        }
    }
}
