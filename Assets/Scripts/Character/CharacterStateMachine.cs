
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

namespace Core
{
    public class CharacterStateMachine : MonoBehaviour
    {
        public CharacterState CurrentState;
        public CharacterState PreviousState;

        Dictionary<CharacterState, ICharacterState> States = new Dictionary<CharacterState, ICharacterState>()
        {
            [CharacterState.Uninitialized] = new UninitializedState(),
            [CharacterState.Idle] = new IdleState(),
            [CharacterState.GroundMove] = new GroundMoveState(),
            [CharacterState.Rolling] = new RollingState(),
            [CharacterState.Attack] = new AttackState(),
        };

        private Character character;
        /*
        public GroundMoveState GroundMoveState;
        public CrouchedState CrouchedState;
        public AirMoveState AirMoveState;
        public WallRunState WallRunState;
        public RollingState RollingState;
        public ClimbingState ClimbingState;
        public DashingState DashingState;
        public SwimmingState SwimmingState;
        public LedgeGrabState LedgeGrabState;
        public LedgeStandingUpState LedgeStandingUpState;
        public FlyingNoCollisionsState FlyingNoCollisionsState;
        public RopeSwingState RopeSwingState;
        */

        private void Awake()
        {
            character = GetComponent<Character>();
            if (CurrentState == CharacterState.Uninitialized)
            {
                TransitionToState(CharacterState.Idle);
            }
        }

        private void Update()
        {
            if (!character) return;
            States[CurrentState].UpdateState(character, this);
        }

        public void TransitionToState(CharacterState newState)
        {
            PreviousState = CurrentState;
            CurrentState = newState;

            OnStateExit(PreviousState, CurrentState);
            OnStateEnter(PreviousState, CurrentState);
        }

        public void OnStateEnter(CharacterState prevState, CharacterState state)
        {
            States[state].OnStateEnter(character, prevState);
        }

        public void OnStateExit(CharacterState state, CharacterState newState)
        {
            States[state].OnStateExit(character, newState);
        }
    }
}