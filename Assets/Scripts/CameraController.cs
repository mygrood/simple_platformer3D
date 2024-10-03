using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

namespace Platformer
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] [NotNull] private Camera mainCam; // Главная камера для одиночной игры
        [SerializeField] [NotNull] private List<Camera> playerCameras; // Список камер для игроков
        private PlayerInputManager playerInputManager;
        private int playerCount = 0;

        private void Awake()
        {
            playerInputManager = FindObjectOfType<PlayerInputManager>();
            mainCam = Camera.main;
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

        private void OnPlayerJoined(PlayerInput player)
        {
            playerCount++;
            UpdateCamera();
        }

        private void OnPlayerLeft(PlayerInput player)
        {
            playerCount--;
            UpdateCamera();
        }

        private void UpdateCamera()
        {
            // Деактивируем главную камеру, если нет игроков
            mainCam.gameObject.SetActive(playerCount == 0);

            switch (playerCount)
            {
                case 0:
                    // Нет игроков
                    break;

                case 1: // Полноэкранный режим для одного игрока
                    SetCameraViewport(playerCameras[0], 0, 0, 1, 1);
                    break;

                case 2: // Для двух игроков
                    SetCameraViewport(playerCameras[0], 0, 0, 0.5f, 1); // Левая половина
                    SetCameraViewport(playerCameras[1], 0.5f, 0, 0.5f, 1); // Правая половина
                    break;

                case 3: // Для трех игроков
                    SetCameraViewport(playerCameras[0], 0, 0, 0.5f, 0.5f); // Левая верхняя
                    SetCameraViewport(playerCameras[1], 0.5f, 0, 0.5f, 0.5f); // Правая верхняя
                    SetCameraViewport(playerCameras[2], 0, 0.5f, 1, 0.5f); // Нижняя половина
                    break;

                case 4: // Для четырех игроков
                    SetCameraViewport(playerCameras[0], 0, 0, 0.5f, 0.5f); // Левая верхняя
                    SetCameraViewport(playerCameras[1], 0.5f, 0, 0.5f, 0.5f); // Правая верхняя
                    SetCameraViewport(playerCameras[2], 0, 0.5f, 0.5f, 0.5f); // Левая нижняя
                    SetCameraViewport(playerCameras[3], 0.5f, 0.5f, 0.5f, 0.5f); // Правая нижняя
                    break;

                default:
                    // В случае больше четырех игроков
                    for (int i = 0; i < playerCameras.Count; i++)
                    {
                        float x = (i % 2) * 0.5f;
                        float y = (i / 2) * 0.5f;
                        playerCameras[i].rect = new Rect(x, y, 0.5f, 0.5f);
                    }
                    break;
            }
        }

        private void SetCameraViewport(Camera camera, float x, float y, float width, float height)
        {
            camera.rect = new Rect(x, y, width, height); // Изменяем область отображения конкретной камеры
        }

    }
}
