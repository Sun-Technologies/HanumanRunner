using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using InMobiMiniJSON;

public class InMobiAndroidManager : MonoBehaviour
{
	#if UNITY_ANDROID
	// Fired when a banner loads	
	public static event Action onBannerRequestSucceededEvent;

	// Fired when a banner fails to load
	public static event Action<string> onBannerRequestFailedEvent;

	// Called when ad expands on top of the screen because of user action
	public static event Action onShowBannerScreenEvent;
	
    // Fired when a banners full screen action is dismissed
	public static event Action onDismissBannerScreenEvent;

	// Fired when a banner is interacted with. Includes the data payload.
	public static event Action<Dictionary<string,object>> onBannerInteractionEvent;

	// Fired when user leave the application.
	public static event Action onBannerLeaveApplicationEvent;

	// Fired when banner incent callbacks completed 
	public static event Action<Dictionary<string,object>> onBannerAdRewardActionCompletedEvent;


	// Fired when a interstitial loads	
	public static event Action onInterstitialLoadedEvent;
	
	// Fired when a banner fails to load
	public static event Action<string> onInterstitialFailedEvent;
	
	// Called when intersitial show method is called
	public static event Action onShowInterstitialScreenEvent;
	
	// Fired when a interstitial full screen action is dismissed
	public static event Action onDismissInterstitialScreenEvent;
	
	// Fired when a banner is interacted with. Includes the data payload.
	public static event Action<Dictionary<string,object>> onInterstitialInteractionEvent;
	
	// Fired when user leave the application.
	public static event Action onInterstitialLeaveApplicationEvent;
	
	// Fired when banner incent callbacks completed 
	public static event Action<Dictionary<string,object>> onInterstitialAdRewardActionCompletedEvent;

	
	void Awake()
	{
		// Set the GameObject name to the class name for easy access from Obj-C
		gameObject.name = this.GetType().ToString();
		DontDestroyOnLoad( this );
	}

	public void onBannerRequestSucceeded( string empty )
	{
		onBannerRequestSucceededEvent ();
	}

	public void onBannerRequestFailed( string error )
	{
		onBannerRequestFailedEvent ( error);
	}

	public void onShowBannerScreen( string empty )
	{
		onShowBannerScreenEvent ();
	}

	public void onDismissBannerScreen( string empty )
	{
		onDismissBannerScreenEvent ();
	}

	public void onBannerInteraction( string json )
	{
		onBannerInteractionEvent(( Dictionary<string,object>)Json.Deserialize(json) );
	}

	public void onBannerLeaveApplication( string empty )
	{
		onBannerLeaveApplicationEvent();
	}

	public void onBannerAdRewardActionCompleted( string json )
	{
		onBannerAdRewardActionCompletedEvent(( Dictionary<string,object>)Json.Deserialize(json));
	}

	public void onInterstitialLoaded( string empty )
	{
		onInterstitialLoadedEvent ();
	}
	
	public void onInterstitialFailed( string error )
	{
		onInterstitialFailedEvent (error);
	}
	
	public void onShowInterstitialScreen( string empty )
	{

		onShowInterstitialScreenEvent ();
	}
	
	public void onDismissInterstitialScreen( string empty )
	{
		onDismissInterstitialScreenEvent ();
	}
	
	public void onInterstitialInteraction( string json )
	{
		onInterstitialInteractionEvent(( Dictionary<string,object>)Json.Deserialize(json) );
	}
	
	public void onInterstitialLeaveApplication( string empty )
	{
		onInterstitialLeaveApplicationEvent();
	}
	
	public void onInterstitialAdRewardActionCompleted( string json )
	{
		onInterstitialAdRewardActionCompletedEvent(( Dictionary<string,object>)Json.Deserialize(json));
	}

	
	#endif
}