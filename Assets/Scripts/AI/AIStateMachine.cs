
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.LowLevel;

namespace Core
{
    public class AIStateMachine : MonoBehaviour
    {
        public AIState CurrentState;
        public AIState PreviousState;

        Dictionary<AIState, IAIState> States = new Dictionary<AIState, IAIState>()
        {
            [AIState.Uninitialized] = new AI.UninitializedState(),
            [AIState.Idle] = new AI.IdleState(),
            [AIState.Wandering] = new AI.WanderingState(),
        };

        public AI.WanderingState wanderingState; 

        public NavMeshAgent Agent;

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
            Agent = GetComponent<NavMeshAgent>();

            character = GetComponent<Character>();
            wanderingState = (AI.WanderingState)States[AIState.Wandering];
            if (CurrentState == AIState.Uninitialized)
            {
                TransitionToState(AIState.Idle);
            }
        }

        private void Update()
        {
            States[CurrentState].UpdateState(character, this);
        }

        public void TransitionToState(AIState newState)
        {
            PreviousState = CurrentState;
            CurrentState = newState;

            OnStateExit(PreviousState, CurrentState);
            OnStateEnter(PreviousState, CurrentState);
        }

        public void OnStateEnter(AIState prevState, AIState state)
        {
            States[state].OnStateEnter(character, prevState);
        }

        public void OnStateExit(AIState state, AIState newState)
        {
            States[state].OnStateExit(character, newState);
        }
    }
}