using UnityEngine;
using System;
using Assets.Scripts.utils;
using Assets.Scripts.game.eggs.data;

namespace Assets.Scripts.game.eggs.views
{
    public class EggView : SpriteObject
    {
        public void Set(EggData data) => SpriteRenderer.sprite = data.GetSprite();

        public void Break(Action onComplete)
        {
            Debug.LogError("*-* Break!!!!!");
            onComplete();
        }
    }
}
