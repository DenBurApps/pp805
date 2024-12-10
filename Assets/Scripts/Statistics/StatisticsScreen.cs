using System;
using System.Collections;
using System.Collections.Generic;
using DanielLochner.Assets.SimpleScrollSnap;
using UnityEngine;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class StatisticsScreen : MonoBehaviour
{
   [SerializeField] private StatisticsPlane[] _statisticsPlanes;
   [SerializeField] private Menu _menu;
   [SerializeField] private MainScreen _mainScreen;
   [SerializeField] private SimpleScrollSnap _simpleScrollSnap;
   
   private ScreenVisabilityHandler _screenVisabilityHandler;

   public event Action HomeClicked;
   public event Action SettingsClicked;
   
   private void Awake()
   {
      _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
   }

   private void Start()
   {
      DisableScreen();
   }

   private void OnEnable()
   {
      _menu.HomeClicked += OnHomeClicked;
      _menu.SettingsClicked += OnSettingsClicked;
      _mainScreen.StatisticsClicked += EnableScreen;
   }

   private void OnDisable()
   {
      StopScrolling();
      _menu.HomeClicked -= OnHomeClicked;
      _menu.SettingsClicked -= OnSettingsClicked;
      _mainScreen.StatisticsClicked -= EnableScreen;
   }
   
   private void StartScrolling()
   {
      InvokeRepeating(nameof(ChangeStats), 5f, 5f);
   }

   private void StopScrolling()
   {
      CancelInvoke(nameof(ChangeStats));
   }
   
   private void ChangeStats()
   {
      _simpleScrollSnap.GoToNextPanel();
   }

   private void EnableScreen()
   {
      _screenVisabilityHandler.EnableScreen();

      _statisticsPlanes[0].SetText(GameProgressSaver.BombEscapeProgress);
      _statisticsPlanes[1].SetText(GameProgressSaver.SpaceJumpProgress);
      _statisticsPlanes[2].SetText(GameProgressSaver.SpaceMissionProgress);
      StartScrolling();
   }

   private void DisableScreen()
   {
      _screenVisabilityHandler.DisableScreen();
      StopScrolling();
   }

   private void OnHomeClicked()
   {
      HomeClicked?.Invoke();
      DisableScreen();
   }

   private void OnSettingsClicked()
   {
      SettingsClicked?.Invoke();
      DisableScreen();
   }
}
