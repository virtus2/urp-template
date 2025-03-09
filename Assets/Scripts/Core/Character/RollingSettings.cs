using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Core.Character
{
    [CreateAssetMenu(fileName = "SO_RollingSettings_Default", menuName = "Scriptable Objects/Character/New Rolling Settings")]
    [System.Serializable]
    public class RollingSettings : ScriptableObject
    {
        public float RollingDuration = 0.25f; // 구르기 상태 지속 시간
        public float RollingCooldownTime = 0.5f; // 구르기 쿨다운 타임 

        [Tooltip("구르기 이동속도")]
        public AnimationCurve RollingSpeedCurve; // 구르기 이동속도

        [Tooltip("구르기 시 목표 속도까지 가/감속")]
        public bool AccelerationToTargetSpeed = false;

        [Tooltip("구르기 가속도")]
        public float Acceleration = 30.0f; // 구르기 가속도

        [Tooltip("구르기 감속도")]
        public float Decceleration = 10.0f; // 구르기 감속도

    }
}

