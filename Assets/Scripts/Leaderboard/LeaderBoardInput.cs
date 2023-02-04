using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LeaderBoardInput : BaseControls<LeaderBoardInput>
{
    /// <summary>
    /// The select action.
    /// </summary>
    private InputAction _select;

    /// <summary>
    /// The manager script.
    /// </summary>
    private Leaderboard _manager;

    protected void Awake()
    {
        _manager = FindObjectOfType<Leaderboard>();
    }

    /// <inheritdoc />
    public override void Bind(PlayerInput playerInput)
    {
        base.Bind(playerInput);
        
        // Switch to the leaderboard action.
        Input.SwitchCurrentActionMap("Leaderboard");

        _select = Input.currentActionMap.FindAction("Select");
        _select.performed += Select;
    }

    /// <inheritdoc />
    public override void Release()
    {
        if (_select != null)
        {
            _select.performed -= Select;
            _select = null;
        }
        
        base.Release();
    }

    /// <summary>
    /// The select action.
    /// </summary>
    /// <param name="ctx">The callback context.</param>
    private void Select(InputAction.CallbackContext ctx)
    {
        Vector2 input = ctx.ReadValue<Vector2>();

        Color color = Color.white;
        if (input.x < 0) color = Color.green;
        if (input.x > 0) color = Color.red;
        if (input.y < 0) color = Color.yellow;
        if (input.y > 0) color = Color.blue;

        GetComponent<Image>().color = color;
        
        Release();
    }
}
