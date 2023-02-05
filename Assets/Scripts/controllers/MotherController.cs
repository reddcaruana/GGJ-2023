using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.game.grabbers;
using Assets.Scripts.game.dispenser;

namespace Assets.Scripts.controllers
{
    public static class MotherController
    {
        private static readonly List<MotherGrabber> mothers = new List<MotherGrabber>();

        public static void Add(MotherGrabber mother) => mothers.Add(mother);

        public static MotherGrabber[] GetAvailableRandom(int count, Dispenser[] dispensers)
        {
            var availaible = GetAllAvailable();
            var result = new List<MotherGrabber>();

            const int FAIL_SAFE = 100;
            int f = 0;
            int i = 0;
            do
            {
                var m = availaible[Random.Range(0, availaible.Length)];

                if (!result.Contains(m) && !DispenserSpace(dispensers, m))
                {
                    result.Add(m);
                    ++i;
                }
                ++f;
            } while (i < count && f < FAIL_SAFE);

            if (f == FAIL_SAFE)
                Debug.LogError("[MotherController] Was unable to Fetch Random Mother Data... FAIL SAFE");

            return result.ToArray();
        }

        private static bool DispenserSpace(Dispenser[] dispensors, MotherGrabber motrher)
        {
            for (int i = 0; i < dispensors.Length; i++)
                if (dispensors[i].MotherOnSameSide == motrher)
                    return true;
            return false;
        }

        private static MotherGrabber[] GetAllAvailable()
        {
            var result = new List<MotherGrabber>();
            for (int i = 0; i < mothers.Count; i++)
                if (!mothers[i].IsActive)
                    result.Add(mothers[i]);
            return result.ToArray();
        }
    }
}
