using Cinemachine;
using System;
using UnityEngine;

namespace Core
{
    public class Player : MonoBehaviour
    {
        public static Player Instance;

        public Character PlayerCharacter { get; private set; }
        public PlayerInputHandler PlayerInput { get; private set; }

        public Action<Character> OnPlayerCharacterSpawned;

        public PlayerSettings PlayerSetting;

        private PlayerHUD playerHUD;
        private PlayerCharacterWidget characterWidget;
        private Camera mainCamera;
        private CinemachineBrain cinemachineBrain;
        private CinemachineVirtualCamera cinemachineVirtualCamera;

        private void Awake()
        {
            Instance = this;
            PlayerInput = GetComponent<PlayerInputHandler>();
        }

        public void SetMainCamera(Camera cam)
        {
            mainCamera = cam;
            cinemachineBrain = cam.GetComponent<CinemachineBrain>();
        }

        public void SetVirtualCamera(CinemachineVirtualCamera cam)
        {
            cinemachineVirtualCamera = cam;
        }

        public void SetPlayerHUD(PlayerHUD hud)
        {
            playerHUD = hud;
        }

        public void SpawnPlayerCharacter(Vector3 position)
        {
            // HACK: 플레이어 캐릭터가 여러개 일때는 고려하지 않음
            // 플레이어 캐릭터 생성 
            PlayerCharacter = Instantiate(PlayerSetting.PlayerCharacterPrefab);
            PlayerCharacter.transform.position = position;
            PlayerCharacter.TransitionToState(CharacterState.Idle);

            // 캐릭터 컨트롤러 설정
            var playerCharacterController = PlayerCharacter.GetComponent<PlayerCharacterController>();
            playerCharacterController.SetCamera(mainCamera);

            // 시네머신 카메라 팔로우 대상 지정
            cinemachineVirtualCamera.Follow = PlayerCharacter.transform;

            OnPlayerCharacterSpawned?.Invoke(PlayerCharacter);
        }
    }
}