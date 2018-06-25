using Pokkt;
using System;
using System.Runtime.InteropServices;
using UnityEngine;


namespace Pokkt.Extensions
{
	#region ANDROID_EXTENSION
	internal class AndroidExtension : INativeExtension
	{
		#if UNITY_ANDROID
		private const string PokktJavaClassName = "com.pokkt.plugin.PokktNativeExtension";
		private const string NativeNotifierMethodName = "notifyNative";
		private const string GetSdkVersionMethodName = "getSDKVersionOnNative";
		private const string IsVideoAdCachedMethodName = "isVideoAdCachedOnNative";
		private const string IsInterstitialCachedMethodName = "isInterstitialCachedOnNative";

		private static readonly AndroidJavaClass _jc = new AndroidJavaClass(PokktJavaClassName);

		private static void NotifyAndroid(string operation, string param)
		{
			_jc.CallStatic(NativeNotifierMethodName, operation, param);
		}

		private static string GetSdkVersionOnNative()
		{
			return _jc.CallStatic<string>(GetSdkVersionMethodName);
		}

		private static bool IsVideoAdCachedOnNative(string screenName, bool isRewarded)
		{
		return _jc.CallStatic<bool>(IsVideoAdCachedMethodName, screenName, isRewarded);
		}

		private static bool IsInterstitialCachedOnNative(string screenName, bool isRewarded)
		{
		return _jc.CallStatic<bool>(IsInterstitialCachedMethodName, screenName, isRewarded);
		}


		#endif

		public void NotifyNative(string operation, string param)
		{
			#if UNITY_ANDROID
			NotifyAndroid(operation, param);
			#else
			throw new InvalidOperationException("Method not implemented!");
			#endif
		}

		public string GetSdkVersion()
		{
			#if UNITY_ANDROID
			return GetSdkVersionOnNative();
			#else
			throw new InvalidOperationException("Method not implemented!");
			#endif
		}

		public bool IsVideoAdCached (string screenName, bool isRewarded)
		{
			#if UNITY_ANDROID
			return IsVideoAdCachedOnNative(screenName, isRewarded);
			#else
			throw new InvalidOperationException("Method not implemented!");
			#endif
		}

		public bool IsInterstitialCached (string screenName, bool isRewarded)
		{
			#if UNITY_ANDROID
			return IsInterstitialCachedOnNative(screenName, isRewarded);
			#else
			throw new InvalidOperationException("Method not implemented!");
			#endif
		}
	}
	#endregion


	#region IOS_EXTENSION
	internal class IosExtension : INativeExtension
	{
		#if UNITY_IOS
		// internal calls
		[DllImport("__Internal")]
		private static extern void notifyNative(string operation, string param);

		[DllImport("__Internal")]
		private static extern string getSDKVersionOnNative();

		[DllImport("__Internal")]
		private static extern bool isVideoAdCachedOnNative(string screenName, bool isRewarded);

		[DllImport("__Internal")]
		private static extern bool isInterstitialCachedOnNative(string screenName, bool isRewarded);
		#endif

		public void NotifyNative(string operation, string param)
		{
			#if UNITY_IOS
			notifyNative(operation, param);
			#else
			throw new InvalidOperationException("Method not implemented!");
			#endif
		}

		public string GetSdkVersion()
		{
			#if UNITY_IOS
			return getSDKVersionOnNative();
			#else
			throw new InvalidOperationException("Method not implemented!");
			#endif
		}


		public bool IsVideoAdCached (string screenName, bool isRewarded)
		{
			#if UNITY_IOS
			return isVideoAdCachedOnNative(screenName, isRewarded);
			#else
			throw new NotImplementedException ();
			#endif
		}

		public bool IsInterstitialCached (string screenName, bool isRewarded)
		{
			#if UNITY_IOS
			return isInterstitialCachedOnNative(screenName, isRewarded);
			#else
			throw new NotImplementedException ();
			#endif
		}
	}
	#endregion


	#region WINDOWS_EXTENSION
	internal class WindowsExtension : INativeExtension
	{
		public WindowsExtension()
		{
			#if UNITY_WSA
			PWPUnity.PokktNativeExtension.PokktDispatcher = PokktManager.DispatcherGO();
			#endif
		}

		public void NotifyNative(string operation, string param)
		{
			#if UNITY_WSA
			PWPUnity.PokktNativeExtension.NotifyNative(operation, param);
			#else
			throw new System.InvalidOperationException("Method not implemented!");
			#endif
		}

		public string GetSdkVersion()
		{
			#if UNITY_WSA
			return PWPUnity.PokktNativeExtension.GetSDKVersionOnNative();
			#else
			throw new System.InvalidOperationException("Method not implemented!");
			#endif
		}
		public bool IsVideoAdCached (string screenName, bool isRewarded)
		{
			#if UNITY_WSA
			//return PWPUnity.PokktNativeExtension.IsVideoAdCachedOnNative(screenName, isRewarded);
			return false;
			#else
			throw new System.InvalidOperationException("Method not implemented!");
			#endif
		}
		public bool IsInterstitialCached (string screenName, bool isRewarded)
		{
			#if UNITY_WSA
			//return PWPUnity.PokktNativeExtension.IsInterstitialCachedOnNative(screenName, isRewarded);
			return false;
			#else
			throw new System.InvalidOperationException("Method not implemented!");
			#endif
		}
	}
	#endregion


	#region EDITOR_EXTENSION
	internal class EditorExtension : INativeExtension
	{
		public void NotifyNative(string operation, string param)
		{
			#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_OSX
			// nothing to do
			#else
			throw new InvalidOperationException("Method not implemented!");
			#endif
		}

		public string GetSdkVersion()
		{
			#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_OSX
			return ""; // nothing to do
			#else
			throw new InvalidOperationException("Method not implemented!");
			#endif
		}
		public bool IsVideoAdCached(string screenName, bool isRewarded)
		{
			#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_OSX
			return false; // nothing to do
			#else
			throw new InvalidOperationException("Method not implemented!");
			#endif
		}

		public bool IsInterstitialCached(string screenName, bool isRewarded)
		{
			#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_OSX
			return false; // nothing to do
			#else
			throw new InvalidOperationException("Method not implemented!");
			#endif
		}
	}
	#endregion


	#region EXTENSION_PROVIDER
	public class PokktExtensionProvider
	{
		public static INativeExtension GetExtension()
		{
			if (Application.platform == RuntimePlatform.Android)
				return new AndroidExtension();

			if (Application.platform == RuntimePlatform.IPhonePlayer)
				return new IosExtension();

			#if UNITY_WSA
			if (Application.platform == RuntimePlatform.WSAPlayerARM || 
				Application.platform == RuntimePlatform.WSAPlayerX64 || 
				Application.platform == RuntimePlatform.WSAPlayerX86)
				return new WindowsExtension();
			#endif

			Debug.LogWarning("[UNITY] Performer not implemented for platform: " + Application.platform);
			return null;
		}

		//public static void SetNativeExtension()
		//{
		//	PokktNativeExtension.NativeExtension = GetExtension();
		//}
	}
	#endregion
}
