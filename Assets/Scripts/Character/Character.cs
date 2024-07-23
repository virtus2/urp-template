using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [System.Serializable]
    public class MovementSettings
    {
        public float WalkingSpeed = 0.83f;
        public float RunningSpeed= 1.25f;
        public float JumpSpeed = 10.0f; // In meters/second
        public float JumpAbortSpeed = 10.0f; // In meters/second

        public float RollingSpeed = 5.0f;
        public float RollingDuration = 0.25f; // seconds
        public float RollingCooldownTime = 0.5f;
    }

    public class Character : MonoBehaviour
    {
        public MovementSettings movementSettings;
        public Vector3 Velocity => Controller.Velocity;
        public bool IsMoving => stateMachine.CurrentState == CharacterState.GroundMove;
        public bool IsRolling = false;
        public float RollingCooldownTime;

        public Controller Controller;

        private CharacterStateMachine stateMachine;
        
        private Animator animator;

        private void Awake()
        {
            stateMachine = GetComponent<CharacterStateMachine>();
            animator = GetComponent<Animator>();

            Initialize();
        }

        private void Initialize()
        {
            IsRolling = false;
            RollingCooldownTime = movementSettings.RollingCooldownTime;
        }

        private void Update()
        {
            RollingCooldownTime += Time.deltaTime;
        }

        public void SetController(Controller controller)
        {
            this.Controller = controller;
        }

        public bool CanRoll()
        {
            bool IsOnCooldown = RollingCooldownTime <= movementSettings.RollingCooldownTime;
            // 캐릭터의 현재 상태가 구르기 상태로 넘어갈 수 있는지도 체크해야할 수도 있음.
            // ...
            return !IsOnCooldown && !IsRolling;
        }
    }
}
