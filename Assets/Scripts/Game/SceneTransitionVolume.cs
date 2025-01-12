using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Core
{
    public class SceneTransitionVolume : TriggerVolume
    {
        public enum AfterTransitionAction
        {
            Nothing,
            MoveTo,
            Spawn,
        }
        public AfterTransitionAction AfterTransition = AfterTransitionAction.Nothing;

        // TODO: ScriptableObject로 관리? GUID based Reference로 관리?
        public string SceneName;
        public string DestinationName;

        protected override void OnTriggerEnter(Collider other)
        {
            // Works only when the player character enters the trigger volume
            PlayerCharacterController controller = other.GetComponent<PlayerCharacterController>();
            if (controller)
            {
                GameManager.Instance.SceneTransition(SceneName, DestinationName);
            }
        }
    }
}
