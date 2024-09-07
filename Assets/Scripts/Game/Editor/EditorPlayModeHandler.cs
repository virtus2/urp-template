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
        private static Scene activeScene;
        static EditorPlayModeHandler()
        {
            Debug.Log(EditorSceneManager.playModeStartScene);
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
                    break;
                case PlayModeStateChange.EnteredPlayMode:
                    Debug.Log("Entered Play Mode");
                    activeScene = EditorSceneManager.GetActiveScene();
                    EditorSceneManager.LoadScene("BootstrapScene", LoadSceneMode.Single);
                    EditorSceneManager.LoadScene("PlayScene", LoadSceneMode.Additive);
                    EditorSceneManager.LoadScene(activeScene.name, LoadSceneMode.Additive);
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    break;
                default:
                    break;
            }
        }
    }
}