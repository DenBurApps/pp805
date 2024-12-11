using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public static class HistoryDataHolder
{
    private static List<HistoryData> Datas = new List<HistoryData>();
    private static readonly string SavePath = Path.Combine(Application.persistentDataPath, "History.json");

    public static List<HistoryData> HistoryDatas => Datas;
    
    static HistoryDataHolder()
    {
        Load();
    }

    public static void AddNewData(GameType type,  int crystals)
    {
        var newData = new HistoryData(type, DateTime.Now, crystals);
        Datas.Add(newData);
        Save();
    }

    private static void Save()
    {
        var wrapper = new HistoryDataWrapper(Datas);
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
        var wrapper = JsonConvert.DeserializeObject<HistoryDataWrapper>(json);

        Datas = wrapper.Datas;
        Debug.Log("data loaded successfully.");
    }
}

[Serializable]
public class HistoryDataWrapper
{
    public List<HistoryData> Datas;

    public HistoryDataWrapper(List<HistoryData> datas)
    {
        Datas = datas;
    }
}