#if UNITY_ANDROID
using UnityEngine;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine.SocialPlatforms;
using System;
using System.IO;

public class GooglePlayServices
{

#region Variables
    private const string        		LEADERBOARD_ID = "CgkImt_Vrr0QEAIQAQ";
	private static bool					mIsReady = false;
    private static InitCompleteCallback	mCallback;

#if ENABLE_CLOUD_SAVE
	private const string				SAVE_FILE_NAME = "Hanuman_SaveGame";
    private static ISavedGameMetadata	mSavedGameMetadata;
    private static byte[]				mSavedGameData;
#endif
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

		PlayGamesClientConfiguration config;
#if ENABLE_CLOUD_SAVE
		mSavedGameData = null;
        config = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();
#else
		config = new PlayGamesClientConfiguration.Builder().Build();
#endif
        PlayGamesPlatform.InitializeInstance(config);

        //PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate ();
        Login();
    }

    private static void Login()
    {
        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate((bool success) => {
                if (success) 
                {
                    Debug.Log ("We're signed in! Welcome " + Social.localUser.userName);
                    if(mCallback != null)
                    	mCallback();
#if ENABLE_CLOUD_SAVE
					LoadSavedGame(SAVE_FILE_NAME);
#endif
                } 
                else
                {
                    Debug.Log ("Oh... we're not signed in.");
                    mIsReady = true;
                }

            });
        }
        else
		{
			Debug.Log ("We're signed in! Welcome " + Social.localUser.userName);
            if(mCallback != null)
            	mCallback();
#if ENABLE_CLOUD_SAVE
			LoadSavedGame(SAVE_FILE_NAME);
#endif
        }
    }

    public static void ReportScore(int highScore)
    {
        if (Social.localUser.authenticated)
            Social.ReportScore(highScore, LEADERBOARD_ID, (bool success) =>{});
    }

    public static void UnlockAchievement(string achievementID, double progress = 100.0f)
    {
		if (Social.localUser.authenticated)
			Social.ReportProgress(achievementID, progress, (bool success) =>{});
    }

    public static void IncrementAchievement(string achievementID, int incrementValue)
    {
        if (PlayGamesPlatform.Instance != null)
        {
			Achievement achievement = PlayGamesPlatform.Instance.GetAchievement(achievementID);
			if(achievement == null)
				Debug.Log("Achievement : " + achievementID.ToString() + " is null.");
			else
			{
				if(!achievement.IsIncremental)
				{
					Debug.Log("Achievement : " + achievementID + " is not of incremental type");
					return;
				}

				PlayGamesPlatform.Instance.IncrementAchievement(achievementID, incrementValue, (bool success) =>{});
			}
		}
    }

    public static double GetAchievementProgress(string achievementID)
    {
		if (PlayGamesPlatform.Instance != null)
        {
			Achievement achievement = PlayGamesPlatform.Instance.GetAchievement(achievementID);
//			if(achievement.IsIncremental)
//			{
//				return achievement.CurrentSteps;
//			}
			return ((double)achievement.CurrentSteps/(double)achievement.TotalSteps);
		}

		return 0;
    }

    public static bool IsAchievementUnlocked(string achievementID)
    {
		if (PlayGamesPlatform.Instance != null)
		{
			Achievement achievement = PlayGamesPlatform.Instance.GetAchievement(achievementID);
			if(achievement != null)
				return achievement.IsUnlocked;
		}

		return false;
    }

    public static bool IsIncrementalAchievement(string achievementID)
	{
		if (PlayGamesPlatform.Instance != null)
        {
			Achievement achievement = PlayGamesPlatform.Instance.GetAchievement(achievementID);
			if(achievement != null)
				return achievement.IsIncremental;
		}

		return false;
    }

    public static void ShowLeaderboard()
    {
        if(PlayGamesPlatform.Instance !=null)
        {
            ((PlayGamesPlatform)Social.Active).ShowLeaderboardUI(LEADERBOARD_ID);
        }
    }

    public static void ShowAchievements()
    {
        if(PlayGamesPlatform.Instance !=null)
        {
            ((PlayGamesPlatform)Social.Active).ShowAchievementsUI();
        }
    }

    public static void Logout()
    {
        PlayGamesPlatform.Instance.SignOut ();
    }

#if ENABLE_CLOUD_SAVE
    private static void LoadSavedGame(string fileName)
    {
		if (Application.platform == RuntimePlatform.Android && PlayGamesPlatform.Instance != null && PlayGamesPlatform.Instance.IsAuthenticated())
		{
	    	ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
			savedGameClient.OpenWithAutomaticConflictResolution(fileName, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, OnSavedDataLoaded);
		}
		else
			mIsReady = true;
    }

    private static void OnSavedDataLoaded(SavedGameRequestStatus status, ISavedGameMetadata savedMetaData)
    {
    	if(status == SavedGameRequestStatus.Success)
    	{
			mSavedGameMetadata = savedMetaData;
    		ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
			savedGameClient.ReadBinaryData(mSavedGameMetadata, OnSavedDataRead);
    	}
    	else
    		mIsReady = true;
    	
    	Debug.Log("GooglePlayServices : load saved data completed with status : " + status.ToString());
    }

    private static void OnSavedDataRead(SavedGameRequestStatus status, byte[] data)
	{    	
		if(status == SavedGameRequestStatus.Success)
    	{
    		mSavedGameData = data;
    		mIsReady = true;
    	}
    	else
    		mIsReady = true;

    	Debug.Log("GooglePlayServices : open saved data completed with status : " + status.ToString());
    }

    public static byte[] GetSavedData()
    {
    	return mSavedGameData;
    }

    public static void SaveGameData(byte[] saveData)
    {
		if (Application.platform == RuntimePlatform.Android && PlayGamesPlatform.Instance != null && PlayGamesPlatform.Instance.IsAuthenticated())
		{
	    	ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
	    	SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();
			TimeSpan updatedPlayTime = (mSavedGameMetadata != null) ? mSavedGameMetadata.TotalTimePlayed.Add(new TimeSpan(0, 0, (int)Time.realtimeSinceStartup)) : new TimeSpan(0, 0, (int)Time.realtimeSinceStartup);
			builder = builder.WithUpdatedPlayedTime(updatedPlayTime).WithUpdatedDescription("Saved at : " + DateTime.Now);
	    	SavedGameMetadataUpdate updatedMetadata = builder.Build();

			savedGameClient.CommitUpdate(mSavedGameMetadata, updatedMetadata, saveData, OnSavedGameWritten);
		}
    }

    private static void OnSavedGameWritten(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
    	if(status == SavedGameRequestStatus.Success)
    	{
    		// send callback if required
    	}
		Debug.Log("GooglePlayServices : Save game completed with status : " + status.ToString());
    		
    }
#endif //ENABLE_CLOUD_SAVE

#endregion
}

#endif