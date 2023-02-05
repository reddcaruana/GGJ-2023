using UnityEngine;
using Assets.Scripts.game.grabbers;
using Assets.Scripts.game.directions.data;

namespace Assets.Scripts.controllers
{
    public static class DebugController
    {
        public static bool Automatic { get; set; }
        private static PlayerGrabber[] ActiveGrabber { get; set; } = new PlayerGrabber[2];

        public static void Add(PlayerGrabber grabber)
        {
            if (ActiveGrabber[0] == null)
            {
                ActiveGrabber[0] = grabber;
                return;
            }

            if (ActiveGrabber[1] == null)
            {
                ActiveGrabber[1] = grabber;
                return;
            }
        }

        public static void Remove(PlayerGrabber grabber) 
        {
            if (ActiveGrabber[0] == grabber)
            {
                ActiveGrabber[0] = null;
                return;
            }

            if (ActiveGrabber[1] == grabber)
            {
                ActiveGrabber[1] = null;
                return;
            }
        }

        public static void ArrowCheks()
        {
            if (!Automatic && ActiveGrabber[0] != null)
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                    ActiveGrabber[0].PassTo(DirectionData.Left);

                else if (Input.GetKeyDown(KeyCode.RightArrow))
                    ActiveGrabber[0].PassTo(DirectionData.Right);

                else if (Input.GetKeyDown(KeyCode.UpArrow))
                    ActiveGrabber[0].PassTo(DirectionData.Up);

                else if (Input.GetKeyDown(KeyCode.DownArrow))
                    ActiveGrabber[0].PassTo(DirectionData.Down);
            }
        }

        public static void WSADCheks()
        {
            if (!Automatic && ActiveGrabber[1] != null)
            {
                if (Input.GetKeyDown(KeyCode.A))
                    ActiveGrabber[1].PassTo(DirectionData.Left);

                else if (Input.GetKeyDown(KeyCode.D))
                    ActiveGrabber[1].PassTo(DirectionData.Right);

                else if (Input.GetKeyDown(KeyCode.W))
                    ActiveGrabber[1].PassTo(DirectionData.Up);

                else if (Input.GetKeyDown(KeyCode.S))
                    ActiveGrabber[1].PassTo(DirectionData.Down);
            }
        }
    }
}
