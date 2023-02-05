using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.game.grabbers.data;

namespace Assets.Scripts.game.eggs.data
{
    public class EggData
    {
        private static readonly List<EggData> eggs = new List<EggData>();
        public static int Count => eggs.Count;

        public static readonly EggData NoEgg = new EggData("NUll", null);
        public static readonly EggData EggRed = new EggData("EggRed", EggSpriteData.EggRed);
        public static readonly EggData EggBlue = new EggData("EggBlue", EggSpriteData.EggBlue);
        public static readonly EggData EggGreen = new EggData("EggGreen", EggSpriteData.EggGreen);
        public static readonly EggData EggYellow = new EggData("EggYellow", EggSpriteData.EggPurple);

        public readonly int Id;
        public readonly string Alias;
        public readonly EggSpriteData SpriteData;


        public EggData(string alias, EggSpriteData spriteData)
        {
            Id = eggs.Count;
            Alias = alias;
            SpriteData = spriteData;
            eggs.Add(this);
        }

        public bool Compare(int id) => Id == id;

        public GrabberSpriteData GetMotherSprite() => GrabberSpriteData.GetMotherSpriteData(Id);

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

        public static EggData GetRandom() => eggs[Random.Range(1, eggs.Count)];
    }
}
