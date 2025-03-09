using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

namespace Core
{
    public static class CameraUtility
    {
        public static void AddCinemachineBlenderSetting(CinemachineBrain cinemachineBrain, CinemachineBlenderSettings.CustomBlend customBlend)
        {
            if (string.IsNullOrEmpty(customBlend.From) && string.IsNullOrEmpty(customBlend.To)) 
                return;

            List<CinemachineBlenderSettings.CustomBlend> settings = new(cinemachineBrain.CustomBlends.CustomBlends);
            int existingCustomBlendIndex = settings.FindIndex(x => x.From == customBlend.From && x.To == customBlend.To);
            if (existingCustomBlendIndex < 0)
            {
                settings.Add(customBlend);
            }
            else
            {
                settings[existingCustomBlendIndex] = customBlend;
            }

            cinemachineBrain.CustomBlends.CustomBlends = settings.ToArray();
        }
    }
}