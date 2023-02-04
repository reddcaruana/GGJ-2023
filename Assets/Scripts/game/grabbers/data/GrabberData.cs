using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.game.grabbers.data
{
    public class GrabberData
    {
        private static readonly List<GrabberData> grabbers = new List<GrabberData>();
        private static readonly List<GrabberData> mothers = new List<GrabberData>();

        public static readonly GrabberData Grabber0 = new GrabberData("BirdRed", isMother: false);
        public static readonly GrabberData Grabber1 = new GrabberData("BirdBlue", isMother: false);
        public static readonly GrabberData Grabber2 = new GrabberData("BirdGreen", isMother: false);
        public static readonly GrabberData Grabber3 = new GrabberData("BirdYellow", isMother: false);

        public static readonly GrabberData Mother0 = new GrabberData("Mother0", isMother: true);
        public static readonly GrabberData Mother1 = new GrabberData("Mother1", isMother: true);
        public static readonly GrabberData Mother2 = new GrabberData("Mother2", isMother: true);
        public static readonly GrabberData Mother3 = new GrabberData("Mother3", isMother: true);

        public readonly int Id;
        public readonly string Alias;
        public readonly bool IsMother;
        public readonly PassToGrabberData[] PassToData;

        public GrabberData(string alias, bool isMother)
        {
            Id = grabbers.Count;
            Alias = alias;
            IsMother = isMother;

            grabbers.Add(this);
            if (IsMother) mothers.Add(this);
        }

        public bool Compare(GrabberData data) => Compare(data.Id);
        public bool Compare(int id) => Id == id;

        public static bool TryFind(int id, out GrabberData data)
        {
            data = Find(id);
            return true;
        }

        public static GrabberData Find(int id)
        {
            for (int i = 0; i < grabbers.Count; i++)
                if (grabbers[i].Compare(id))
                    return grabbers[i];
            return null;
        }

        public static GrabberData GetRandom() => grabbers[Random.Range(0, grabbers.Count)];
        public static GrabberData GetRandomMother() => mothers[Random.Range(0, mothers.Count)];
    }
}
