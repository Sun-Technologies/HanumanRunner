using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SocialPlatforms;
using System.Collections;
using System.Collections.Generic;


#pragma warning disable 0162

public delegate void RewardedAdResult(bool shown);
public delegate void InitCompleteCallback();

public class AvGameServices
{
    #region variables
    private static RewardedAdResult mRewardedAdCallback = null;
    #endregion

    #region Properties
  /*  public static bool pIsReady
    {
        get
        {
#if UNITY_EDITOR
            return true;
#else
#if UNITY_ANDROID
       		return GooglePlayServices.pIsReady;
#elif UNITY_IOS
			return iOSGameServices.pIsReady;
#endif
#endif
        }
    }*/
    #endregion

    #region class specific funtions
    public static void Init()
    {
#if !UNITY_EDITOR
#if UNITY_ANDROID
		GooglePlayServices.Init(OnInitComplete);
#elif UNITY_IOS
		iOSGameServices.Init(OnInitComplete);
#endif
#endif
    }

    private static void OnInitComplete()
    {
        if (Social.localUser.authenticated)
        {
            Debug.Log("UserId = " + Social.localUser.id);
            PlayerPrefsStorage.UserId = Social.localUser.id;
            GameSaveUtil.Load(Social.localUser.id);
        }
    }

    public static void SubmitScore(int score)
    {
#if UNITY_ANDROID
        GooglePlayServices.ReportScore(score);
#elif UNITY_IOS
		iOSGameServices.ReportScore(score);
#endif
    }

    public static void ShowLeaderBoard()
    {
        if (Social.localUser.authenticated)
        {
#if UNITY_ANDROID
            GooglePlayServices.ShowLeaderboard();
#elif UNITY_IOS
			iOSGameServices.ShowLeaderboard ();
#endif
        }
        else
            Init();
    }

    public static void ShowAchievements()
    {
        if (Social.localUser.authenticated)
            Social.ShowAchievementsUI();
        else
            Init();
    }

    public static void UnlockAchievement(string achievementID)
    {
        if (Social.localUser.authenticated)
            Social.ReportProgress(achievementID, 100.0f, (bool success) => { });
    }

    public static void IncrementAchievement(string achievementID, int steps, double progress)
    {
#if UNITY_ANDROID
        GooglePlayServices.IncrementAchievement(achievementID, steps);
#elif UNITY_IOS
		iOSGameServices.UnlockAchievement(achievementID, progress);
#endif
    }

    public static bool IsIncrementalAchievement(string achievementID)
    {
#if UNITY_ANDROID
        //		return true;
        return GooglePlayServices.IsIncrementalAchievement(achievementID);
#elif UNITY_IOS
		return false;
#endif
        return false;
    }

    //    public static void IncrementAchievement(string achievementID, int incrementValue)
    //    {
    //#if UNITY_ANDROID
    //    	GooglePlayServices.IncrementAchievement(achievementID, incrementValue);
    //#elif UNITY_IOS
    //		
    //#endif
    //    }

    public static bool IsAchievementUnlocked(string achievementID)
    {
#if UNITY_ANDROID
        return GooglePlayServices.IsAchievementUnlocked(achievementID);
#elif UNITY_IOS
		return iOSGameServices.IsAchievementUnlocked(achievementID);
#endif
        return false;
    }

    public static double GetAchievementProgress(string achievementID)
    {
#if UNITY_ANDROID
        return GooglePlayServices.GetAchievementProgress(achievementID);
#elif UNITY_IOS
		return iOSGameServices.GetAchievementProgress(achievementID);
#endif
        return 0;
    }

    public static byte[] GetSavedData()
    {
#if ENABLE_CLOUD_SAVE
#if UNITY_ANDROID
		return GooglePlayServices.GetSavedData();
#elif UNITY_IOS
		return iOSGameServices.GetSavedData();
#endif
#endif
        return null;
    }

    public static void SaveData(byte[] saveData)
    {
#if ENABLE_CLOUD_SAVE
#if UNITY_ANDROID
		GooglePlayServices.SaveGameData(saveData);
#elif UNITY_IOS
		iOSGameServices.SaveGameData(saveData);
#endif
#endif
    }

    //Watch video Ad to continue the game
    public static void ShowRewardedAd(RewardedAdResult callback)
    {
//        mRewardedAdCallback = callback;
//#if UNITY_EDITOR
//        var options = new ShowOptions { resultCallback = HandleRewardedAdResult };
//        Advertisement.Show("rewardedVideo", options);
//#else
//        if (Advertisement.IsReady("rewardedVideo"))
//        {
//			var options = new ShowOptions { resultCallback = HandleRewardedAdResult };
//            Advertisement.Show("rewardedVideo", options);
//        }
//        else
//        {
//            Utilities.ShowNotificationDB("", "No Ads Found", OnNoAdsFound);
//            Debug.Log("No Ads Found");
//        }
//#endif
    }

   // private static void HandleRewardedAdResult(ShowResult result)
   // {
        //switch (result)
        //{
        //    case ShowResult.Finished:
        //        if (mRewardedAdCallback != null)
        //            mRewardedAdCallback(true);
        //        break;
        //    case ShowResult.Skipped:
        //    case ShowResult.Failed:
        //        if (mRewardedAdCallback != null)
        //            mRewardedAdCallback(false);
        //        break;
        //}

        //mRewardedAdCallback = null;
   // }

    private static void OnNoAdsFound()
    {
        if (mRewardedAdCallback != null)
        {
            mRewardedAdCallback(false);
            mRewardedAdCallback = null;
        }
    }
    #endregion
}
