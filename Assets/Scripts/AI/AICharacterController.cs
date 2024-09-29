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
        private BehaviorGraphAgent agent;
        public GameObject chaseTarget;

        public override void SetCharacter(Character character)
        {
            base.SetCharacter(character); 
        }
    }
}