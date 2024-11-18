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
        private Camera playerCamera;

        public void SetCamera(Camera camera)
        {
            playerCamera = camera;
        }

        private void Update()
        {
            HandleCharacterInput();
        }

        private void LateUpdate()
        {
            HandleCameraInput();   
        }

        private void HandleCharacterInput()
        {
            Vector2 playerInput = Player.Instance.PlayerInput.MoveInput;
            playerInput.Normalize();
            SetMovementInput(playerInput);

            RunPressed = Player.Instance.PlayerInput.RunInput;
            RollPressed = Player.Instance.PlayerInput.RollInput;
            AttackPressed = Player.Instance.PlayerInput.AttackInput;
            InteractPressed = Player.Instance.PlayerInput.InteractInput;
            if (character)
            {
                LookVector = Player.Instance.PlayerInput.MousePositionWorld - character.transform.position;
                // HACK: Top view이기때문에 y축은 사용안함
                LookVector.y = 0;
            }
        }

        private void HandleCameraInput()
        {

        }

    }
}