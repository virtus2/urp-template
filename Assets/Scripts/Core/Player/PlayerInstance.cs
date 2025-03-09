using Core.UI;
using System;
using Unity.Cinemachine;
using UnityEngine;

namespace Core.Player
{
    public class PlayerInstance : MonoBehaviour
    {
        public static PlayerInstance Instance;

        public Character.BaseCharacter PlayerCharacter { get; private set; }
        public PlayerInputHandler PlayerInput { get; private set; }
        public PlayerCharacterFollowCamera PlayerCharacterFollowCamera { get; private set; }
        public CinemachineBrain CinemachineBrain;

        public Action<Core.Character.BaseCharacter> OnPlayerCharacterSpawned;

        public PlayerSettings PlayerSetting;

        private PlayerHUD playerHUD;
        private PlayerCharacterWorldUI characterWidget;
        private Camera mainCamera;

        private void Awake()
        {
            Instance = this;
            PlayerInput = GetComponent<PlayerInputHandler>();
        }

        public void SetMainCamera(Camera cam)
        {
            mainCamera = cam;
            CinemachineBrain = cam.GetComponent<CinemachineBrain>();
        }

        public void SetPlayerCharacterFollowCamera(PlayerCharacterFollowCamera camera)
        {
            PlayerCharacterFollowCamera = camera;
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
            PlayerCharacter.Controller.SetPosition(position);

            // 캐릭터 컨트롤러 설정
            var playerCharacterController = PlayerCharacter.GetComponent<PlayerCharacterController>();
            playerCharacterController.SetCamera(mainCamera);

            // 시네머신 카메라 팔로우 대상 지정
            PlayerCharacterFollowCamera.SetFollowTarget(PlayerCharacter.transform);

            OnPlayerCharacterSpawned?.Invoke(PlayerCharacter);
        }

        public void RespawnPlayerCharacter(Vector3 position)
        {
            if (PlayerCharacter != null)
            {
                Destroy(PlayerCharacter.gameObject);
            }
            SpawnPlayerCharacter(position);
        }

        public void TeleportPlayerCharacter(Vector3 position)
        {
            if(PlayerCharacter == null)
            {
                Debug.LogError("PlayerCharacter is null. Unable to teleport");
            }
            PlayerCharacter.Controller.SetPosition(position);
        }
    }
}