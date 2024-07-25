using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.TextCore.Text;
using UnityEngine.Windows;

namespace Core
{
    [System.Serializable]
    public class MovementSettings
    {
        /* In meters/second */
        public float WalkSpeed = 1.25f; // 걷기 이동 속도
        public float SprintSpeed= 3.5f; // 달리기 이동 속도
        public float Acceleration = 10.0f; // 걷기<->달리기 가속도
        public float Decceleration = 10.0f; // 걷기<->달리기 감속도
        public float JumpSpeed = 10.0f; //  점프 속도
        public float JumpAbortSpeed = 10.0f;

        public float RollingSpeed = 5.0f; // 구르기 이동속도
        public float RollingDuration = 0.25f; // 구르기 상태 지속 시간
        public float RollingCooldownTime = 0.5f; // 구르기 쿨다운 타임 

        public float MaxRotationSpeed = 1200.0f;
        public float MinRotationSpeed = 600.0f;

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
        public Controller Controller;
        public Vector3 Velocity => Controller.Velocity;
        public bool IsMoving => stateMachine.CurrentState == CharacterState.GroundMove;
        public bool IsRolling = false;
        public float RollingCooldownTime;
        public bool IsGrounded = false;

        public Vector3 MovementOverrideVector;
        public float HorizontalSpeed = 0f;
        public float TargetHorizontalSpeed = 0f;
        public float VerticalSpeed = 0f;
        public bool AccelerateToTargetHorizontalSpeed = false;

        private CharacterStateMachine stateMachine;
        private Animator animator;
        private float animationBlend = 0f;
        private int AnimationID_Speed = Animator.StringToHash("Speed");
        private int AnimationID_Grounded = Animator.StringToHash("Grounded");
        private int AnimationID_MotionSpeed = Animator.StringToHash("MotionSpeed");
        private Quaternion targetRotation = Quaternion.identity;

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

            Initialize();
        }

        private void Initialize()
        {
            IsRolling = false;
            RollingCooldownTime = movementSettings.RollingCooldownTime;
        }

        private void Update()
        {
            // 로코모션
            CheckIsGrounded();
            UpdateHorizontalSpeed();
            UpdateVerticalSpeed();
            UpdateRotation();
            UpdateAnimations();
            // TODO: 구르기는 MovementInput이 0이어도 구르기상태에 들어갔던 방향 그대로 움직여야함
            Controller.MovementVector = new Vector3(Controller.MovementInput.x, 0, Controller.MovementInput.y) * HorizontalSpeed + Vector3.up * VerticalSpeed;

            // 게임 시스템
            UpdateRollCooldownTime();
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

        private void UpdateHorizontalSpeed()
        {
            // 현재 수평 속력
            float currentHorizontalSpeed = new Vector3(Velocity.x, 0.0f, Velocity.z).magnitude;
            float inputMagnitude = Controller.MovementInput.magnitude; // _input.analogMovement ? _input.move.magnitude : 1f;\

            // 자연스러운 속도 변경을 위해서 목표 속도에 가까워지게 감속 또는 가속한다.
            if (AccelerateToTargetHorizontalSpeed)
            {
                float acceleration = currentHorizontalSpeed < TargetHorizontalSpeed ? movementSettings.Acceleration : movementSettings.Decceleration;
                HorizontalSpeed = Mathf.MoveTowards(HorizontalSpeed, TargetHorizontalSpeed, acceleration * Time.deltaTime);
            }
            else
            {
                HorizontalSpeed = TargetHorizontalSpeed;
            }
        }

        private void UpdateVerticalSpeed()
        {
            if(IsGrounded)
            {
                VerticalSpeed = -9.81f;
            }
            else
            {
                VerticalSpeed = Mathf.MoveTowards(VerticalSpeed, -40.0f, 9.81f * Time.deltaTime);
            }
        }

        private void UpdateRotation()
        {
            Vector3 horizontalMovementVector = new Vector3(Controller.MovementVector.x, 0, Controller.MovementVector.z);
            if (horizontalMovementVector.sqrMagnitude > 0.0f)
            {
                float rotationSpeed = Mathf.Lerp(movementSettings.MaxRotationSpeed, movementSettings.MinRotationSpeed, HorizontalSpeed / TargetHorizontalSpeed);

                Quaternion targetRotation = Quaternion.LookRotation(horizontalMovementVector, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }

        private void UpdateRollCooldownTime()
        {
            // 구르기중이 아닐때에만 구르기 쿨다운시간이 흐른다.
            // TODO: 쿨다운타임 있는 행동들은 Ability클래스를 따로 만들어서 하는게... 나을려나... 굳이인가?
            if (!IsRolling)
            {
                RollingCooldownTime += Time.deltaTime;
            }
        }

        private void UpdateAnimations()
        {
            /*
            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            animator.SetBool(AnimationID_Grounded, IsGrounded);
            animator.SetFloat(AnimationID_Speed, 1f);
            animator.SetFloat(AnimationID_MotionSpeed, 1f);
            animationBlend = Mathf.Lerp(animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (animationBlend < 0.01f) animationBlend = 0f;
            */
        }

        /// <summary>
        /// 현재 캐릭터가 구르기 행동을 할 수 있는지 검사한다.
        /// </summary>
        public bool CanRoll()
        {
            bool IsOnCooldown = RollingCooldownTime <= movementSettings.RollingCooldownTime;
            // 캐릭터의 현재 상태가 구르기 상태로 넘어갈 수 있는지도 체크해야할 수도 있음.
            // ...
            return !IsOnCooldown && !IsRolling;
        }

        // TODO: AnimationEvent를 통해서 발소리 내기 등등 함수 만들기
        // ...


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
