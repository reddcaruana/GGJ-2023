using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaderboardLoader : MonoBehaviour
{
    [Tooltip("The leaderboard scene name.")]
    [SerializeField] private string leaderboard = "Leaderboard";

    [Tooltip("The game scene name")]
    [SerializeField] private string game = "GameScene";

    /// <summary>
    /// Loads the leaderboard.
    /// </summary>
    public void LoadLeaderboard()
    {
        DOTween.KillAll();
        StartCoroutine(LoadSceneRoutine());
    }

    /// <summary>
    /// Routine to load the leaderboard.
    /// </summary>
    private IEnumerator LoadSceneRoutine()
    {
        AsyncOperation leaderAsync = SceneManager.LoadSceneAsync(leaderboard, LoadSceneMode.Additive);

        while (!leaderAsync.isDone)
            yield return null;

        SceneManager.UnloadSceneAsync(game);
    }
}