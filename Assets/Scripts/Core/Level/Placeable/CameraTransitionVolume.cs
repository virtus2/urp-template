using UnityEngine;
using Unity.Cinemachine;
using Core.Player;

namespace Core.Level.Placeable
{
    public class CameraTransitionVolume : TriggerVolume
    {
        [SerializeField] 
        private CinemachineCamera CameraToBlendIn;

        [SerializeField]
        private CinemachineBlendDefinition BlendInSetting;

        [SerializeField]
        private CinemachineBlendDefinition BlendOutSetting;

        private CinemachineCameraEvents cameraEvents;

        private void Awake()
        {
            // Add custom blend settings in runtime.
            CameraUtility.AddCinemachineBlenderSetting(PlayerInstance.Instance.CinemachineBrain, new CinemachineBlenderSettings.CustomBlend()
            {
                From = PlayerInstance.Instance.PlayerCharacterFollowCamera.name,
                To = CameraToBlendIn.name,
                Blend = BlendInSetting
            });

            CameraUtility.AddCinemachineBlenderSetting(PlayerInstance.Instance.CinemachineBrain, new CinemachineBlenderSettings.CustomBlend()
            {
                From = CameraToBlendIn.name,
                To = PlayerInstance.Instance.PlayerCharacterFollowCamera.name,
                Blend = BlendOutSetting
            });

            // Deactivate the CinemachineCamera for performance.
            // see https://docs.unity3d.com/Packages/com.unity.cinemachine@3.1/manual/concept-essential-elements.html#processing-power-consumption
            CameraToBlendIn.gameObject.SetActive(false);
        }

        private void OnValidate()
        {
            // Rename the CinemachineCamera for convenience.
            CameraToBlendIn = transform.parent.GetComponentInChildren<CinemachineCamera>();
            CameraToBlendIn.name = string.Format(Constants.kCameraTransitionVolumeCameraName, transform.parent.name);
        }

        protected override void OnTriggerEnter(Collider other)
        {
            // Set the priority of the CinemachineCamera that will blend.
            CameraToBlendIn.gameObject.SetActive(true);
            CameraToBlendIn.Priority = Constants.kCameraDefaultPriority + 1;
        }

        protected override void OnTriggerExit(Collider other)
        {
            // Reset the priority of the CinemachineCamera.
            CameraToBlendIn.Priority = 0;
            CameraToBlendIn.gameObject.SetActive(false);
        }
    }
}
