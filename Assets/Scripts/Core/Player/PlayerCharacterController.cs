﻿using Core.Character;
using UnityEngine;

namespace Core.Player
{
    /// <summary>
    /// 플레이어의 입력값을 기반으로 한 컨트롤러
    /// </summary>
    public class PlayerCharacterController : BaseCharacterController
    {
        private PlayerCharacterFollowCamera playerCharacterFollowCamera;
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
            // Calculate camera direction and rotation on the character plane
            Vector3 cameraPlanarDirection = Vector3.ProjectOnPlane(playerCamera.transform.rotation * Vector3.forward, motor.CharacterUp).normalized;
            if (cameraPlanarDirection.sqrMagnitude == 0f)
            {
                cameraPlanarDirection = Vector3.ProjectOnPlane(playerCamera.transform.rotation * Vector3.up, motor.CharacterUp).normalized;
            }
            Quaternion cameraPlanarRotation = Quaternion.LookRotation(cameraPlanarDirection, motor.CharacterUp);

            Vector2 playerInput = PlayerInstance.Instance.PlayerInput.MoveInput;
            Vector3 playerMovementInput = Vector3.ClampMagnitude(new Vector3(playerInput.x, 0f, playerInput.y), 1f);

            Vector3 movementInputVector = cameraPlanarRotation * playerMovementInput;
            SetMovementInput(movementInputVector);
            if (OrientationMethod == OrientationMethod.TowardsMovement)
            {
                SetLookInput(movementInputVector);
            }
            else if (OrientationMethod == OrientationMethod.TowardsCursor)
            {
                // Top view이기때문에 y축은 사용안함
                Vector3 toCursor = PlayerInstance.Instance.PlayerInput.MousePositionWorld - character.transform.position;
                toCursor.y = 0;
                toCursor.Normalize();
                SetLookInput(toCursor);
                UseDirectionalMovement = true;
            }
            else if (OrientationMethod == OrientationMethod.TowardsCamera)
            {

            }

            RunPressed = PlayerInstance.Instance.PlayerInput.RunInput;
            RollPressed = PlayerInstance.Instance.PlayerInput.RollInput;
            AttackPressed = PlayerInstance.Instance.PlayerInput.AttackInput;
            InteractPressed = PlayerInstance.Instance.PlayerInput.InteractInput;
        }

        private void HandleCameraInput()
        {
            // Create the look input vector for the camera
            float mouseLookAxisUp = PlayerInstance.Instance.PlayerInput.LookInput.y;
            float mouseLookAxisRight = PlayerInstance.Instance.PlayerInput.LookInput.x;
            Vector3 lookInputVector = new Vector3(mouseLookAxisRight, mouseLookAxisUp, 0f);

            // Prevent moving the camera while the cursor isn't locked
            if (Cursor.lockState != CursorLockMode.Locked)
            {
                lookInputVector = Vector3.zero;
            }

            float scrollInput = 0;
            /*
            // Input for zooming the camera (disabled in WebGL because it can cause problems)
            float scrollInput = -Input.GetAxis(MouseScrollInput);
#if UNITY_WEBGL
        scrollInput = 0f;
#endif
            */      
            // Apply inputs to the camera
            PlayerInstance.Instance.PlayerCharacterFollowCamera.UpdateWithInput(Time.deltaTime, scrollInput, lookInputVector);

            /*
            // Handle toggling zoom level
            if (Input.GetMouseButtonDown(1))
            {
                CharacterCamera.TargetDistance = (CharacterCamera.TargetDistance == 0f) ? CharacterCamera.DefaultDistance : 0f;
            }
            */
        }

    }
}