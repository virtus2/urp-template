using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;
using static Core.AI.WanderingState;

namespace Core.AI
{
    public class ChaseState : IAIState
    {
        public AIState State => AIState.Chase;

        private float timeElapsed = 0f;
        private float maxDistance = 1.0f; // TODO: 적당한 값 찾기 https://docs.unity3d.com/ScriptReference/AI.NavMesh.SamplePosition.html

        public void OnStateEnter(Character character, AIState prevState, AIStateMachine stateMachine)
        {
            timeElapsed = 0f;

            stateMachine.Path.ClearCorners();
        }

        public void OnStateExit(Character character, AIState newState, AIStateMachine stateMachine)
        {
            timeElapsed = 0f;
        }

        public void UpdateState(Character character, AIStateMachine stateMachine)
        {
            // TODO: 왜 이 상태에서는 이동속도가 더 빠르지??? 버그 수정하기

            // 대상에 도착했을 때
            if ((character.ChaseTarget.transform.position - character.transform.position).magnitude <= stateMachine.Agent.radius + character.CapsuleRadius)
            {
                // TODO: 공격이나 다른 행동
                character.Controller.MovementInput = Vector2.zero;
                stateMachine.TransitionToState(AIState.Idle);
                return;
            }

            NavMeshHit hit;
            bool positionFound = NavMesh.SamplePosition(character.ChaseTarget.transform.position, out hit, maxDistance, NavMesh.AllAreas);
            if (positionFound)
            {
                if (stateMachine.Agent.CalculatePath(hit.position, stateMachine.Path))
                {
                    if (stateMachine.Path.corners.Length >= 2)
                    {
                        Vector3 currentDestination = stateMachine.Path.corners[1];
                        stateMachine.Destination = currentDestination;

                        Vector3 toTarget = currentDestination - character.transform.position;
                        toTarget.Normalize();
                        character.Controller.MovementInput = new Vector2(toTarget.x, toTarget.z);
                    }
                }
            }
        }
    }
}