using UnityEngine;

namespace Core.Level.Placeable
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

        [NaughtyAttributes.Scene]
        public string SceneName;
        public GuidReference Destination;

        protected override void OnTriggerEnter(Collider other)
        {
            // Works only when the player character enters the trigger volume
            Player.PlayerCharacterController controller = other.GetComponent<Player.PlayerCharacterController>();
            if (controller)
            {
                GameManager.Instance.SceneTransition(SceneName, Destination);
            }
        }
    }
}
