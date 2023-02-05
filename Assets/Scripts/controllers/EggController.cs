using UnityEngine;
using Assets.Scripts.game.eggs;
using System.Collections.Generic;
using Assets.Scripts.game.eggs.data;
using Assets.Scripts.game.grabbers.views;
using System.Runtime.InteropServices;
using System;
using Assets.Scripts.utils;
using System.Collections;

namespace Assets.Scripts.controllers
{
    public static class EggController
    {
        private static List<Egg> eggs = new List<Egg>();

        public static Egg[] GetAvailable(int count)
        {
            var dataInUse = GetDataInUse();
            var data = new List<EggData>();

            const int FAIL_SAFE = 100;
            int f = 0;
            int i = 0;
            do
            {
                var e = EggData.GetRandom();
                if (!dataInUse.Contains(e) && !data.Contains(e))
                {
                    data.Add(e);
                    ++i;
                }
                ++f;
            } while (i < count && f < FAIL_SAFE);

            if (f == FAIL_SAFE)
                Debug.LogError("[EggController] Was unable to Fetch Random egg Data... FAIL SAFE");

            var eggs = new Egg[data.Count];
            for (int j = 0; j < data.Count; j++)
            {
                var egg = Spawn();
                egg.Set(data[j]);
                egg.SetIdle();
                eggs[j] = egg;
            }

            return eggs;
        }

        public static Egg Spawn()
        {
            var egg = Find();
            egg.Spawn();
            return egg;
        }

        private static Egg Find()
        {
            for (int i = 0; i < eggs.Count; i++)
                if (!eggs[i].IsSpawned)
                    return eggs[i];

            var egg = new Egg();
            egg.CreateView(null);
            eggs.Add(egg);

            return egg;
        }

        private static List<EggData> GetDataInUse()
        {
            var result = new List<EggData>();
            for (int i = 0; i < eggs.Count; i++)
                if (eggs[i].IsSpawned)
                    result.Add(eggs[i].Data);

            return result;
        }

        public static int ActiveCount()
        {
            var count = 0;
            for (int i = 0; i < eggs.Count; i++)
                if (eggs[i].IsActive)
                    ++count;

            return count;
        }

        public static bool CheckForCollision(Egg egg)
        {
            for (int i = 0; i < eggs.Count; i++)
            {
                var e = eggs[i];
                if (!e.IsSpawned || e == egg || !egg.OnCollisiosnCourse(e)) continue;

                var egg1Pos = e.GetCurrentPosition();
                var egg2Pos = egg.GetCurrentPosition();

                var distance = Vector3.Distance(egg1Pos, egg2Pos);

                distance = distance / 2f;
                var duration = Egg.DurationToPosition(distance);

                var axis = e.DirectionData.Axis;
                var direction = e.DirectionData.DirectionMultiplier;
                egg1Pos[axis] += distance * direction;

                axis = egg.DirectionData.Axis;
                direction = egg.DirectionData.DirectionMultiplier;
                egg2Pos[axis] += distance * direction;

                e.MoveToAndBreack(egg1Pos, duration);
                egg.MoveToAndBreack(egg2Pos, duration);

                return true;
            }

            return false;
        }
    }
}
