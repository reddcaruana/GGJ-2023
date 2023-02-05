using UnityEngine;

namespace Assets.Scripts.game.directions.data
{
    public enum DirectionType { Left, Right, Up, Down }

    public class DirectionData
    {
        public static readonly DirectionData Left = new DirectionData(DirectionType.Left, 0, -1, 0f);
        public static readonly DirectionData Right = new DirectionData(DirectionType.Right, 0, 1, 180f);
        public static readonly DirectionData Up = new DirectionData(DirectionType.Up, 1, 1, 270f);
        public static readonly DirectionData Down = new DirectionData(DirectionType.Down, 1, -1, 90f);

        public readonly DirectionType DirectionType;
        public readonly int Axis;
        public readonly int DirectionMultiplier;
        public readonly Quaternion Quaternion;

        public DirectionData(DirectionType directionType, int axis, int directionMultiplier, float zAxisRotation)
        {
            DirectionType = directionType;
            Axis = axis;
            DirectionMultiplier = directionMultiplier;
            Quaternion = Quaternion.Euler(0f, 0f, zAxisRotation);
        }

        public bool Compare(DirectionData directionData) => Compare(directionData.DirectionType);
        public bool Compare(DirectionType directionType) => DirectionType == directionType;

    }
}
