using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        public string SceneName_MainMenu = "MainMenuScene";
        public string SceneName_Play= "PlayScene";
        public string SceneName_Test = "TestScene";

        public Player PlayerInstance;
        public Image SceneTransitionImage; 
        public float SceneTransitionDuration = 0.5f;

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
            PlayerCharacterFollowCamera camera = FindAnyObjectByType<PlayerCharacterFollowCamera>();
            PlayerInstance.SetPlayerCharacterFollowCamera(camera);
            PlayerInstance.SetMainCamera(Camera.main);

            PlayerHUD playerHUD = FindAnyObjectByType<PlayerHUD>();
            PlayerInstance.SetPlayerHUD(playerHUD);
        }

        public void SceneTransition(string sceneName, GuidReference destination)
        {
            StartCoroutine(InternalSceneTransition(sceneName, destination));
        }

        private IEnumerator TransitionFadeInEffect()
        {
            float elapsedTime = 0f;
            while (elapsedTime < SceneTransitionDuration)
            {
                elapsedTime += Time.deltaTime;
                SceneTransitionImage.color = Color.Lerp(Color.clear, Color.black, elapsedTime / SceneTransitionDuration);
                yield return null;
            }
        }

        private IEnumerator TransitionFadeOutEffect()
        {
            float elapsedTime = 0f;
            while (elapsedTime < SceneTransitionDuration)
            {
                elapsedTime += Time.deltaTime;
                SceneTransitionImage.color = Color.Lerp(Color.black, Color.clear, elapsedTime / SceneTransitionDuration);
                yield return null;
            }
        }

        private IEnumerator InternalSceneTransition(string sceneName, GuidReference destination)
        {
            yield return TransitionFadeInEffect();
            yield return LoadSceneAsync(sceneName, false, (Scene loadedScene) =>
            {
                // TODO: 텔포 이후 걸어가는 연출이나 스크린 연출 (동숲같은거)
                // TODO: 게임매니저 클래스 외에 다른 클래스로 분리 (SceneTransitionManager라던가 PortalManager?)
                // TODO: 지역 이름 출력
                if (destination.gameObject != null)
                {
                    Vector3 destinationPosition = destination.gameObject.transform.position;
                    PlayerInstance.TeleportPlayerCharacter(destinationPosition);
                }
            });
            yield return TransitionFadeOutEffect();
        }
        private void OnGUI()
        {
            if (GUILayout.Button("Load Test Scene"))
            {
                StartCoroutine(LoadSceneAsync(SceneName_Test));
            }
            if (GUILayout.Button("Respawn Player Character"))
            {
                PlayerCharacterSpawner spawner = FindAnyObjectByType<PlayerCharacterSpawner>();
                Vector3 spawnPosition = spawner != null ? spawner.transform.position : Vector3.zero;
                PlayerInstance.RespawnPlayerCharacter(spawnPosition);
            }
        }
    }
}