using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.XR;
using UnityEngine.TextCore.Text;
using UnityEngine.Windows;

namespace Core
{
    [Serializable]
    public class ComponentReferenceCharacter : ComponentReference<Character>
    {
        public ComponentReferenceCharacter(string guid) : base(guid) { }
    }

    public class Character : MonoBehaviour
    {
        public enum CharacterControllerType { None, Player, AI };

        public MovementSettings MovementSettings;
        public RollingSettings RollingSettings;
        public BaseCharacterController Controller;
        public CharacterControllerType ControllerType;

        public Vector3 Velocity => Controller.Velocity;
        public float CapsuleRadius => Controller.Radius;
        public bool IsMoving => stateMachine.CurrentState == CharacterState.GroundMove;
        public bool IsRolling = false;
        public float RollingCooldownTime;
        public bool IsGrounded = false;
        public bool IsAttacking = false;
        public bool IsWalkedOffALedge = false; // Grounded상태였다가 떨어지기 시작한 순간의 프레임에 true

        /* Controller의 입력과 상관없이 캐릭터를 강제로 움직임 */
        protected bool IsMovementVectorOverrided = false; 
        protected Vector2 OverridedMovementVector = Vector2.zero;

        /* 캐릭터의 이동 */
        public float HorizontalSpeed = 0f; // 현재 수평 속도 (x축, z축)
        public float TargetHorizontalSpeed = 0f; // 타겟 수평 속도 (가/감속 전)
        public float VerticalSpeed = 0f; // 현재 수직 속도 (y축)
        public bool AccelerateToTargetHorizontalSpeed = false; // 타겟 속도까지 가/감속 할 지 여부
        public float Acceleration = 0f;

        private Quaternion targetRotation = Quaternion.identity;
        private Vector3 targetDirection = Vector3.forward;

        private CharacterAttributeComponent attributeComponent;
        private CharacterHealthComponent healthComponent;
        private CharacterStateMachine stateMachine;
        private Animator animator;


        private float animationBlend = 0f;
        private int AnimationID_Speed = Animator.StringToHash("Speed");
        private int AnimationID_IsGrounded = Animator.StringToHash("IsGrounded");
        private int AnimationID_IsRolling = Animator.StringToHash("IsRolling"); 
        private int AnimationID_MotionSpeed = Animator.StringToHash("MotionSpeed");
        private int AnimationID_Attack = Animator.StringToHash("Attack"); // TODO: 애니메이터 컨트롤러에 매개변수 추가
        private int AnimationID_FreeFall = Animator.StringToHash("FreeFall");

        // AI 관련
        public bool IsReachedDestination = false;
        public Character ChaseTarget;

        /// <summary>
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

        public void TransitionToState(CharacterState characterState)
        {
            stateMachine.TransitionToState(characterState);
        }

        private void Awake()
        {
            stateMachine = GetComponent<CharacterStateMachine>();
            if (!stateMachine)
            {
                Debug.LogWarning($"{name}의 CharacterStateMachine 컴포넌트가 없습니다. 새로 추가해주세요.");
            }

            animator = GetComponent<Animator>();
            if (!animator)
            {
                Debug.LogWarning($"{name}의 Animator 컴포넌트가 없습니다. 새로 추가해주세요.");
            }

            attributeComponent = GetComponent<CharacterAttributeComponent>();
            if(!attributeComponent)
            {
                Debug.LogWarning($"{name}의 CharacterAttributeComponent 컴포넌트가 없습니다. 새로 추가해주세요.");
            }

            healthComponent = GetComponent<CharacterHealthComponent>();
            if (!healthComponent)
            {
                Debug.LogWarning($"{name}의 CharacterHealthComponent 컴포넌트가 없습니다. 새로 추가해주세요.");
            }

            Initialize();
        }

        private void Initialize()
        {
            IsRolling = false;
            RollingCooldownTime = RollingSettings.RollingCooldownTime;
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
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - MovementSettings.GroundedOffset, transform.position.z);
            IsGrounded = Physics.CheckSphere(spherePosition, MovementSettings.GroundedRadius, MovementSettings.GroundLayers, QueryTriggerInteraction.Ignore);
        }

