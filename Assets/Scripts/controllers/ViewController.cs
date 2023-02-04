using UnityEngine;

namespace Assets.Scripts.controllers
{
    public static class ViewController
    {
        public static float SizX { get; private set; }
        public static float SizY { get; private set; }

        public static void Updatedata()
        {
            var camera = Camera.main;
            SizY = camera.orthographicSize * 2f;
            SizX = SizY * camera.aspect;
        }
    }
}
