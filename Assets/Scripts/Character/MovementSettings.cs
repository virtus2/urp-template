using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Core
{
    [CreateAssetMenu(fileName = "Default MovementSettings", menuName = "Scriptable Objects/Character/New Movement Settings")]
    [System.Serializable]
    public class MovementSettings : ScriptableObject
    {
        [Header("이동 관련 변수들")]
        [Header("주의사항: CharacterAnimationController에서도 Blend Tree등에 같은 값으로 맞춰줘야함")]
        [Header("미터/초")]
        public float WalkSpeed = 1.25f; // 걷기 이동 속도
        public float RunSpeed = 3.5f; // 달리기 이동 속도
        public float Acceleration = 10.0f; // 걷기<->달리기 가속도
        public float Decceleration = 10.0f; // 걷기<->달리기 감속도
        public float JumpSpeed = 10.0f; //  점프 속도
        public float JumpAbortSpeed = 10.0f;

        public float MaxRotationSpeed = 1200.0f;
        public float MinRotationSpeed = 600.0f;

        public float GroundedGravity = 5.0f;
        public float Gravity = 20.0f;
        public float MaxFallSpeed = 40.0f;

        [Tooltip("Useful for rough ground")]
        public float GroundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;
    }
}

