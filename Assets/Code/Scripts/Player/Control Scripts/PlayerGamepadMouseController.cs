using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace PlayerControls.GamepadMouseControls
{
    public class PlayerGamepadMouseController : MonoBehaviour
    {
        private readonly Vector2 _sensitivity = new Vector2(1500f, 1500f);
        private readonly Vector2 _bias = new Vector2(0f, -1f);

        private Vector2 _mousePosition;
        private Vector2 _warpPosition;
        private Vector2 _overflow;

        public UnityAction<Vector2> cursorAction;

        private void Awake()
        {
            cursorAction += OnCursorAction;
        }

        private void OnCursorAction(Vector2 rightStick)
        {
            if (rightStick.magnitude < 0.1f) return;
            
            _mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            _warpPosition = _mousePosition + _bias + _overflow + _sensitivity * Time.deltaTime * rightStick;
            _warpPosition = new Vector2(Mathf.Clamp(_warpPosition.x, 0, Screen.width), Mathf.Clamp(_warpPosition.y, 0, Screen.height));
            _overflow = new Vector2(_warpPosition.x % 1, _warpPosition.y % 1);
            
            Mouse.current.WarpCursorPosition(_warpPosition);
        }
    }
}

