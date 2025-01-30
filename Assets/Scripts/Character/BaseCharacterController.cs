using Core;
using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public enum OrientationMethod
    {
        TowardsCamera,
        TowardsMovement,
        TowardsCursor, // Only in Top View !!!
    }

    public abstract class BaseCharacterController : MonoBehaviour, ICharacterController
    {
        public KinematicCharacterMotor Motor => motor ? motor : null;
        public Vector3 Velocity => motor ? motor.Velocity : Vector3.zero;
        public float Radius => motor ? motor.Capsule.radius : 0f;
        public Vector3 Forward => motor ? motor.CharacterForward : Vector3.forward;
        public Vector3 Right => motor ? motor.CharacterRight : Vector3.right;

        // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        public Vector2 LastMovementInput;

        // Inputs
        public bool RollPressed;
        public bool AttackPressed;
        public bool RunPressed;
        public bool InteractPressed;

        [Header("Stable Movement")]
        [Tooltip("Movement input from the Player or AI.")]
        [ReadOnly]
        public Vector3 MovementInputVector;

        [Tooltip("Maximum movement speed on stable ground. It can be tweaked in MovementSettings.")]
        [ReadOnly]
        public float MaxStableMoveSpeed = 10f;

        // [Tooltip("Linear interpolation speed for character's movement speed. It can be tweaked in MovementSettings.")]
        // [ReadOnly]
        // public float StableMovementSharpness = 15;


        [Header("Air Movement")]
        [Tooltip("Maximum movement speed in the air.")]
        [ReadOnly]
        public float MaxAirMoveSpeed = 10f; // TODO: 필요하면 MovementSettings에서 수정 가능하게 변경

        [Tooltip("Acceleration of movement speed in the air.")]
        [ReadOnly]
        public float AirAccelerationSpeed = 5f; // TODO: 필요하면 MovementSettings에서 수정 가능하게 변경

        [Tooltip("Drag coefficient of the object. The higher the value, the faster the object slows down.")]
        public float Drag = 0.1f; // TODO: 필요하면 MovementSettings에서 수정 가능하게 변경

        [Header("Misc")]
        [Tooltip("Look input from the Player or AI.")]
        [ReadOnly]
        public Vector3 LookInputVector;

        [Tooltip("Velocity")]
        [ReadOnly]
        public Vector3 internalVelocityAdd = Vector3.zero;

        [Tooltip("Force")]
        [ReadOnly]
        public Vector3 internalForceAdd = Vector3.zero;

        [Tooltip("Character's orientation method")]
        public OrientationMethod OrientationMethod = OrientationMethod.TowardsMovement;

        [Tooltip("Orientation interpolation speed")]
        public float OrientationSharpness = 10f;

        [Tooltip("Bonus orientation interpolation speed")]
        public float BonusOrientationSharpness = 10f; // TODO: 보너스가 뭐지? 안 쓸 것 같은데

        [Tooltip("This character will ignore these colliders.")]
        public List<Collider> IgnoredColliders = new List<Collider>();

        [Tooltip("Gravity")]
        public Vector3 Gravity = new Vector3(0, -30f, 0);

        [Tooltip("Is the character on the ground?")]
        public bool IsGrounded { get; private set; }

        [Tooltip("The pushing force of the character.")]

        public float PushForce = 3f;

        // TODO: 점프 안쓸 것 같으면 지우기
        public float JumpUpSpeed = 10f; 
        public float JumpScalableForwardSpeed = 0f;

        // 내가 추가한 필드
        [Header("FOR TEST")]
        public bool IgnoreMovementInput = false;
        public float HorizontalSpeed => motor ? motor.BaseVelocity.magnitude : 0f;

        protected Character character;
        protected CharacterStateMachine stateMachine;
        protected KinematicCharacterMotor motor;

        // A pre-allocated array of colliders used for physics-related logic such as collision detection or triggers.
        protected Collider[] probedColliders = new Collider[8];


        private void Awake()
        {
            character = GetComponent<Character>();
            stateMachine = GetComponent<CharacterStateMachine>();
            motor = GetComponent<KinematicCharacterMotor>();
            motor.CharacterController = this;
        }

        public void SetMovementInput(in Vector3 input)
        {
            MovementInputVector = input;
        }

        public void SetLookInput(in Vector3 input)
        {
            LookInputVector = input;
        }

        public void SetCollisions(bool enable)
        {
            motor.SetCapsuleCollisionsActivation(enable);
            motor.SetMovementCollisionsSolvingActivation(enable);
            motor.SetGroundSolvingActivation(enable);
        }

        public void SetPosition(in Vector3 position)
        {
            motor.SetPosition(position);
        }

        public void AddVelocity(in Vector3 velocity)
        {
            internalVelocityAdd += velocity;
        }
        
        public void AddForce(in Vector3 force)
        {
            internalForceAdd += force;
        }

        public int CharacterOverlap(LayerMask layerMask, QueryTriggerInteraction triggerInteraction, out Collider[] overlappedColliders)
        {
            overlappedColliders = probedColliders;
            return motor.CharacterOverlap(motor.TransientPosition, motor.TransientRotation, probedColliders, layerMask, triggerInteraction);
        }

        /// <param name="deltaTime"></param>
        public void BeforeCharacterUpdate(float deltaTime)
        {
            // throw new System.NotImplementedException();
        }


        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            stateMachine.GetCurrentState().UpdateRotation(character, motor, ref currentRotation, deltaTime);
            Vector3 currentUp = (currentRotation * Vector3.up);
            /*
            if (BonusOrientationMethod == BonusOrientationMethod.TowardsGravity)
            {
                // Rotate from current up to invert gravity
                Vector3 smoothedGravityDir = Vector3.Slerp(currentUp, -Gravity.normalized, 1 - Mathf.Exp(-BonusOrientationSharpness * deltaTime));
                currentRotation = Quaternion.FromToRotation(currentUp, smoothedGravityDir) * currentRotation;
            }
            else if (BonusOrientationMethod == BonusOrientationMethod.TowardsGroundSlopeAndGravity)
            {
                if (Motor.GroundingStatus.IsStableOnGround)
                {
                    Vector3 initialCharacterBottomHemiCenter = Motor.TransientPosition + (currentUp * Motor.Capsule.radius);

                    Vector3 smoothedGroundNormal = Vector3.Slerp(Motor.CharacterUp, Motor.GroundingStatus.GroundNormal, 1 - Mathf.Exp(-BonusOrientationSharpness * deltaTime));
                    currentRotation = Quaternion.FromToRotation(currentUp, smoothedGroundNormal) * currentRotation;

                    // Move the position to create a rotation around the bottom hemi center instead of around the pivot
                    Motor.SetTransientPosition(initialCharacterBottomHemiCenter + (currentRotation * Vector3.down * Motor.Capsule.radius));
                }
                else
                {
                    Vector3 smoothedGravityDir = Vector3.Slerp(currentUp, -Gravity.normalized, 1 - Mathf.Exp(-BonusOrientationSharpness * deltaTime));
                    currentRotation = Quaternion.FromToRotation(currentUp, smoothedGravityDir) * currentRotation;
                }
            }
            else
            */
            {
                Vector3 smoothedGravityDir = Vector3.Slerp(currentUp, Vector3.up, 1 - Mathf.Exp(-BonusOrientationSharpness * deltaTime));
                currentRotation = Quaternion.FromToRotation(currentUp, smoothedGravityDir) * currentRotation;
            }
        }

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            stateMachine.GetCurrentState().UpdateVelocity(character, motor, ref currentVelocity, deltaTime);

            // Take into account additive velocity
            if (internalVelocityAdd.sqrMagnitude > 0f)
            {
                currentVelocity += internalVelocityAdd;
                internalVelocityAdd = Vector3.zero;
            }

            if (internalForceAdd.sqrMagnitude > 0f)
            {
                currentVelocity += internalForceAdd * Time.deltaTime;
                internalForceAdd = Vector3.Lerp(internalForceAdd, Vector3.zero, Drag * Time.deltaTime);
            }
        }

        public void AfterCharacterUpdate(float deltaTime)
        {
            // throw new System.NotImplementedException();
        }

        public void PostGroundingUpdate(float deltaTime)
        {
            // Handle landing and leaving ground
            if (motor.GroundingStatus.IsStableOnGround && !motor.LastGroundingStatus.IsStableOnGround)
            {
                OnLanded();
            }
            else if (!motor.GroundingStatus.IsStableOnGround && motor.LastGroundingStatus.IsStableOnGround)
            {
                OnLeaveStableGround();
            }
        }

        public bool IsColliderValidForCollisions(Collider coll)
        {
            if (IgnoredColliders.Count == 0)
            {
                return true;
            }

            if (IgnoredColliders.Contains(coll))
            {
                return false;
            }

            return true;
        }

        public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {
            // throw new System.NotImplementedException();
        }

        public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {
            // TODO: 충돌 시 다른 캐릭터를 밀어내기
            BaseCharacterController controller = hitCollider.GetComponent<BaseCharacterController>();
            if (controller)
            {
                controller.AddForce(-hitNormal * PushForce);
            }
        }

        public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
        {
            // throw new System.NotImplementedException();
        }

        public void OnDiscreteCollisionDetected(Collider hitCollider)
        {
            // throw new System.NotImplementedException();
        }

        protected void OnLanded()
        {
            IsGrounded = true;
        }

        protected void OnLeaveStableGround()
        {
            IsGrounded = false;
        }
    }
}