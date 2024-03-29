using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
    private BaseCanvasManager _manager;

    /// <summary>
    /// The Rect Transform component.
    /// </summary>
    private RectTransform _rectTransform;

    /// <summary>
    /// The sprite renderer component.
    /// </summary>
    private Image _image;

    /// <summary>
    /// The starting position.
    /// </summary>
    private Vector3 _startingPosition;

    /// <summary>
    /// The character identifier reference.
    /// </summary>
    private CharacterReference _reference;

    /// <summary>
    /// The character stats component.
    /// </summary>
    private CharacterStats _stats;

    // The cursor actions
    private InputAction _move, _confirm, _cancel;

    /// <summary>
    /// The player's color.
    /// </summary>
    public Color Color => color;

    /// <summary>
    /// Component allocation.
    /// </summary>
    protected void Awake()
    {
        _rectTransform = (RectTransform)transform;
        _startingPosition = _rectTransform.anchoredPosition;

        _image = GetComponent<Image>();

        _manager = FindObjectOfType<BaseCanvasManager>();
    }

    /// <summary>
    /// Deactivates the cursor.
    /// </summary>
    private void Start()
    {
        Deactivate();
    }

    /// <summary>
    /// The movement controller.
    /// </summary>
    private void Update()
    {
        _rectTransform.Translate(speed * Time.unscaledDeltaTime * _manager.ScaleFactor * _inputDirection);
        CalculateBounds();
    }

    /// <summary>
    /// Release the controls on destroy.
    /// </summary>
    private void OnDestroy()
    {
        Release();
    }

    /// <summary>
    /// The activation function.
    /// </summary>
    private void Activate()
    {
        _image.color = color;
    }

    /// <inheritdoc />
    public override void Bind(PlayerInput playerInput)
    {
        base.Bind(playerInput);

        _stats = Input.GetComponent<CharacterStats>();
        
        // Switch to the cursor action.
        Input.SwitchCurrentActionMap("Cursor");

        _move = Input.currentActionMap.FindAction("Move");
        _move.performed += Move;
        _move.canceled += Move;

        _confirm = Input.currentActionMap.FindAction("Confirm");
        _confirm.performed += Confirm;

        _cancel = Input.currentActionMap.FindAction("Cancel");
        _cancel.performed += Cancel;
        
        Activate();
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
        _image.color = color * new Color(1, 1, 1, 0.4f);
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

        _stats = null;
        base.Release();
    }

    /// <summary>
    /// The confirm action.
    /// </summary>
    /// <param name="ctx">The callback context.</param>
    private void Confirm(InputAction.CallbackContext ctx)
    {
        if (_manager is ScoreScreenManager screenManager)
        {
            if (!screenManager.returnToLobby)
            {
                Raycast();
            }
            else
            {
                screenManager.ReturnToLobby();
            }
        }
        
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(_rectTransform.position);
        
        Collider2D c = Physics2D.OverlapPoint(worldPos);
        if (!c) return;

        CharacterReference reference = c.GetComponent<CharacterReference>();
        if (reference.IsTaken) return;
        
        reference.SetPlayer(ID);
        _reference = reference;
        _stats.SetID(_reference.ReferenceID);

        _inputDirection = Vector2.zero;

        Vector2 viewport = Camera.main.WorldToViewportPoint(_reference.CursorPoint);
        Vector2 position = _manager.ViewportToCanvasSpace(viewport);
        _rectTransform.anchoredPosition = position;

        _rectTransform.localScale = Vector3.one * 0.75f;
        _manager.Check();
        enabled = false;
    }

    /// <summary>
    /// The leave action.
    /// </summary>
    /// <param name="ctx">The callback context.</param>
    private void Cancel(InputAction.CallbackContext ctx)
    {
        // if (gameObject.activeSelf)
        // {
        //     Destroy(Input.gameObject);
        //     _manager.Deregister(this);
        // }
        // else
        // {
            if (_reference)
            {
                _reference.Clear();
                _reference = null;
            }
            gameObject.SetActive(true);
        // }

        enabled = true;
        _rectTransform.localScale = Vector3.one;
        
        if (!(_manager is ScoreScreenManager))
            _manager.Check();
    }

    /// <summary>
    /// The move control.
    /// </summary>
    /// <param name="ctx">The callback context.</param>
    private void Move(InputAction.CallbackContext ctx)
    {
        if (!enabled) return;
        _inputDirection = ctx.ReadValue<Vector2>();
    }

    /// <summary>
    /// Raycasts and clicks the button.
    /// </summary>
    private void Raycast()
    {
        Debug.Log($"{gameObject.name} Raycast");
        PointerEventData data = new PointerEventData(EventSystem.current)
        {
            position = _rectTransform.anchoredPosition * _manager.ScaleFactor
        };

        List<RaycastResult> results = new List<RaycastResult>();
        _manager.Raycaster.Raycast(data, results);

        foreach (RaycastResult result in results)
        {
            ExecuteEvents.ExecuteHierarchy(result.gameObject, new BaseEventData(EventSystem.current),
                ExecuteEvents.submitHandler);
        }
    }
}
