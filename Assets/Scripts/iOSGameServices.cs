using UnityEngine;
using UnityEngine.SocialPlatforms;
using System.Collections;
using System.Collections.Generic;
#if UNITY_IOS
using UnityEngine.SocialPlatforms.GameCenter;
#endif

public class iOSGameServices
{
#region Variables
    private const string				LEADERBOARD_ID = "Hanuman_Leaderboard";
#if ENABLE_CLOUD_SAVE
	private const string				ICLOUD_SAVEDATA_KEY = "iCloud_Save_Data";
#endif
	private static bool					mIsReady = false;

	private static List<IAchievement>	mAchievementsList;
    private static InitCompleteCallback	mCallback;
#endregion

#region Properties
	public static bool			pIsReady
	{
		get { return mIsReady; }
	}
#endregion

#region Class specific functions
	public static void Init(InitCompleteCallback callback = null)
    {
		mCallback = callback;
    	mAchievementsList = new List<IAchievement>();
        Login();
#if UNITY_IOS
        GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);
#endif
    }

    private static void Login()
    {
		if(!Social.localUser.authenticated)
    	{
			Social.localUser.Authenticate((bool success) => {
	                if (success) 
	                {
						Debug.Log("We're signed in! Welcome " + Social.localUser.userName);
	                    LoadAchievements();
	                } 
	                else
	                {
						Debug.Log("Oh... we're not signed in.");
						mIsReady = true;
	                }});
        }
        else
		{
			Debug.Log("We're signed in! Welcome " + Social.localUser.userName);
	    	LoadAchievements();
        }
    }

	public static void UnlockAchievement(string achievementID, double progress = 100.0f)
	{
        if(Social.localUser.authenticated)
		    Social.ReportProgress(achievementID, progress, (bool success) => {});
	}

	private static void LoadAchievements()
	{
		// Loads list of completed or partially completed achievements
		Social.LoadAchievements (achievements => {
			mAchievementsList = new List<IAchievement>(achievements);
			if(mCallback != null)
				mCallback();
			mIsReady = true;
		});
	}

	public static void ShowLeaderboard()
	{
		if(Social.localUser.authenticated)
			Social.Active.ShowLeaderboardUI();
	}

    public static void ReportScore(int highScore)
    {
        if (Social.localUser.authenticated)
            Social.ReportScore(highScore, LEADERBOARD_ID, (bool success) =>{});
    }

	public static bool IsAchievementUnlocked(string achievementID)
	{
        if (Social.localUser.authenticated)
        {
            IAchievement achievement = mAchievementsList.Find(x => (x.id == achievementID));
            if (achievement != null)
                return achievement.completed;
        }

		return false;
	}

	public static double GetAchievementProgress(string achievementID)
	{
		IAchievement achievement = mAchievementsList.Find(x => (x.id == achievementID));
		if(achievement != null)
			return achievement.percentCompleted;

		return 0;
	}

#if ENABLE_CLOUD_SAVE
	public static byte[] GetSavedData()
    {
		if (Application.platform == RuntimePlatform.IPhonePlayer)// && iCloudWrapper.IsICloudAvailable()) 
		{
			string encodedString = iCloudWrapper.GetString (ICLOUD_SAVEDATA_KEY);
			if(!string.IsNullOrEmpty(encodedString))
				return System.Convert.FromBase64String (encodedString);
		}

		return null;
    }

    public static void SaveGameData(byte[] saveData)
    {
		if (Application.platform == RuntimePlatform.IPhonePlayer)// && iCloudWrapper.IsICloudAvailable())
		{
			string encodedString = System.Convert.ToBase64String (saveData);
			if(!string.IsNullOrEmpty(encodedString))
				iCloudWrapper.SetString (ICLOUD_SAVEDATA_KEY, encodedString);
		}
    }
#endif //ENABLE_CLOUD_SAVE
   
#endregion
}
