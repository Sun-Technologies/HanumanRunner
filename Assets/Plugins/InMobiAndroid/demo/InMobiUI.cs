using UnityEngine;
using System.Collections.Generic;
using InMobiMiniJSON;

public class InMobiUI : MonoBehaviour
{
	#if UNITY_ANDROID
	public static string nativeContent = null;
	public static string nativeError;

	void OnGUI ()
	{
		
		float yPos = 5.0f;
		float xPos = 5.0f;
		float width = (Screen.width >= 960 || Screen.height >= 960) ? 600 : 500;
		float height = (Screen.width >= 960 || Screen.height >= 960) ? 60 : 55;
		float heightPlus = height + 10.0f;
		if (GUI.Button (new Rect (xPos, yPos, width, height), "Initialize")) {
			var dict = new Dictionary<string,string> ();
			dict.Add ("age", "20");
			dict.Add ("gender", "GENDER_MALE");
			InMobiAndroid.init ("4028cb8b2c3a0b45012c406824e800ba", dict);			
		}
		
		
		if (GUI.Button (new Rect (xPos, yPos += heightPlus, width, height), "Set Log Level to Debug")) {
			InMobiAndroid.setLogLevel (InMobiLogLevel.Verbose);
		}


		
		if (GUI.Button (new Rect (xPos, yPos += heightPlus, width, height), "Create Banner (320x50) centered")) {
			var dict = new Dictionary<string,string> ();
			dict.Add ("hi", "hello2");
			InMobiAndroid.createBanner (1467162141987, "ban1", InMobiAdPosition.Centered, 320, 50, 30, "test1,test2,test3", dict);

			
		}
		
		if (GUI.Button (new Rect (xPos, yPos += heightPlus, width, height), "Load Banner (320x50) centered")) {
			InMobiAndroid.loadBanner ("ban1");
			
			
		}
		
		if (GUI.Button (new Rect (xPos, yPos += heightPlus, width, height), "Destroy Banner centered ")) {
			InMobiAndroid.destroyBanner ("ban1");
		}



		if (GUI.Button (new Rect (xPos, yPos += heightPlus, width, height), "Load Interstitial1")) {
			var dict = new Dictionary<string,string> ();
			dict.Add ("hi", "hello");
			InMobiAndroid.loadInterstitial (1469137441636, "interstitial1", "test1", dict);
		}
		
		if (GUI.Button (new Rect (xPos, yPos += heightPlus, width, height), "Is Interstitial1 Ready")) {
			if (InMobiAndroid.getInterstitialState ("interstitial1") == true)
				Debug.Log ("Interstitial1 is Ready");
			else
				Debug.Log ("Interstitial1 is not Ready");
		}
		
		if (GUI.Button (new Rect (xPos, yPos += heightPlus, width, height), "show Interstitial1")) {
			InMobiAndroid.showInterstitial ("interstitial1");
		}
		if (GUI.Button (new Rect (xPos, yPos += heightPlus, width, height), "Load Rewarded Video 1")) {
			var dict = new Dictionary<string,string> ();
			dict.Add ("hi", "hello");
			InMobiAndroid.loadInterstitial (1465237901291, "rewardedVideo1", "test1", dict);
		}

		if (GUI.Button (new Rect (xPos, yPos += heightPlus, width, height), "Is Rewarded Video 1 Ready")) {
			if (InMobiAndroid.getInterstitialState ("rewardedVideo1") == true)
				Debug.Log ("Rewarded Video 1 is Ready");
			else
				Debug.Log ("Rewarded Video 1 is not Ready");
		}

		if (GUI.Button (new Rect (xPos, yPos += heightPlus, width, height), "show Rewarded Video 1")) {
			InMobiAndroid.showInterstitial ("rewardedVideo1");
		}
		


	}	
	#endif
	

}