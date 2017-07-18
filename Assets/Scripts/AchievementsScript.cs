using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class AchievementsScript : MonoBehaviour
{

    const string ACHIEVEMENT_100LADDUS = "CgkImt_Vrr0QEAIQAg";
    const string ACHIEVEMENT_500LADDUS= "CgkImt_Vrr0QEAIQAw";
    const string ACHIEVEMENT_1000LADDUS = "CgkImt_Vrr0QEAIQBA";
    const string ACHIEVEMENT_UNLOCK_ALLMAPS = "CgkImt_Vrr0QEAIQBQ";
    const string ACHIEVEMENT_UNLOCK_SILVER_ARMOR = "CgkImt_Vrr0QEAIQBg";
    const string ACHIEVEMENT_UNLOCK_GOLD_ARMOR = "CgkImt_Vrr0QEAIQBw";
    const string ACHIEVEMENT_KILL10ENEMIES = "CgkImt_Vrr0QEAIQCA";
    const string ACHIEVEMENT_KILL50ENEMIES = "CgkImt_Vrr0QEAIQCg";
    const string ACHIEVEMENT_KILL100ENEMIES = "CgkImt_Vrr0QEAIQCg";


    public const string STR_COLLECT_100_LADDUS = "Collect 100 Laddus";
    public const string STR_COLLECT_500_LADDUS = "Collect 500 Laddus";
    public const string STR_COLLECT_1000_LADDUS = "Collect 1000 Laddus";
    public const string STR_UNLOCK_ALL_MAPS = "Unlock all maps";
    public const string STR_UNLOCK_SILVER_ARMOR = "Unlock Silver Arrmor";
    public const string STR_UNLOCK_GOLD_ARMOR = "Unlock Gold Armor";
    public const string STR_KILL_10_ENEMIES = "Kill 10 enemies";
    public const string STR_KILL_50_ENEMIES = "Kill 50 enemies";
    public const string STR_KILL_100_ENEMIES = "Kill 100 enemies";

    static int LaddusCount = 0;
    static int MapsCount = 0;
    static int SilverUnlocked = 0;
    static int GoldUnlocked = 0;
    static int EnemiesKilledCount = 0;

    void Start()
    {
        GetAchievementsCountsFromDisk();
    }

    public static void SaveDataAndUnlockAchievements(int laddus = 0, int maps = 0, int silver = 0, int gold = 0, int enemiesKilled = 0)
    {
        Debug.Log("Saving data:");
        GetAchievementsCountsFromDisk();
        int _laddus = LaddusCount + laddus;
        int _maps = MapsCount + maps;
        int _enemiesKilled = EnemiesKilledCount + enemiesKilled;

        PlayerPrefsStorage.SaveData(GameData.KEY_LADDUS_COLLECTED_COUNT, _laddus);
        PlayerPrefsStorage.SaveData(GameData.KEY_MAPS_UNLOCKED, _maps);
        PlayerPrefsStorage.SaveData(GameData.KEY_SILVER_UNLOCKED, silver);
        PlayerPrefsStorage.SaveData(GameData.KEY_GOLD_UNLOCKED, gold);
        PlayerPrefsStorage.SaveData(GameData.KEY_ENEMY_KILLS, _enemiesKilled);
        UnlockAchievements();
    }

    static void GetAchievementsCountsFromDisk()
    {
        LaddusCount = PlayerPrefsStorage.GetIntData(GameData.KEY_LADDUS_COLLECTED_COUNT, 0);
        MapsCount = PlayerPrefsStorage.GetIntData(GameData.KEY_MAPS_UNLOCKED, 0);
        SilverUnlocked = PlayerPrefsStorage.GetIntData(GameData.KEY_SILVER_UNLOCKED, 0);
        GoldUnlocked = PlayerPrefsStorage.GetIntData(GameData.KEY_GOLD_UNLOCKED, 0);
        EnemiesKilledCount = PlayerPrefsStorage.GetIntData(GameData.KEY_ENEMY_KILLS, 0);
    }

    public static void UnlockAchievements()
    {
        GetAchievementsCountsFromDisk();
        Debug.Log(string.Format("Data: LaddusCount = {0}, MapsCount = {1}, SilverUnlocked = {2}, GoldUnlocked = {3}, EnemyKills = {4}", LaddusCount, MapsCount, SilverUnlocked, GoldUnlocked, EnemiesKilledCount));
        Debug.Log("Attempting to unlock achievents");
        if (LaddusCount >= 100)
        {
            if (!AvGameServices.IsAchievementUnlocked(ACHIEVEMENT_100LADDUS))
            {
                AvGameServices.UnlockAchievement(ACHIEVEMENT_100LADDUS);
                Analytics.CustomEvent("Unlocked achievement: " + STR_COLLECT_100_LADDUS);
                Debug.Log("Unlocked achievement: " + STR_COLLECT_100_LADDUS);
            }
        }

        if (LaddusCount >= 500)
        {
            if (!AvGameServices.IsAchievementUnlocked(ACHIEVEMENT_500LADDUS))
            {
                AvGameServices.UnlockAchievement(ACHIEVEMENT_500LADDUS);
                Analytics.CustomEvent("Unlocked achievement: " + STR_COLLECT_500_LADDUS);
                Debug.Log("Unlocked achievement: " + STR_COLLECT_500_LADDUS);
            }
        }

        if (LaddusCount >= 1000)
        {
            if (!AvGameServices.IsAchievementUnlocked(ACHIEVEMENT_1000LADDUS))
            {
                AvGameServices.UnlockAchievement(ACHIEVEMENT_1000LADDUS);
                Analytics.CustomEvent("Unlocked achievement: " + STR_COLLECT_1000_LADDUS);
                Debug.Log("Unlocked achievement: " + STR_COLLECT_1000_LADDUS);
            }
        }

        if (MapsCount == 2)
        {
            if (!AvGameServices.IsAchievementUnlocked(ACHIEVEMENT_UNLOCK_ALLMAPS))
            {
                AvGameServices.UnlockAchievement(ACHIEVEMENT_UNLOCK_ALLMAPS);
                Analytics.CustomEvent("Unlocked achievement: " + STR_UNLOCK_ALL_MAPS);
                Debug.Log("Unlocked achievement: " + STR_UNLOCK_ALL_MAPS);
            }
        }

        if (SilverUnlocked == 1)
        {
            if (!AvGameServices.IsAchievementUnlocked(ACHIEVEMENT_UNLOCK_SILVER_ARMOR))
            {
                AvGameServices.UnlockAchievement(ACHIEVEMENT_UNLOCK_SILVER_ARMOR);
                Analytics.CustomEvent("Unlocked achievement: " + STR_UNLOCK_SILVER_ARMOR);
                Debug.Log("Unlocked achievement: " + STR_UNLOCK_SILVER_ARMOR);
            }
        }

        if (GoldUnlocked == 1)
        {
            if (!AvGameServices.IsAchievementUnlocked(ACHIEVEMENT_UNLOCK_GOLD_ARMOR))
            {
                AvGameServices.UnlockAchievement(ACHIEVEMENT_UNLOCK_GOLD_ARMOR);
                Analytics.CustomEvent("Unlocked achievement: " + STR_UNLOCK_GOLD_ARMOR);
                Debug.Log("Unlocked achievement: " + STR_UNLOCK_GOLD_ARMOR);
            }
        }

        if (EnemiesKilledCount >= 10)
        {
            if (!AvGameServices.IsAchievementUnlocked(ACHIEVEMENT_KILL10ENEMIES))
            {
                AvGameServices.UnlockAchievement(ACHIEVEMENT_KILL10ENEMIES);
                Analytics.CustomEvent("Unlocked achievement: " + STR_KILL_10_ENEMIES);
                Debug.Log("Unlocked achievement: " + STR_KILL_10_ENEMIES);
            }
        }

        if (EnemiesKilledCount >= 50)
        {
            if (!AvGameServices.IsAchievementUnlocked(ACHIEVEMENT_KILL50ENEMIES))
            {
                AvGameServices.UnlockAchievement(ACHIEVEMENT_KILL50ENEMIES);
                Analytics.CustomEvent("Unlocked achievement: " + STR_KILL_50_ENEMIES);
                Debug.Log("Unlocked achievement: " + STR_KILL_50_ENEMIES);
            }
        }

        if (EnemiesKilledCount >= 100)
        {
            if (!AvGameServices.IsAchievementUnlocked(ACHIEVEMENT_KILL100ENEMIES))
            {
                AvGameServices.UnlockAchievement(ACHIEVEMENT_KILL100ENEMIES);
                Analytics.CustomEvent("Unlocked achievement: " + STR_KILL_100_ENEMIES);
                Debug.Log("Unlocked achievement: " + STR_KILL_100_ENEMIES);
            }
        }
    }
}