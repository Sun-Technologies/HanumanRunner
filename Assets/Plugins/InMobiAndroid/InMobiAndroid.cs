using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InMobiMiniJSON;


#if UNITY_ANDROID

public enum InMobiLogLevel
{
	None      = 0,
	Debug     = 1,
	Verbose   = 2,
	
}

public enum InMobiAdPosition
{
	TopLeft,
	TopCenter,
	TopRight,
	Centered,
	BottomLeft,
	BottomCenter,
	BottomRight,
}



public class InMobiAndroid
{
	private static AndroidJavaObject _plugin;
	
	static InMobiAndroid()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		// find the plugin instance
		using( var pluginClass = new AndroidJavaClass( "com.unity.InMobiPlugin" ) )
			_plugin = pluginClass.CallStatic<AndroidJavaObject>( "instance" );
	}
	
	
	// Sets the log level for the InMobi SDK
	public static void setLogLevel( InMobiLogLevel logLevel )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		_plugin.Call( "setLogLevel", (int)logLevel );
	}
	
	
	public static void init(string accountId, Dictionary<string,string> optionalInMobiParams = null )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		optionalInMobiParams.Add( "tp-ver", Application.unityVersion );
		_plugin.Call( "initialize",accountId,optionalInMobiParams != null ? Json.Serialize(optionalInMobiParams) : string.Empty);
	}
	
	
	#region interstitials
	
	// Preloads an interstitial
	public static void loadInterstitial(long placementId, string key ,string keywords = null,Dictionary<string,string> publisherExtra = null )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		_plugin.Call( "loadInterstitial", placementId,key, keywords,publisherExtra != null ? Json.Serialize(publisherExtra) : string.Empty);
	}
	
	
	
	// Gets the current state of the interstitial either true or false
	public static bool getInterstitialState(string key)
	{
		if( Application.platform != RuntimePlatform.Android )
			return false;
		return _plugin.Call<bool>( "getInterstitialState" ,key);
	}
	
	
	// Presents the interstitial if it is loaded and ready
	public static void showInterstitial(string key)
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		_plugin.Call( "showInterstitial" ,key);
	}

	// Destroys the intersitial
	public static void removeInterstitial(string key) {
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		_plugin.Call( "removeInterstitial",key);
	}
	
	#endregion
	
	
	#region banners
	
	// Creates a banner
	public static void createBanner(long placementId, string key,InMobiAdPosition alignment, int width,  int height, int refreshInterval, string keywords = null,Dictionary<string,string>  publisherExtra =null)
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "createBanner", (int)alignment,   width,  height,placementId, refreshInterval,key, keywords, publisherExtra != null ? Json.Serialize(publisherExtra) : string.Empty);
	}
	
	// Reloads the banner
	public static void loadBanner(string key)
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		_plugin.Call( "loadBanner",key);
	}
	
	
	
	// Destroys the banner
	public static void destroyBanner(string key)
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		_plugin.Call( "destroyBanner",key);
	}
	
	#endregion
	
}

#endif