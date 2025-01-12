using NaughtyAttributes.Test;
using System;
using System.Collections;
using System.Collections.Generic;
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
        public Dictionary<string, Scene> loadedSceneByName = new Dictionary<string, Scene>();


        private void Awake()
        {
            Instance = this;

            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
            // TODO: 메인메뉴 로드
            // TODO: 메인메뉴에서 게임 시작 이후 PlayScene 로드
            StartCoroutine(LoadSceneAsync(SceneName_Play, true, InitializePlayer));
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (loadedSceneByName.ContainsKey(scene.name))
            {
                loadedSceneByName.Add(scene.name, scene);
            }
        }

        private void OnSceneUnloaded(Scene scene)
        {
            if (loadedSceneByName.ContainsKey(scene.name))
            {
                loadedSceneByName.Remove(scene.name);
            }
        }

        public IEnumerator LoadSceneAsync(string sceneName, bool activateLoadedScene = false, Action<Scene> onLoadComplete = null)
        {
            if(!loadedSceneByName.ContainsKey(sceneName))
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

                loadedSceneByName.Add(sceneName, loadedScene);
            }

            onLoadComplete?.Invoke(loadedSceneByName[sceneName]);
        }

        private void InitializePlayer(Scene _)
        {
            CinemachineCamera playerFollowCamera = FindAnyObjectByType<CinemachineCamera>();
            PlayerInstance.SetPlayerFollowCamera(playerFollowCamera);
            PlayerInstance.SetMainCamera(Camera.main);

            PlayerHUD playerHUD = FindAnyObjectByType<PlayerHUD>();
            PlayerInstance.SetPlayerHUD(playerHUD);
        }

        public void SceneTransition(string sceneName, string destinationName)
        {
            StartCoroutine(LoadSceneAsync(sceneName, false, (Scene loadedScene) =>
            {
                // TODO: 메모리 최적화
                // TODO: Root Object를 찾으면 안되고 child에서도 찾을 수 있어야함. 태그 사용 고려
                List<GameObject> rootGameObjects = new List<GameObject>(loadedScene.rootCount);
                loadedScene.GetRootGameObjects(rootGameObjects);

                // TODO: 텔포 이후 걸어가는 연출이나 스크린 연출 (동숲같은거)
                // TODO: 게임매니저 클래스 외에 다른 클래스로 분리 (SceneTransitionManager라던가 PortalManager?)
                Vector3 entrancePosition = rootGameObjects.Find(x => x.name.Equals(destinationName)).transform.position;
                PlayerInstance.TeleportPlayerCharacter(entrancePosition);
            }));
        }


        private void OnGUI()
        {
            if (GUILayout.Button("Load Test Scene"))
            {
                StartCoroutine(LoadSceneAsync(SceneName_Test));
            }
        }
    }
}