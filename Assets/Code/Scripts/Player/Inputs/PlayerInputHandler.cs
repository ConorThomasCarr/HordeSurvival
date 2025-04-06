using System;

using UnityEngine;
using UnityEngine.InputSystem;

using PlayerControls.PointAndClickControls;
using PlayerControls.CharacterControls;
using PlayerControls.GamepadMouseControls;

namespace PlayerControls.PlayerHandler
{
    public class PlayerInputHandler : MonoBehaviour
    {
        private InputAction _move;
        private InputAction _cursor;
        private InputAction _rotate;
        private InputAction _swapWeapon;

        private PlayerPointAndClickController _pacControls;
        private PlayerCharacterController _pcControls;
        private PlayerGamepadMouseController _pgmControls;
        private WeaponInventory _weaponInventory;

        private bool HasGamepad ()
        {
            return Gamepad.current != null;
        }
    
        private void Awake()
        {
            _pacControls = FindFirstObjectByType<PlayerPointAndClickController>();
            _pcControls = FindFirstObjectByType<PlayerCharacterController>();
            _pgmControls = FindFirstObjectByType<PlayerGamepadMouseController>();
            _weaponInventory = FindFirstObjectByType<WeaponInventory>();
            
            _move = InputSystem.actions.FindAction("Move");
            _cursor = InputSystem.actions.FindAction("Cursor");
            _rotate = InputSystem.actions.FindAction("Rotate");
            _swapWeapon = InputSystem.actions.FindAction("Swap Weapon");
        }


        private void Start()
        {
            _move.canceled += MoveCanceled;
            _swapWeapon.canceled += SwapWeaponCanceled;
        }

        private void FixedUpdate()
        {
            if (HasGamepad())
            {
                if (_cursor.IsInProgress())
                {
                    // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
                    _pgmControls.cursorAction.Invoke(CursorPosition());
                    _pcControls.rotateAction.Invoke(Input.mousePosition);
                }
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
        
        private void SwapWeaponCanceled(InputAction.CallbackContext context)
        {
            _weaponInventory.SwapWeapons?.Invoke();
        }

        private Vector2 CursorPosition()
        {
            return _cursor.ReadValue<Vector2>();
        }
    }
}
 

