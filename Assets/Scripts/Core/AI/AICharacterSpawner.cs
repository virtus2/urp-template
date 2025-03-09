using Core.Character;

namespace Core.AI
{
    public class AICharacterSpawner : Level.Placeable.Spawner
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
                Core.Character.BaseCharacter spawnedCharacter = handle.Result;
                spawnedCharacter.transform.position = transform.position;
            };
        }
    }
}