using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.game.eggs.data;

namespace Assets.Scripts.game.grabbers.data
{
    public class GrabberSpriteData
    {
        private const string SPRITES_PATH = "Sprites/Grabbers";
        private static readonly List<GrabberSpriteData> grabberSpriteData = new List<GrabberSpriteData>();

        public static readonly GrabberSpriteData BirdRed = new GrabberSpriteData("BirdRed");
        public static readonly GrabberSpriteData BirdBlue = new GrabberSpriteData("BirdBlue");
        public static readonly GrabberSpriteData BirdGreen = new GrabberSpriteData("BirdGreen");
        public static readonly GrabberSpriteData BirdPurple = new GrabberSpriteData("BirdPurple");

        public static readonly GrabberSpriteData MotherRed = new GrabberSpriteData("MotherRed");
        public static readonly GrabberSpriteData MotherBlue = new GrabberSpriteData("MotherBlue");
        public static readonly GrabberSpriteData MotherGreen = new GrabberSpriteData("MotherGreen");
        public static readonly GrabberSpriteData MotherPurple = new GrabberSpriteData("MotherPurple");

        public readonly int Id;
        public readonly string Alias;

        public GrabberSpriteData(string alias)
        {
            Id = grabberSpriteData.Count;
            Alias = alias;
            grabberSpriteData.Add(this);
        }

        private string FullPath() => $"{SPRITES_PATH}/{Alias}/";

        public Sprite GetIdleSprite() => Resources.Load<Sprite>(FullPath() + Alias + "Idle");
        public Sprite GetRecieveSprite() => Resources.Load<Sprite>(FullPath() + Alias + "Hold");
        public Sprite GetPassSprite() => Resources.Load<Sprite>(FullPath() + Alias + "Shoot");
        public Sprite GetHitSprite() => Resources.Load<Sprite>(FullPath() + Alias + "Hit");
        public Sprite GetFallingSprite() => Resources.Load<Sprite>(FullPath() + Alias + "Falling");
        public Sprite GetGoodSprite() => Resources.Load<Sprite>(FullPath() + Alias + "Good");
        public Sprite GetBadSprite() => Resources.Load<Sprite>(FullPath() + Alias + "Bad");

        public static GrabberSpriteData GetMotherSpriteData(int eggId)
        {
            if (EggData.EggRed.Compare(eggId))
                return MotherRed;

            if (EggData.EggBlue.Compare(eggId))
                return MotherBlue;

            if (EggData.EggGreen.Compare(eggId))
                return MotherGreen;

            if (EggData.EggYellow.Compare(eggId))
                return MotherPurple;

            Debug.LogError("[GrabberSpriteData] Could not find Mother to match with Egg id: " + eggId);
            return MotherRed;
        }
    }

}
