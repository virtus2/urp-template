using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ScreenResolutionDropdown : MonoBehaviour
{
    [SerializeField]
    private TMPro.TMP_Dropdown ResolutionDropdown;

    [SerializeField]
    private Sprite DropdownImage;

    [SerializeField]
    private TMPro.TMP_Dropdown FullscreenDropdown;

    [SerializeField]
    private Toggle VSyncToggle;

    [SerializeField]
    private Slider FrameRateLimitSlider;

    private List<TMPro.TMP_Dropdown.OptionData> optionData = new List<TMPro.TMP_Dropdown.OptionData>();
    private List<Resolution> optionDataResolutions = new List<Resolution>();

    private void Awake()
    {
        ResolutionDropdown.onValueChanged.AddListener(OnValueChanged_ResolutionDropdown);
        FullscreenDropdown.onValueChanged.AddListener(OnValueChanged_FullscreenDropdown);

        UpdateResolutionDropdown();
        UpdateFullscreenDropdown();
        UpdateVSyncToggle();
        UpdateFrameRateLimitSlider();
        SelectCurrentResolution();
    }

    private void OnValidate()
    {
        UpdateResolutionDropdown();
        UpdateFullscreenDropdown();
        UpdateVSyncToggle();
        UpdateFrameRateLimitSlider();
        SelectCurrentResolution();
    }

    private void UpdateResolutionDropdown()
    {
        if (ResolutionDropdown == null)
            return;

        ResolutionDropdown.ClearOptions();
        optionData.Clear();
        optionDataResolutions.Clear();

        foreach (Resolution resolution in Screen.resolutions)
        {
            string optionText = $"{resolution.width}x{resolution.height}";
            TMPro.TMP_Dropdown.OptionData data = new TMPro.TMP_Dropdown.OptionData(optionText, DropdownImage, Color.white);

            optionData.Add(data);
            optionDataResolutions.Add(resolution);
        }

        ResolutionDropdown.AddOptions(optionData);
    }

    private void SelectCurrentResolution()
    {
        for (int i = 0; i < optionDataResolutions.Count; i++)
        {
            if (Screen.currentResolution.width == optionDataResolutions[i].width &&
                Screen.currentResolution.height == optionDataResolutions[i].height)
                ResolutionDropdown.value = i;
        }
    }

    private void UpdateFullscreenDropdown()
    {
        if (FullscreenDropdown == null)
            return;

        FullscreenDropdown.ClearOptions();

        List<string> fullscreenModes = new List<string>();
        fullscreenModes.Add("Fullscreen"); // FullScreenMode.ExclusiveFullScreen
        fullscreenModes.Add("Borderless Fullscreen"); // FullScreenMode.FullScreenWindow
        fullscreenModes.Add("Windowed"); // FullScreenMode.Windowed
        FullscreenDropdown.AddOptions(fullscreenModes);
        // FullscreenDropdown.isOn = Screen.fullScreen;
    }

    private void UpdateVSyncToggle()
    {
        if (VSyncToggle == null)
            return;

        VSyncToggle.isOn = QualitySettings.vSyncCount == 1;
    }

    private void UpdateFrameRateLimitSlider()
    {
        if (FrameRateLimitSlider == null)
            return;

        FrameRateLimitSlider.maxValue = (float)Screen.currentResolution.refreshRateRatio.value;

        // V Sync is off
        if (QualitySettings.vSyncCount == 0)
        {
            FrameRateLimitSlider.value = Application.targetFrameRate;
        }
        else
        {
            FrameRateLimitSlider.value = (float)(Screen.currentResolution.refreshRateRatio.value / (double)QualitySettings.vSyncCount);
        }
    }

    private void OnValueChanged_ResolutionDropdown(int index)
    {
        Resolution selectedResolution = optionDataResolutions[index];
        // TODO: Fullscreen support
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, false);
    }

    private void OnValueChanged_FullscreenDropdown(int index)
    {
    }
}
