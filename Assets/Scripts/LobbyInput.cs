using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class LobbyInput : BasePlayerInput<LobbyInput>
{
    /// <summary>
    /// The leave action.
    /// </summary>
    private InputAction _leaveAction;

    /// <summary>
    /// The manager script.
    /// </summary>
    private LobbyManager _manager;
    
    /// <summary>
    /// Component allocation.
    /// </summary>
    private void Awake()
    {
        _manager = FindObjectOfType<LobbyManager>();
    }

    /// <inheritdoc />
    public override void Bind(PlayerInput playerInput)
    {
        base.Bind(playerInput);
        
        Input.SwitchCurrentActionMap("Lobby");

        _leaveAction = Input.currentActionMap.FindAction("Leave");
        _leaveAction.performed += OnLeave;
    }

    /// <inheritdoc />
    public override void Release()
    {
        if (_leaveAction != null)
        {
            _leaveAction.performed -= OnLeave;
            _leaveAction = null;
        }

        base.Release();
    }

    /// <summary>
    /// The leave action.
    /// </summary>
    /// <param name="ctx">The callback context.</param>
    private void OnLeave(InputAction.CallbackContext ctx)
    {
        Release();
        _manager.Restock(this);
    }
}
