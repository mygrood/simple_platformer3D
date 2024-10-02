using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Platformer
{
    public class CoopController : MonoBehaviour
    {
        public List<GameObject> players;
        
        private List<PlayerInputHandler> playersInput = new List<PlayerInputHandler>();
        
        private List<PlayerManager> playerManagers = new List<PlayerManager>();
        
        [SerializeField] 
        private List<Transform> startingPoints;
        
        [SerializeField]
        private List<LayerMask> playerLayers;
        
        private PlayerInputManager playerInputManager;

        private void Awake()
        {
            playerInputManager = FindObjectOfType<PlayerInputManager>();
        }

        private void OnEnable()
        {
            foreach (var p in players)
            {
                p.SetActive(false);
            }
            playerInputManager.onPlayerJoined += AddPlayer;
        }

        private void OnDisable()
        {
            playerInputManager.onPlayerJoined -= AddPlayer;
        }

        public void AddPlayer(PlayerInput playerInput)
        {
            int playerIndex = playerInput.playerIndex;
            
            PlayerInputHandler inputHandler = playerInput.GetComponent<PlayerInputHandler>();
            playersInput.Add(inputHandler);
            
            if ( players[playerIndex] != null)
            {
                players[playerIndex].SetActive(true);
                
                //StartingPoint
                players[playerIndex].transform.position = startingPoints[0].position;

                Debug.Log($"Player {playerIndex} added and initialized.");
            }
            else
            {
                Debug.LogError("Player could not be initialized.");
            }
        }
    }
}