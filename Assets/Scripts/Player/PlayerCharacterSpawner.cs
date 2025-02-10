using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Core
{
    public class PlayerCharacterSpawner : Spawner
    {
        public enum ESpawnType
        {
            Manual,
            OnLoaded,
        }

        public ESpawnType SpawnType = ESpawnType.Manual;

        private void Start()
        {
            if(SpawnType == ESpawnType.OnLoaded)
            {
                Spawn();
            }
        }

        public override void Spawn()
        {
            Player.Instance.SpawnPlayerCharacter(transform.position);
        }
    }
}