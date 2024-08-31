using Cinemachine;
using System;
using UnityEngine;

namespace Core
{
    public class Player : MonoBehaviour
    {
        public static Player Instance;

        public Character PlayerCharacter { get; private set; }

        public Action<Character> OnPlayerCharacterSpawned;

        [SerializeField]
        private PlayerSettings PlayerSetting;

        private PlayerInputHandler inputHandler;
        private PlayerCharacterController characterController;
        private PlayerHUD playerHUD;
        private PlayerCharacterWidget characterWidget;
        private Camera mainCamera;
        private CinemachineBrain cinemachineBrain;
        private CinemachineVirtualCamera cinemachineVirtualCamera;

        private void Awake()
        {
            Instance = this;

            inputHandler = GetComponent<PlayerInputHandler>();
            characterController = GetComponent<PlayerCharacterController>();
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

        public void SpawnPlayerCharacter()
        {
            // HACK: 플레이어 캐릭터가 여러개 일때는 고려하지 않음
            // 플레이어 캐릭터 생성 
            PlayerCharacter = Instantiate(PlayerSetting.PlayerCharacter);
            PlayerCharacter.TransitionToState(CharacterState.Idle);

            // 캐릭터 컨트롤러 설정
            PlayerCharacter.SetController(characterController);
            characterController.SetCamera(mainCamera);
            characterController.SetCharacter(PlayerCharacter);

            // 시네머신 카메라 팔로우 대상 지정
            cinemachineVirtualCamera.Follow = PlayerCharacter.transform;

            OnPlayerCharacterSpawned?.Invoke(PlayerCharacter);
        }
    }
}