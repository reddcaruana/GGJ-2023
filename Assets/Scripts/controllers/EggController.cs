using UnityEngine;
using Assets.Scripts.game.eggs;
using System.Collections.Generic;
using Assets.Scripts.game.eggs.data;
using Assets.Scripts.game.grabbers.views;
using System.Runtime.InteropServices;
using System;
using Assets.Scripts.utils;
using System.Collections;
using System.Linq;
using Random = UnityEngine.Random;

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
                egg.SetGood();
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
            foreach (Egg e in eggs.Where(e => !e.IsSpawned))
                return e;

            var egg = new Egg();
            egg.CreateView(GameController.ME.LevelManager.transform);
            eggs.Add(egg);

            return egg;
        }

        private static List<EggData> GetDataInUse()
            => (from egg in eggs where egg.IsSpawned select egg.Data).ToList();

        public static int ActiveCount()
            => eggs.Count(t => t.IsActive);

        public static bool CheckForCollision(Egg egg)
        {
            if (egg.IsDelivery) return false;

            foreach (Egg e in eggs)
            {
                if (!e.IsSpawned || e.IsDelivery || e == egg || !egg.OnCollisionCourse(e)) continue;

                var egg1Pos = e.GetCurrentPosition();
                var egg2Pos = egg.GetCurrentPosition();

                var distance = Vector3.Distance(egg1Pos, egg2Pos) / 2f;

                var duration = Egg.DurationToPosition(distance);

                var axis = e.DirectionData.Axis;
                var direction = e.DirectionData.DirectionMultiplier;
                egg1Pos[axis] += distance * direction;

                axis = egg.DirectionData.Axis;
                direction = egg.DirectionData.DirectionMultiplier;
                egg2Pos[axis] += distance * direction;

                e.MoveToAndBreak(egg1Pos, duration);
                egg.MoveToAndBreak(egg2Pos, duration);

                return true;
            }

            return false;
        }

        public static void Reset()
        {
            eggs.Clear();
        }
    }
}
