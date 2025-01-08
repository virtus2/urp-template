using KinematicCharacterController;
using System;

namespace Core
{
    public enum CharacterState
    {
        Uninitialized,

        Idle,
        GroundMove,
        Attack,
        Dead,

        // Not in use yet.
        Crouched,
        AirMove,
        WallRun,
        Rolling,
        LedgeGrab,
        LedgeStandingUp,
        Dashing,
        Swimming,
        Climbing,
        FlyingNoCollisions,
        RopeSwing,
    }

    public interface ICharacterState
    {
        CharacterState State { get; }
        void OnStateEnter(Character character, CharacterState prevState);
        void OnStateExit(Character character, CharacterState newState);
        void UpdateState(Character character, CharacterStateMachine stateMachine);
        void UpdateVelocity(Character character, KinematicCharacterMotor motor, ref UnityEngine.Vector3 currentVelocity, float deltaTime);
        UnityEngine.Quaternion GetCurrentRotation(Character character, KinematicCharacterMotor motor, float deltaTime);
    }
}