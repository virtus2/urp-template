using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.AI
{
    public class WanderingState : IAIState
    {
        public AIState State => AIState.Wandering;

        private float timeElapsed = 0f;

        public void OnStateEnter(Character character, AIState prevState)
        {
        }

        public void OnStateExit(Character character, AIState newState)
        {
        }

        public void UpdateState(Character character, AIStateMachine stateMachine)
        {
            timeElapsed += Time.deltaTime;
        }
    }
}