using System;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public event Action HomeClicked;
    public event Action StatisticsClicked;
    public event Action SettingsClicked;

    public void OnHomeClicked()
    {
        HomeClicked?.Invoke();
    }

    public void OnStatisticsClicked()
    {
        StatisticsClicked?.Invoke();
    }

    public void OnSettingsClicked()
    {
        SettingsClicked?.Invoke();
    }
}
