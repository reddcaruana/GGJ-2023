using UnityEngine;
using UnityEngine.SceneManagement;

public class GameReset : MonoBehaviour
{
    public void ToLobby()
    {
        SceneManager.LoadScene("PlayerLobby", LoadSceneMode.Single);
    }
}