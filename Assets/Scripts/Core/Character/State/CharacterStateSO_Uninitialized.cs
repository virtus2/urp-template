using KinematicCharacterController;
using UnityEngine;

namespace Core.Character.State
{
    [CreateAssetMenu(fileName = "SO_CharacterState_Uninitialized", menuName = "Scriptable Objects/Character/State/Uninitialized")]
    public class CharacterStateSO_Uninitialized : CharacterStateSO
    {
        public override CharacterState CreateInstance()
        {
            return new CharacterState_Uninitialized();
        }
    }

    public class CharacterState_Uninitialized : CharacterState
    {
        public override void OnStateEnter(BaseCharacter character, ECharacterState prevState)
        {
        }

        public override void OnStateExit(BaseCharacter character, ECharacterState newState)
        {
        }

        public override void UpdateRotation(BaseCharacter character, KinematicCharacterMotor motor, ref Quaternion currentRotation, float deltaTime)
        {
        }

        public override void UpdateState(BaseCharacter character, CharacterStateMachine stateMachine)
        {
            Debug.LogWarning($"UninitializedState Being Updated!!! Check {character.name}'s CharacterStateMachine.");
        }

        public override void UpdateVelocity(BaseCharacter character, KinematicCharacterMotor motor, ref Vector3 currentVelocity, float deltaTime)
        {
        }
    }
}