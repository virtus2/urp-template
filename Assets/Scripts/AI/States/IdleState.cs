using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.AI
{
    public class IdleState : IAIState
    {
        public AIState State => AIState.Idle;

        private float timeElapsed = 0f;

        public void OnStateEnter(Character character, AIState prevState)
        {
            Debug.Log("IdleState OnStateEnter");
        }

        public void OnStateExit(Character character, AIState newState)
        {
            Debug.Log("IdleState OnStateExit");
        }

        public void UpdateState(Character character, AIStateMachine stateMachine)
        {
            timeElapsed += Time.deltaTime;
        }
    }
}