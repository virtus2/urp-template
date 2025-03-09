using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Core.Character.BaseCharacter;

namespace Core.AI.State
{
    public class AIStateMachine : MonoBehaviour
    {
        public AIState CurrentState;
        public AIState PreviousState;

        Dictionary<AIState, IAIState> States = new Dictionary<AIState, IAIState>()
        {
            [AIState.Uninitialized] = new UninitializedState(),
            [AIState.Idle] = new IdleState(),
            [AIState.Wandering] = new WanderingState(),
            [AIState.Chase] = new ChaseState(),
        };

        public WanderingState wanderingState; 

        public NavMeshAgent Agent;
        public NavMeshPath Path;
        public Vector3 Destination;

        private Core.Character.BaseCharacter character;
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
            Path = new NavMeshPath();

            character = GetComponent<Core.Character.BaseCharacter>();
            wanderingState = (WanderingState)States[AIState.Wandering];
            if (CurrentState == AIState.Uninitialized)
            {
                TransitionToState(AIState.Idle);
            }
        }

        private void Update()
        {
            if (!character) return;
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
            States[state].OnStateEnter(character, prevState, this);
        }

        public void OnStateExit(AIState state, AIState newState)
        {
            States[state].OnStateExit(character, newState, this);
        }

        private void OnDrawGizmos()
        {
            if(Path != null)
            {
                if(Path.corners != null && Path.corners.Length > 0)
                {
                    Gizmos.color = UnityEngine.Color.yellow;
                    foreach (var point in Path.corners)
                    {
                        Gizmos.DrawCube(point, Vector3.one * 0.2f);
                    }

                    Gizmos.color = UnityEngine.Color.magenta;
                    Gizmos.DrawCube(Destination, Vector3.one * 0.2f);
                }
            }
        }
    }
}