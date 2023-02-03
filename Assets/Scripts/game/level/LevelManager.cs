using UnityEngine;
using Assets.Scripts.game.grabbers;
using Assets.Scripts.game.dispenser;
using Assets.Scripts.game.grabbers.data;
using Assets.Scripts.game.grabbers.views;

namespace Assets.Scripts.game.level
{
    public class LevelManager : MonoBehaviour
    {
        private PlayerGrabbor grabber0 = new PlayerGrabbor();
        private PlayerGrabbor grabber1 = new PlayerGrabbor();
        private PlayerGrabbor grabber2 = new PlayerGrabbor();
        private PlayerGrabbor grabber3 = new PlayerGrabbor();

        private Dispenser dispenser0 = new Dispenser();

        public void Init()
        {
            SetGrabbers();
            SetGrabbersViews();
            SetDispensers();
            SetDispensorView();
        }

        public void Test()
        {
            dispenser0.GenerateEgg();
            dispenser0.Pass();
        }

        private void SetGrabbers()
        {
            grabber0.Set(new[]
            {
                new PassToGrabberData(DirectionType.Left, grabber1),
                new PassToGrabberData(DirectionType.Up, grabber3),
                new PassToGrabberData(DirectionType.Right, dispenser0.Grabber),
            });

            grabber1.Set(new[]
            {
                new PassToGrabberData(DirectionType.Up, grabber2),
                new PassToGrabberData(DirectionType.Right, grabber0)
            });

            grabber2.Set(new[]
            {
                new PassToGrabberData(DirectionType.Down, grabber1),
                new PassToGrabberData(DirectionType.Right, grabber3)
            });

            grabber3.Set(new[]
            {
                new PassToGrabberData(DirectionType.Down, grabber0),
                new PassToGrabberData(DirectionType.Left, grabber2)
            });
        }

        private void SetGrabbersViews()
        {
            var prefab = Resources.Load<GameObject>("Prefabs/Grabbers/GrabberView");

            SetView(grabber0, "Grabber0");
            SetView(grabber1, "Grabber1");
            SetView(grabber2, "Grabber2");
            SetView(grabber3, "Grabber3");

            void SetView(Grabber grabber, string parentName)
            {
                var parent = transform.Find("Grabbers/" + parentName);
                var view = Instantiate(prefab, parent).AddComponent<GrabberView>();
                view.name = parentName + "View";
                grabber.SetView(view);
            }
        }

        private void SetDispensers()
        {
            dispenser0.Set(new PassToGrabberData(DirectionType.Left, grabber0));
        }

        private void SetDispensorView()
        {
            var parent = transform.Find("Dispensers/Dispenser0");
            dispenser0.CreateView(parent);
        }
    }
}
