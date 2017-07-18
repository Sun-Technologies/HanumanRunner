//#define USE_CLOUD_SAVE

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;

public class GameSaveUtil
{
	private const string META_DATA_KEY = "TOTAL_PLAY_TIME";
	private static Dictionary<string, object> mDataDictionary = new Dictionary<string, object>();

	public static void SetValue(string key, object value)
	{
		if(mDataDictionary.ContainsKey(key))
			mDataDictionary[key] = value;
		else
			mDataDictionary.Add(key, value);
	}

	public static void SetValueAndSave(string key, object value)
	{
		SetValue(key, value);
		Save();
	}

	public static object GetValue(string key)
	{
		if(mDataDictionary.ContainsKey(key))
			return mDataDictionary[key];

		return null;
	}

	public static void Save()
	{
		if(mDataDictionary.ContainsKey(META_DATA_KEY))
		{
			float totalPlayTime = (float)mDataDictionary[META_DATA_KEY];
			totalPlayTime += Time.realtimeSinceStartup;
			mDataDictionary[META_DATA_KEY] = totalPlayTime;
		}
		else
			mDataDictionary.Add(META_DATA_KEY, Time.realtimeSinceStartup);

		BinaryFormatter bf = new BinaryFormatter();
		string filePath = Application.persistentDataPath + "/gamedata.dat";
		FileStream file = File.Open(filePath, FileMode.OpenOrCreate);
		bf.Serialize(file, mDataDictionary);
		file.Close();

#if USE_CLOUD_SAVE
		MemoryStream stream = new MemoryStream();
		bf.Serialize(stream, mDataDictionary);
		AvGameServices.SaveData(stream.ToArray());
#endif
	}

	public static void Load()
	{
		Dictionary<string, object> cloudSave = null;
		Dictionary<string, object> localSave = null;
		BinaryFormatter bf = new BinaryFormatter();
#if USE_CLOUD_SAVE
		byte[] savedData = AvGameServices.GetSavedData();
		if(savedData != null && savedData.Length > 0)
		{
			MemoryStream stream = new MemoryStream();
			stream.Write(savedData, 0, savedData.Length);
			stream.Seek(0, SeekOrigin.Begin);
			cloudSave = (Dictionary<string, object>)bf.Deserialize(stream);
		}
#endif
		string filePath = Application.persistentDataPath + "/gamedata.dat";
		if(System.IO.File.Exists(filePath))
		{
			FileStream file = File.Open(filePath, FileMode.Open);
			localSave = (Dictionary<string, object>)bf.Deserialize(file);
			file.Close();
		}

		mDataDictionary = ResolveConflict(cloudSave, localSave);
	}

	private static Dictionary<string, object> ResolveConflict(Dictionary<string, object> cloudSave, Dictionary<string, object> localSave)
	{
		Dictionary<string, object> resolvedSaveData = null;

		float cloudSavePlayTime = 0;
		if(cloudSave != null && cloudSave.ContainsKey(META_DATA_KEY))
			cloudSavePlayTime = (float)cloudSave[META_DATA_KEY];

		float localSavePlayTime = 0;
		if(localSave!= null && localSave.ContainsKey(META_DATA_KEY))
			localSavePlayTime = (float)localSave[META_DATA_KEY];

		resolvedSaveData = (cloudSavePlayTime > localSavePlayTime) ? cloudSave : localSave;
#if USE_CLOUD_SAVE

		if (resolvedSaveData == null)
		{
			resolvedSaveData = new Dictionary<string, object> ();

			AvDebug.Log ("No save found");
		}
		else
		{
			if (resolvedSaveData == localSave)
				AvDebug.Log ("Using local save");
			else
				AvDebug.Log ("Using cloud save");
		}
#endif
		return resolvedSaveData;
	}
}
