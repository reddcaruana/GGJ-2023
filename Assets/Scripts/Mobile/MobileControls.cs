using System;
using Assets.Scripts.game.directions.data;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mobile
{
    [RequireComponent(typeof(PlayerInput), typeof (ActivateIfMobile))]
    public class MobileControls : BaseControls<MobileControls>
    {
        [Tooltip("The minimum swipe distance.")]
        [SerializeField] private float swipeDistance = 5f;

        [Tooltip("The camera.")]
        [SerializeField] private Camera mainCamera;
        
        /// <summary>
        /// The press action.
        /// </summary>
        private InputAction _touch, _delta, _primaryPosition;

        /// <summary>
        /// The pressing value.
        /// </summary>
        private bool _pressing = false;

        /// <summary>
        /// True if the action has been performed.
        /// </summary>
        private bool _actionPerformed = false;

        /// <summary>
        /// The grabber ID.
        /// </summary>
        private int _grabberID = -1;
        
        /// <summary>
        /// Binds the player input if active.
        /// </summary>
        private void Start()
        {
            Bind(GetComponent<PlayerInput>());
        }

        /// <summary>
        /// Releases the controls on destroy.
        /// </summary>
        private void OnDestroy()
        {
            Release();
        }

        /// <inheritdoc />
        public override void Bind(PlayerInput playerInput)
        {
            if (SystemInfo.deviceType != DeviceType.Handheld) return;
            
            base.Bind(playerInput);
            Input.SwitchCurrentActionMap("Gameplay");

            _touch = Input.currentActionMap.FindAction("Press");
            _touch.started += OnPress;
            _touch.canceled += OnRelease;

            _delta = Input.currentActionMap.FindAction("Delta");
            _delta.performed += OnDelta;

            _primaryPosition = Input.currentActionMap.FindAction("PrimaryPosition");
            _primaryPosition.performed += GetPosition;
        }

        /// <inheritdoc />
        public override void Release()
        {
            if (SystemInfo.deviceType != DeviceType.Handheld) return;

            if (_touch != null)
            {
                _touch.started -= OnPress;
                _touch.canceled -= OnRelease;
                _touch = null;
            }

            if (_delta != null)
            {
                _delta.performed -= OnDelta;
                _delta = null;
            }

            if (_primaryPosition != null)
            {
                _primaryPosition.performed -= GetPosition;
                _primaryPosition = null;
            }
            
            base.Release();
        }

        /// <summary>
        /// Processes the touch starting position.
        /// </summary>
        /// <param name="ctx">The callback context.</param>
        private void GetPosition(InputAction.CallbackContext ctx)
        {
            Vector2 point = ctx.ReadValue<Vector2>();
            Vector2 worldPoint = mainCamera.ScreenToWorldPoint(point);

            Collider2D overlap = Physics2D.OverlapPoint(worldPoint);
            if (!overlap) return;
            
            GrabberID grabber = overlap.GetComponent<GrabberID>();
            if (!grabber) return;
            
            _grabberID = grabber.ID;
        }

        /// <summary>
        /// Processes the pressing action.
        /// </summary>
        /// <param name="ctx">The callback context.</param>
        private void OnPress(InputAction.CallbackContext ctx)
        {
            _actionPerformed = false;
            _pressing = true;
        }

        /// <summary>
        /// Processes the delta distance.
        /// </summary>
        /// <param name="ctx">The callback context.</param>
        private void OnDelta(InputAction.CallbackContext ctx)
        {
            if (_actionPerformed) return;
            if (!_pressing) return;
            
            
            Vector2 swipeDelta = ctx.ReadValue<Vector2>();
            if (Vector2.Distance(Vector2.zero, swipeDelta) < swipeDistance) return;

            Vector2 direction = swipeDelta.normalized;
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                GameController.ME.LevelManager.OnPlayerInput(_grabberID,
                    direction.x > 0 ? DirectionData.Right : DirectionData.Left);
            }
            else
            {
                GameController.ME.LevelManager.OnPlayerInput(_grabberID,
                    direction.y > 0 ? DirectionData.Up : DirectionData.Down);
            }
            
            _actionPerformed = true;
        }
        
        /// <summary>
        /// Processes the release action.
        /// </summary>
        /// <param name="ctx">The callback context.</param>
        private void OnRelease(InputAction.CallbackContext ctx)
        {
            _pressing = false;
            _actionPerformed = false;
            _grabberID = -1;
        }
    }
}