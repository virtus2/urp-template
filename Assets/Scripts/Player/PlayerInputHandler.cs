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
        public bool RollInput { get; private set; }
        public bool AttackInput { get; private set; }
        public Vector3 MousePositionWorld;
        public Vector2 MousePositionScreen;
        public AnimalSpawner spawner;
        public PlayerData playerData;
        
        private Camera mainCamera;

        private void Awake()
        {
            mainCamera = Camera.main;
        }

        private void Update()
        {
            MousePositionScreen = Mouse.current.position.ReadValue();

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = mainCamera.ScreenPointToRay(MousePositionScreen);
                LayerMask layerMask = 1 << LayerMask.NameToLayer("Default");
                if (Physics.Raycast(ray, out RaycastHit hitInfo, 1000f, layerMask))
                {
                    float increaseAmount = 1.0f + 0.1f * PlayerData.foodUpgrade;
                    if (hitInfo.collider.CompareTag("Gold"))
                    {
                        PlayerData.balance += (uint)Mathf.Floor((float)playerData.gameData.goldAmount * increaseAmount);
                        Destroy(hitInfo.collider.gameObject);
                        AudioManager.Instance.sfxCoin.Play();
                        return;
                    }
                    if (hitInfo.collider.CompareTag("FoodSpawn"))
                    {
                        spawner.SpawnFood(hitInfo.point);
                        return;
                    }
                    if (hitInfo.collider.CompareTag("Diamond"))
                    {
                        PlayerData.balance += (uint)Mathf.Floor((float)playerData.gameData.diamondAmount * increaseAmount);
                        Destroy(hitInfo.collider.gameObject);
                        AudioManager.Instance.sfxCoin.Play();
                        return;
                    }
                    if (hitInfo.collider.CompareTag("Pearl"))
                    {
                        PlayerData.balance += (uint)Mathf.Floor((float)playerData.gameData.pearlAmount * increaseAmount);
                        Destroy(hitInfo.collider.gameObject);
                        AudioManager.Instance.sfxCoin.Play();
                        return;
                    }
                }
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

        public void OnAttackAction(InputAction.CallbackContext context)
        {
            if (context.started || context.performed)
            {
                AttackInput = true;
            }
            else if (context.canceled)
            {
                AttackInput = false;
            }
        }
    }
}