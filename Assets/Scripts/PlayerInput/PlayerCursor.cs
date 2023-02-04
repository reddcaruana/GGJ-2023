using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerCursor : BaseControls<PlayerCursor>
{
    [Tooltip("The cursor's travel speed.")]
    [SerializeField] private float speed = 750f;

    [Tooltip("The color to use for this player.")]
    [SerializeField] private Color color = Color.white;

    /// <summary>
    /// The input direction.
    /// </summary>
    private Vector2 _inputDirection = Vector2.zero;

    /// <summary>
    /// The lobby manager.
    /// </summary>
    private LobbyManager _manager;

    /// <summary>
    /// The Rect Transform component.
    /// </summary>
    private RectTransform _rectTransform;

    /// <summary>
    /// The starting position.
    /// </summary>
    private Vector3 _startingPosition;

    /// <summary>
    /// The character identifier reference.
    /// </summary>
    private CharacterIdentifier _identifier;

    // The cursor actions
    private InputAction _move, _confirm, _cancel;

    /// <summary>
    /// The player's color.
    /// </summary>
    public Color Color => color;

    /// <summary>
    /// Component allocation.
    /// </summary>
    protected override void Awake()
    {
        _rectTransform = (RectTransform)transform;
        _startingPosition = _rectTransform.anchoredPosition;

        _manager = FindObjectOfType<LobbyManager>();

        if (Input == null)
            base.Awake();
    }

    /// <summary>
    /// The movement controller.
    /// </summary>
    private void Update()
    {
        _rectTransform.Translate(speed * Time.unscaledDeltaTime * _manager.ScaleFactor * _inputDirection);
        CalculateBounds();
    }

    /// <inheritdoc />
    public override void Bind(PlayerInput playerInput)
    {
        base.Bind(playerInput);
        
        // Switch to the cursor action.
        Input.SwitchCurrentActionMap("Cursor");

        _move = Input.currentActionMap.FindAction("Move");
        _move.performed += Move;
        _move.canceled += Move;

        _confirm = Input.currentActionMap.FindAction("Confirm");
        _confirm.performed += Confirm;

        _cancel = Input.currentActionMap.FindAction("Cancel");
        _cancel.performed += Cancel;
    }
    
    /// <summary>
    /// Calculates the object bounds.
    /// </summary>
    private void CalculateBounds()
    {
        if (!enabled) return;

        Vector2 anchoredPosition = _rectTransform.anchoredPosition;

        Vector4 bounds = new Vector4(
            // Left
            _rectTransform.pivot.x * _rectTransform.rect.size.x,
            // Bottom
            _rectTransform.pivot.y * _rectTransform.rect.size.y * 0.5f,
            // Right
            Screen.width / _manager.ScaleFactor - (1 - _rectTransform.pivot.x) * _rectTransform.rect.size.x * 0.5f,
            // Top
            Screen.height / _manager.ScaleFactor - (1 - _rectTransform.pivot.y) * _rectTransform.rect.size.y);

        if (anchoredPosition.x < bounds.x || anchoredPosition.x > bounds.z)
            anchoredPosition.x = Mathf.Clamp(anchoredPosition.x, bounds.x, bounds.z);

        if (anchoredPosition.y < bounds.y | anchoredPosition.y > bounds.w)
            anchoredPosition.y = Mathf.Clamp(anchoredPosition.y, bounds.y, bounds.w);

        _rectTransform.anchoredPosition = anchoredPosition;
    }

    /// <summary>
    /// Deactivates the cursor.
    /// </summary>
    public void Deactivate()
    {
        _rectTransform.anchoredPosition = _startingPosition;
        gameObject.SetActive(false);
    }

    /// <inheritdoc />
    public override void Release()
    {
        if (_move != null)
        {
            _move.performed -= Move;
            _move.canceled -= Move;
            _move = null;
        }

        if (_confirm != null)
        {
            _confirm.performed -= Confirm;
            _confirm = null;
        }

        if (_cancel != null)
        {
            _cancel.performed -= Cancel;
            _cancel = null;
        }
        
        base.Release();
    }

    /// <summary>
    /// The confirm action.
    /// </summary>
    /// <param name="ctx">The callback context.</param>
    private void Confirm(InputAction.CallbackContext ctx)
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(_rectTransform.position);
        
        Collider2D c = Physics2D.OverlapPoint(worldPos);
        if (!c) return;

        CharacterIdentifier identifier = c.GetComponent<CharacterIdentifier>();
        if (identifier.IsTaken) return;
        
        identifier.SetPlayer(ID);
        _identifier = identifier;
        gameObject.SetActive(false);
        
        _manager.Check();
    }

    /// <summary>
    /// The leave action.
    /// </summary>
    /// <param name="ctx">The callback context.</param>
    private void Cancel(InputAction.CallbackContext ctx)
    {
        if (gameObject.activeSelf)
        {
            Destroy(Input.gameObject);
            _manager.Deregister(this);
        }
        else
        {
            if (_identifier)
            {
                _identifier.Clear();
                _identifier = null;
            }
            gameObject.SetActive(true);
        }
        
        _manager.Check();
    }

    /// <summary>
    /// The move control.
    /// </summary>
    /// <param name="ctx">The callback context.</param>
    private void Move(InputAction.CallbackContext ctx)
    {
        if (!gameObject.activeSelf) return;
        _inputDirection = ctx.ReadValue<Vector2>();
    }
}
