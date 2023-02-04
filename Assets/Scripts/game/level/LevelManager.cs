using UnityEngine;
using Assets.Scripts.game.eggs;
using Assets.Scripts.controllers;
using Assets.Scripts.game.grabbers;
using Assets.Scripts.game.dispenser;
using Assets.Scripts.game.eggs.data;
using Assets.Scripts.game.grabbers.data;
using Assets.Scripts.game.grabbers.views;
using static Assets.Scripts.game.grabbers.MotherGrabber;

namespace Assets.Scripts.game.level
{
    public class LevelManager : MonoBehaviour
    {
        private readonly PlayerGrabber grabber0 = new PlayerGrabber();
        private readonly PlayerGrabber grabber1 = new PlayerGrabber();
        private readonly PlayerGrabber grabber2 = new PlayerGrabber();
        private readonly PlayerGrabber grabber3 = new PlayerGrabber();

        private readonly Dispenser dispenser0 = new Dispenser();
        private readonly Dispenser dispenser1 = new Dispenser();
        private readonly Dispenser dispenser2 = new Dispenser();
        private readonly Dispenser dispenser3 = new Dispenser();

        private readonly MotherGrabber motherGrabber0 = new MotherGrabber();
        private readonly MotherGrabber motherGrabber1 = new MotherGrabber();
        private readonly MotherGrabber motherGrabber2 = new MotherGrabber();
        private readonly MotherGrabber motherGrabber3 = new MotherGrabber();
        private readonly MotherGrabber motherGrabber4 = new MotherGrabber();
        private readonly MotherGrabber motherGrabber5 = new MotherGrabber();
        private readonly MotherGrabber motherGrabber6 = new MotherGrabber();
        private readonly MotherGrabber motherGrabber7 = new MotherGrabber();

        public void Init()
        {
            SetGrabbers();
            SetGrabbersViews();
            SetMotherViews();
            SetDispensers();
            SetDispensorView();
        }

        public void Dispense() => Dispense(DispensorController.GetDispensCount());

        private void Dispense(int count)
        {
            count -= EggManager.ActiveCount();

            var dispensers = Dispenser.GetAvailableRandom(count);
            var eggs = EggManager.GetAvailable(count);

            for (int i = 0; i < dispensers.Length; i++)
                DispenseInternal(dispensers[i], eggs[i]);
        }

        private void DispenseInternal(Dispenser dispenser, Egg egg)
        {
            if (!TryGetAvailableMother(out MotherGrabber mother))
            {
                Debug.LogError("[LevelManager] No Mother Was Available");
                return;
            }

            dispenser.SetEgg(egg);
            egg.SetMother(mother);
            dispenser.Pass();

            mother.SetSpriteData(egg.Data);
        }

        private void SetGrabbers()
        {
            grabber0.Set(new[]
            {
                new PassToGrabberData(DirectionType.Left, grabber1),
                new PassToGrabberData(DirectionType.Up, grabber3),
                new PassToGrabberData(DirectionType.Right, motherGrabber0),
                new PassToGrabberData(DirectionType.Down, motherGrabber1)
            });

            grabber1.Set(new[]
            {
                new PassToGrabberData(DirectionType.Up, grabber2),
                new PassToGrabberData(DirectionType.Right, grabber0),
                new PassToGrabberData(DirectionType.Down, motherGrabber2),
                new PassToGrabberData(DirectionType.Left, motherGrabber3)
            });

            grabber2.Set(new[]
            {
                new PassToGrabberData(DirectionType.Down, grabber1),
                new PassToGrabberData(DirectionType.Right, grabber3),
                new PassToGrabberData(DirectionType.Left, motherGrabber4),
                new PassToGrabberData(DirectionType.Up, motherGrabber5)
            });

            grabber3.Set(new[]
            {
                new PassToGrabberData(DirectionType.Down, grabber0),
                new PassToGrabberData(DirectionType.Left, grabber2),
                new PassToGrabberData(DirectionType.Up, motherGrabber6),
                new PassToGrabberData(DirectionType.Right, motherGrabber7)
            });
        }

        private void SetGrabbersViews()
        {
            var prefab = Resources.Load<GameObject>("Prefabs/Grabbers/GrabberView");

            SetGrabberView(prefab, grabber0, "Grabber0", GrabberSpriteData.BirdRed);
            SetGrabberView(prefab, grabber1, "Grabber1", GrabberSpriteData.BirdBlue);
            SetGrabberView(prefab, grabber2, "Grabber2", GrabberSpriteData.BirdGreen);
            SetGrabberView(prefab, grabber3, "Grabber3", GrabberSpriteData.BirdYellow);
        }

        private void SetGrabberView(GameObject prefab, PlayerGrabber grabber, string parentName, GrabberSpriteData spriteData)
        {
            var parent = transform.Find("Grabbers/" + parentName);
            var view = Instantiate(prefab, parent).AddComponent<GrabberView>();
            view.name = parentName + "View";
            grabber.SetView(view);
            grabber.SetSpriteData(spriteData);
        }
        private void SetMotherViews()
        {
            var prefab = Resources.Load<GameObject>("Prefabs/Grabbers/GrabberView");

            SetMotherView(prefab, motherGrabber0, "Mother0", PositionAlignmentType.BottomRightRight);
            SetMotherView(prefab, motherGrabber1, "Mother1", PositionAlignmentType.BottomRight);
            SetMotherView(prefab, motherGrabber2, "Mother2", PositionAlignmentType.BottomLeft);
            SetMotherView(prefab, motherGrabber3, "Mother3", PositionAlignmentType.BottomLeftLeft);
            SetMotherView(prefab, motherGrabber4, "Mother4", PositionAlignmentType.UpperLeftLeft);
            SetMotherView(prefab, motherGrabber5, "Mother5", PositionAlignmentType.UpperLeft);
            SetMotherView(prefab, motherGrabber6, "Mother6", PositionAlignmentType.UpperRight);
            SetMotherView(prefab, motherGrabber7, "Mother7", PositionAlignmentType.UpperRightRight);
        }

        private void SetMotherView(GameObject prefab, MotherGrabber grabber, string parentName, PositionAlignmentType posType) 
        {
            var parent = transform.Find("Mothers/" + parentName);
            var view = Instantiate(prefab, parent).AddComponent<GrabberView>();
            view.name = parentName + "View";
            grabber.SetView(view);
            grabber.FixPosition(posType);
            grabber.SetSpriteData(EggData.NoEgg);
        }

        private void SetDispensers()
        {
            dispenser0.Set(new PassToGrabberData(DirectionType.Left, grabber0));
            dispenser1.Set(new PassToGrabberData(DirectionType.Right, grabber1));
            dispenser2.Set(new PassToGrabberData(DirectionType.Right, grabber2));
            dispenser3.Set(new PassToGrabberData(DirectionType.Left, grabber3));
        }

        private void SetDispensorView()
        {
            CreateView(dispenser0, 0);
            CreateView(dispenser1, 1);
            CreateView(dispenser2, 2);
            CreateView(dispenser3, 3);

            void CreateView(Dispenser dispenser, int index)
            {
                var parent = transform.Find("Dispensers/Dispenser" + index);
                dispenser.CreateView(parent);
            }
        }

        public void OnPlayerInput(int playerId, DirectionType directionType)
        {
            switch (playerId)
            {
                case 0: grabber0.PassTo(directionType); break;
                case 1: grabber1.PassTo(directionType); break;
                case 2: grabber2.PassTo(directionType); break;
                case 3: grabber3.PassTo(directionType); break;
            }                
        }
    }
}
