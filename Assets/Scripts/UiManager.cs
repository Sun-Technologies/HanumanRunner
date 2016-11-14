using Facebook.Unity;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum GameState { MainMenu, InGame, GameOver, Pause };
public class UiManager : MonoBehaviour
{
    public static UiManager instance = null;
    GameState gameState;

    #region FacebookVariables
    // Custom Feed Share
    private string feedTo = string.Empty;
    private string feedLink = "http://ragemaker.net/images/treecomics/stoned%20what.png";
    private string feedTitle = "Jai Hanuman";
    private string feedCaption = "Share and get good news in 5 days";
    private string feedDescription = "If you don't like and share this, you'll lose all your gainz #RepsForJesus";
    private string feedImage = "http://i.imgur.com/zkYlB.jpg";
    private string feedMediaSource = string.Empty;
     
    #endregion

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        SetInit();
    }

    public void PostToFacebook()
    {
        if (FB.IsInitialized)
        {
            if (!FB.IsLoggedIn)
            {
                FB.LogInWithPublishPermissions(new List<string>() { "publish_actions" }, HandleResult);
            }
            else
            {
                FB.FeedShare(
                    feedTo,
                    string.IsNullOrEmpty(feedLink) ? null : new Uri(feedLink),
                    feedTitle,
                    feedCaption,
                    feedDescription,
                    string.IsNullOrEmpty(feedImage) ? null : new Uri(feedImage),
                    feedMediaSource,
                    HandleResult);
            }
        }
        else
        {
            FacebookLog("initialize facebook first!");
        }
    }

    private void HandleResult(IResult result)
    {
        if (result == null)
        {
            FacebookLog("Null Response\n");
            return;
        }

        // Some platforms return the empty string instead of null.
        if (!string.IsNullOrEmpty(result.Error))
        {
            FacebookLog("Error Response:\n" + result.Error);
        }
        else if (result.Cancelled)
        {
            FacebookLog("Cancelled Response:\n" + result.RawResult);
        }
        else if (!string.IsNullOrEmpty(result.RawResult))
        {
            FacebookLog("Success Response:\n" + result.RawResult);
        }
        else
        {
            FacebookLog("Empty Response\n");
        }

    }

    private void SetInit()
    {
        FacebookLog("SetInit");
        FB.Init(OnInitComplete, OnHideUnity);
        FacebookLog("FB.Init() called with " + FB.AppId);
        if (FB.IsLoggedIn)
        {
            FacebookLog("Already logged in");
        }
    }

    private void OnInitComplete()
    {
        FacebookLog("Success - Check log for details");
        FacebookLog("Success Response: OnInitComplete Called\n");
        string logMessage = string.Format(
            "OnInitCompleteCalled IsLoggedIn='{0}' IsInitialized='{1}'",
            FB.IsLoggedIn,
            FB.IsInitialized);
        FacebookLog(logMessage);
        if (AccessToken.CurrentAccessToken != null)
        {
            FacebookLog(AccessToken.CurrentAccessToken.ToString());
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
        FacebookLog("Success - Check log for details");
        FacebookLog(string.Format("Success Response: OnHideUnity Called {0}\n", isGameShown));
        FacebookLog("Is game shown: " + isGameShown);
    }

    private void FacebookLog(string data)
    {
        Debug.Log(data);
    }

    public void PauseGame()
    {

    }

    public void StartGame()
    {

    }

    public void ResumeGame()
    {

    }

    public void QuitToMainMenu()
    {

    }

    public void QuitConfirmYes()
    {

    }

    public void QuitConfirmNo()
    {

    }

    public void RestartGame()
    {

    }

    public void ToggleMusic()
    {

    }

    public void InfoScreen()
    {

    }

    public void CloseInfoScreen()
    {

    }
}
