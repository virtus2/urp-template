using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.XR;
using UnityEngine.TextCore.Text;
using UnityEngine.Windows;

namespace Core
{
    public class Character : MonoBehaviour
    {
        public enum CharacterControllerType { None, Player, AI };

        public MovementSettings MovementSettings;
        public BaseCharacterController Controller;
        public CharacterControllerType ControllerType;

        public Vector3 Velocity => Controller.Velocity;
        public float CapsuleRadius => Controller.Radius;
        public bool IsMoving => stateMachine.CurrentState == CharacterState.GroundMove;
        public bool IsRolling = false;
        public float RollingCooldownTime;
        public bool IsGrounded = false;
        public bool IsAttacking = false;
        public bool IsWalkedOffALedge = false; // 

        /* Controller의 입력과 상관없이 캐릭터를 강제로 움직임 */
        public bool OverrideMovementVector = false; 
        public Vector2 OverridedMovementVector = Vector2.zero;

        /* 캐릭터의 이동 */
        public float HorizontalSpeed = 0f; // 현재 수평 속도 (x축, z축)
        public float TargetHorizontalSpeed = 0f; // 타겟 수평 속도 (가/감속 전)
        public float VerticalSpeed = 0f; // 현재 수직 속도 (y축)
        public bool AccelerateToTargetHorizontalSpeed = false; // 타겟 속도까지 가/감속 할 지 여부

        private Quaternion targetRotation = Quaternion.identity;

        private CharacterStateMachine stateMachine;
        private Animator animator;
        private float animationBlend = 0f;
        private int AnimationID_Speed = Animator.StringToHash("Speed");
        private int AnimationID_Grounded = Animator.StringToHash("Grounded");
        private int AnimationID_MotionSpeed = Animator.StringToHash("MotionSpeed");
        private int AnimationID_Attack = Animator.StringToHash("Attack"); // TODO: 애니메이터 컨트롤러에 매개변수 추가
        private int AnimationID_FreeFall = Animator.StringToHash("FreeFall");

        // AI 관련
        public bool IsReachedDestination = false;
        public Character ChaseTarget;

        /// 이 캐릭터를 조종할 컨트롤러를 설정한다. 
        /// </summary>
        /// <param name="controller"></param>
        public void SetController(BaseCharacterController controller)
        {
            this.Controller = controller;
            if (controller is AICharacterController)
            {
                ControllerType = CharacterControllerType.AI;
            }
            else if (controller is PlayerCharacterController)
            {
                ControllerType = CharacterControllerType.Player;
            }
            else
            {
                ControllerType = CharacterControllerType.None;
            }
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
            RollingCooldownTime = MovementSettings.RollingCooldownTime;
        }

        private void Update()
        {
            // 로코모션
            CheckIsGrounded();
            UpdateHorizontalSpeed();
            UpdateVerticalSpeed();
            UpdateMovementVector();
            UpdateRotation();
            UpdateAnimations();

            // 게임 시스템
            UpdateRollCooldownTime();
        }

        /// <summary>
        /// 현재 캐릭터가 땅에 닿아있는지 검사한다.
        /// </summary>
        private void CheckIsGrounded()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - MovementSettings.GroundedOffset, transform.position.z);
            IsGrounded = Physics.CheckSphere(spherePosition, MovementSettings.GroundedRadius, MovementSettings.GroundLayers, QueryTriggerInteraction.Ignore);
        }

        private void UpdateHorizontalSpeed()
        {
            // 현재 수평 속력
            float currentHorizontalSpeed = new Vector3(Velocity.x, 0.0f, Velocity.z).magnitude;
            float inputMagnitude = Controller.MovementInput.magnitude; // _input.analogMovement ? _input.move.magnitude : 1f;\

            // 자연스러운 속도 변경을 위해서 목표 속도에 가까워지게 감속 또는 가속한다.
            if (AccelerateToTargetHorizontalSpeed)
            {
                float acceleration = currentHorizontalSpeed < TargetHorizontalSpeed ? MovementSettings.Acceleration : MovementSettings.Decceleration;
                HorizontalSpeed = Mathf.MoveTowards(HorizontalSpeed, TargetHorizontalSpeed, acceleration * Time.deltaTime);
            }
            else
            {
                HorizontalSpeed = TargetHorizontalSpeed;
            }
        }

        private void UpdateVerticalSpeed()
        {
            if (IsGrounded)
            {
                VerticalSpeed = -MovementSettings.GroundedGravity;
            }
            else
            {
                if(IsWalkedOffALedge)
                {
                    VerticalSpeed = 0.0f;
                }

                VerticalSpeed = Mathf.MoveTowards(VerticalSpeed, -MovementSettings.MaxFallSpeed, MovementSettings.Gravity * Time.deltaTime);
            }
        }

        private void UpdateMovementVector()
        {
            Vector3 movement = OverrideMovementVector ?
                new Vector3(OverridedMovementVector.x, 0, OverridedMovementVector.y) :
                new Vector3(Controller.MovementInput.x, 0, Controller.MovementInput.y);

            Controller.MovementVector = movement * HorizontalSpeed + Vector3.up * VerticalSpeed;
        }

        private void UpdateRotation()
        {
            Vector3 horizontalMovementVector = new Vector3(Controller.MovementVector.x, 0, Controller.MovementVector.z);
            if (horizontalMovementVector.sqrMagnitude > 0.0f)
            {
                float rotationSpeed = Mathf.Lerp(MovementSettings.MaxRotationSpeed, MovementSettings.MinRotationSpeed, HorizontalSpeed / TargetHorizontalSpeed);

                targetRotation = Quaternion.LookRotation(horizontalMovementVector, Vector3.up);
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
            animator.SetFloat(AnimationID_MotionSpeed, 1f);
            animator.SetFloat(AnimationID_Speed, HorizontalSpeed); // TODO: Override MovementVector때문에 캐릭터의 최종 속도 기준으로 값을 변경해야한다.
            // animator.SetBool(AnimationID_Attack, IsAttacking); // TODO: 애니메이터 컨트롤러에 매개변수 추가
            animator.SetBool(AnimationID_Grounded, IsGrounded);
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
            bool IsOnCooldown = RollingCooldownTime <= MovementSettings.RollingCooldownTime;
            // 캐릭터의 현재 상태가 구르기 상태로 넘어갈 수 있는지도 체크해야할 수도 있음.
            // ...
            return !IsOnCooldown && !IsRolling;
        }


        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                // TODO: 발소리 내기?
            }
        }


        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (IsGrounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Vector3 sphereCenter = new Vector3(transform.position.x, transform.position.y - MovementSettings.GroundedOffset, transform.position.z);
            Gizmos.DrawSphere(sphereCenter, MovementSettings.GroundedRadius);

            if (Controller)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawRay(transform.position, Controller.MovementVector);

                Gizmos.color = Color.red;
                Gizmos.DrawRay(transform.position, Controller.MovementInput);
            }
        }
    }
}
