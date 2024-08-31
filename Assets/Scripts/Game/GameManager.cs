using Cinemachine;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        public string SceneName_MainMenu = "MainMenuScene";
        public string SceneName_Play= "PlayScene";
        public string SceneName_Test = "TestScene";

        public Player PlayerInstance;

        private Scene currentActiveScene;

        private void Awake()
        {
            StartCoroutine(LoadSceneAsync(SceneName_Play, true, () =>
            {
                var vcam = FindAnyObjectByType<CinemachineVirtualCamera>();
                PlayerInstance.SetVirtualCamera(vcam);
                PlayerInstance.SetMainCamera(Camera.main);

                var playerHUD = FindAnyObjectByType<PlayerHUD>();
                PlayerInstance.SetPlayerHUD(playerHUD);
            }));
        }

        private IEnumerator LoadSceneAsync(string sceneName, bool activateLoadedScene = false, Action onLoadComplete = null)
        {
            AsyncOperation asyncSceneLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            while (!asyncSceneLoad.isDone)
            {
                yield return null;
            }

            Scene loadedScene = SceneManager.GetSceneByName(sceneName);
            if (activateLoadedScene)
            {
                SceneManager.SetActiveScene(loadedScene);
                currentActiveScene = loadedScene;
            }

            onLoadComplete?.Invoke();
        }

        private void OnGUI()
        {
            if(GUILayout.Button("Load TestScene"))
            {
                StartCoroutine(LoadSceneAsync(SceneName_Test));
            }
            if (GUILayout.Button("Spawn Player Character"))
            {
                PlayerInstance.SpawnPlayerCharacter();
            }
        }
    }
}