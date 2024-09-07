using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class PlayerCharacterSpawner : Spawner
    {
        private Player player;

        private IEnumerator Start()
        {
            while (!GameManager.Instance)
            {
                yield return null;
            }
            player = GameManager.Instance.PlayerInstance;
            Spawn();
        }

        public override void Spawn()
        {
            player.SpawnPlayerCharacter(transform.position);
        }
    }
}