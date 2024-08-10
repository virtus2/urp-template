using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        public string SceneName_MainMenu = "MainMenuScene";
        public string SceneName_Test = "TestScene";

        public Core.Player Player;

        private void Awake()
        {
            DontDestroyOnLoad(this);

            var loadSceneOp = SceneManager.LoadSceneAsync(SceneName_MainMenu, LoadSceneMode.Additive);
            loadSceneOp.completed += (op) =>
            {

            };
        }

        public async void LoadSceneAsync(string sceneName)
        {

        }
    }
}