using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class InputController : MonoBehaviour
{
        public Vector2 MoveInput { get; private set; } = Vector2.zero;
        public Vector2 MoveInput2 { get; private set; } = Vector2.zero;
        InputActions _input = null;

        private void OnEnable()
        {
                _input = new InputActions();
                _input.Player.Enable();
                _input.Player2.Enable();

                _input.Player.Move.performed += SetMove;
                _input.Player.Move.canceled += SetMove;
                
                _input.Player2.Move.performed += SetMove2;
                _input.Player2.Move.canceled += SetMove2;
                
        }
        private void OnDisable()
        {
                _input.Player.Move.performed -= SetMove;
                _input.Player.Move.canceled -= SetMove;

                _input.Player2.Move.performed -= SetMove2;
                _input.Player2.Move.canceled -= SetMove2;
                
                _input.Player.Disable();
                _input.Player2.Disable();
        }
        
        private void SetMove(InputAction.CallbackContext ctx)
        {
                MoveInput = ctx.ReadValue<Vector2>();
        }
        private void SetMove2(InputAction.CallbackContext ctx)
        {
                MoveInput2 = ctx.ReadValue<Vector2>();
        }
}


