using System.Collections.Generic;

namespace Assets.Scripts.game.eggs
{
    public static class EggManager
    {
        private static List<Egg> eggs = new List<Egg>();

        public static Egg Spawn()
        {
            var egg = Find();
            egg.Spawn();
            return egg;
        }

        private static Egg Find()
        {
            for (int i = 0; i < eggs.Count; i++)
                if (!eggs[i].IsSpawned)
                    return eggs[i];

            var egg = new Egg();
            egg.CreateView(null);
            eggs.Add(egg);

            return egg;
        }
    }
}
