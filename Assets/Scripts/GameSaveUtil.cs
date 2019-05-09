#define USE_CLOUD_SAVE

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;

public class GameSaveUtil
{
	private const string META_DATA_KEY = "TOTAL_PLAY_TIME";
    private static string SaveFolderPath = Application.persistentDataPath + "/gamedata";
	private static Hashtable mDataHashtable = new Hashtable();

    public static void SetValue(string key, object value)
	{
		if(mDataHashtable.ContainsKey(key))
			mDataHashtable[key] = value;
		else
			mDataHashtable.Add(key, value);
	}

	public static void SetValueAndSave(string id, string key, object value)
	{
		SetValue(key, value);
		Save(id);
      
	}

	public static object GetValue(string key)
	{
		if(mDataHashtable.ContainsKey(key))
			return mDataHashtable[key];
        Debug.Log("data keys" + mDataHashtable);
		return null;
	}

	public static void Save(string id)
	{
        if (string.IsNullOrEmpty(id))
        {
            id = "offline";
        }
		if(mDataHashtable.ContainsKey(META_DATA_KEY))
		{
			float totalPlayTime = (float)mDataHashtable[META_DATA_KEY];
			totalPlayTime += Time.realtimeSinceStartup;
			mDataHashtable[META_DATA_KEY] = totalPlayTime;
            Debug.Log("total playtime" + mDataHashtable);
		}
		else
			mDataHashtable.Add(META_DATA_KEY, Time.realtimeSinceStartup);
        Debug.Log("real time" + mDataHashtable);

		BinaryFormatter bf = new BinaryFormatter();
        if (!Directory.Exists(SaveFolderPath))
        {
            Directory.CreateDirectory(SaveFolderPath);
        }

        string filePath = SaveFolderPath + "/" + id + ".dat";
        Debug.Log("Filepath: " + filePath);
        FileStream file = File.Open(filePath, FileMode.OpenOrCreate);
        try
        {
            bf.Serialize(file, mDataHashtable);
        }
        catch (Exception ex)
        {
            Debug.Log("Sad dude :( " + ex.Message);
        }
        file.Close();

#if USE_CLOUD_SAVE
        MemoryStream stream = new MemoryStream();
		bf.Serialize(stream, mDataHashtable);
		AvGameServices.SaveData(stream.ToArray());
#endif
    }

    public static void Load(string id)
	{
        if (string.IsNullOrEmpty(id))
        {
            id = "offline";
        }
        Hashtable cloudSave = null;
		Hashtable localSave = null;
		BinaryFormatter bf = new BinaryFormatter();
#if USE_CLOUD_SAVE
		byte[] savedData = AvGameServices.GetSavedData();
		if(savedData != null && savedData.Length > 0)
		{
			MemoryStream stream = new MemoryStream();
			stream.Write(savedData, 0, savedData.Length);
			stream.Seek(0, SeekOrigin.Begin);
			cloudSave = (Hashtable)bf.Deserialize(stream);
		}
#endif
        if (!Directory.Exists(SaveFolderPath))
        {
            Debug.Log("file not found");
            Directory.CreateDirectory(SaveFolderPath);
        }


        string filePath = SaveFolderPath + "/" + id + ".dat";

        Debug.Log("____Filepath = " + filePath);
        if (System.IO.File.Exists(filePath))
        {
            Debug.Log("file found");
            FileStream file = File.Open(filePath, FileMode.Open);
            localSave = (Hashtable)bf.Deserialize(file);

            Debug.Log("localdata" + localSave);
            file.Close();

        }
            mDataHashtable = ResolveConflict(cloudSave, localSave);
            Debug.Log("data save in local" + mDataHashtable);
        
    }
    private static Hashtable ResolveConflict(Hashtable cloudSave, Hashtable localSave)
	{
        Hashtable resolvedSaveData = null;

		float cloudSavePlayTime = 0;
		if(cloudSave != null && cloudSave.ContainsKey(META_DATA_KEY))
			cloudSavePlayTime = (float)cloudSave[META_DATA_KEY];

		float localSavePlayTime = 0;
		if(localSave!= null && localSave.ContainsKey(META_DATA_KEY))
			localSavePlayTime = (float)localSave[META_DATA_KEY];

		resolvedSaveData = localSave;
#if USE_CLOUD_SAVE

		if (resolvedSaveData == null)
		{
			resolvedSaveData = new Hashtable ();

			Debug.Log ("No save found");
		}
		else
		{
			if (resolvedSaveData == localSave)
				Debug.Log ("Using local save");
			else
				Debug.Log ("Using cloud save");
		}
#endif
		return resolvedSaveData;
	}
}
