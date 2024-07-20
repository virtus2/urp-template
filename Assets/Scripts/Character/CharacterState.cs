
public enum CharacterState
{
    Uninitialized,

    Idle,
    GroundMove,
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
    void OnStateEnter();
    void OnStateExit();
    void GetMoveVectorFromPlayerInput();
}