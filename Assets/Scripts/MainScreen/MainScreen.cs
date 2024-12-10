using System;
using System.Collections;
using System.Collections.Generic;
using DanielLochner.Assets.SimpleScrollSnap;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class MainScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text _balanceText;
    [SerializeField] private Menu _menu;
    [SerializeField] private StatisticsScreen _statisticsScreen;
    [SerializeField] private SimpleScrollSnap _simpleScrollSnap;

    private ScreenVisabilityHandler _screenVisabilityHandler;
    
    public event Action StatisticsClicked;
    public event Action SettingsClicked;

    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }
    
    private void Start()
    {
        StartScrolling();
        EnableScreen();
    }

    private void OnEnable()
    {
        _menu.SettingsClicked += OnSettingsClicked;
        _menu.StatisticsClicked += OnStatisticsClicked;
        _statisticsScreen.HomeClicked += EnableScreen;
    }

    private void OnDisable()
    {
        StopScrolling();
        _menu.SettingsClicked -= OnSettingsClicked;
        _menu.StatisticsClicked -= OnStatisticsClicked;
        _statisticsScreen.HomeClicked -= EnableScreen;
    }

    public void EnableScreen()
    {
        _screenVisabilityHandler.EnableScreen();
        _balanceText.text = PlayerBalanceController.CurrentBalance.ToString();
    }

    public void DisableScreen()
    {
        _screenVisabilityHandler.DisableScreen();
    }

    private void StartScrolling()
    {
        InvokeRepeating(nameof(ChangeGame), 5f, 5f);
    }

    private void StopScrolling()
    {
        CancelInvoke(nameof(ChangeGame));
    }
    
    private void OnSettingsClicked()
    {
        SettingsClicked?.Invoke();
        DisableScreen();
    }

    private void OnStatisticsClicked()
    {
        StatisticsClicked?.Invoke();
        DisableScreen();
    }

    private void ChangeGame()
    {
        _simpleScrollSnap.GoToNextPanel();
    }
}
