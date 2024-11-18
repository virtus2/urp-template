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

        private void Start()
        {

            if (SpawnCondition == ESpawnCondition.OnStart)
            {
                Spawn();
            }
        }

        public override void Spawn()
        {
            var asyncOperation = characterPrefab.InstantiateAsync();
            asyncOperation.Completed += (handle) =>
            {
                Character spawnedCharacter = handle.Result;
                spawnedCharacter.transform.position = transform.position;
            };
        }
    }
}