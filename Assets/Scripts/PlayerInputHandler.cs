using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using static UnityEngine.InputSystem.InputAction;


namespace Platformer
{
    public class PlayerInputHandler : MonoBehaviour
    {
        public PlayerManager PlayerManager;
        
        private PlayerInput playerInput;
        private PlayerMovement playerMovement;
        
        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
            // var movers = FindObjectsOfType<PlayerMovement>();
            // Debug.Log($"Found {movers.Length} PlayerMover(s)");
            //
            var players = FindObjectsOfType<PlayerManager>(true);
            Debug.Log($"Found {players.Length} Player(s)");
            
            var index = playerInput.playerIndex;
            PlayerManager = players.FirstOrDefault(m => m.GetPlayerIndex() == index);
            
            
           
            
            if (PlayerManager != null)
            {
                playerMovement = PlayerManager.gameObject.GetComponent<PlayerMovement>();
                if (playerMovement == null)Debug.LogError($"No Movement found for Player with index {index}");
            
                Camera playerCamera = PlayerManager.GetComponentInChildren<Camera>();
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

        public void OnMove(CallbackContext context)
        {
            Vector2 input = context.ReadValue<Vector2>();
            //Debug.Log($"Input received: {input}");
            
            playerMovement.SetInputVector(input);
            
        }
        
        public PlayerManager GetPlayer() {return PlayerManager;}
    }
}