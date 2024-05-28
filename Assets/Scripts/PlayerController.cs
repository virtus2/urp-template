using UnityEngine;

namespace Core
{
    [System.Serializable]
    public class PlayerCameraSettings
    {
        public Vector3 offset;
        public Vector3 angle;
    }
    [RequireComponent(typeof(PlayerInputComponent))]
    public class PlayerController : Controller
    {
        public PlayerCameraSettings playerCameraSettings;
        protected PlayerCamera playerCamera;
        protected PlayerInputComponent playerInputComponent;
        
        public override void Init(Character character)
        {
            base.Init(character);

            playerInputComponent = GetComponent<PlayerInputComponent>();
            playerCamera = FindFirstObjectByType<PlayerCamera>();
            playerCamera.transform.position = transform.position + playerCameraSettings.offset;
            playerCamera.transform.rotation = Quaternion.Euler(playerCameraSettings.angle.x, playerCameraSettings.angle.y, playerCameraSettings.angle.z);
        }

        public void FixedUpdate()   
        {
            Vector3 forward = playerInputComponent.MoveInput.y * Vector3.forward;
            Vector3 right = playerInputComponent.MoveInput.x * Vector3.right;
            Vector3 movement = (forward + right);

            movement *= character.movementSettings.Acceleration;

            characterController.Move(movement * Time.deltaTime);
        }

        public void LateUpdate()
        {
            playerCamera.SetPosition(transform.position + playerCameraSettings.offset);
        }
    }
}
