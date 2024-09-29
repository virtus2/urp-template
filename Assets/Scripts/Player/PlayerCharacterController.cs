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
            SetMovementInput(playerInput);

            RunPressed = inputHandler.RunInput;
            RollPressed = inputHandler.RollInput;
            AttackPressed = inputHandler.AttackInput;
            InteractPressed = inputHandler.InteractInput;
            if (character)
            {
                LookVector = inputHandler.MousePositionWorld - character.transform.position;
                // HACK: Top view이기때문에 y축은 사용안함
                LookVector.y = 0;
            }
            

            base.Update();
        }
    }
}