using Assets.Scripts.game.grabbers;

namespace Assets.Scripts.controllers
{
    public static class DebugController
    {
        public static bool Automatic { get; set; }
        public static PlayerGrabber ActiveGrabber { get; set; }
    }
}
