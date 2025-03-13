using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LocaleSettingsUI : MonoBehaviour
{
    [SerializeField]
    private TMPro.TMP_Dropdown LocaleDropdown;

    [SerializeField]
    private Sprite LocaleDropdownImage;

    private List<TMPro.TMP_Dropdown.OptionData> localeOptionsData = new List<TMPro.TMP_Dropdown.OptionData>();
    private List<Locale> localeOptions = new List<Locale>();


    [Header("Localized String")]
    [SerializeField]
    private LocalizedString LocalizedStr_Korean;

    [SerializeField]
    private LocalizedString LocalizedStr_Japanese;

    [SerializeField]
    private LocalizedString LocalizedStr_English;


    private void Awake()
    {
        LocaleDropdown.onValueChanged.AddListener(OnValueChanged_LocaleDropdown);

        UpdateLocaleDropdown();
        SelectCurrentLocale();
    }

    private IEnumerator Start()
    {
        yield return LocalizationSettings.InitializationOperation;

        UpdateLocaleDropdown();
        SelectCurrentLocale();
    }

    private void OnValidate()
    {
        UpdateLocaleDropdown();
        SelectCurrentLocale();
    }

    private void UpdateLocaleDropdown()
    {
        if (LocaleDropdown == null)
            return;

        LocaleDropdown.ClearOptions();
        localeOptions.Clear();
        localeOptionsData.Clear();

        IOrderedEnumerable<Locale> sortedLocales = LocalizationSettings.AvailableLocales.Locales.OrderBy(locale => locale.SortOrder);
        foreach (Locale locale in sortedLocales)
        {
            string localeName = locale.Identifier.Code switch
            {
                "ko" => LocalizedStr_Korean.GetLocalizedString(),
                "en" => LocalizedStr_English.GetLocalizedString(),
                "ja" => LocalizedStr_Japanese.GetLocalizedString(),
                _ => string.Empty
            };

            localeOptions.Add(locale);
            localeOptionsData.Add(new TMPro.TMP_Dropdown.OptionData(localeName, LocaleDropdownImage, Color.white));
        }

        LocaleDropdown.AddOptions(localeOptionsData);
    }

    private void SelectCurrentLocale()
    {
        if (LocaleDropdown == null)
            return;
    }

    private void OnValueChanged_LocaleDropdown(int index)
    {
        LocalizationSettings.SelectedLocale = localeOptions[index];
        Debug.Log($"Current Locale: {LocalizationSettings.SelectedLocale.LocaleName}");
    }
}
