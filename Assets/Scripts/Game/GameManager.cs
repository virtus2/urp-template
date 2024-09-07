using Cinemachine;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        public string SceneName_MainMenu = "MainMenuScene";
        public string SceneName_Play= "PlayScene";
        public string SceneName_Test = "TestScene";

        public Player PlayerInstance;

        public static Action OnPlaySceneLoadCompleted;
        private Scene currentActiveScene;


        private void Awake()
        {
            Instance = this;

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (scene.name == SceneName_Play)
            {
                OnPlaySceneLoadCompleted?.Invoke();
            }
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
                PlayerInstance.SpawnPlayerCharacter(Vector3.zero);
            }
        }
    }
}