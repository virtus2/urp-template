using Core.Player;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Level
{
    public class GameScene : MonoBehaviour
    {
        public enum SceneType
        {
            Overworld,
            Interiors,
        }

        public SceneType Type;
        public string AreaName;
        public List<PlayerCharacterSpawner> PlayerCharacterSpawner;
        public List<Placeable.SceneTransitionVolume> SceneTransitionVolume;


        private void OnValidate()
        {
            Scene scene = gameObject.scene;
            gameObject.name = scene.name;
        }
    }
}