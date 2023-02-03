using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputManager))]
public class LobbyManager : BasePlayerManager<LobbyInput>
{
    /// <summary>
    /// The number of connected players.
    /// </summary>
    public int PlayerCount => PlayerInput.all.Count();

    /// <summary>
    /// Component cache.
    /// </summary>
    private PlayerInputManager _playerInputManager;

    /// <summary>
    /// The lobby input stack.
    /// </summary>
    private Stack<LobbyInput> _lobbyInputs;

    /// <summary>
    /// Assigns the class variables.
    /// </summary>
    private void Awake()
    {
        // Adjust the input manager behavior.
        _playerInputManager = GetComponent<PlayerInputManager>();
        _playerInputManager.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;

        _lobbyInputs = new Stack<LobbyInput>(playerControlledObjects);
    }
    
    /// <summary>
    /// Handles functions when the object is deactivated.
    /// </summary>
    private void OnDisable()
    {
        _playerInputManager.onPlayerJoined -= RegisterPlayer;
        _playerInputManager.onPlayerLeft -= RemovePlayer;
    }

    /// <summary>
    /// Handles functions when the object is active.
    /// </summary>
    private void OnEnable()
    {
        _playerInputManager.onPlayerJoined += RegisterPlayer;
        _playerInputManager.onPlayerLeft += RemovePlayer;
    }

    /// <summary>
    /// Handles the connect event.
    /// </summary>
    /// <param name="playerInput">The connected PlayerInput object.</param>
    private void RegisterPlayer(PlayerInput playerInput)
    {
        // Prepare the object.
        playerInput.gameObject.name = $"Player-{playerInput.playerIndex}";
        playerInput.transform.SetParent(transform);

        LobbyInput input = _lobbyInputs.Pop();
        input.Bind(playerInput);
        input.gameObject.SetActive(true);

        if (PlayerCount >= 4)
        {
            _playerInputManager.DisableJoining();
        }
    }

    /// <summary>
    /// Handles the disconnect event.
    /// </summary>
    /// <param name="playerInput">The disconnected PlayerInput object.</param>
    private void RemovePlayer(PlayerInput playerInput)
    {
        // Process any other methods.
        Destroy(playerInput.gameObject);
        
        if (PlayerCount >= 4)
        {
            _playerInputManager.EnableJoining();
        }
    }

    /// <summary>
    /// Restocks the lobby input.
    /// </summary>
    /// <param name="input">The lobby input.</param>
    public void Restock(LobbyInput input)
    {
        if (!LobbyInput.All.Contains(input)) return;
        
        input.gameObject.SetActive(false);
        _lobbyInputs.Push(input);
    }
}