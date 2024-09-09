using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class AICharacterSpawner : Spawner
    {
        public enum ESpawnCondition
        {
            OnStart,
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

        private void Start()
        {

            if (SpawnCondition == ESpawnCondition.OnStart)
            {
                Spawn();
            }
        }


        private void OnTriggerEnter(Collider other)
        {
            foreach (string tag in SpawnableTags)
            {
                if (other.CompareTag(tag))
                {
                    Spawn();
                }
            }
        }

        public override void Spawn()
        {
            var asyncOperation = characterPrefab.InstantiateAsync();
            asyncOperation.Completed += (handle) =>
            {
                Character spawnedCharacter = handle.Result;
                var controller = Instantiate(characterController);
                controller.SetCharacter(spawnedCharacter);
                spawnedCharacter.SetController(controller);
                spawnedCharacter.transform.position = transform.position;
            };
        }
    }
}