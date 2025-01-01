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
    }

    public abstract class BaseCharacterController : MonoBehaviour, ICharacterController
    {
        public Vector3 Velocity => motor ? motor.Velocity : Vector3.zero;
        public float Radius => motor ? motor.Capsule.radius : 0f;

        // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        public Vector2 LastMovementInput;
        
        public Vector3 MovementVector; // 최종적으로 캐릭터가 움직이는 방향
        public Vector3 LookVector; 
        public float VerticalVelocity;
        public bool HasMovementInput;
        public bool RollPressed;
        public bool AttackPressed;
        public bool RunPressed;
        public bool InteractPressed;

        // TODO: 새로 추가된 필드, 추후에 정리 (2024-11-18)
        public Vector3 MovementInputVector;
        public Vector3 LookInputVector;
        public List<Collider> IgnoredColliders = new List<Collider>();
        public float MaxStableMoveSpeed = 10f;
        public float StableMovementSharpness = 15;
        public float AirAccelerationSpeed = 5f;
        public float MaxAirMoveSpeed = 10f;
        public float Drag = 0.1f;
        public float JumpUpSpeed = 10f;
        public float JumpScalableForwardSpeed = 0f;
        public Vector3 Gravity = new Vector3(0, -30f, 0);
        public Vector3 _internalVelocityAdd = Vector3.zero;
        public OrientationMethod OrientationMethod = OrientationMethod.TowardsMovement;
        public float OrientationSharpness = 10f;
        public float BonusOrientationSharpness = 10f;

        // 내가 추가한 필드
        public bool IgnoreMovementInput = false;
        public AnimationCurve AttackVelocityCurve;
        public float AttackVelocityMultiplier = 1f;
        public float AttackDuration = 0.2f;
        public bool IsGrounded { get; private set; }
        public float HorizontalSpeed => motor ? motor.BaseVelocity.magnitude : 0f;

        protected Character character;
        protected CharacterStateMachine stateMachine;
        protected KinematicCharacterMotor motor;

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

        public void AddVelocity(Vector3 velocity)
        {
            _internalVelocityAdd += velocity;
        }

        public void BeforeCharacterUpdate(float deltaTime)
        {
            // throw new System.NotImplementedException();
        }


        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            currentRotation = stateMachine.GetCurrentState().GetCurrentRotation(character, motor, deltaTime);
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
            if (_internalVelocityAdd.sqrMagnitude > 0f)
            {
                currentVelocity += _internalVelocityAdd;
                _internalVelocityAdd = Vector3.zero;
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
            // throw new System.NotImplementedException();
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