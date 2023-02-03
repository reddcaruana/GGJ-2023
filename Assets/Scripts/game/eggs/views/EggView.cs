using UnityEngine;
using Assets.Scripts.game.eggs.data;

namespace Assets.Scripts.game.eggs.views
{
    public class EggView : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Set(int id)
        {
            if (!EggData.TryFind(id, out EggData data))
            {
                Debug.LogError("[EggView] unable to find Egg data for Id: " + id);
                return;
            }

            spriteRenderer.sprite = data.GetSprite();
        }
    }
}
