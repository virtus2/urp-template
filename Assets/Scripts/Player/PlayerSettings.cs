using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "SO_PlayerSettings_Default", menuName = "Scriptable Objects/Player/Player Settings")]
    public class PlayerSettings : ScriptableObject
    {
        [NaughtyAttributes.ShowAssetPreview]
        public Character PlayerCharacterPrefab;

    }
}