using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.game.eggs.data
{
    public class EggData
    {
        private static readonly List<EggData> eggs = new List<EggData>();

        public static readonly EggData EggRed = new EggData("EggRed");
        public static readonly EggData EggBlue = new EggData("EggBlue");
        public static readonly EggData EggGreen = new EggData("EggGreen");
        public static readonly EggData EggYellow = new EggData("EggYellow");

        public int Id;
        public string Alias;

        public EggData(string alias)
        {
            Id = eggs.Count;
            Alias = alias;
            eggs.Add(this);
        }

        public bool Compare(int id) => Id == id;

        public Sprite GetSprite() => Resources.Load<Sprite>("Sprites/Eggs/" + Alias);

        public Sprite GetMotherSprite() => Resources.Load<Sprite>("Sprites/Mothers/" + Alias);

        public static bool TryFind(int id, out EggData data)
        {
            data = Find(id);
            return true;
        }

        public static EggData Find(int id)
        {
            for (int i = 0; i < eggs.Count; i++)
                if (eggs[i].Compare(id))
                    return eggs[i];
            return null;
        }

        public static EggData GetRandom() => eggs[Random.Range(0, eggs.Count)];
    }
}
