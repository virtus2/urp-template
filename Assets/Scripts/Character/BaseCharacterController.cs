using Core;
using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public abstract class BaseCharacterController : MonoBehaviour, ICharacterController
    {
        public Vector3 Velocity => motor ? motor.Velocity : Vector3.zero;
        public float Radius => motor ? motor.Capsule.radius : 0f;

        // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        public Vector2 LastMovementInput;
        [ReadOnly] public Vector2 MovementInput; // 컨트롤러의 움직이는 방향 입력
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

        public void SetMovementInput(Vector2 input)
        {
            bool inputChanged = input.sqrMagnitude > 0.0f;

            if (HasMovementInput && !inputChanged)
            {
                LastMovementInput = MovementInput;
            }

            MovementInput = input;
            HasMovementInput = inputChanged;
        }

        public void SetInput(in Vector3 input)
        {
            MovementInputVector = input;
        }

        public void SetLookInput(Vector2 input)
        {

        }

        public void SetCollisions(bool enable)
        {
            motor.SetCapsuleCollisionsActivation(enable);
            motor.SetMovementCollisionsSolvingActivation(enable);
            motor.SetGroundSolvingActivation(enable);
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
            // throw new System.NotImplementedException();
        }

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            // Ground movement
            if (motor.GroundingStatus.IsStableOnGround)
            {
                float currentVelocityMagnitude = currentVelocity.magnitude;

                Vector3 effectiveGroundNormal = motor.GroundingStatus.GroundNormal;

                // Reorient velocity on slope
                currentVelocity = motor.GetDirectionTangentToSurface(currentVelocity, effectiveGroundNormal) * currentVelocityMagnitude;

                // Calculate target velocity
                Vector3 inputRight = Vector3.Cross(MovementInputVector, motor.CharacterUp);
                Vector3 reorientedInput = Vector3.Cross(effectiveGroundNormal, inputRight).normalized * MovementInputVector.magnitude;
                Vector3 targetMovementVelocity = reorientedInput * MaxStableMoveSpeed;

                // Smooth movement Velocity
                currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity, 1f - Mathf.Exp(-StableMovementSharpness * deltaTime));
            }
            // Air movement
            else
            {
                // Add move input
                if (MovementInputVector.sqrMagnitude > 0f)
                {
                    Vector3 addedVelocity = MovementInputVector * AirAccelerationSpeed * deltaTime;

                    Vector3 currentVelocityOnInputsPlane = Vector3.ProjectOnPlane(currentVelocity, motor.CharacterUp);

                    // Limit air velocity from inputs
                    if (currentVelocityOnInputsPlane.magnitude < MaxAirMoveSpeed)
                    {
                        // clamp addedVel to make total vel not exceed max vel on inputs plane
                        Vector3 newTotal = Vector3.ClampMagnitude(currentVelocityOnInputsPlane + addedVelocity, MaxAirMoveSpeed);
                        addedVelocity = newTotal - currentVelocityOnInputsPlane;
                    }
                    else
                    {
                        // Make sure added vel doesn't go in the direction of the already-exceeding velocity
                        if (Vector3.Dot(currentVelocityOnInputsPlane, addedVelocity) > 0f)
                        {
                            addedVelocity = Vector3.ProjectOnPlane(addedVelocity, currentVelocityOnInputsPlane.normalized);
                        }
                    }

                    // Prevent air-climbing sloped walls
                    if (motor.GroundingStatus.FoundAnyGround)
                    {
                        if (Vector3.Dot(currentVelocity + addedVelocity, addedVelocity) > 0f)
                        {
                            Vector3 perpenticularObstructionNormal = Vector3.Cross(Vector3.Cross(motor.CharacterUp, motor.GroundingStatus.GroundNormal), motor.CharacterUp).normalized;
                            addedVelocity = Vector3.ProjectOnPlane(addedVelocity, perpenticularObstructionNormal);
                        }
                    }

                    // Apply added velocity
                    currentVelocity += addedVelocity;
                }

                // Gravity
                currentVelocity += Gravity * deltaTime;

                // Drag
                currentVelocity *= (1f / (1f + (Drag * deltaTime)));
            }

            /*
            // Handle jumping
            _jumpedThisFrame = false;
            _timeSinceJumpRequested += deltaTime;
            if (_jumpRequested)
            {
                // See if we actually are allowed to jump
                if (!_jumpConsumed && ((AllowJumpingWhenSliding ? motor.GroundingStatus.FoundAnyGround : motor.GroundingStatus.IsStableOnGround) || _timeSinceLastAbleToJump <= JumpPostGroundingGraceTime))
                {
                    // Calculate jump direction before ungrounding
                    Vector3 jumpDirection = motor.CharacterUp;
                    if (motor.GroundingStatus.FoundAnyGround && !motor.GroundingStatus.IsStableOnGround)
                    {
                        jumpDirection = motor.GroundingStatus.GroundNormal;
                    }

                    // Makes the character skip ground probing/snapping on its next update. 
                    // If this line weren't here, the character would remain snapped to the ground when trying to jump. Try commenting this line out and see.
                    motor.ForceUnground();

                    // Add to the return velocity and reset jump state
                    currentVelocity += (jumpDirection * JumpUpSpeed) - Vector3.Project(currentVelocity, motor.CharacterUp);
                    currentVelocity += (_moveInputVector * JumpScalableForwardSpeed);
                    _jumpRequested = false;
                    _jumpConsumed = true;
                    _jumpedThisFrame = true;
                }
            }
            */

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
        }

        protected void OnLeaveStableGround()
        {
        }
    }
}