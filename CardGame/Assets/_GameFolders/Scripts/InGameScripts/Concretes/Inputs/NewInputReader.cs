using CardGame.Abstracts.Inputs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CardGame.Inputs
{
    public class NewInputReader : IInputReader
    {
        GameInputActions _input;
        
        public Vector2 TouchPosition { get; private set; }
        public bool IsTouch => _input.Player.Touch.WasPressedThisFrame();

        public NewInputReader()
        {
            _input = new GameInputActions();

            _input.Player.TouchPosition.performed += HandleOnTouchPosition;
            _input.Player.TouchPosition.canceled += HandleOnTouchPosition;
            
            _input.Enable();
        }

        ~NewInputReader()
        {
            _input.Player.TouchPosition.performed -= HandleOnTouchPosition;
            _input.Player.TouchPosition.canceled -= HandleOnTouchPosition;
            
            _input.Disable();
        }
        
        void HandleOnTouchPosition(InputAction.CallbackContext context)
        {
            TouchPosition = context.ReadValue<Vector2>();
        }
    }
}