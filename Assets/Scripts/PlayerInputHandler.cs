using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using static UnityEngine.InputSystem.InputAction;


namespace Platformer
{
    public class PlayerInputHandler : MonoBehaviour
    {
        private PlayerInput playerInput;
        private PlayerMovement playerMovement;

        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
            var movers = FindObjectsOfType<PlayerMovement>();
            Debug.Log($"Found {movers.Length} PlayerMover(s)");
            var index = playerInput.playerIndex;
            playerMovement = movers.FirstOrDefault(m => m.GetPlayerIndex() == index);
           
            // Проверка на null перед доступом к камере
            if (playerMovement != null)
            {
                // Присваиваем камеру игроку, если она существует
                Camera playerCamera = playerMovement.GetComponentInChildren<Camera>();
                if (playerCamera != null)
                {
                    playerInput.camera = playerCamera;
                }
                else
                {
                    Debug.LogError($"No Camera found for PlayerMover with index {index}");
                }
            }
            else
            {
                Debug.LogError($"No PlayerMover found for player index: {index}");
            }
        }

        public void OnMove(CallbackContext context)
        {
            Vector2 input = context.ReadValue<Vector2>();
            //Debug.Log($"Input received: {input}");
            if (playerMovement != null)
            {
                playerMovement.SetInputVector(input);
            }
            else
            {
                Debug.Log($"No mover found");
            }
        }
    }
}