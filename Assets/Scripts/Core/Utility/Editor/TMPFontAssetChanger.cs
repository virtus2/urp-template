using TMPro;
using UnityEditor;
using UnityEngine;

public class TMPFontAssetChanger : EditorWindow
{
    private TMP_FontAsset newFontAsset;

    [MenuItem("Tools/Change TMP Font Asset")]
    public static void ShowWindow()
    {
        GetWindow<TMPFontAssetChanger>("TMP Font Changer");
    }

    private void OnGUI()
    {
        GUILayout.Label("Change TMP Font Asset", EditorStyles.boldLabel);
        newFontAsset = (TMP_FontAsset)EditorGUILayout.ObjectField("New Font Asset", newFontAsset, typeof(TMP_FontAsset), false);

        if (GUILayout.Button("Change Font in Current Scene"))
        {
            ChangeFontInCurrentScene();
        }

        if (GUILayout.Button("Change Font in All Scenes"))
        {
            ChangeFontInAllScenes();
        }

        if (GUILayout.Button("Change Font in Prefabs"))
        {
            ChangeFontInPrefabs();
        }
    }

    private void ChangeFontInCurrentScene()
    {
        if (newFontAsset == null)
        {
            Debug.LogError("Please assign a TMP Font Asset.");
            return;
        }

        TextMeshProUGUI[] tmps = FindObjectsOfType<TextMeshProUGUI>(true);
        int count = 0;
        foreach (var tmp in tmps)
        {
            tmp.font = newFontAsset;
            EditorUtility.SetDirty(tmp);
            count++;
        }

        Debug.Log($"Changed font in {count} TextMeshProUGUI components.");
    }

    private void ChangeFontInAllScenes()
    {
        if (newFontAsset == null)
        {
            Debug.LogError("Please assign a TMP Font Asset.");
            return;
        }

        string[] scenePaths = AssetDatabase.FindAssets("t:Scene");
        int totalCount = 0;

        foreach (string scenePath in scenePaths)
        {
            string path = AssetDatabase.GUIDToAssetPath(scenePath);
            UnityEditor.SceneManagement.EditorSceneManager.OpenScene(path);

            TextMeshProUGUI[] tmps = FindObjectsOfType<TextMeshProUGUI>(true);
            foreach (var tmp in tmps)
            {
                tmp.font = newFontAsset;
                EditorUtility.SetDirty(tmp);
                totalCount++;
            }

            UnityEditor.SceneManagement.EditorSceneManager.SaveScene(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
        }

        Debug.Log($"Changed font in all scenes. Total TMP components updated: {totalCount}");
    }

    public static void ChangeFontInPrefabs()
    {
        TMP_FontAsset newFont = (TMP_FontAsset)AssetDatabase.LoadAssetAtPath<TMP_FontAsset>("Assets/Art/Font/nanum-square/NanumSquareR SDF.asset");

        if (newFont == null)
        {
            Debug.LogError("Font Asset 경로를 확인하세요.");
            return;
        }

        string[] guids = AssetDatabase.FindAssets("t:Prefab");
        int count = 0;

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (prefab == null) continue;

            TextMeshProUGUI[] tmps = prefab.GetComponentsInChildren<TextMeshProUGUI>(true);
            bool modified = false;

            foreach (var tmp in tmps)
            {
                if (tmp.font != newFont)
                {
                    tmp.font = newFont;
                    EditorUtility.SetDirty(tmp);
                    modified = true;
                }
            }

            if (modified)
            {
                PrefabUtility.SavePrefabAsset(prefab);
                count++;
            }
        }

        Debug.Log($"Changed font in {count} prefabs.");
    }
}
