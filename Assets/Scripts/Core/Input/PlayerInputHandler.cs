using Core.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core
{
    /// <summary>
    /// 플레이어의 입력을 처리하는 컴포넌트
    /// </summary>
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInputHandler : MonoBehaviour
    {
        public PlayerInput PlayerInput;

        public Vector2 MoveInput { get; private set; }
        public Vector2 LookInput { get; private set; }
        public bool JumpInput { get; private set; }
        public bool RollInput { get; private set; }
        public bool RunInput { get; private set; }
        public bool AttackInput { get; private set; }
        public bool InteractInput { get; private set; }

        public Vector3 MousePositionWorld;
        public Vector2 MousePositionScreen;

        private Camera mainCamera;
        private bool DisableActions = false;

        private void Awake()
        {
            PlayerInput = GetComponent<PlayerInput>();
        }

        private void Update()
        {
            MousePositionScreen = Mouse.current.position.ReadValue();

            if (mainCamera)
            {
                Ray ray = mainCamera.ScreenPointToRay(MousePositionScreen);
                if (Physics.Raycast(ray, out RaycastHit hitInfo))
                {
                    MousePositionWorld = hitInfo.point;
                }
                // MousePositionWorld = mainCamera.ScreenToWorldPoint(new Vector3(MousePositionScreen.x, MousePositionScreen.y, -mainCamera.transform.position.z));
            }
            else
            {
                mainCamera = Camera.main;
            }
        }

        public void OnMoveAction(InputAction.CallbackContext context)
        {
            MoveInput = context.ReadValue<Vector2>();
        }

        public void OnLookAction(InputAction.CallbackContext context)
        {
            LookInput = context.ReadValue<Vector2>();
        }

        public void OnJumpAction(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                JumpInput = true;
            }
            else if (context.canceled)
            {
                JumpInput = false;
            }
        }

        public void OnRollAction(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                RollInput = true;
            }
            else if (context.canceled)
            {
                RollInput = false;
            }
        }

        public void OnRunAction(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                RunInput = true;
            }
            else if (context.canceled)
            {
                RunInput = false;
            }
        }

        public void OnAttackAction(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                AttackInput = true;
            }
            else if (context.canceled)
            {
                AttackInput = false;
            }
        }

        public void OnInteractAction(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                InteractInput = true;
            }
            else if (context.canceled)
            {
                InteractInput = false;
            }
        }    
        
        public void OnMenuAction(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                DisableActions = !DisableActions;

                PlayerInstance.Instance.UserSettingsUI.Show();
                Debug.Log("MENU");
            }
        }

        public void OnInventoryAction(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                PlayerInstance.Instance.Inventory.Show();
            }
        }
    }
}