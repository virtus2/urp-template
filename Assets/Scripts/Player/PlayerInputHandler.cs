using UnityEngine;
using UnityEngine.InputSystem;

namespace Core
{
    /// <summary>
    /// �÷��̾��� �Է��� ó���ϴ� ������Ʈ
    /// </summary>
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInputHandler : MonoBehaviour
    {
        public Vector2 MoveInput { get; private set; }
        public Vector2 LookInput { get; private set; }
        public bool JumpInput { get; private set; }
        public bool RollInput { get; private set; }

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

        public void OnRollAction(InputAction.CallbackContext context)
        {
            if (context.started || context.performed)
            {
                RollInput = true;
            }
            else if (context.canceled)
            {
                RollInput = false;
            }
        }
    }
}