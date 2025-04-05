using System;

using UnityEngine;
using UnityEngine.InputSystem;

using PlayerControls.PointAndClickControls;
using PlayerControls.CharacterControls;
using PlayerControls.GamepadMouseControls;

namespace PlayerControls.InputHandler
{
    public class PlayerInputHandler : MonoBehaviour
    {
        private InputAction _move;
        private InputAction _cursor;
        private InputAction _rotate;
        
        
        private PlayerPointAndClickController _pacControls;
        private PlayerCharacterController _pcControls;
        private PlayerGamepadMouseController _pgmControls;
        private void Awake()
        {
    
            _pacControls = FindFirstObjectByType<PlayerPointAndClickController>();
            _pcControls = FindFirstObjectByType<PlayerCharacterController>();
            _pgmControls = FindFirstObjectByType<PlayerGamepadMouseController>();
            
            _move = InputSystem.actions.FindAction("Move");
            _cursor = InputSystem.actions.FindAction("Cursor");
            _rotate = InputSystem.actions.FindAction("Rotate");
        }


        private void Start()
        {
            _move.canceled += MoveCanceled;
        }

        private void FixedUpdate()
        {
            if (_cursor.IsInProgress())
            {
                // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
                _pgmControls.cursorAction.Invoke(CursorPosition());
            }
            
            if (_rotate.IsInProgress())
            {
                // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
                _pcControls.rotateAction.Invoke(Input.mousePosition);
            }
        }

        private void MoveCanceled(InputAction.CallbackContext context)
        {
            _pacControls.moveAction?.Invoke(Mouse.current.position.ReadValue());
        }

        private Vector2 CursorPosition()
        {
            return _cursor.ReadValue<Vector2>();
        }
    }
}
 

