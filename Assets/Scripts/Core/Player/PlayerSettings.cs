using UnityEngine;

namespace Core.Player
{
    [CreateAssetMenu(fileName = "SO_PlayerSettings_Default", menuName = "Scriptable Objects/Player/Player Settings")]
    public class PlayerSettings : ScriptableObject
    {
        [NaughtyAttributes.ShowAssetPreview]
        public Character.BaseCharacter PlayerCharacterPrefab;

    }
}