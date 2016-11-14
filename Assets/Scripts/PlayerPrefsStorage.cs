using UnityEngine;
using System;

public class PlayerPrefsStorage : MonoBehaviour
{
    public static string GetStringData(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            return PlayerPrefs.GetString(key);
        }
        else
        {
            StorageLog(string.Format("THE QUERIED KEY {0} DOES NOT EXIST!", key));
            return null;
        }
    }

    public static int GetIntData(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            return PlayerPrefs.GetInt(key);
        }
        else
        {
            StorageLog(string.Format("THE QUERIED KEY {0} DOES NOT EXIST!", key));
            return 0;
        }
    }

    public static void SaveData(string key, string data)
    {
        try
        {
            PlayerPrefs.SetString(key, data);
            PlayerPrefs.Save();
        }
        catch (Exception ex)
        {
            StorageLog(ex.Message);
        }
    }

    public static void SaveData(string key, int data)
    {
        try
        {
            PlayerPrefs.SetInt(key, data);
            PlayerPrefs.Save();
        }
        catch (Exception ex)
        {
            StorageLog(ex.Message);
        }
    }

    public static void SaveData(string key, float data)
    {
        try
        {
            PlayerPrefs.SetFloat(key, data);
            PlayerPrefs.Save();
        }
        catch (Exception ex)
        {
            StorageLog(ex.Message);
        }
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