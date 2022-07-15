using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class PlayerData
{
    public int highscore;
    public int money;
    public int powerupJump;
    public int powerupShield;
    public int powerupBonus;
}

public class Database : MonoBehaviour
{
    public PlayerData playerData;
    public string filePath;
    public string fileName;
    private string path;
    public Text[] texts;

    private void Awake()
    {
        path = Application.persistentDataPath + filePath + fileName;

        if (!File.Exists(path))
        {
            SaveGame();
        }
        LoadGame();
        Assigner();
    }

    [Button]
    public void SaveGame()
    {
        if (!File.Exists(path))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(Application.persistentDataPath + filePath + fileName));
        }
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + filePath + fileName);

        bf.Serialize(file, playerData);
        file.Close();
    }

    [Button]
    public void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + filePath + fileName))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file =
                       File.Open(Application.persistentDataPath + filePath + fileName, FileMode.Open);
            playerData = (PlayerData)bf.Deserialize(file);
            file.Close();
        }
    }

    public void Assigner()
    {
        texts[0].text = playerData.highscore.ToString();
        texts[1].text = playerData.money.ToString();
        texts[2].text = playerData.powerupJump.ToString();
        texts[3].text = playerData.powerupShield.ToString();
        texts[4].text = playerData.powerupBonus.ToString();
    }
}
