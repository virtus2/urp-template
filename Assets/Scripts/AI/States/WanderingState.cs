using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AI;
using static Core.AI.WanderingState;

namespace Core.AI
{
    public class WanderingState : IAIState
    {
        public AIState State => AIState.Wandering;

        public enum PathfindingState
        {
            Begin, Move, Completed
        }

        private float maxWanderingTime = 3f;
        private float maxDistance = 5f; // TODO: 적당한 값 찾기 https://docs.unity3d.com/ScriptReference/AI.NavMesh.SamplePosition.html

        private PathfindingState pathfindingState;
        private float timeElapsed = 0f;

        private Vector3 currentDestination;
        private int currentPathIndex = 0;

        public void OnStateEnter(Character character, AIState prevState, AIStateMachine stateMachine)
        {
            timeElapsed = 0f;

            currentPathIndex = 0;
            pathfindingState = PathfindingState.Begin;
        }

        public void OnStateExit(Character character, AIState newState, AIStateMachine stateMachine)
        {
            timeElapsed = 0f;
        }
            
        public void UpdateState(Character character, AIStateMachine stateMachine)
        {
            timeElapsed += Time.deltaTime;
            
            // 일정 시간이 지나면 강제로 종료 한다.
            if(timeElapsed > maxWanderingTime)
            {
                pathfindingState = PathfindingState.Completed;
            }

            switch (pathfindingState)
            {
                // Begin 단계에서는 주변 무작위 위치를 계산한다.
                case PathfindingState.Begin:
                    Vector3 randomPosition = Random.insideUnitSphere * maxDistance + character.transform.position;
                    stateMachine.Destination = randomPosition;
                    NavMeshHit hit;
                    bool positionFound = NavMesh.SamplePosition(randomPosition, out hit, maxDistance, NavMesh.AllAreas);

                    if (!positionFound)
                    {

                    }
                    else
                    {
                        if(stateMachine.Agent.CalculatePath(hit.position, stateMachine.Path))
                        {
                            pathfindingState = PathfindingState.Move;
                            currentPathIndex = 0;
                            currentDestination = stateMachine.Path.corners[currentPathIndex];
                        }
                    }

                    break;
                // Move 단계에서는 계산된 경로를 따라 움직인다.
                case PathfindingState.Move:
                    if (currentPathIndex >= stateMachine.Path.corners.Length)
                    {
                        pathfindingState = PathfindingState.Completed;
                        break;
                    }
                    if ((character.transform.position - currentDestination).sqrMagnitude < 0.01f)
                    {
                        currentPathIndex++;
                        if(currentPathIndex >= stateMachine.Path.corners.Length)
                        {
                            pathfindingState = PathfindingState.Completed;
                            break;
                        }
                        currentDestination = stateMachine.Path.corners[currentPathIndex];
                    }
                    Vector3 toTarget = currentDestination - character.transform.position;
                    toTarget.Normalize();
                    character.Controller.MovementInput = new Vector2(toTarget.x, toTarget.z);


                    break;
                // Completed 단계에선 Idle 상태로 넘어가게한다.
                case PathfindingState.Completed:
                    character.Controller.MovementInput = Vector2.zero;
                    stateMachine.TransitionToState(AIState.Idle);

                    break;
                default:
                    break;
            }
        }
    }
}