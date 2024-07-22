using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class AICharacterController : Controller
    {
        public override Vector3 GetMovementVector3()
        {
            // TODO: 캐릭터의 상태에 따라서 AI의 움직임 벡터를 반환
            return Vector3.zero;
        }
    }
}