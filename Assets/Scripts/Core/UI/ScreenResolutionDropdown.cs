using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ScreenResolutionDropdown : MonoBehaviour
{
    [Header("Resolution")]
    [SerializeField]
    private TMPro.TMP_Dropdown ResolutionDropdown;

    [SerializeField]
    private Sprite ResolutionDropdownImage;

    [Header("Fullscreen Mode")]
    [SerializeField]
    private TMPro.TMP_Dropdown FullScreenModeDropdown;

    [SerializeField]
    private Sprite FullScreenModeDropdownImage;

    [Header("V Sync")]
    [SerializeField]
    private Toggle VSyncToggle;

    [Header("Target Frame Rate")]
    [SerializeField]
    private Slider FrameRateLimitSlider;

    private List<TMPro.TMP_Dropdown.OptionData> resolutionOptionsData = new List<TMPro.TMP_Dropdown.OptionData>();
    private List<Resolution> resolutionOptions = new List<Resolution>();

    private List<TMPro.TMP_Dropdown.OptionData> fullScreenModeOptionsData = new List<TMPro.TMP_Dropdown.OptionData>();
    private List<FullScreenMode> fullScreenModeOptions = new List<FullScreenMode>()
    {
        FullScreenMode.ExclusiveFullScreen,
        FullScreenMode.FullScreenWindow,
        FullScreenMode.Windowed,
    };

    private void Awake()
    {
        ResolutionDropdown.onValueChanged.AddListener(OnValueChanged_ResolutionDropdown);
        FullScreenModeDropdown.onValueChanged.AddListener(OnValueChanged_FullScreenDropdown);

        UpdateResolutionDropdown();
        UpdateFullScreenDropdown();
        UpdateVSyncToggle();
        UpdateFrameRateLimitSlider();
        SelectCurrentResolution();
        SelectCurrentFullScreenMode();
    }

    private void OnValidate()
    {
        UpdateResolutionDropdown();
        UpdateFullScreenDropdown();
        UpdateVSyncToggle();
        UpdateFrameRateLimitSlider();
        SelectCurrentResolution();
        SelectCurrentFullScreenMode();
    }

    private void UpdateResolutionDropdown()
    {
        if (ResolutionDropdown == null)
            return;

        ResolutionDropdown.ClearOptions();
        resolutionOptionsData.Clear();
        resolutionOptions.Clear();

        foreach (Resolution resolution in Screen.resolutions)
        {
            string optionText = $"{resolution.width}x{resolution.height}";
            TMPro.TMP_Dropdown.OptionData data = new TMPro.TMP_Dropdown.OptionData(optionText, ResolutionDropdownImage, Color.white);

            resolutionOptionsData.Add(data);
            resolutionOptions.Add(resolution);
        }

        ResolutionDropdown.AddOptions(resolutionOptionsData);
    }

    private void SelectCurrentResolution()
    {
        if (ResolutionDropdown == null)
            return;

        for (int i = 0; i < resolutionOptions.Count; i++)
        {
            if (Screen.currentResolution.width == resolutionOptions[i].width &&
                Screen.currentResolution.height == resolutionOptions[i].height)
                ResolutionDropdown.value = i;
        }
    }

    private void UpdateFullScreenDropdown()
    {
        if (FullScreenModeDropdown == null)
            return;

        FullScreenModeDropdown.ClearOptions();

        foreach (FullScreenMode mode in fullScreenModeOptions)
        {
            TMPro.TMP_Dropdown.OptionData data = new TMPro.TMP_Dropdown.OptionData(mode.ToString(), FullScreenModeDropdownImage, Color.white);

            fullScreenModeOptionsData.Add(data);
        }
        
        FullScreenModeDropdown.AddOptions(fullScreenModeOptionsData);
    }

    private void SelectCurrentFullScreenMode()
    {
        if (FullScreenModeDropdown == null)
            return;

        for (int i = 0; i < fullScreenModeOptions.Count; i++)
        {
            if (fullScreenModeOptions[i] == Screen.fullScreenMode)
                FullScreenModeDropdown.value = i;
        }
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
        Resolution selectedResolution = resolutionOptions[index];
        // TODO: Fullscreen support
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, false);
    }

    private void OnValueChanged_FullScreenDropdown(int index)
    {
    }
}