        /// <summary>
        /// 캐릭터의 수평 속력을 갱신한다.
        /// </summary>
        private void UpdateHorizontalSpeed()
        {
            // 현재 수평 속력
            float currentHorizontalSpeed = new Vector3(Velocity.x, 0.0f, Velocity.z).magnitude;
            float inputMagnitude = Controller.MovementInput.magnitude; // _input.analogMovement ? _input.move.magnitude : 1f;\

            // 자연스러운 속도 변경을 위해서 목표 속도에 가까워지게 감속 또는 가속한다.
            if (AccelerateToTargetHorizontalSpeed)
            {
                if (IsRolling)
                {
                    Acceleration = currentHorizontalSpeed < TargetHorizontalSpeed ? RollingSettings.Acceleration : RollingSettings.Decceleration;
                    inputMagnitude = 1f;
                }
                else
                {
                    Acceleration = currentHorizontalSpeed < TargetHorizontalSpeed ? MovementSettings.Acceleration : MovementSettings.Decceleration;
                }

                HorizontalSpeed = Mathf.MoveTowards(HorizontalSpeed, TargetHorizontalSpeed * inputMagnitude, Acceleration * Time.deltaTime);
            }
            else
            {
                HorizontalSpeed = TargetHorizontalSpeed;
            }
        }


        /// <summary>
        /// 캐릭터의 수직 속력을 갱신한다.
        /// </summary>
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

        /// <summary>
        /// 컨트롤러의 입력과 상관없이 캐릭터를 강제로 움직이게 한다.
        /// </summary>
        /// <param name="movement">강제로 움직일 방향</param>
        public void SetOverrideMovementVector(bool overrideMovement, Vector2 movement)
        {
            IsMovementVectorOverrided = overrideMovement;
            OverridedMovementVector = movement;

            // HACK: 인풋값도 초기화시켜준다.
            Controller.LastMovementInput = Vector2.zero;
            Controller.MovementInput = Vector2.zero;
        }

        /// <summary>
        /// 캐릭터의 최종 이동 속도 벡터를 계산한다.
        /// </summary>
        private void UpdateMovementVector()
        {
            Vector3 movement = Controller.HasMovementInput ?
                new Vector3(Controller.MovementInput.x, 0, Controller.MovementInput.y) :
                new Vector3(Controller.LastMovementInput.x, 0, Controller.LastMovementInput.y);

            if (IsMovementVectorOverrided)
            {
                movement = new Vector3(OverridedMovementVector.x, 0, OverridedMovementVector.y);
            }

            Controller.MovementVector = movement * HorizontalSpeed + Vector3.up * VerticalSpeed;
        }

        /// <summary>
        /// 캐릭터의 회전각을 갱신한다.
        /// </summary>
        private void UpdateRotation()
        {
            Vector3 horizontalMovementVector = new Vector3(Controller.MovementVector.x, 0, Controller.MovementVector.z);
            if (horizontalMovementVector.sqrMagnitude > 0.0f)
            {
                float rotationSpeed = Mathf.Lerp(MovementSettings.MaxRotationSpeed, MovementSettings.MinRotationSpeed, HorizontalSpeed / TargetHorizontalSpeed);

                targetRotation = Quaternion.LookRotation(horizontalMovementVector, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                targetDirection = horizontalMovementVector.normalized;
            }
        }

        /// <summary>
        /// 캐릭터의 애니메이터 파라미터값들을 갱신한다.
        /// </summary>
        private void UpdateAnimations()
        {
            animator.SetFloat(AnimationID_MotionSpeed, 1f);
            animator.SetFloat(AnimationID_Speed, Velocity.magnitude); // TODO: Override MovementVector때문에 캐릭터의 최종 속도 기준으로 값을 변경해야한다.
            // animator.SetBool(AnimationID_Attack, IsAttacking); // TODO: 애니메이터 컨트롤러에 매개변수 추가
            animator.SetBool(AnimationID_IsGrounded, IsGrounded);
            animator.SetBool(AnimationID_IsRolling, IsRolling);
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

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                // TODO: 발소리 내기?
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

        /// <summary>
        /// 현재 캐릭터가 구르기 행동을 할 수 있는지 검사한다.
        /// </summary>
        public bool CanRoll()
        {
            bool IsOnCooldown = RollingCooldownTime <= RollingSettings.RollingCooldownTime;
            // 캐릭터의 현재 상태가 구르기 상태로 넘어갈 수 있는지도 체크해야할 수도 있음.
            // ...
            return !IsOnCooldown && !IsRolling;
        }

        private void OnDrawGizmos()
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
                Gizmos.DrawRay(transform.position, new Vector3(Controller.MovementVector.x, 0, Controller.MovementVector.z));

                Gizmos.color = Color.red;
                Gizmos.DrawRay(transform.position, Controller.MovementInput);

                Gizmos.color = Color.magenta;
                Gizmos.DrawRay(transform.position, Controller.LastMovementInput);
            }
        }
    }
}
