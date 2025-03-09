using UnityEngine;

namespace Core.Level.Placeable
{
    public class Spawner : MonoBehaviour
    {
        // TODO: 스폰 전 카메라 연출, 지연 스폰
        // TODO: 스폰 이펙트
        public bool SpawnOnlyOnce;
        public bool Spawned;

        public virtual void Spawn()
        {
        }

        protected void OnDrawGizmos()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Gizmos.color = transparentGreen;
            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Vector3 sphereCenter = transform.position;
            Gizmos.DrawSphere(sphereCenter, 0.5f);
        }
    }
}
