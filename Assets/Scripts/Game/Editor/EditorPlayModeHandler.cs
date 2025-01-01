using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Editor
{
    [InitializeOnLoad]
    public class EditorPlayModeHandler : MonoBehaviour
    {
        private const string EditorPrefsCurrentScene = "CurrentScene";

        static EditorPlayModeHandler()
        {
            EditorApplication.playModeStateChanged += PlayModeStateChanged;
        }

        private static void PlayModeStateChanged(PlayModeStateChange state)
        {
            switch (state)
            {
                case PlayModeStateChange.EnteredEditMode:
                    break;
                case PlayModeStateChange.ExitingEditMode:
                    EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                    EditorPrefs.SetString(EditorPrefsCurrentScene, EditorApplication.currentScene);
                    EditorSceneManager.playModeStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(EditorBuildSettings.scenes[0].path);
                    break;
                case PlayModeStateChange.EnteredPlayMode:
                    Debug.Log("Entered Play Mode");
                    string currentSceneName = EditorPrefs.GetString(EditorPrefsCurrentScene);
                    Debug.Log($"Load ({currentSceneName}) Scene");
                    EditorSceneManager.LoadScene(currentSceneName, LoadSceneMode.Additive);
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    break;
                default:
                    break;
            }
        }
    }
}