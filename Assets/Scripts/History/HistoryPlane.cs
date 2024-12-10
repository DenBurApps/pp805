using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HistoryPlane : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameText, _dateText, _crystalsText;

    public HistoryData HistoryData { get; private set; }
    public bool IsActive { get; private set; }

    public void Enable(HistoryData data)
    {
        if (data == null) throw new ArgumentNullException(nameof(data));

        gameObject.SetActive(true);
        IsActive = true;

        HistoryData = data;

        _nameText.text = data.Type switch
        {
            GameType.BombEscape => "Bomb Escape",
            GameType.SpaceJump => "Space Jump",
            GameType.SpaceMission => "Space Mission",
            _ => "Unknown Game"
        };

        _dateText.text = data.Date.ToString("dd MMMM ") + " at " + data.Date.ToString("t");
        _crystalsText.text = data.Crystals.ToString();
    }

    public void Disable()
    {
        gameObject.SetActive(false);
        IsActive = false;
        HistoryData = null;
    }
}

[Serializable]
public class HistoryData
{
    public GameType Type;
    public DateTime Date;
    public int Crystals;

    public HistoryData(GameType type, DateTime date, int crystals)
    {
        Type = type;
        Date = date;
        Crystals = crystals;
    }
}