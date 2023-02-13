using System.Collections.Generic;
using UnityEngine;

public class DataLoader : MonoBehaviour
{
    private static DataLoader _instance = null;

    private DataLoader()
    {
        _instance = this;
    }

    public static DataLoader GetInstance()
    {
        if (_instance == null)
        {
            _instance = new DataLoader();
        }
        return _instance;
    }

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void LoadData()
    {

    }

    public void InitDefaultSaveData()
    {
    }

    public void InitSaveData()
    {
        Save save = Save.GetInstance();
        Save.UISaveData UIData = save.Load<Save.UISaveData>(SaveDataType.UIData);

    }
}
public class LevelList
{
    public int chapterId;
    public List<int> levelId;
    public List<int> levelEvaluate;
    public List<bool> ifPass;

    public LevelList(int chapter)
    {
        chapterId = chapter;
        levelId = new List<int>();
    }
}
