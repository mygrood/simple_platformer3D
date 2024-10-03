using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.Serialization;

namespace Platformer
{
    public class PlayerInputHandler : MonoBehaviour
    {
        public PlayerManager playerManager;

        private PlayerInput playerInput;
        private PlayerMovement playerMovement;

        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
            var index = playerInput.playerIndex;
            
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            Debug.Log($"Found {players.Length} Player(s)");
            
            playerManager = FindPlayerManager(players, index);
            if (playerManager != null)
            {
                playerMovement = playerManager.GetComponent<PlayerMovement>();
                if (playerMovement == null)
                {
                    Debug.LogError($"No Movement found for Player with index {index}");
                }

                Camera playerCamera = playerManager.GetComponentInChildren<Camera>();
                if (playerCamera != null)
                {
                    playerInput.camera = playerCamera;
                }
                else
                {
                    Debug.LogError($"No Camera found for Player with index {index}");
                }
            }
            else
            {
                Debug.LogError($"No Player found for player index: {index}");
            }
        }

        private PlayerManager FindPlayerManager(GameObject[] players, int index)
        {
            foreach (var player in players)
            {
                var manager = player.GetComponent<PlayerManager>();
                if (manager != null && manager.GetPlayerIndex() == index)
                {
                    return manager;
                }
            }
            return null;
        }

        public void OnMove(CallbackContext context)
        {
            Vector2 input = context.ReadValue<Vector2>();
            playerMovement.SetInputVector(input);
        }
    }
}
