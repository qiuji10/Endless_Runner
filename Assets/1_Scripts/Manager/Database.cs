using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using NaughtyAttributes;
using System.IO;

[System.Serializable]
public class PlayerData
{
    public int highscore;
    public int money;
    public int powerupJump;
    public int powerupRevive;
    public int powerupBonus;
}

public class Database : MonoBehaviour
{
    public PlayerData playerData;
    public TextAsset jsonData;
    public string filePath;
    public string fileName;
    private string path;
    public Text[] texts;

    private void Awake()
    {
        path = Application.persistentDataPath + filePath + fileName;

    #if UNITY_ANDROID && !UNITY_EDITOR
        if (!File.Exists(path))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            string json = JsonConvert.SerializeObject(playerData);
            File.WriteAllText(path, json);
        }
    #endif


        string jsonData = File.ReadAllText(path);
        playerData = JsonConvert.DeserializeObject<PlayerData>(jsonData);
        
        Assigner();
    }

    [Button]
    public void WriteToJson()
    {
    #if UNITY_EDITOR
        path = Application.persistentDataPath + filePath + fileName;
    #endif
        string json = JsonConvert.SerializeObject(playerData);
        File.WriteAllText(path, json);
    }

    public void Assigner()
    {
        texts[0].text = playerData.highscore.ToString();
        texts[1].text = playerData.money.ToString();
        texts[2].text = playerData.powerupJump.ToString();
        texts[3].text = playerData.powerupRevive.ToString();
        texts[4].text = playerData.powerupBonus.ToString();
    }
}
