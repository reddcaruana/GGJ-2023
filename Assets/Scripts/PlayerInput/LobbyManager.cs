using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class LobbyManager : BaseCanvasManager
{
    /// <summary>
    /// The maximum number of players.
    /// </summary>
    private static readonly int MaxPlayers = 4;

    [Tooltip("The timer object.")]
    [SerializeField] private GameObject timerObject;

    [Tooltip("The character identifiers.")]
    [SerializeField] private CharacterReference[] characterIdentifiers;
    
    [Tooltip("The transform to parent inputs to.")]
    [SerializeField] private Transform playerInputParent;

    [Tooltip("The title screen object.")]
    [SerializeField] private GameObject titleScreenObject;
    
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
    private Dictionary<int, PlayerCursor> _lobbyInputs;

    /// <summary>
    /// Assigns the class variables.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        
        // Adjust the input manager behavior.
        _playerInputManager = FindObjectOfType<PlayerInputManager>();
        Assert.IsNotNull(_playerInputManager, "PlayerInputManager doesn't exist!");
        
        _playerInputManager.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;

        _lobbyInputs = new Dictionary<int, PlayerCursor>();
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
        
        if (PlayerCount < MaxPlayers)
            _playerInputManager.EnableJoining();
    }

    /// <summary>
    /// Checks for player updates.
    /// </summary>
    public override void Check()
    {
        int count = characterIdentifiers.Count(c => c.IsTaken);
        timerObject.SetActive(count >= MaxPlayers);
    }

    /// <summary>
    /// Handles the connect event.
    /// </summary>
    /// <param name="playerInput">The connected PlayerInput object.</param>
    private void RegisterPlayer(PlayerInput playerInput)
    {
        // Prepare the object.
        playerInput.gameObject.name = $"Player-{playerInput.playerIndex}";
        playerInput.transform.SetParent(playerInputParent);

        PlayerCursor controls = _lobbyInputs[playerInput.playerIndex];
        controls.Bind(playerInput);
        controls.gameObject.SetActive(true);

        titleScreenObject.SetActive(PlayerInput.all.Count == 0);
        
        if (PlayerCount >= MaxPlayers)
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
        
        if (PlayerCount < MaxPlayers)
        {
            _playerInputManager.EnableJoining();
        }
    }
}