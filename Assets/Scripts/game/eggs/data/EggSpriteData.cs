using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.game.eggs.data
{
    public class EggSpriteData
    {
        private const string SPRITES_PATH = "Sprites/Eggs";
        private static readonly List<EggSpriteData> eggSpriteData = new List<EggSpriteData>();

        public static readonly EggSpriteData EggRed = new EggSpriteData("EggRed");
        public static readonly EggSpriteData EggBlue = new EggSpriteData("EggBlue");
        public static readonly EggSpriteData EggGreen = new EggSpriteData("EggGreen");
        public static readonly EggSpriteData EggPurple = new EggSpriteData("EggPurple");

        public readonly int Id;
        public readonly string Alias;

        public EggSpriteData(string alias)
        {
            Id = eggSpriteData.Count;
            Alias = alias;
            eggSpriteData.Add(this);
        }

        private string FullPath() => $"{SPRITES_PATH}/{Alias}/";

        public Sprite GetGoodSprite() => Resources.Load<Sprite>(FullPath() + Alias + "Good");
        public Sprite GetBadSprite() => Resources.Load<Sprite>(FullPath() + Alias + "Bad");
    }
}
