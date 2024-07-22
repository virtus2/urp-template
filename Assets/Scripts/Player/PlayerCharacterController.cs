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

            // ž��ϱ� ī�޶�� ��꿡�� �����Ѵ�.
            // Quaternion cameraRotation = playerCamera.transform.rotation;

            Vector3 movementVector = Vector3.right * movementInput.x + Vector3.forward * movementInput.y;

            switch (stateMachine.CurrentState)
            {
                // Idle, GroundMove ���´� �÷��̾� ��ǲ�� ���� �״�� �����δ�.
                case CharacterState.Idle:
                case CharacterState.GroundMove:
                    return movementVector;

                // ���Ŀ� ��ٸ�Ÿ�⳪ ��Ÿ�� ���� ���¿����� �ٸ��� ����������Ѵ�.
                case CharacterState.LedgeStandingUp:
                case CharacterState.WallRun:
                    break;
            }

            return Vector3.zero;
        }
    }
}