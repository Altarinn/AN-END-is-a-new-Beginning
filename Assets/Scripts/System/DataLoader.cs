using System.Collections.Generic;
using UnityEngine;

public class DataLoader : SingletonMonoBehaviour<DataLoader>
{
    public static DataLoader GetInstance() => Instance;

    protected override void Awake()
    {
        base.Awake();
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
        //Save save = Save.GetInstance();
        //Save.UISaveData UIData = save.Load<Save.UISaveData>(SaveDataType.UIData);

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
