using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

public class Settings : MonoBehaviour
{
    [SerializeField] private GameObject _settingsCanvas;
    [SerializeField] private GameObject _privacyCanvas;
    [SerializeField] private GameObject _termsCanvas;
    [SerializeField] private GameObject _contactCanvas;
    [SerializeField] private GameObject _versionCanvas;
    [SerializeField] private TMP_Text _versionText;
    [SerializeField] private Menu _menu;
    [SerializeField] private TMP_Text[] _balanceTexts;
    
    private string _version = "Application version:\n";

    public event Action HomeClicked;
    public event Action StatisticsClicked;
    
    private void Awake()
    {
        _settingsCanvas.SetActive(false);
        _privacyCanvas.SetActive(false);
        _termsCanvas.SetActive(false);
        _contactCanvas.SetActive(false);
        _versionCanvas.SetActive(false);
        SetVersion();
    }

    private void OnEnable()
    {
        _menu.HomeClicked += OnHomeClicked;
        _menu.StatisticsClicked += OnStatisticsClicked;
    }

    private void OnDisable()
    {
        _menu.HomeClicked -= OnHomeClicked;
        _menu.StatisticsClicked -= OnStatisticsClicked;
    }

    private void SetVersion()
    {
        _versionText.text = _version + Application.version;
    }

    public void ShowSettings()
    {
        _settingsCanvas.SetActive(true);
        foreach (var text in _balanceTexts)
        {
            text.text = PlayerBalanceController.CurrentBalance.ToString();
        }
    }

    private void OnHomeClicked()
    {
        HomeClicked?.Invoke();
        _settingsCanvas.SetActive(false);
    }

    private void OnStatisticsClicked()
    {
        StatisticsClicked?.Invoke();
        _settingsCanvas.SetActive(false);
    }

    public void RateUs()
    {
#if UNITY_IOS
        Device.RequestStoreReview();
#endif
    }
}
