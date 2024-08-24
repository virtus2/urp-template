using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "Default PlayerSettings", menuName = "Scriptable Objects/Player/Player Settings")]
    public class PlayerSettings : ScriptableObject
    {
        public Character PlayerCharacter;

    }
}