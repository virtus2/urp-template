using KinematicCharacterController;
using UnityEngine;

namespace Core.Character.State
{
    public abstract class CharacterStateSO : ScriptableObject
    {
        public abstract CharacterState CreateInstance();
    }

    public abstract class CharacterState
    {
        public abstract void OnStateEnter(BaseCharacter character, ECharacterState prevState);
        public abstract void OnStateExit(BaseCharacter character, ECharacterState newState);
        public abstract void UpdateState(BaseCharacter character, CharacterStateMachine stateMachine);
        public abstract void UpdateVelocity(BaseCharacter character, KinematicCharacterMotor motor, ref Vector3 currentVelocity, float deltaTime);
        public abstract void UpdateRotation(BaseCharacter character, KinematicCharacterMotor motor, ref Quaternion currentRotation, float deltaTime);
    }
}
