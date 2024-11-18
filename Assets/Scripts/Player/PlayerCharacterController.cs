using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
using static UnityEditor.AddressableAssets.Build.BuildPipelineTasks.GenerateLocationListsTask;

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
            // Calculate camera direction and rotation on the character plane
            Vector3 cameraPlanarDirection = Vector3.ProjectOnPlane(playerCamera.transform.rotation * Vector3.forward, motor.CharacterUp).normalized;
            if (cameraPlanarDirection.sqrMagnitude == 0f)
            {
                cameraPlanarDirection = Vector3.ProjectOnPlane(playerCamera.transform.rotation * Vector3.up, motor.CharacterUp).normalized;
            }
            Quaternion cameraPlanarRotation = Quaternion.LookRotation(cameraPlanarDirection, motor.CharacterUp);

            Vector2 playerInput = Player.Instance.PlayerInput.MoveInput;
            Vector3 playerMovementInput = Vector3.ClampMagnitude(new Vector3(playerInput.x, 0f, playerInput.y), 1f);

            SetInput(cameraPlanarRotation * playerMovementInput);
            // SetMovementInput(playerInput);

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
            // Create the look input vector for the camera
            float mouseLookAxisUp = Player.Instance.PlayerInput.LookInput.y;
            float mouseLookAxisRight = Player.Instance.PlayerInput.LookInput.x;
            Vector3 lookInputVector = new Vector3(mouseLookAxisRight, mouseLookAxisUp, 0f);

            // Prevent moving the camera while the cursor isn't locked
            if (Cursor.lockState != CursorLockMode.Locked)
            {
                lookInputVector = Vector3.zero;
            }

            /*
            // Input for zooming the camera (disabled in WebGL because it can cause problems)
            float scrollInput = -Input.GetAxis(MouseScrollInput);
#if UNITY_WEBGL
        scrollInput = 0f;
#endif

            // Apply inputs to the camera
            CharacterCamera.UpdateWithInput(Time.deltaTime, scrollInput, lookInputVector);

            // Handle toggling zoom level
            if (Input.GetMouseButtonDown(1))
            {
                CharacterCamera.TargetDistance = (CharacterCamera.TargetDistance == 0f) ? CharacterCamera.DefaultDistance : 0f;
            }
            */
        }

    }
}