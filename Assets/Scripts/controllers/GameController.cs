using UnityEngine;
using Assets.Scripts.game.level;
using Assets.Scripts.controllers;

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
        //DebugController.ArrowCheks();
        //DebugController.WSADCheks();
        DebugController.ArrowCheks(LevelManager.grabber0);
    }

    private void LoadView()
    {
        var levelPrefab = Resources.Load<GameObject>("Prefabs/LevelView/Level");
        LevelManager = Instantiate(levelPrefab).AddComponent<LevelManager>();
        LevelManager.Init();
    }
}
