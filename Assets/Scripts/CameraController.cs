using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

namespace Platformer
{
    public class CameraController : MonoBehaviour
    {
        // Serialized fields for camera objects and player transforms
        [SerializeField] [NotNull] private GameObject mainCamGO; // Main virtual camera
        [SerializeField] [NotNull] private List<GameObject> playerCamerasGO; // List of player cameras
        public List<Transform> playerTransforms = new List<Transform>(); // List of player transforms
        [SerializeField] private Transform cameraFocusPoint;

        // Configuration for split-screen
        public bool splitScreen = true;
        public float splitScreenThreshold = 5f; // Distance threshold for switching to split-screen

        private PlayerInputManager playerInputManager;
        private int playerCount = 0;
        private List<Camera> playerCameras = new List<Camera>();

        private void Awake()
        {
            playerInputManager = FindObjectOfType<PlayerInputManager>();
        }

        private void OnEnable()
        {
            playerInputManager.onPlayerJoined += OnPlayerJoined;
            playerInputManager.onPlayerLeft += OnPlayerLeft;
        }

        private void OnDisable()
        {
            playerInputManager.onPlayerJoined -= OnPlayerJoined;
            playerInputManager.onPlayerLeft -= OnPlayerLeft;
        }

        private void LateUpdate()
        {
            UpdateCamera(); // Update the camera every frame
            UpdateCameraFocus(); // Update the camera focus
        }

        // Event handlers for player joining and leaving
        private void OnPlayerJoined(PlayerInput player)
        {
            playerCount++;
            UpdateCamera(); // Update camera when a player joins
        }

        private void OnPlayerLeft(PlayerInput player)
        {
            playerCount--;
            UpdateCamera(); // Update camera when a player leaves
        }

        // Updates the camera based on the distance between players
        private void UpdateCamera()
        {
            if (playerCount <= 0)
            {
                // Если игроков нет, не выполняем переключение камер
                Debug.Log("No players currently in the game.");
                return; // Возвращаемся, не изменяя состояние камеры
            }
            
            // Check if there are players and update camera mode accordingly
            if (playerCount == 1)
            {
                SwitchToSinglePlayerCamera(); // Switch to single player camera
            }
            else if (ArePlayersFarApart(splitScreenThreshold))
            {
                SwitchToSplitScreen(); // Switch to split-screen mode
            }
            else
            {
                SwitchToMainCamera(); // Switch to the main camera
            }
        }

        // Switch to the main camera
        public void SwitchToMainCamera()
        {
            mainCamGO.SetActive(true); // Enable the main virtual camera
            foreach (Camera playerCam in playerCameras)
            {
                playerCam.gameObject.SetActive(false); // Disable player cameras
            }
        }

        // Switch to single player camera setup
        public void SwitchToSinglePlayerCamera()
        {
            mainCamGO.SetActive(false); // Disable the main camera

            if (playerCameras.Count > 0) // Ensure there is at least one player camera available
            {
                Camera playerCam = playerCameras[0];
                playerCam.gameObject.SetActive(true); // Enable the single player camera
                SetCameraViewport(playerCam, 0, 0, 1, 1); // Fullscreen for one player
            }
        }

        // Updates the camera focus based on player positions
        private void UpdateCameraFocus()
        {
            if (playerCount == 2)
            {
                Vector3 middlePoint = (playerTransforms[0].position + playerTransforms[1].position) / 2;
                cameraFocusPoint.position = middlePoint; // Set focus on the midpoint between two players
            }
            else if (playerCount == 1)
            {
                cameraFocusPoint.position = playerTransforms[0].position; // Set focus on the position of one player
            }
        }

        // Switch to split-screen mode
        public void SwitchToSplitScreen()
        {
            splitScreen = true;
            mainCamGO.SetActive(false); // Disable the main camera

            if (playerCount == 1)
            {
                SetSinglePlayerCamera(); // Set up camera for one player
            }
            else if (playerCount == 2)
            {
                SetDualPlayerCameras(); // Set up cameras for two players
            }
        }

        // Set up camera for a single player
        private void SetSinglePlayerCamera()
        {
            mainCamGO.SetActive(false);
            if (playerCameras.Count < 1)
            {
                Debug.LogWarning("No player cameras available for single player mode.");
                return; // Выход из метода, если камер недостаточно
            }
            Camera playerCam1 = playerCameras[0];
            playerCam1.gameObject.SetActive(true);
            SetCameraViewport(playerCam1, 0, 0, 1, 1); // Fullscreen for one player
        }

        // Set up cameras for two players
        private void SetDualPlayerCameras()
        {
            if (playerCameras.Count < 2)
            {
                Debug.LogWarning($"Not enough player cameras for dual player mode. Required: 2, Found: {playerCameras.Count}");
                return; // Если камер недостаточно, выходим
            }
            splitScreen = true;
            Camera playerCam1 = playerCameras[0];
            Camera playerCam2 = playerCameras[1];

            // Set left and right camera viewports for two players
            SetCameraViewport(playerCam1, 0, 0, 0.5f, 1); // Left half
            SetCameraViewport(playerCam2, 0.5f, 0, 0.5f, 1); // Right half
        }

        // Check if players are far apart based on a distance threshold
        public bool ArePlayersFarApart(float thresholdDistance)
        {
            if (playerTransforms.Count < 2) return false; // Ensure at least two players
            float distance = Vector3.Distance(playerTransforms[0].position, playerTransforms[1].position);
            return distance > thresholdDistance; // Check if distance exceeds threshold
        }

        // Set the viewport for a specific camera
        private void SetCameraViewport(Camera camera, float x, float y, float width, float height)
        {
            camera.rect = new Rect(x, y, width, height); // Update camera's viewport
        }
    }
}
