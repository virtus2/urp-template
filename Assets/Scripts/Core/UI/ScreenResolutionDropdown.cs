using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class ScreenResolutionDropdown : MonoBehaviour
{
    [Header("Resolution")]
    [SerializeField]
    private TMPro.TMP_Dropdown ResolutionDropdown;

    [SerializeField]
    private Sprite ResolutionDropdownImage;

    private List<TMPro.TMP_Dropdown.OptionData> resolutionOptionsData = new List<TMPro.TMP_Dropdown.OptionData>();
    private List<Resolution> resolutionOptions = new List<Resolution>();

    [Header("Fullscreen Mode")]
    [SerializeField]
    private TMPro.TMP_Dropdown FullScreenModeDropdown;

    [SerializeField]
    private Sprite FullScreenModeDropdownImage;


    [Header("Localized String")]
    [SerializeField]
    private LocalizedString LocalizedStr_ExclusiveFullScreeen;

    [SerializeField]
    private LocalizedString LocalizedStr_FullScreenWindow;

    [SerializeField]
    private LocalizedString LocalizedStr_Windowed;

    [SerializeField]
    private LocalizedString LocalizedStr_TargetFrameRate;

    private List<TMPro.TMP_Dropdown.OptionData> fullScreenModeOptionsData = new List<TMPro.TMP_Dropdown.OptionData>();
    private List<FullScreenMode> fullScreenModeOptions = new List<FullScreenMode>()
    {
        FullScreenMode.ExclusiveFullScreen,
        FullScreenMode.FullScreenWindow,
        FullScreenMode.Windowed,
    };

    [Header("V Sync")]
    [SerializeField]
    private Toggle VSyncToggle;

    [Header("Target Frame Rate")]
    [SerializeField]
    private TMPro.TMP_Text TargetFrameRateText;

    [SerializeField]
    private Slider TargetFrameRateSlider;

    [Header("Vignette")]
    [SerializeField]
    private Toggle VignetteToggle;

    [Header("Motion Blur")]
    [SerializeField]
    private Toggle MotionBlurToggle;

    private void Awake()
    {
        ResolutionDropdown.onValueChanged.AddListener(OnValueChanged_ResolutionDropdown);
        FullScreenModeDropdown.onValueChanged.AddListener(OnValueChanged_FullScreenDropdown);
        VSyncToggle.onValueChanged.AddListener(OnValueChanged_VSyncToggle);
        TargetFrameRateSlider.onValueChanged.AddListener(OnValueChanged_TargetFrameRateSlider);


        Application.targetFrameRate = (int)Screen.mainWindowDisplayInfo.refreshRate.value;

        UpdateResolutionDropdown();
        UpdateFullScreenDropdown();
        UpdateVSyncToggle();
        UpdateTargetFrameRateSliderAndText();
        SelectCurrentResolution();
        SelectCurrentFullScreenMode();
    }

    private void OnEnable()
    {
        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;

        // TODO:
        ClientPrefs.GetScreenResolution();
    }

    private void OnDisable()
    {
        LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
    }

    private void OnValidate()
    {
        UpdateResolutionDropdown();
        UpdateFullScreenDropdown();
        UpdateVSyncToggle();
        UpdateTargetFrameRateSliderAndText();
        SelectCurrentResolution();
        SelectCurrentFullScreenMode();
    }

    private void OnLocaleChanged(Locale locale)
    {
        UpdateFullScreenDropdown();
        UpdateTargetFrameRateSliderAndText();
        SelectCurrentFullScreenMode();
    }

    private void UpdateResolutionDropdown()
    {
        if (ResolutionDropdown == null)
            return;

        ResolutionDropdown.ClearOptions();
        resolutionOptionsData.Clear();
        resolutionOptions.Clear();

        HashSet<Vector2Int> uniqueResolutions = new HashSet<Vector2Int>();

        foreach (Resolution resolution in Screen.resolutions)
        {
            Vector2Int res = new Vector2Int(resolution.width, resolution.height);

            if (uniqueResolutions.Contains(res))
                continue;

            string optionText = $"{resolution.width}x{resolution.height}";
            TMPro.TMP_Dropdown.OptionData data = new TMPro.TMP_Dropdown.OptionData(optionText, ResolutionDropdownImage, Color.white);

            resolutionOptionsData.Add(data);
            resolutionOptions.Add(resolution);
            uniqueResolutions.Add(res);
        }

        ResolutionDropdown.AddOptions(resolutionOptionsData);
        ResolutionDropdown.interactable = true;


        // If current fullscreen mode is borderless windowed, resolution dropdown is unavailable.
        if (FullScreenModeDropdown == null)
            return;

        if (fullScreenModeOptions[FullScreenModeDropdown.value] == FullScreenMode.FullScreenWindow)
            ResolutionDropdown.interactable = false;
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
        fullScreenModeOptionsData.Clear();

        foreach (FullScreenMode mode in fullScreenModeOptions)
        {
            string fullScreenModeName = mode switch
            {
                FullScreenMode.FullScreenWindow => LocalizedStr_FullScreenWindow.GetLocalizedString(),
                FullScreenMode.ExclusiveFullScreen => LocalizedStr_ExclusiveFullScreeen.GetLocalizedString(),
                FullScreenMode.Windowed => LocalizedStr_Windowed.GetLocalizedString(),
                _ => string.Empty
            };

            TMPro.TMP_Dropdown.OptionData data = new TMPro.TMP_Dropdown.OptionData(fullScreenModeName, FullScreenModeDropdownImage, Color.white);

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

    private void UpdateTargetFrameRateSliderAndText()
    {
        if (TargetFrameRateSlider == null)
            return;

        if (TargetFrameRateText == null)
            return;

        if (QualitySettings.vSyncCount >= 1)
        {
            TargetFrameRateSlider.interactable = false;
            TargetFrameRateSlider.minValue = 30;
            TargetFrameRateSlider.maxValue = (float)Screen.currentResolution.refreshRateRatio.value;
            TargetFrameRateSlider.value = (float)Screen.currentResolution.refreshRateRatio.value;
            TargetFrameRateText.text = string.Format(LocalizedStr_TargetFrameRate.GetLocalizedString(), 
                Mathf.CeilToInt((float)Screen.currentResolution.refreshRateRatio.value).ToString());
            return;
        }

        TargetFrameRateSlider.interactable = true;
        TargetFrameRateSlider.minValue = 30;
        TargetFrameRateSlider.maxValue = (float)Screen.currentResolution.refreshRateRatio.value;
        TargetFrameRateSlider.value = Application.targetFrameRate;
        TargetFrameRateText.text = string.Format(LocalizedStr_TargetFrameRate.GetLocalizedString(), Application.targetFrameRate.ToString());
    }

    private void OnValueChanged_ResolutionDropdown(int index)
    {
        Resolution selectedResolution = resolutionOptions[index];
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, fullScreenModeOptions[FullScreenModeDropdown.value]);
        ClientPrefs.SetScreenResolution(selectedResolution);
    }

    private void OnValueChanged_FullScreenDropdown(int index)
    {
        Resolution currentResolution = Screen.currentResolution;
        FullScreenMode selectedFullScreenMode = fullScreenModeOptions[index];
        Screen.SetResolution(currentResolution.width, currentResolution.height, selectedFullScreenMode);
        ClientPrefs.SetFullScreenMode(selectedFullScreenMode);

        UpdateResolutionDropdown();
        SelectCurrentResolution();
    }

    private void OnValueChanged_VSyncToggle(bool isOn)
    {
        QualitySettings.vSyncCount = isOn ? 1 : 0;

        UpdateTargetFrameRateSliderAndText();
    }

    private void OnValueChanged_TargetFrameRateSlider(float value)
    {
        Application.targetFrameRate = Mathf.CeilToInt(value);

        UpdateTargetFrameRateSliderAndText();
    }
}
