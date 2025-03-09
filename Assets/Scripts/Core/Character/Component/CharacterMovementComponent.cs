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
        public bool IsWalkedOffALedge = false; // Grounded���¿��ٰ� �������� ������ ������ �����ӿ� true

        [Header("Movement")]
        public MovementSettings MovementSettings;
        protected bool IsMovementVectorOverrided = false;
        protected Vector2 OverridedMovementVector = Vector2.zero;
        public float HorizontalSpeed = 0f; // ���� ���� �ӵ� (x��, z��)
        public float TargetHorizontalSpeed = 0f; // Ÿ�� ���� �ӵ� (��/���� ��)
        public float VerticalSpeed = 0f; // ���� ���� �ӵ� (y��)
        public bool AccelerateToTargetHorizontalSpeed = true; // Ÿ�� �ӵ����� ��/���� �� �� ����

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
        /// ���� ĳ���Ͱ� ���� ����ִ��� �˻��Ѵ�.
        /// </summary>
        private void CheckIsGrounded()
        {
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - MovementSettings.GroundedOffset, transform.position.z);
            IsGrounded = Physics.CheckSphere(spherePosition, MovementSettings.GroundedRadius, MovementSettings.GroundLayers, QueryTriggerInteraction.Ignore);
        }


        /// <summary>
        /// ĳ������ ���� �ӷ��� �����Ѵ�.
        /// </summary>
        private void UpdateHorizontalSpeed()
        {
            // ���� ���� �ӷ�
            float currentHorizontalSpeed = new Vector3(Controller.Velocity.x, 0.0f, Controller.Velocity.z).magnitude;
            float inputMagnitude = Controller.MovementInput.magnitude; // _input.analogMovement ? _input.move.magnitude : 1f;\

            // �ڿ������� �ӵ� ������ ���ؼ� ��ǥ �ӵ��� ��������� ���� �Ǵ� �����Ѵ�.
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
        /// ĳ������ ���� �ӷ��� �����Ѵ�.
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
        /// ��Ʈ�ѷ��� �Է°� ������� ĳ���͸� ������ �����̰� �Ѵ�.
        /// </summary>
        /// <param name="movement">������ ������ ����</param>
        public void SetOverrideMovementVector(bool overrideMovement, Vector2 movement)
        {
            // TODO: TargetHorizontalSpeed�� �����ؾ� ������
            // �� �Լ��� �Ű������� �Ű����� �Ѱ��༭ ó������ �ƴϸ� ȣ���Ҷ� ���� �� ������������� ���
            IsMovementVectorOverrided = overrideMovement;
            OverridedMovementVector = movement;

            // HACK: ��ǲ���� �ʱ�ȭ�����ش�.
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
        /// ĳ������ ���� �̵� �ӵ� ���͸� ����Ѵ�.
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
        /// ĳ������ ȸ������ �����Ѵ�.
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