using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Core.AI
{
    public class IdleState : IAIState
    {
        public AIState State => AIState.Idle;

        private float timeElapsed = 0f;

        public void OnStateEnter(Character character, AIState prevState)
        {
            timeElapsed = 0f;
        }

        public void OnStateExit(Character character, AIState newState)
        {
            timeElapsed = 0f;
        }

        public void UpdateState(Character character, AIStateMachine stateMachine)
        {
            timeElapsed += Time.deltaTime;

            if (timeElapsed > 3f)
            {
                stateMachine.TransitionToState(AIState.Wandering);
            }
        }
    }
}