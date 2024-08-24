using Cinemachine;
using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace Core
{
    public class Player : MonoBehaviour
    {
        public Character PlayerCharacter { get; private set; }

        [SerializeField]
        private PlayerSettings PlayerSetting;

        private PlayerInputHandler inputHandler;
        private PlayerCharacterController characterController;
        private Camera mainCamera;
        private CinemachineBrain cinemachineBrain;
        private CinemachineVirtualCamera cinemachineVirtualCamera;

        private void Awake()
        {
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

        public void SpawnPlayerCharacter()
        {
            // 플레이어 캐릭터 생성 
            PlayerCharacter = Instantiate(PlayerSetting.PlayerCharacter);
            PlayerCharacter.TransitionToState(CharacterState.Idle);

            // 캐릭터 컨트롤러 설정
            PlayerCharacter.SetController(characterController);
            characterController.SetCamera(mainCamera);
            characterController.SetCharacter(PlayerCharacter);

            // 시네머신 카메라 팔로우 대상 지정
            cinemachineVirtualCamera.Follow = PlayerCharacter.transform;
        }
    }
}