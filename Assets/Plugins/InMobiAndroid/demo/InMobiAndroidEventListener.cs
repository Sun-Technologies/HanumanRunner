using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class InMobiAndroidEventListener : MonoBehaviour
{
	#if UNITY_ANDROID
	
	void OnEnable()
	{
		// Listen to all events for illustration purposes
		InMobiAndroidManager.onBannerRequestSucceededEvent += onBannerRequestSucceededEvent;
		InMobiAndroidManager.onBannerRequestFailedEvent += onBannerRequestFailedEvent;
		InMobiAndroidManager.onShowBannerScreenEvent += onShowBannerScreenEvent;
		InMobiAndroidManager.onDismissBannerScreenEvent += onDismissBannerScreenEvent;
		InMobiAndroidManager.onBannerInteractionEvent += onBannerInteractionEvent;
		InMobiAndroidManager.onBannerLeaveApplicationEvent += onBannerLeaveApplicationEvent;
		InMobiAndroidManager.onBannerAdRewardActionCompletedEvent += onBannerAdRewardActionCompletedEvent;
		InMobiAndroidManager.onInterstitialLoadedEvent += onInterstitialLoadedEvent;
		InMobiAndroidManager.onInterstitialFailedEvent += onInterstitialFailedEvent;
		InMobiAndroidManager.onInterstitialInteractionEvent += onInterstitialInteractionEvent;
		InMobiAndroidManager.onShowInterstitialScreenEvent += onShowInterstitialScreenEvent;
		InMobiAndroidManager.onDismissInterstitialScreenEvent += onDismissInterstitialScreenEvent;
		InMobiAndroidManager.onInterstitialInteractionEvent += onInterstitialInteractionEvent;
		InMobiAndroidManager.onInterstitialLeaveApplicationEvent += onInterstitialLeaveApplicationEvent;
		InMobiAndroidManager.onInterstitialAdRewardActionCompletedEvent += onInterstitialAdRewardActionCompletedEvent;
		
	}
	
	
	void OnDisable()
	{
		// Remove all event handlers
		InMobiAndroidManager.onBannerRequestSucceededEvent -= onBannerRequestSucceededEvent;
		InMobiAndroidManager.onBannerRequestFailedEvent -= onBannerRequestFailedEvent;
		InMobiAndroidManager.onShowBannerScreenEvent -= onShowBannerScreenEvent;
		InMobiAndroidManager.onDismissBannerScreenEvent -= onDismissBannerScreenEvent;
		InMobiAndroidManager.onBannerInteractionEvent -= onBannerInteractionEvent;
		InMobiAndroidManager.onBannerLeaveApplicationEvent -= onBannerLeaveApplicationEvent;
		InMobiAndroidManager.onBannerAdRewardActionCompletedEvent -= onBannerAdRewardActionCompletedEvent;
		InMobiAndroidManager.onInterstitialLoadedEvent -= onInterstitialLoadedEvent;
		InMobiAndroidManager.onInterstitialFailedEvent -= onInterstitialFailedEvent;
		InMobiAndroidManager.onInterstitialInteractionEvent -= onInterstitialInteractionEvent;
		InMobiAndroidManager.onShowInterstitialScreenEvent -= onShowInterstitialScreenEvent;
		InMobiAndroidManager.onDismissInterstitialScreenEvent -= onDismissInterstitialScreenEvent;
		InMobiAndroidManager.onInterstitialInteractionEvent -= onInterstitialInteractionEvent;
		InMobiAndroidManager.onInterstitialLeaveApplicationEvent -= onInterstitialLeaveApplicationEvent;
		InMobiAndroidManager.onInterstitialAdRewardActionCompletedEvent -= onInterstitialAdRewardActionCompletedEvent;
		
	}

	
	void onDismissBannerScreenEvent()
	{
		Debug.Log( "onDismissBannerScreenEvent" );
	}
	
	
	void onBannerRequestFailedEvent( string error )
	{
		Debug.Log( "onBannerRequestFailedEvent: " + error );
	}
	
	
	void onBannerInteractionEvent( Dictionary<string,object> data )
	{
		Debug.Log( "onBannerInteractionEvent" + data );
	}
	
	
	void onBannerRequestSucceededEvent()
	{
		Debug.Log( "onBannerRequestSucceededEvent" );
	}
	
	
	void onBannerLeaveApplicationEvent()
	{
		Debug.Log( "onBannerLeaveApplicationEvent" );
	}
	
	
	void onShowBannerScreenEvent()
	{
		Debug.Log( "onShowBannerScreenEvent" );
	}
	
	void  onBannerAdRewardActionCompletedEvent( Dictionary<string,object> data)
	{
		Debug.Log( "onBannerAdRewardActionCompletedEvent" );

	}

	void onDismissInterstitialScreenEvent()
	{
		Debug.Log( "onDismissInterstitialScreenEvent" );
	}
	
	
	void onInterstitialFailedEvent( string error )
	{
		Debug.Log( "onInterstitialFailedEvent: " + error );
	}
	
	
	void onInterstitialInteractionEvent( Dictionary<string,object> data )
	{
		Debug.Log( "onInterstitialInteractionEvent" );
	}
	
	
	void onInterstitialLoadedEvent()
	{
		Debug.Log( "onInterstitialLoadedEvent" );
	}
	
	
	void onInterstitialLeaveApplicationEvent()
	{
		Debug.Log( "onInterstitialLeaveApplicationEvent" );
	}
	
	
	void onShowInterstitialScreenEvent()
	{
		Debug.Log( "onShowInterstitialScreenEvent" );
	}
	
	void  onInterstitialAdRewardActionCompletedEvent( Dictionary<string,object> data)
	{
		Debug.Log( "onInterstitialAdRewardActionCompletedEvent" + data);
		
	}

	#endif
}