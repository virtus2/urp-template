using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

namespace Core
{
    /// <summary>
    /// 플레이어의 입력값을 기반으로 한 컨트롤러
    /// </summary>
    public class PlayerCharacterController : BaseCharacterController
    {
        private PlayerInputHandler inputHandler;
        private Character playerCharacter;
        private Camera playerCamera;

        private void Awake()
        {
            inputHandler = GetComponent<PlayerInputHandler>();
        }

        public void SetCamera(Camera camera)
        {
            playerCamera = camera;
        }

        protected override void Update()
        {
            Vector2 playerInput = inputHandler.MoveInput;
            playerInput.Normalize();
            MovementInput = playerInput;

            RollPressed = inputHandler.RollInput;

            base.Update();
        }
    }
}