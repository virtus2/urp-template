using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Core
{
    [CreateAssetMenu(fileName = "SO_MovementSettings_Default", menuName = "Scriptable Objects/Character/New Movement Settings")]
    [System.Serializable]
    public class MovementSettings : ScriptableObject
    {
        [Header("이동 관련 변수들")]
        [Header("미터/초")]
        public float WalkSpeed = 1.25f; // 걷기 이동 속도
        public float RunSpeed = 3.5f; // 달리기 이동 속도
        public float Acceleration = 10.0f; // 걷기<->달리기 가속도
        public float Decceleration = 10.0f; // 걷기<->달리기 감속도
        public float StableMovementSharpness = 15.0f; // 이동 속도 변화에 대한 부드러움
        public float JumpSpeed = 10.0f; //  점프 속도
        public float JumpAbortSpeed = 10.0f;
        public float LadderClimbingSpeed = 2.0f; // 사다리 타기 속도

        public float MaxRotationSpeed = 1200.0f;
        public float MinRotationSpeed = 600.0f;

        public float GroundedGravity = 5.0f;
        public float Gravity = 20.0f;
        public float MaxFallSpeed = 40.0f;

        [Header("Useful for rough ground")]
        public float GroundedOffset = -0.14f;

        [Header("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;

        [Header("What layers the character uses as ground")]
        public LayerMask GroundLayers;
    }
}

