using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;
using TMPro;
using System;

public class changeLanguage : MonoBehaviour
{
    public int levelsCount = 1;
    [SerializeField] private TextMeshProUGUI textToUpdate;
    [SerializeField] private LocalizedString stringToUpdate;

    public void ChangeLocale()
    {
        //Get activate locale
        Locale L = LocalizationSettings.SelectedLocale;
        int index = LocalizationSettings.AvailableLocales.Locales.IndexOf(L);
        //If locale is last set first from list
        if (index >= LocalizationSettings.AvailableLocales.Locales.Count - 1)
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
        }
        else
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index+1];
        }
    }

    public void Levels()
    {
        ++levelsCount;
        if(levelsCount > 3)
        {
            levelsCount = 1;
        }
        stringToUpdate.Arguments[0] = levelsCount;
        stringToUpdate.RefreshString();
        return;
    }

    private void OnEnable()
    {
        stringToUpdate.Arguments = new object[] { levelsCount };
        stringToUpdate.StringChanged += UpdateText;
    }

    private void UpdateText(string value)
    {
        textToUpdate.text = value;
    }
}
