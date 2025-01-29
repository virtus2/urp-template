using KinematicCharacterController;
using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "SO_CharacterState_GroundMove", menuName = "Scriptable Objects/Character/State/Ground Move")]
    public class CharacterStateSO_GroundMove : CharacterStateSO
    {
        public override CharacterState CreateInstance()
        {
            return new CharacterState_GroundMove();
        }
    }

    public class CharacterState_GroundMove : CharacterState
    {
        public override void OnStateEnter(Character character, ECharacterState prevState)
        {
            character.Controller.MaxStableMoveSpeed = character.Controller.RunPressed ?
                character.MovementSettings.RunSpeed : character.MovementSettings.WalkSpeed;
        }

        public override void OnStateExit(Character character, ECharacterState newState)
        {

        }

        public override void UpdateState(Character character, CharacterStateMachine stateMachine)
        {
            if(character.Controller.RunPressed)
            {
                if(Mathf.Approximately(character.Controller.MaxStableMoveSpeed,character.MovementSettings.RunSpeed))
                {
                    character.Controller.MaxStableMoveSpeed = character.MovementSettings.RunSpeed;
                }
                else
                {
                    character.Controller.MaxStableMoveSpeed = Mathf.MoveTowards(character.Controller.MaxStableMoveSpeed,
                        character.MovementSettings.RunSpeed, character.MovementSettings.Acceleration * Time.deltaTime);
                }
            }
            else
            {
                if(Mathf.Approximately(character.Controller.MaxStableMoveSpeed, character.MovementSettings.WalkSpeed))
                {
                    character.Controller.MaxStableMoveSpeed = character.MovementSettings.WalkSpeed;
                }
                else
                {
                    character.Controller.MaxStableMoveSpeed = Mathf.MoveTowards(character.Controller.MaxStableMoveSpeed,
                        character.MovementSettings.WalkSpeed, character.MovementSettings.Decceleration * Time.deltaTime);
                }
            }
        }

        public override void UpdateVelocity(Character character, KinematicCharacterMotor motor, ref Vector3 currentVelocity, float deltaTime)
        {// Ground movement
            if (motor.GroundingStatus.IsStableOnGround)
            {
                // Method 1: Interpolate target speed first, then multiply the velocity with it
                float targetSpeed = character.Controller.MaxStableMoveSpeed;

                Vector3 effectiveGroundNormal = motor.GroundingStatus.GroundNormal;
                Vector3 inputRight = Vector3.Cross(character.Controller.MovementInputVector, motor.CharacterUp);
                Vector3 reorientedInput = Vector3.Cross(effectiveGroundNormal, inputRight).normalized * character.Controller.MovementInputVector.magnitude;
                
                float currentSpeed = Mathf.MoveTowards(currentVelocity.magnitude, targetSpeed, 
                    character.MovementSettings.StableMovementSharpness * deltaTime);
                
                currentVelocity = reorientedInput * currentSpeed;

                // Method 2: Calculate and Interpolate velocity vector
                /*
                Vector3 effectiveGroundNormal = motor.GroundingStatus.GroundNormal;

                // Reorient velocity on slope
                currentVelocity = motor.GetDirectionTangentToSurface(currentVelocity, effectiveGroundNormal) * currentVelocityMagnitude;

                // Calculate target velocity
                Vector3 inputRight = Vector3.Cross(character.Controller.MovementInputVector, motor.CharacterUp);
                Vector3 reorientedInput = Vector3.Cross(effectiveGroundNormal, inputRight).normalized * character.Controller.MovementInputVector.magnitude;
                Vector3 targetMovementVelocity = reorientedInput * character.Controller.MaxStableMoveSpeed;

                // Smooth movement Velocity
                currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity, 1f - Mathf.Exp(-character.MovementSettings.StableMovementSharpness * deltaTime));
                */
            }
            // Air movement
            else
            {
                // Add move input
                if (character.Controller.MovementInputVector.sqrMagnitude > 0f)
                {
                    Vector3 addedVelocity = character.Controller.MovementInputVector * character.Controller.AirAccelerationSpeed * deltaTime;

                    Vector3 currentVelocityOnInputsPlane = Vector3.ProjectOnPlane(currentVelocity, motor.CharacterUp);

                    // Limit air velocity from inputs
                    if (currentVelocityOnInputsPlane.magnitude < character.Controller.MaxAirMoveSpeed)
                    {
                        // clamp addedVel to make total vel not exceed max vel on inputs plane
                        Vector3 newTotal = Vector3.ClampMagnitude(currentVelocityOnInputsPlane + addedVelocity, character.Controller.MaxAirMoveSpeed);
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
                currentVelocity += character.Controller.Gravity * deltaTime;

                // Drag
                currentVelocity *= (1f / (1f + (character.Controller.Drag * deltaTime)));
            }
        }
        public override void UpdateRotation(Character character, KinematicCharacterMotor motor, ref Quaternion currentRotation, float deltaTime)
        {
            if (character.Controller.LookInputVector.sqrMagnitude > 0f && character.Controller.OrientationSharpness > 0f)
            {
                // Smoothly interpolate from current to target look direction
                Vector3 smoothedLookInputDirection = Vector3.Slerp(motor.CharacterForward, character.Controller.LookInputVector, 1 - Mathf.Exp(-character.Controller.OrientationSharpness * deltaTime)).normalized;

                // Set the current rotation (which will be used by the KinematicCharacterMotor)
                currentRotation = Quaternion.LookRotation(smoothedLookInputDirection, motor.CharacterUp);
            }
        }
    }
}