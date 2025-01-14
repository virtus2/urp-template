using KinematicCharacterController;
using UnityEngine;

namespace Core
{
    public abstract class CharacterStateSO : ScriptableObject
    {
        public abstract CharacterState CreateInstance();
    }

    public abstract class CharacterState
    {
        public abstract void OnStateEnter(Character character, ECharacterState prevState);
        public abstract void OnStateExit(Character character, ECharacterState newState);
        public abstract void UpdateState(Character character, CharacterStateMachine stateMachine);
        public abstract void UpdateVelocity(Character character, KinematicCharacterMotor motor, ref Vector3 currentVelocity, float deltaTime);
        public abstract void UpdateRotation(Character character, KinematicCharacterMotor motor, ref Quaternion currentRotation, float deltaTime);
    }
}
