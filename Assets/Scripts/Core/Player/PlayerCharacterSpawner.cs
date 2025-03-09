using Core.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Core.Player
{
    public class PlayerCharacterSpawner : Level.Placeable.Spawner
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
            PlayerInstance.Instance.SpawnPlayerCharacter(transform.position);
        }
    }
}