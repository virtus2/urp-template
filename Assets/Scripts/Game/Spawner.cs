using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Core
{
    public class Spawner : MonoBehaviour
    {
        public enum ESpawnCondition
        {
            OnAwake,
            OnTriggerEnter,
            OnInteract,
        }
        public ComponentReferenceCharacter characterPrefab;
        public BaseCharacterController characterController;

        public ESpawnCondition SpawnCondition;

        [Header("이 태그가 붙은 오브젝트가 트리거 안에 들어올 시 프리팹을 스폰")]
        // Spawn a prefab when an object with this tag enters the trigger.
        public List<string> SpawnableTags;
        public Collider SpawnTrigger;

        [Header("이 물체에 상호작용 한 뒤 스폰")]
        public IInteractable SpawnableInteract;

        // TODO: 스폰 전 카메라 연출, 지연 스폰
        // TODO: 스폰 이펙트
        public bool SpawnOnlyOnce;
        public bool Spawned;

        private void OnTriggerEnter(Collider other)
        {
            foreach(string tag in SpawnableTags)
            {
                if(other.CompareTag(tag))
                {
                    Spawn();
                }
            }
        }

        public void Spawn()
        {
            var asyncOperation = characterPrefab.InstantiateAsync();
            asyncOperation.Completed += (handle) =>
            {
                Character spawnedCharacter = handle.Result;
            };
        }

        private void OnDrawGizmos()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Gizmos.color = transparentGreen;
            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Vector3 sphereCenter = transform.position;
            Gizmos.DrawSphere(sphereCenter, 0.5f);
        }
    }
}
