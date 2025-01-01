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
        private Scene currentActiveScene;


        private void Awake()
        {
            Instance = this;

            // TODO: 메인메뉴 로드
            // TODO: 메인메뉴에서 게임 시작 이후 PlayScene 로드
            StartCoroutine(LoadSceneAsync(SceneName_Play, true, InitializePlayer));
#if UNITY_EDITOR
#endif
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

        private void InitializePlayer()
        {
            var vcam = FindAnyObjectByType<CinemachineVirtualCamera>();
            PlayerInstance.SetVirtualCamera(vcam);
            PlayerInstance.SetMainCamera(Camera.main);

            var playerHUD = FindAnyObjectByType<PlayerHUD>();
            PlayerInstance.SetPlayerHUD(playerHUD);
        }
    }
}