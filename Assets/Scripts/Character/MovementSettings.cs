using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Core
{
    [CreateAssetMenu(fileName = "Default MovementSettings", menuName = "Scriptable Objects/Character/Movement Settings")]
    [System.Serializable]
    public class MovementSettings : ScriptableObject
    {
        /* In meters/second */
        public float WalkSpeed = 1.25f; // 걷기 이동 속도
        public float SprintSpeed = 3.5f; // 달리기 이동 속도
        public float Acceleration = 10.0f; // 걷기<->달리기 가속도
        public float Decceleration = 10.0f; // 걷기<->달리기 감속도
        public float JumpSpeed = 10.0f; //  점프 속도
        public float JumpAbortSpeed = 10.0f;

        public float RollingSpeed = 5.0f; // 구르기 이동속도
        public float RollingDuration = 0.25f; // 구르기 상태 지속 시간
        public float RollingCooldownTime = 0.5f; // 구르기 쿨다운 타임 

        public float MaxRotationSpeed = 1200.0f;
        public float MinRotationSpeed = 600.0f;

        [Tooltip("Useful for rough ground")]
        public float GroundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;
    }
}

