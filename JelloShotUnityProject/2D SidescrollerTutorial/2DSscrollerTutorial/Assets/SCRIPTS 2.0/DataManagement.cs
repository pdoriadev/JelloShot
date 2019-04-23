using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
/// <summary>
/// Saves and accesses game score data. Serializes save data down to binary.
/// </summary>
public class DataManagement : MonoBehaviour
{
    internal static DataManagement instance;

    [SerializeField]
    internal int dManHighScore;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        if (instance == !this)
        {
            Destroy(gameObject);
        }
    }

    internal void SaveData() // serializes save data down to binary
    {
        BinaryFormatter BinForm = new BinaryFormatter(); // creates bin formatter
        FileStream file = File.Create(Application.persistentDataPath + "/gameInfo.dat"); // creates File
        gameData data = new gameData(); // creates container for data
        data.gDatHighScore = dManHighScore; 
        BinForm.Serialize(file, data); // serializes
        file.Close(); // closes file
    }

    internal void LoadData()
    {
        if (File.Exists (Application.persistentDataPath + "/gameInfo.dat"))
        {
            BinaryFormatter BinForm = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gameInfo.dat", FileMode.Open);
            gameData data = (gameData)BinForm.Deserialize(file);
            file.Close();
            dManHighScore = data.gDatHighScore;           
        }
    }
}

[Serializable]
class gameData
{
    internal int gDatHighScore;
}

