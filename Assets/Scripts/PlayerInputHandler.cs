using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



namespace Platformer
{
    public class PlayerInputHandler:MonoBehaviour
    {
        private PlayerMover mover;

        private void Awake()
        {
            mover = GetComponent<PlayerMover>();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            mover.SetInputVector(context.ReadValue<Vector2>());
        }
    }
}