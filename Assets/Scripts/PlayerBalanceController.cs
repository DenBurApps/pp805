using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerBalanceController
{
    private static readonly string Key = "Balance";

    static PlayerBalanceController()
    {
        CurrentBalance = 0;

        if (PlayerPrefs.HasKey(Key))
        {
            CurrentBalance = PlayerPrefs.GetInt(Key);
        }
    }
    public static int CurrentBalance { get; private set; }

    public static void IncreaseBalance(int value)
    {
        CurrentBalance += value;
        PlayerPrefs.SetInt(Key, CurrentBalance);
    }
}