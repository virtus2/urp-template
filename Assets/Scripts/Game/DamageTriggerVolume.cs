using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Game
{
    public class DamageTriggerVolume : MonoBehaviour
    {
        public float Damage = 5f;
        public BoxCollider boxCollider;

        private void OnTriggerEnter(Collider other)
        {
            var damageableObject = other.GetComponent<DamageableComponent>();
            if(damageableObject != null && damageableObject.CanTakeDamage)
            {
                damageableObject.TakeDamage(Damage);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
            Gizmos.DrawCube(transform.position, boxCollider.size);
            Handles.Label(transform.position, $"DamageVolume Damage:{Damage}");
        }
    }
}
