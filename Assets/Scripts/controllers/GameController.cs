using UnityEngine;
using Assets.Scripts.game.level;

public class GameController : MonoBehaviour
{
    public static GameController ME;

    public LevelManager Level;

    private void Awake()
    {
        ME = this;
        LoadView();
    }

    private void Start()
    {
        Level.Test();
    }

    private void LoadView()
    {
        var levelPrefab = Resources.Load<GameObject>("Prefabs/LevelView/Level");
        Level = Instantiate(levelPrefab).AddComponent<LevelManager>();
        Level.Init();
    }
}
