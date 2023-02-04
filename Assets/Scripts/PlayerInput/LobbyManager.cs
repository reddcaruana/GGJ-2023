using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class LobbyManager : BasePlayerManager<LobbyControls>
{
    /// <summary>
    /// The maximum number of players.
    /// </summary>
    private static int _maxPlayers = 4;
    
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
    private Dictionary<int, LobbyControls> _lobbyInputs;

    /// <summary>
    /// Assigns the class variables.
    /// </summary>
    private void Awake()
    {
        // Adjust the input manager behavior.
        _playerInputManager = FindObjectOfType<PlayerInputManager>();
        Assert.IsNotNull(_playerInputManager, "PlayerInputManager doesn't exist!");
        
        _playerInputManager.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;

        _lobbyInputs = new Dictionary<int, LobbyControls>();
        for (int i = 0; i < playerControlledObjects.Length; i++)
            _lobbyInputs.Add(i, playerControlledObjects[i]);
    }
    
    /// <summary>
    /// Handles functions when the object is deactivated.
    /// </summary>
    private void OnDisable()
    {
        _playerInputManager.onPlayerJoined -= RegisterPlayer;
        _playerInputManager.onPlayerLeft -= RemovePlayer;
        
        _playerInputManager.DisableJoining();
    }

    /// <summary>
    /// Handles functions when the object is active.
    /// </summary>
    private void OnEnable()
    {
        _playerInputManager.onPlayerJoined += RegisterPlayer;
        _playerInputManager.onPlayerLeft += RemovePlayer;
        
        if (PlayerCount < _maxPlayers)
            _playerInputManager.EnableJoining();
    }

    /// <summary>
    /// Handles the connect event.
    /// </summary>
    /// <param name="playerInput">The connected PlayerInput object.</param>
    private void RegisterPlayer(PlayerInput playerInput)
    {
        // Prepare the object.
        playerInput.gameObject.name = $"Player-{playerInput.playerIndex}";
        playerInput.transform.SetParent(_playerInputManager.transform);

        LobbyControls controls = _lobbyInputs[playerInput.playerIndex];
        controls.Bind(playerInput);
        controls.gameObject.SetActive(true);

        if (PlayerCount >= _maxPlayers)
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
    /// <param name="controls">The lobby input.</param>
    public void Restock(LobbyControls controls)
    {
        if (!LobbyControls.All.Contains(controls)) return;
        controls.gameObject.SetActive(false);
    }
}