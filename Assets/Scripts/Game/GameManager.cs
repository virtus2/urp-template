using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        public IEnumerator LoadSceneAsync(string sceneName, bool activateLoadedScene = false, Action<Scene> onLoadComplete = null)
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

            onLoadComplete?.Invoke(loadedScene);
        }

        private void InitializePlayer(Scene _)
        {
            CinemachineCamera playerFollowCamera = FindAnyObjectByType<CinemachineCamera>();
            PlayerInstance.SetPlayerFollowCamera(playerFollowCamera);
            PlayerInstance.SetMainCamera(Camera.main);

            PlayerHUD playerHUD = FindAnyObjectByType<PlayerHUD>();
            PlayerInstance.SetPlayerHUD(playerHUD);
        }
    }
}