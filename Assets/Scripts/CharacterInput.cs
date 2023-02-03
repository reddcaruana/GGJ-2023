using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterInput : BasePlayerInput<CharacterInput>
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

    /// <inheritdoc />
    public override void Bind(PlayerInput playerInput)
    {
        base.Bind(playerInput);
        
        Input.SwitchCurrentActionMap("Gameplay");
        
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
        transform.Translate(Vector3.down);
    }
    
    /// <summary>
    /// The left action.
    /// </summary>
    /// <param name="ctx">The callback context.</param>
    private void OnLeft(InputAction.CallbackContext ctx)
    {
        transform.Translate(Vector3.left);
    }
    
    /// <summary>
    /// The right action.
    /// </summary>
    /// <param name="ctx">The callback context.</param>
    private void OnRight(InputAction.CallbackContext ctx)
    {
        transform.Translate(Vector3.right);
    }
    
    /// <summary>
    /// The up action.
    /// </summary>
    /// <param name="ctx">The callback context.</param>
    private void OnUp(InputAction.CallbackContext ctx)
    {
        transform.Translate(Vector3.up);
    }
}
