using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Platformer
{
    public class CoopController : MonoBehaviour
    {
        [SerializeField] private List<GameObject> allPlayers = new List<GameObject>();
        [SerializeField] private List<Transform> startingPoints = new List<Transform>();
        [SerializeField] private List<LayerMask> playerLayers= new List<LayerMask>();
        
        public List<GameObject> ActivePlayers { get; private set; } = new List<GameObject>();
        private List<PlayerInputHandler> playersInput = new List<PlayerInputHandler>();
        private PlayerInputManager playerInputManager;

        private void Awake()
        {
            playerInputManager = FindObjectOfType<PlayerInputManager>();
        }

        
        
        private void OnEnable()
        {
            SetAllPlayersActiveState(false);
            playerInputManager.onPlayerJoined += AddPlayer;
        }

        private void OnDisable()
        {
            playerInputManager.onPlayerJoined -= AddPlayer;
        }

        private void AddPlayer(PlayerInput playerInput)
        {
            int playerIndex = playerInput.playerIndex;
            if (playerIndex < 0 || playerIndex >= allPlayers.Count)
            {
                Debug.LogError("Player index is out of bounds.");
                return;
            }

            PlayerInputHandler inputHandler = playerInput.GetComponent<PlayerInputHandler>();
            playersInput.Add(inputHandler);

            GameObject playerPrefab = allPlayers[playerIndex];
            if (playerPrefab != null)
            {
                ActivePlayers.Add(playerPrefab);
                PlayerTeleport(playerPrefab, startingPoints[playerIndex]);
                playerPrefab.SetActive(true);

                Debug.Log($"Player {playerIndex} added and initialized.");
            }
            else
            {
                Debug.LogError("Player could not be initialized.");
            }
        }
        
        private void SetAllPlayersActiveState(bool isActive)
        {
            foreach (var player in allPlayers)
            {
                player.SetActive(isActive);
            }
        }
        public void PlayerTeleport(GameObject player, Transform target)
        {
            player.transform.position = target.position;
        }
    }
}