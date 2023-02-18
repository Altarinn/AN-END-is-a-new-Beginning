//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System.IO;
//using System.Runtime.Serialization.Formatters.Binary;

//public class Save
//{
//    private static Save _instance = null;

//    private Save()
//    {
//        _instance = this;
//    }

//    public static Save GetInstance()
//    {
//        if (_instance == null)
//        {
//            _instance = new Save();
//        }
//        return _instance;
//    }

//    public void SaveFunc()
//    {
//        foreach (SaveDataType i in System.Enum.GetValues(typeof(SaveDataType)))
//            SaveFunc(i);
//    }

//    public void SaveFunc(SaveDataType saveDataType)
//    {
//        GameController gameController = GameController.GetInstance();
//        BinaryFormatter bf = new BinaryFormatter();
//        FileStream file = File.Create(TransforToSaveDataPath(saveDataType)); ;
//        switch (saveDataType)
//        {
//            case SaveDataType.UIData:
//                UISaveData UIdata = new UISaveData();
//                UIdata.logoutTime = System.DateTime.Now.Ticks / 10000000;
//                bf.Serialize(file, UIdata);
//                break;
//        }
//        file.Close();
//        Debug.Log(saveDataType.ToString() + "成功存档至" + TransforToSaveDataPath(saveDataType));
//    }

//    public T Load<T>(SaveDataType saveDataType)
//    {
//        BinaryFormatter bf = new BinaryFormatter();
//        FileStream file = File.Open(TransforToSaveDataPath(saveDataType), FileMode.Open);
//        T _data = (T)bf.Deserialize(file);
//        file.Close();
//        Debug.Log("读档成功");
//        return _data;
//    }

//    public List<bool> ClearSave()
//    {
//        List<bool> clearResult = new List<bool>();
//        foreach (SaveDataType i in System.Enum.GetValues(typeof(SaveDataType)))
//        {
//            if (ifSaveDataExists(i))
//            {
//                File.Delete(TransforToSaveDataPath(i));
//                clearResult.Add(true);
//            }
//            clearResult.Add(false);
//        }
//        return clearResult;
//    }

//    public bool ClearSave(SaveDataType saveDataType)
//    {
//        if (ifSaveDataExists(saveDataType))
//        {
//            File.Delete(TransforToSaveDataPath(saveDataType));
//            return true;
//        }
//        return false;
//    }

//    public bool FirstLog()
//    {
//        foreach (SaveDataType i in System.Enum.GetValues(typeof(SaveDataType)))
//        {
//            if (ifSaveDataExists(i))
//                return false;
//        }
//        return true;
//    }

//    public string TransforToSaveDataPath(SaveDataType saveDataType)
//    {
//        return Application.persistentDataPath + "/" + System.Enum.GetName(typeof(SaveDataType), saveDataType) + ".save";
//    }

//    private bool ifSaveDataExists(SaveDataType saveDataType)
//    {
//        return File.Exists(TransforToSaveDataPath(saveDataType));
//    }

//    [System.Serializable]
//    public class UISaveData
//    {
//        public double logoutTime;       //上次下线的时间
//    }
//}

//public enum SaveDataType
//{
//    /// <summary>
//    /// UI存档数据(test)
//    /// </summary>
//    UIData,
//}
