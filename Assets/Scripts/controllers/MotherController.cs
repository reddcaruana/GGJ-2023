using UnityEngine;
using System.Collections.Generic;
using System.Linq;
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
            var available = GetAllAvailable();
            var result = new List<MotherGrabber>();

            const int FAIL_SAFE = 100;
            int f = 0;
            int i = 0;
            do
            {
                var m = available[Random.Range(0, available.Length)];

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

        private static bool DispenserSpace(Dispenser[] dispensers, MotherGrabber mother)
            => dispensers.Any(dispenser => dispenser.MotherOnSameSide == mother);

        private static MotherGrabber[] GetAllAvailable()
            => mothers.Where(mother => !mother.IsActive).ToArray();

        public static void Reset()
        {
            mothers.Clear();
        }
    }
}
