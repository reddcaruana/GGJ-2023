using Assets.Scripts.game.directions.data;

namespace Assets.Scripts.game.grabbers.data
{
    public class PassToGrabberData
    {
        public readonly DirectionData DirectionData;
        public readonly Grabber Grabber;

        public PassToGrabberData(DirectionData directionData, Grabber grabber)
        {
            DirectionData = directionData;
            Grabber = grabber;
        }
    }
}
