using UnityEngine;

namespace Assets.Scripts.utils
{
    public abstract class SpriteObject : MonoBehaviour
    {
        protected SpriteRenderer SpriteRenderer { get; private set; }

        protected virtual void Awake()
        {
            SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }
    }
}
