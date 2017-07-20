using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayerPrefs : MonoBehaviour
{
    public int KEY_LEVEL_TYPE;
    public int KEY_GEAR_TYPE;
    public string KEY_LANGUAGE;
    public int KEY_HIGHSCORE;
    public int KEY_LIVES;
    public int KEY_FBSHARE;
    public string KEY_DATETIME;
    public int KEY_TAPTOPLAY;
    public int KEY_ENEMY_KILLS;
    public int KEY_DAYS;
    public int KEY_LADDUS_COLLECTED_COUNT;
    public int KEY_MAPS_UNLOCKED;
    public int KEY_SILVER_UNLOCKED;
    public int KEY_GOLD_UNLOCKED;
    public int KEY_GADA_UNLOCKED;

    void Start()
    {
        GetPrefs();
    }

    void GetPrefs()
    {
        KEY_LEVEL_TYPE = PlayerPrefs.GetInt(GameData.KEY_LEVEL_TYPE, 0);
        KEY_GEAR_TYPE = PlayerPrefs.GetInt(GameData.KEY_GEAR_TYPE, 0);
        KEY_LANGUAGE = PlayerPrefs.GetString(GameData.KEY_LANGUAGE);
        KEY_HIGHSCORE = PlayerPrefs.GetInt(GameData.KEY_HIGHSCORE, 0);
        KEY_DATETIME = PlayerPrefs.GetString(GameData.KEY_DATETIME, "");
        KEY_ENEMY_KILLS = PlayerPrefs.GetInt(GameData.KEY_ENEMY_KILLS, 0);
        KEY_DAYS = PlayerPrefs.GetInt(GameData.KEY_DAYS, 0);
        KEY_LADDUS_COLLECTED_COUNT = PlayerPrefs.GetInt(GameData.KEY_LADDUS_COLLECTED_COUNT, 0);
        KEY_MAPS_UNLOCKED = PlayerPrefs.GetInt(GameData.KEY_MAPS_UNLOCKED, 0);
        KEY_SILVER_UNLOCKED = PlayerPrefs.GetInt(GameData.KEY_SILVER_UNLOCKED, 0);
        KEY_GOLD_UNLOCKED = PlayerPrefs.GetInt(GameData.KEY_GOLD_UNLOCKED, 0);
        KEY_GADA_UNLOCKED = PlayerPrefs.GetInt(GameData.KEY_GADA_UNLOCKED, 0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            GetPrefs();
        }
    }
}
