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

        public string SceneName;

        protected override void OnTriggerEnter(Collider other)
        {
            PlayerCharacterController controller = other.GetComponent<PlayerCharacterController>();
            if (controller)
            {
                StartCoroutine(GameManager.Instance.LoadSceneAsync(SceneName, false, OnSceneTransition));
            }
        }

        private void OnSceneTransition(UnityEngine.SceneManagement.Scene loadedScene)
        {
            // TODO: memory optimization
            List<GameObject> rootGameObjects = new List<GameObject>(loadedScene.rootCount);

            // Please make sure the list capacity is bigger than Scene.rootCount, then Unity will not allocate memory internally.
            loadedScene.GetRootGameObjects(rootGameObjects);
            Vector3 entrancePosition = rootGameObjects.Find(x => x.name.Equals("Entrance")).transform.position;
            // �� Ŭ�������� Ʈ������ �� �÷��̾� ĳ���͸� �ǵ帮�°� �̻���. ���߿� ������ Ŭ������ �̵�.
            if (AfterTransition == AfterTransitionAction.MoveTo)
            {
                MovePlayerCharacter(entrancePosition);
            }
        }

        private void SpawnPlayerCharacter() { }
        private void MovePlayerCharacter(in Vector3 position) 
        {
            PlayerCharacterController controller = Player.Instance.PlayerCharacter.GetComponent<PlayerCharacterController>();
            if (controller)
            {
                controller.SetPosition(position);
            }
        }
    }
}
