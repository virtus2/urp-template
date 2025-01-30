using KinematicCharacterController;
using System;

namespace Core
{
    public enum ECharacterState
    {
        Uninitialized = 0,

        // Default
        Idle = 1,
        GroundMove = 2,
        Attack = 3,
        Dead = 4,

        // Special
        Attack2 = 1000,

        ClimbingLadder = 2000,

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
}