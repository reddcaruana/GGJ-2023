using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Test
{
    public class ChangeScene : MonoBehaviour
    {
        public void GoToSample()
        {
            StartCoroutine(LoadGameScene());
        }

        private IEnumerator LoadGameScene()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("SampleScene", LoadSceneMode.Additive);

            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            Scene scene = SceneManager.GetSceneByName("SampleScene");
            SceneManager.MoveGameObjectToScene(PlayerInputManager.instance.gameObject, scene);

            SceneManager.UnloadSceneAsync("PlayerLobby");
        }
    }
}