using KinematicCharacterController;
using UnityEngine;

namespace Core.Character.State
{
    [CreateAssetMenu(fileName = "SO_CharacterState_Dead", menuName = "Scriptable Objects/Character/State/Dead")]
    public class CharacterStateSO_Dead : CharacterStateSO
    {
        public override CharacterState CreateInstance()
        {
            return new CharacterState_Dead();
        }
    }

    public class CharacterState_Dead : CharacterState
    {
        public override void OnStateEnter(BaseCharacter character, ECharacterState prevState)
        {
            character.IsDead = true;
            character.InputEnabled = false;
            character.Controller.SetCollisions(false);
            // TODO: 래그돌?
        }

        public override void OnStateExit(BaseCharacter character, ECharacterState newState)
        {
            // TODO: 다음 상태가 Dead가 아니면 무조건 원복
            if (newState != ECharacterState.Dead)
            {
                character.IsDead = false;
                character.InputEnabled = true;
                character.Controller.SetCollisions(true);
            }
        }

        public override void UpdateState(BaseCharacter character, CharacterStateMachine stateMachine)
        {

        }

        public override void UpdateVelocity(BaseCharacter character, KinematicCharacterMotor motor, ref Vector3 currentVelocity, float deltaTime)
        {
            if (motor.GroundingStatus.IsStableOnGround)
            {
                // currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, 1f - Mathf.Exp(-character.MovementSettings.StableMovementSharpness * deltaTime));
            }
            else
            {
                // Gravity
                currentVelocity += character.Controller.Gravity * deltaTime;
            }
        }

        public override void UpdateRotation(BaseCharacter character, KinematicCharacterMotor motor, ref Quaternion currentRotation, float deltaTime)
        {

        }
    }
}