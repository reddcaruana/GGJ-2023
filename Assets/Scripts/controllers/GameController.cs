using UnityEngine;
using Assets.Scripts.game.level;
using Assets.Scripts.controllers;
using Assets.Scripts.game.grabbers;

public class GameController : MonoBehaviour
{
    public static GameController ME;

    public LevelManager LevelManager;

    private void Awake()
    {
        ME = this;
        ViewController.Updatedata();
        LoadView();
    }

    private void Start()
    {
        LevelManager.Dispense();
    }

    private void Update()
    {
        if (!DebugController.Automatic && DebugController.ActiveGrabber != null)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                DebugController.ActiveGrabber.PassTo(DirectionType.Left);
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                DebugController.ActiveGrabber.PassTo(DirectionType.Right);
            else if (Input.GetKeyDown(KeyCode.UpArrow))
                DebugController.ActiveGrabber.PassTo(DirectionType.Up);
            else if (Input.GetKeyDown(KeyCode.DownArrow))
                DebugController.ActiveGrabber.PassTo(DirectionType.Down);
        }
    }

    private void LoadView()
    {
        var levelPrefab = Resources.Load<GameObject>("Prefabs/LevelView/Level");
        LevelManager = Instantiate(levelPrefab).AddComponent<LevelManager>();
        LevelManager.Init();
    }
}
