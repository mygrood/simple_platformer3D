using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;



namespace Platformer
{
    public class PlayerInputHandler:MonoBehaviour
    {
        private PlayerInput playerInput;
        public PlayerMover mover;

        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
            var movers = FindObjectsOfType<PlayerMover>();
            Debug.Log($"Found {movers.Length} PlayerMover(s)");
            var index = playerInput.playerIndex;
            mover = movers.FirstOrDefault(m=>m.GetPlayerIndex() == index);
            if (mover == null)
            {
                Debug.LogError($"No PlayerMover found for player index: {index}");
            }
        }

        public void OnMove(CallbackContext context)    
        {
            Vector2 input = context.ReadValue<Vector2>();
            Debug.Log($"Input received: {input}");
            if (mover != null)
            {
                mover.SetInputVector(input);
                
            }
            else
            {
                Debug.Log($"No mover found");
            }
        }
    }
}