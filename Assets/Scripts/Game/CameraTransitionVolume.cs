using Cinemachine;
using System.Collections;
using UnityEngine;

namespace Core
{
    public class CameraTransitionVolume : TriggerVolume
    {
        [SerializeField] 
        private CinemachineVirtualCamera VirtualCamera;
        private float defaultBlendTime;
        private int defaultPriority;

        protected override void OnTriggerEnter(Collider other)
        {
            if (Player.Instance.PlayerCharacterFollowCamera == null)
            {
                Debug.LogError("PlayerCharacterFollowCamera is null!");
                return;
            }
            
            // TODO: Ŀ���� ���� Ÿ�� �����ϴ� �� ���
            // ������ �ó׸ӽ� �극�ο� Custom blends �� �߰��ؼ� blend time �����ϴ� ������� �ߴµ�
            // �ó׸ӽ� ���߾� ī�޶� ���� ���������� ����������ϴ� �������� ����.

            // Save default time and priority of PlayerCharacterFollow camera.
            defaultBlendTime = Player.Instance.CinemachineBrain.m_DefaultBlend.m_Time;
            defaultPriority = Player.Instance.PlayerCharacterFollowCamera.Priority;

            // Set the time and priority of the virtual camera that will blend.
            VirtualCamera.Priority = defaultPriority + 1;
        }

        protected override void OnTriggerExit(Collider other)
        {
            VirtualCamera.Priority = 0;
        }
    }
}
