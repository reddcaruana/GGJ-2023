using Assets.Scripts.game.directions.data;
using Assets.Scripts.game.grabbers;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterControls : BaseControls<CharacterControls>
{
    /// <summary>
    /// The down action.
    /// </summary>
    private InputAction _downAction;
    
    /// <summary>
    /// The left action.
    /// </summary>
    private InputAction _leftAction;
    
    /// <summary>
    /// The right action.
    /// </summary>
    private InputAction _rightAction;
    
    /// <summary>
    /// The up action.
    /// </summary>
    private InputAction _upAction;

    /// <summary>
    /// The character ID.
    /// </summary>
    private int _id = -1;

    /// <inheritdoc />
    public override void Bind(PlayerInput playerInput)
    {
        base.Bind(playerInput);
        CharacterStats stats = Input.GetComponent<CharacterStats>();
        _id = stats.ID;
        
        Input.SwitchCurrentActionMap("Game");
        
        _downAction = Input.currentActionMap.FindAction("Down");
        _downAction.performed += OnDown;
        
        _leftAction = Input.currentActionMap.FindAction("Left");
        _leftAction.performed += OnLeft;
        
        _rightAction = Input.currentActionMap.FindAction("Right");
        _rightAction.performed += OnRight;
        
        _upAction = Input.currentActionMap.FindAction("Up");
        _upAction.performed += OnUp;
    }

    /// <inheritdoc />
    public override void Release()
    {
        if (_downAction != null)
        {
            _downAction.performed -= OnDown;
            _downAction = null;
        }

        if (_leftAction != null)
        {
            _leftAction.performed -= OnLeft;
            _leftAction = null;
        }

        if (_rightAction != null)
        {
            _rightAction.performed -= OnRight;
            _rightAction = null;
        }

        if (_upAction != null)
        {
            _upAction.performed -= OnUp;
            _upAction = null;
        }
        
        base.Release();
    }

    /// <summary>
    /// The down action.
    /// </summary>
    /// <param name="ctx">The callback context.</param>
    private void OnDown(InputAction.CallbackContext ctx)
    {
        if (!GameController.ME) return;
        GameController.ME.LevelManager.OnPlayerInput(_id, DirectionData.Down);
    }
    
    /// <summary>
    /// The left action.
    /// </summary>
    /// <param name="ctx">The callback context.</param>
    private void OnLeft(InputAction.CallbackContext ctx)
    {
        if (!GameController.ME) return;
        GameController.ME.LevelManager.OnPlayerInput(_id, DirectionData.Left);
    }
    
    /// <summary>
    /// The right action.
    /// </summary>
    /// <param name="ctx">The callback context.</param>
    private void OnRight(InputAction.CallbackContext ctx)
    {
        if (!GameController.ME) return;
        GameController.ME.LevelManager.OnPlayerInput(_id, DirectionData.Right);
    }
    
    /// <summary>
    /// The up action.
    /// </summary>
    /// <param name="ctx">The callback context.</param>
    private void OnUp(InputAction.CallbackContext ctx)
    {
        if (!GameController.ME) return;
        GameController.ME.LevelManager.OnPlayerInput(_id, DirectionData.Up);
    }
}
