using UnityEngine;
using Assets.Scripts.game.level;
using Assets.Scripts.controllers;

public class GameController : MonoBehaviour
{
    public static GameController ME;

    public LevelManager LevelManager { get; private set; }

    private void Awake()
    {
        ME = this;
        
        MotherController.Reset();
        EggController.Reset();
        DispenserController.Reset();
        ScoreController.Reset();
        
        ViewController.UpdateData();
        LoadView();
    }

    private void Start()
    {
        LevelManager.Dispense();
    }

    private void Update()
    {
        DebugController.ArrowChecks();
        DebugController.WASDChecks();
        // DebugController.ArrowChecks(LevelManager.grabber0);
    }

    private void LoadView()
    {
        var levelPrefab = Resources.Load<GameObject>("Prefabs/LevelView/Level");
        LevelManager = Instantiate(levelPrefab).AddComponent<LevelManager>();
        LevelManager.Init();
    }
}
