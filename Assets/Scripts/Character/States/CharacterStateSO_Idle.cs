using KinematicCharacterController;
using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "SO_CharacterState_Idle", menuName = "Scriptable Objects/Character/State/Idle")]
    public class CharacterStateSO_Idle : CharacterStateSO
    {
        public override CharacterState CreateInstance()
        {
            return new CharacterState_Idle();
        }
    }

    public class CharacterState_Idle : CharacterState
    {
        public override void OnStateEnter(Character character, ECharacterState prevState)
        {

        }

        public override void OnStateExit(Character character, ECharacterState newState)
        {

        }

        public override void UpdateState(Character character, CharacterStateMachine stateMachine)
        {

        }

        public override void UpdateVelocity(Character character, KinematicCharacterMotor motor, ref Vector3 currentVelocity, float deltaTime)
        {
            if (motor.GroundingStatus.IsStableOnGround)
            {
                currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, 1f - Mathf.Exp(-character.MovementSettings.StableMovementSharpness * deltaTime));
            }
            else
            {
                // Gravity
                currentVelocity += character.Controller.Gravity * deltaTime;
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