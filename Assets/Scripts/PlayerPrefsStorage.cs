using UnityEngine;
using System;

public class PlayerPrefsStorage : MonoBehaviour
{
    public static string UserId = string.Empty;
    public static string GetStringData(string key)
    {
        //return PlayerPrefs.GetString(key);
        string str = string.Empty;
        try
        {
            str = GameSaveUtil.GetValue(key).ToString();
        }
        catch (Exception ex)
        {
            Debug.Log("Exception: " + ex.Message);
        }
        return str;
    }

    public static int GetIntData(string key, int defaultValue)
    {
        //return PlayerPrefs.GetInt(key, defaultValue);
        return Convert.ToInt32(GameSaveUtil.GetValue(key));
    }

    public static void SaveData(string key, object data)
    {
        GameSaveUtil.SetValueAndSave(UserId, key, data);
    }

    public static void ClearLocalStorageData(bool ClearAllData)
    {
        if (ClearAllData)
        {
            PlayerPrefs.DeleteAll();
        }
    }

    public static void ClearLocalStorageData(string keyToClear)
    {
        PlayerPrefs.DeleteKey(keyToClear);
    }

    static void StorageLog(string data)
    {
#if UNITY_EDITOR
        Debug.Log(data);
#endif
    }
}