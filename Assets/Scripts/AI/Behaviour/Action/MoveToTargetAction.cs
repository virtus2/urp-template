using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

namespace Core.AI
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "MoveToTargetAction", story: "[Self] moves to the [Target] until distance <= [StopDistance]", category: "Custom/Action/Movement", id: "ba483300b2b697df5e084fe9af472735")]
    public partial class MoveToTargetAction : Action
    {
        [SerializeReference] public BlackboardVariable<GameObject> Self;
        [SerializeReference] public BlackboardVariable<GameObject> Target;
        [SerializeReference] public BlackboardVariable<float> StopDistance;
        private Character character;
        private NavMeshAgent agent;

        protected override Status OnStart()
        {
            character = Self.Value.GetComponent<Character>();
            agent = Self.Value.GetComponent<NavMeshAgent>();
            return Status.Running;
        }

        protected override Status OnUpdate()
        {
            if (Vector3.Distance(Self.Value.transform.position, Target.Value.transform.position) <= StopDistance)
            {
                character.Controller.SetMovementInput(Vector3.zero);
                return Status.Success;
            }

            NavMeshHit hit;
            bool positionFound = NavMesh.SamplePosition(Target.Value.transform.position, out hit, 1.5f, NavMesh.AllAreas);
            if (positionFound)
            {
                if (agent.CalculatePath(hit.position, character.NavMeshPath))
                {
                    if (character.NavMeshPath.corners.Length >= 2)
                    {
                        Vector3 currentDestination = character.NavMeshPath.corners[1];
                        Vector3 toTarget = currentDestination - character.transform.position;
                        toTarget.Normalize();
                        character.Controller.SetMovementInput(toTarget);
                    }
                }
            }
            return Status.Running;
        }

        protected override void OnEnd()
        {
        }
    }
}
