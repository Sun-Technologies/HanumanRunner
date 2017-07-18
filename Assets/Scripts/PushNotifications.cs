using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushNotifications : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        // Enable line below to enable logging if you are having issues setting up OneSignal. (logLevel, visualLogLevel)
        // OneSignal.SetLogLevel(OneSignal.LOG_LEVEL.INFO, OneSignal.LOG_LEVEL.INFO);

        OneSignal.StartInit("98983226-0518-4704-9623-a30a9c64d46c")
          .HandleNotificationOpened(HandleNotificationOpened)
          .EndInit();

        OneSignal.inFocusDisplayType = OneSignal.OSInFocusDisplayOption.Notification;

        // Call syncHashedEmail anywhere in your app if you have the user's email.
        // This improves the effectiveness of OneSignal's "best-time" notification scheduling feature.
        // OneSignal.syncHashedEmail(userEmail);
    }

    // Gets called when the player opens the notification.
    private static void HandleNotificationOpened(OSNotificationOpenedResult result)
    {
        Debug.Log("Opened notification!");
    }

    void SelfNotify()
    {

    }
}
