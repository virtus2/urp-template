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
            
            // TODO: 커스텀 블렌딩 타임 설정하는 법 고민
            // 지금은 시네머신 브레인에 Custom blends 에 추가해서 blend time 설정하는 방식으로 했는데
            // 시네머신 버추얼 카메라를 따로 프리팹으로 저장해줘야하는 불편함이 있음.

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
