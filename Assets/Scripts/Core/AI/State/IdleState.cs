using UnityEngine;

namespace Core.AI.State
{
    public class IdleState : IAIState
    {
        public AIState State => AIState.Idle;

        private float timeElapsed = 0f;

        public void OnStateEnter(Core.Character.BaseCharacter character, AIState prevState, AIStateMachine stateMachine)
        {
            timeElapsed = 0f;
        }

        public void OnStateExit(Core.Character.BaseCharacter character, AIState newState, AIStateMachine stateMachine)
        {
            timeElapsed = 0f;
        }

        public void UpdateState(Core.Character.BaseCharacter character, AIStateMachine stateMachine)
        {
            timeElapsed += Time.deltaTime;

            if (timeElapsed > 3f)
            {
                stateMachine.TransitionToState(AIState.Wandering);
            }
        }
    }
}