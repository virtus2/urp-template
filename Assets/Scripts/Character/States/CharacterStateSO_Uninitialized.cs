using KinematicCharacterController;
using UnityEngine;

namespace Core
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
        public override void OnStateEnter(Character character, ECharacterState prevState)
        {
        }

        public override void OnStateExit(Character character, ECharacterState newState)
        {
        }

        public override void UpdateRotation(Character character, KinematicCharacterMotor motor, ref Quaternion currentRotation, float deltaTime)
        {
        }

        public override void UpdateState(Character character, CharacterStateMachine stateMachine)
        {
            Debug.LogWarning($"UninitializedState Being Updated!!! Check {character.name}'s CharacterStateMachine.");
        }

        public override void UpdateVelocity(Character character, KinematicCharacterMotor motor, ref Vector3 currentVelocity, float deltaTime)
        {
        }
    }
}