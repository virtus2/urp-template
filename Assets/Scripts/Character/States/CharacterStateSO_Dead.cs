using KinematicCharacterController;
using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "SO_CharacterState_Dead", menuName = "Scriptable Objects/Character/State/Dead")]
    public class CharacterStateSO_Dead : CharacterStateSO
    {
        public override void OnStateEnter(Character character, CharacterState prevState)
        {

        }

        public override void OnStateExit(Character character, CharacterState newState)
        {

        }

        public override void UpdateState(Character character, CharacterStateMachine stateMachine)
        {

        }

        public override void UpdateVelocity(Character character, KinematicCharacterMotor motor, ref Vector3 currentVelocity, float deltaTime)
        {
            if (motor.GroundingStatus.IsStableOnGround)
            {
                currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, 1f - Mathf.Exp(-character.Controller.StableMovementSharpness * deltaTime));
            }
            else
            {
                // Gravity
                currentVelocity += character.Controller.Gravity * deltaTime;
            }
        }

        public override void UpdateRotation(Character character, KinematicCharacterMotor motor, ref Quaternion currentRotation, float deltaTime)
        {

        }
    }
}