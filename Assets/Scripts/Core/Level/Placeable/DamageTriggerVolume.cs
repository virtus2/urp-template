using UnityEditor;
using UnityEngine;

namespace Core.Level.Placeable
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

        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
            Gizmos.DrawCube(transform.position, boxCollider.size);
            Handles.Label(transform.position, $"DamageVolume Damage:{Damage}");
        }
        #endif
    }
}
