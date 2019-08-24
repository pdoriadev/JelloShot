using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
#if UNITY_EDITOR
using UnityEditor;
#endif
/// <summary>
/// Saves and accesses game score data. Serializes save data down to binary.
/// </summary>
public class DataManagement : MonoBehaviour
{
    public static DataManagement instance;

    [SerializeField]
    public int dManHighScore;

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

    public void SaveData() // serializes save data down to binary
    {
        BinaryFormatter BinForm = new BinaryFormatter(); // creates bin formatter
        FileStream file = File.Create(Application.persistentDataPath + "/gameInfo.dat"); // creates File
        gameData data = new gameData(); // creates container for data
        data.savedHighScore = dManHighScore; 
        BinForm.Serialize(file, data); // serializes
        file.Close(); // closes file
    }

    public void LoadData()
    {
        if (File.Exists (Application.persistentDataPath + "/gameInfo.dat"))
        {
            BinaryFormatter BinForm = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gameInfo.dat", FileMode.Open);
            gameData data = (gameData)BinForm.Deserialize(file);
            file.Close();
            dManHighScore = data.savedHighScore;           
        }
    }

    [SerializeField]
    public bool _ShouldClearHighScoreKeyOne;
    [SerializeField]
    public bool _ShouldClearHighScoreKeyTwo;
    public void ResetHighScore()
    {
        dManHighScore = 0;
        SaveData();
        _ShouldClearHighScoreKeyOne = false;
        _ShouldClearHighScoreKeyTwo = false;
        Debug.Log("You just wiped the high score. New score is: " + dManHighScore);
    }
}

[Serializable]
class gameData
{
    public int savedHighScore;
}

#if UNITY_EDITOR
[CustomEditor(typeof(DataManagement))]
public class DataClearer : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // for other non-HideInInspector fields

        DataManagement _DataM = (DataManagement)target;
        if (_DataM._ShouldClearHighScoreKeyOne && _DataM._ShouldClearHighScoreKeyTwo)
        {
            _DataM.ResetHighScore();
        }
    }
}
#endif
