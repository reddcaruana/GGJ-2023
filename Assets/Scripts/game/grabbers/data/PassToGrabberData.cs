namespace Assets.Scripts.game.grabbers.data
{
    public class PassToGrabberData
    {
        public readonly DirectionType Direction;
        public readonly Grabber Grabber;

        public PassToGrabberData(DirectionType direction, Grabber grabber)
        {
            Direction = direction;
            Grabber = grabber;
        }
    }
}
