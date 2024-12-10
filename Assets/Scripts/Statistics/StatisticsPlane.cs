using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatisticsPlane : MonoBehaviour
{
    [SerializeField] private TMP_Text _bombsText;
    [SerializeField] private TMP_Text _crystalsText;

    public void SetText(GameProgress progress)
    {
        _bombsText.text = progress.Bombs.ToString();
        _crystalsText.text = progress.Crystals.ToString();
    }
}
