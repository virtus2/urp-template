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

        [Tooltip("Useful for rough ground")]
        public float GroundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;
    }

    public class Character : MonoBehaviour
    {
        public MovementSettings movementSettings;
        public Vector3 Velocity => Controller.Velocity;
        public bool IsMoving => stateMachine.CurrentState == CharacterState.GroundMove;
        public bool IsRolling = false;
        public bool IsGrounded = false;
        public float RollingCooldownTime;

        public Controller Controller;

        private CharacterStateMachine stateMachine;
        
        private Animator animator;

        /// <summary>
        /// 이 캐릭터를 조종할 컨트롤러를 설정한다. 
        /// </summary>
        /// <param name="controller"></param>
        public void SetController(Controller controller)
        {
            this.Controller = controller;
        }

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
            CheckIsGrounded();

            if(!IsGrounded)
            {
                Controller.VerticalVelocity = -9.81f;
            }
            else
            {
                Controller.VerticalVelocity = 0.0f;
            }
            // 구르기중이 아닐때에만 구르기 쿨다운시간이 흐른다.
            // TODO: 쿨다운타임 있는 행동들은 Ability클래스를 따로 만들어서 하는게... 나을려나... 굳이인가?
            if(!IsRolling)
            {
                RollingCooldownTime += Time.deltaTime;
            }
        }

        /// <summary>
        /// 현재 캐릭터가 땅에 닿아있는지 검사한다.
        /// </summary>
        private void CheckIsGrounded()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - movementSettings.GroundedOffset, transform.position.z);
            IsGrounded = Physics.CheckSphere(spherePosition, movementSettings.GroundedRadius, movementSettings.GroundLayers, QueryTriggerInteraction.Ignore);
        }


        public bool CanRoll()
        {
            bool IsOnCooldown = RollingCooldownTime <= movementSettings.RollingCooldownTime;
            // 캐릭터의 현재 상태가 구르기 상태로 넘어갈 수 있는지도 체크해야할 수도 있음.
            // ...
            return !IsOnCooldown && !IsRolling;
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (IsGrounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Vector3 sphereCenter = new Vector3(transform.position.x, transform.position.y - movementSettings.GroundedOffset, transform.position.z);
            Gizmos.DrawSphere(sphereCenter, movementSettings.GroundedRadius);
        }
    }
}
