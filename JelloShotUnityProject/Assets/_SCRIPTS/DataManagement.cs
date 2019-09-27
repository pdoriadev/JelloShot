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
    public int dManDiffKOScore, dManKOScore;

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

    public void SaveData() // Saves high score then serializes it down to binary. 
    {
        FileStream file = File.Create(Application.persistentDataPath + "/gameInfo.dat"); 
        gameData data = new gameData(); 
        data.savedDiffTimesKOScore = dManDiffKOScore;
        data.savedHighKOScore = dManKOScore;

        BinaryFormatter BinForm = new BinaryFormatter();
        BinForm.Serialize(file, data); 
        file.Close(); 
    }

    public void LoadData()
    {
        if (File.Exists (Application.persistentDataPath + "/gameInfo.dat"))
        {
            FileStream file = File.Open(Application.persistentDataPath + "/gameInfo.dat", FileMode.Open);

            BinaryFormatter BinForm = new BinaryFormatter();
            gameData data = (gameData)BinForm.Deserialize(file);
            file.Close();
            dManDiffKOScore = data.savedDiffTimesKOScore;
            dManKOScore = data.savedHighKOScore;
        }
    }

    [SerializeField]
    public bool _ShouldClearHighScoreKeyOne;
    [SerializeField]
    public bool _ShouldClearHighScoreKeyTwo;
    public void ResetHighScore()
    {
        if (File.Exists(Application.persistentDataPath + "/gameInfo.dat"))
        {
            BinaryFormatter BinForm = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gameInfo.dat", FileMode.Open);
            gameData data = (gameData)BinForm.Deserialize(file);
            Debug.Log("Previous Difficulty times KO score was: " + data.savedDiffTimesKOScore);
            Debug.Log("Previous KO Score was" + data.savedHighKOScore);
            data.savedDiffTimesKOScore = 0;
            data.savedHighKOScore = 0;
            file.Close();

            SaveData();
            Debug.Log("You just wiped the high score. New score is: " + dManDiffKOScore);
        }       
    }
}

[Serializable]
class gameData
{
    public int savedDiffTimesKOScore;
    public int savedHighKOScore;
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
