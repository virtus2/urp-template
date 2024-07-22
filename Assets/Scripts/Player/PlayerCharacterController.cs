using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

namespace Core
{
    public class PlayerCharacterController : Controller
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

        public override Vector3 GetMovementVector3()
        {
            Vector2 movementInput = inputHandler.MoveInput;
            movementInput.Normalize();

            // 탑뷰니까 카메라는 계산에서 제외한다.
            // Quaternion cameraRotation = playerCamera.transform.rotation;

            Vector3 movementVector = Vector3.right * movementInput.x + Vector3.forward * movementInput.y;

            switch (stateMachine.CurrentState)
            {
                // Idle, GroundMove 상태는 플레이어 인풋을 따라서 그대로 움직인다.
                case CharacterState.Idle:
                case CharacterState.GroundMove:
                    return movementVector;

                // 추후에 사다리타기나 벽타기 등의 상태에서는 다르게 움직여줘야한다.
                case CharacterState.LedgeStandingUp:
                case CharacterState.WallRun:
                    break;
            }

            return Vector3.zero;
        }
    }
}