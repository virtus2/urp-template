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
        public Vector2 MoveInput { get; private set; }
        public Vector2 LookInput { get; private set; }
        public bool JumpInput { get; private set; }
        public bool DashInput { get; private set; }

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
            if (context.started || context.performed)
            {
                JumpInput = true;
            }
            else if (context.canceled)
            {
                JumpInput = false;
            }
        }

        public void OnDashAction(InputAction.CallbackContext context)
        {
            if (context.started || context.performed)
            {
                DashInput = true;
            }
            else if (context.canceled)
            {
                DashInput = false;
            }
        }
    }
}