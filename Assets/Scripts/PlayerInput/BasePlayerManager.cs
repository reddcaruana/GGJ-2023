using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class BasePlayerManager<T> : MonoBehaviour
    where T : MonoBehaviour, IBindable
{
    [Tooltip("The control handlers")]
    [SerializeField] protected T[] playerControlledObjects;

    /// <summary>
    /// Binds the player controls.
    /// </summary>
    protected void BindControls()
    {
        PlayerInput[] activePlayers = PlayerInput.all.ToArray();
        Queue<T> controls = new Queue<T>(playerControlledObjects);

        // Bind all player controls.
        foreach (PlayerInput input in activePlayers)
        {
            if (controls.Count == 0) break;
            
            T player = controls.Dequeue();
            
            // Release the already mapped player.
            if (player.ID != -1) player.Release();
            
            // Map the player.
            player.Bind(input);
            player.gameObject.SetActive(true);
        }
    }
}