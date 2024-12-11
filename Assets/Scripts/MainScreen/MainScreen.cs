using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using DanielLochner.Assets.SimpleScrollSnap;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class MainScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text _balanceText;
    [SerializeField] private Menu _menu;
    [SerializeField] private StatisticsScreen _statisticsScreen;
    [SerializeField] private SimpleScrollSnap _simpleScrollSnap;
    [SerializeField] private Settings _settings;

    private ScreenVisabilityHandler _screenVisabilityHandler;

    public event Action StatisticsClicked;

    private void Awake()
    {
        Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
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
        _menu.SettingsClicked += OnSettingsClicked;
        _statisticsScreen.HomeClicked += EnableScreen;
        _settings.HomeClicked += EnableScreen;
    }

    private void OnDisable()
    {
        StopScrolling();
        _menu.SettingsClicked -= OnSettingsClicked;
        _menu.StatisticsClicked -= OnStatisticsClicked;
        _menu.SettingsClicked -= OnSettingsClicked;
        _statisticsScreen.HomeClicked -= EnableScreen;
        _settings.HomeClicked -= EnableScreen;
    }

    public void OpenSpaceJump()
    {
        SceneManager.LoadScene("SpaceJumpScene");
    }

    public void OpenSpaceMission()
    {
        SceneManager.LoadScene("SpaceMission");
    }

    public void OpenBombEscape()
    {
        SceneManager.LoadScene("BombEscape");
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
        _settings.ShowSettings();
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