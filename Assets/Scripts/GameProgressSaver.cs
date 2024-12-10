using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public static class GameProgressSaver
{
    private static readonly string SavePath = Path.Combine(Application.persistentDataPath, "GameProgress.json");

    static GameProgressSaver()
    {
        BombEscapeProgress = new GameProgress(0, 0);
        SpaceJumpProgress = new GameProgress(0, 0);
        SpaceMissionProgress = new GameProgress(0, 0);
        Load();
    }

    public static GameProgress BombEscapeProgress { get; private set; }
    public static GameProgress SpaceJumpProgress { get; private set; }
    public static GameProgress SpaceMissionProgress { get; private set; }

    public static void UpdateGameProgress(GameType type, int bombs, int crystals)
    {
        switch (type)
        {
            case GameType.BombEscape:
                SetGameProgress(BombEscapeProgress, bombs, crystals);
                break;
            case GameType.SpaceJump:
                SetGameProgress(SpaceJumpProgress, bombs, crystals);
                break;
            case GameType.SpaceMission:
                SetGameProgress(SpaceMissionProgress, bombs, crystals);
                break;
        }
        
        Save();
    }

    private static void SetGameProgress(GameProgress gameProgress, int bombs, int crystals)
    {
        gameProgress.Bombs += bombs;
        gameProgress.Crystals += crystals;
    }

    private static void Save()
    {
        List<GameProgress> gameProgresses = new List<GameProgress>();
        gameProgresses.Add(BombEscapeProgress);
        gameProgresses.Add(SpaceJumpProgress);
        gameProgresses.Add(SpaceMissionProgress);

        GameProgressWrapper wrapper = new GameProgressWrapper(gameProgresses);
        string json = JsonConvert.SerializeObject(wrapper, Formatting.Indented);
        File.WriteAllText(SavePath, json);
    }

    private static void Load()
    {
        if (!File.Exists(SavePath))
        {
            Debug.Log("No save file found.");
            return;
        }

        var json = File.ReadAllText(SavePath);
        var wrapper = JsonConvert.DeserializeObject<GameProgressWrapper>(json);

        BombEscapeProgress = wrapper.Progresses[0];
        SpaceJumpProgress = wrapper.Progresses[1];
        SpaceMissionProgress = wrapper.Progresses[2];

        Debug.Log("data loaded successfully.");
    }
}

[Serializable]
public class GameProgressWrapper
{
    public List<GameProgress> Progresses;

    public GameProgressWrapper(List<GameProgress> progresses)
    {
        Progresses = progresses;
    }
}

[Serializable]
public class GameProgress
{
    public int Bombs;
    public int Crystals;

    public GameProgress(int bombs, int crystals)
    {
        Bombs = bombs;
        Crystals = crystals;
    }
}

public enum GameType
{
    BombEscape,
    SpaceJump,
    SpaceMission
}