namespace Core
{
    public enum CharacterState
    {
        Uninitialized,

        Idle,
        GroundMove,
        Attack,
        Dead,


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
    }
}