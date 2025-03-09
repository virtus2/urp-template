using System;
using UnityEngine;

namespace Core.Character.Component
{
    [RequireComponent(typeof(BaseCharacter))]
    [Obsolete]
    public class CharacterMovementComponent : MonoBehaviour
    {
        /*
        public bool IsGrounded = false;
        public bool IsWalkedOffALedge = false; // Grounded상태였다가 떨어지기 시작한 순간의 프레임에 true

        [Header("Movement")]
        public MovementSettings MovementSettings;
        protected bool IsMovementVectorOverrided = false;
        protected Vector2 OverridedMovementVector = Vector2.zero;
        public float HorizontalSpeed = 0f; // 현재 수평 속도 (x축, z축)
        public float TargetHorizontalSpeed = 0f; // 타겟 수평 속도 (가/감속 전)
        public float VerticalSpeed = 0f; // 현재 수직 속도 (y축)
        public bool AccelerateToTargetHorizontalSpeed = true; // 타겟 속도까지 가/감속 할 지 여부

        [Header("Rotation")]
        private Quaternion targetRotation = Quaternion.identity;
        private Vector3 targetDirection = Vector3.forward;
        public bool LookAt = false;
        public Vector3 LookDirection = Vector3.forward;

        [Header("Physics")]
        [NonSerialized] public float Acceleration = 0f;
        public Vector3 ImpulseToApply = Vector3.zero;
        public Vector3 ForceToApply = Vector3.zero;
        public float Mass = 1f;

        private Character character;
        private BaseCharacterController Controller => character.Controller;

        private void Awake()
        {
            character = GetComponent<Character>();
        }

        private void Update()
        {
            CheckIsGrounded();
            UpdateHorizontalSpeed();
            UpdateVerticalSpeed();
            UpdateRotation();
        }

        private void FixedUpdate()
        {
            UpdateMovementVector();
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
            float currentHorizontalSpeed = new Vector3(Controller.Velocity.x, 0.0f, Controller.Velocity.z).magnitude;
            float inputMagnitude = Controller.MovementInput.magnitude; // _input.analogMovement ? _input.move.magnitude : 1f;\

            // 자연스러운 속도 변경을 위해서 목표 속도에 가까워지게 감속 또는 가속한다.
            if (AccelerateToTargetHorizontalSpeed)
            {
                Acceleration = currentHorizontalSpeed < TargetHorizontalSpeed ? MovementSettings.Acceleration : MovementSettings.Decceleration;
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
                if (IsWalkedOffALedge)
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
            // TODO: TargetHorizontalSpeed도 변경해야 움직임
            // 이 함수에 매개변수에 매개변수 넘겨줘서 처리할지 아니면 호출할때 따로 값 변경해줘야할지 고민
            IsMovementVectorOverrided = overrideMovement;
            OverridedMovementVector = movement;

            // HACK: 인풋값도 초기화시켜준다.
            Controller.LastMovementInput = Vector2.zero;
            Controller.MovementInput = Vector2.zero;
        }

        public void AddForce(Vector3 force)
        {
            ForceToApply += force / Mass;
        }

        public void AddImpulse(Vector3 impulse)
        {
            ImpulseToApply += impulse / Mass;
        }
        /// <summary>
        /// 캐릭터의 최종 이동 속도 벡터를 계산한다.
        /// </summary>
        private void UpdateMovementVector()
        {
            Vector3 movement = Controller.HasMovementInput ?
            new Vector3(Controller.MovementInput.x, 0, Controller.MovementInput.y) :
            new Vector3(Controller.LastMovementInput.x, 0, Controller.LastMovementInput.y);

            if (!character.InputEnabled)
            {
                movement = Vector3.zero;
            }

            if (IsMovementVectorOverrided)
            {
                movement = new Vector3(OverridedMovementVector.x, 0, OverridedMovementVector.y);
            }

            Controller.MovementVector = movement * HorizontalSpeed + Vector3.up * VerticalSpeed;
            Controller.MovementVector += ImpulseToApply + (ForceToApply * Time.deltaTime);
            ImpulseToApply = Vector3.zero;
            ForceToApply = Vector3.zero;
        }

        /// <summary>
        /// 캐릭터의 회전각을 갱신한다.
        /// </summary>
        private void UpdateRotation()
        {
            if (LookAt)
            {
                LookDirection = Controller.LookVector;
                float rotationSpeed = MovementSettings.MaxRotationSpeed;
                targetRotation = Quaternion.LookRotation(LookDirection, Vector3.up);
                character.transform.rotation = Quaternion.RotateTowards(character.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                targetDirection = LookDirection.normalized;
            }
            else
            {
                Vector3 horizontalMovementVector = new Vector3(Controller.MovementVector.x, 0, Controller.MovementVector.z);
                if (IsMovementVectorOverrided)
                {
                    horizontalMovementVector = new Vector3(Controller.MovementInput.x, 0, Controller.MovementInput.y);
                }
                if (horizontalMovementVector.sqrMagnitude > 0.0f)
                {
                    float rotationSpeed = Mathf.Lerp(MovementSettings.MaxRotationSpeed, MovementSettings.MinRotationSpeed, HorizontalSpeed / TargetHorizontalSpeed);

                    targetRotation = Quaternion.LookRotation(horizontalMovementVector, Vector3.up);
                    character.transform.rotation = Quaternion.RotateTowards(character.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                    targetDirection = horizontalMovementVector.normalized;
                }
            }
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
        */
    }
}