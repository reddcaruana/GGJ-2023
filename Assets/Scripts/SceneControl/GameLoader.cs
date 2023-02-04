using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameLoader : MonoBehaviour
{
    [Tooltip("The player storage Scene name.")]
    [SerializeField] private string playerStorage = "PlayerStorage";
    
    [Tooltip("The game scene name")]
    [SerializeField] private string game = "GameScene";
    
    [Tooltip("The objects to move between scenes.")]
    [SerializeField] private GameObject playerInputParent;
    
    /// <summary>
    /// Starts the game.
    /// </summary>
    public void StartGame()
    {
        StartCoroutine(StartGameRoutine());
    }

    /// <summary>
    /// Routine to start the game.
    /// </summary>
    private IEnumerator StartGameRoutine()
    {
        AsyncOperation playerAsync = SceneManager.LoadSceneAsync(playerStorage, LoadSceneMode.Additive);

        while (!playerAsync.isDone)
            yield return null;

        Scene scene = SceneManager.GetSceneByName(playerStorage);
        SceneManager.MoveGameObjectToScene(playerInputParent, scene);
        
        SceneManager.UnloadSceneAsync("PlayerLobby");
        
        AsyncOperation gameAsync = SceneManager.LoadSceneAsync(game, LoadSceneMode.Additive);
            
        while (!gameAsync.isDone)
            yield return null;
    }
}