using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Core.Character
{
    [CreateAssetMenu(fileName = "SO_RollingSettings_Default", menuName = "Scriptable Objects/Character/New Rolling Settings")]
    [System.Serializable]
    public class RollingSettings : ScriptableObject
    {
        public float RollingDuration = 0.25f; // ������ ���� ���� �ð�
        public float RollingCooldownTime = 0.5f; // ������ ��ٿ� Ÿ�� 

        [Tooltip("������ �̵��ӵ�")]
        public AnimationCurve RollingSpeedCurve; // ������ �̵��ӵ�

        [Tooltip("������ �� ��ǥ �ӵ����� ��/����")]
        public bool AccelerationToTargetSpeed = false;

        [Tooltip("������ ���ӵ�")]
        public float Acceleration = 30.0f; // ������ ���ӵ�

        [Tooltip("������ ���ӵ�")]
        public float Decceleration = 10.0f; // ������ ���ӵ�

    }
}

