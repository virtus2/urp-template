using System.Collections;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;

namespace Core
{
    /// <summary>
    /// AI를 기반으로 한 캐릭터 컨트롤러
    /// </summary>
    public class AICharacterController : BaseCharacterController
    {
        [NaughtyAttributes.HorizontalLine]
        [Header("AI Character Controller")]
        public GameObject Target;
        public Blackboard Blackboard;

        private Coroutine attackCoroutine = null;
        private BehaviorGraphAgent agent;

        public void SetTarget(GameObject target)
        {
            Target = target;
            if (Target == null)
            {
                OrientationMethod = OrientationMethod.TowardsMovement;
            }
            else
            {
                OrientationMethod = OrientationMethod.TowardsTarget;
            }
        }

        public void Attack()
        {
            if(attackCoroutine != null)
                StopCoroutine(attackCoroutine);
            attackCoroutine = StartCoroutine(AttackInternal());
        }

        private IEnumerator AttackInternal()
        {
            AttackPressed = true;
            yield return new WaitUntil(()=> character.IsAttacking == false);
            AttackPressed = false;
        }
    }
}