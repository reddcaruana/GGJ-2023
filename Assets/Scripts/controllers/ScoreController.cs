using Assets.Scripts.game.eggs.data;

namespace Assets.Scripts.controllers
{
    public static class ScoreController
    {
        private static int[] score = new int[EggData.Count];

        public static void IncrimentScore(int eggDataId) => score[eggDataId] += 1;

        public static int TotalScore() 
        {
            int total = 0;
            for (int i = 0; i < score.Length; i++)
                total += score[i];
            return total;
        }

        public static void Reset()
        {
            for (int i = 0; i < score.Length; i++)
                score[i] = 0;
        }
    }
}
