﻿using Facebook.Unity;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameState { MainMenu, InGame, GameOver, Pause, Info };
public class UiManager : MonoBehaviour
{
    public static UiManager instance = null;
    public GameState gameState;

    public int coins = 0;

    public int lives = 5;

    public int shareCount = 0;

    public const string ScoreKey = "ScoreKey";

    public const string LivesKey = "LivesKey";

    public const string FbShareKey = "FbShareKey";

    public bool isGamePaused = false;

    #region UI Screens

    public GameObject MainMenuScreen;
    public GameObject PauseScreen;
    public GameObject GameOverScreen;
    public GameObject InfoScreen;
    public GameObject HUDScreen;
    public GameObject QuitConfirmation;

    #endregion


    #region Game over screen items

    public GameObject HighScore_Panel;
    public GameObject NewBest_TextObj;
    public Text HighScore_Text;
    public Text Lives_Text;
    public Text Score_Text;
    public Button Restart_Button;
    public Button Home_Button;
    public Text GameShare_Text;
    public Text ScoreShare_Text;

    #endregion

    #region PauseScreen items

    public Toggle MusicToggle_PauseMenu;

    #endregion

    #region MainMenu Screen items

    public Toggle MusicToggle_MainMenu;

    #endregion

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

    void Start()
    {
        coins = PlayerPrefsStorage.GetIntData(ScoreKey);
        lives = PlayerPrefsStorage.GetIntData(LivesKey);
        shareCount = PlayerPrefsStorage.GetIntData(FbShareKey);
        SwitchGameState(GameState.MainMenu);
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
            shareCount++;
            PlayerPrefsStorage.SaveData(FbShareKey, shareCount);
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
        gameState = GameState.Pause;
        SwitchGameState(GameState.Pause);
    }

    public void StartGame()
    {
        gameState = GameState.InGame;
        SwitchGameState(GameState.InGame);
    }

    public void ResumeGame()
    {
        gameState = GameState.InGame;
        SwitchGameState(GameState.InGame);
    }

    public void QuitToMainMenu()
    {
        QuitConfirmation.SetActive(true);
    }

    public void QuitConfirmYes()
    {
        gameState = GameState.MainMenu;
        SwitchGameState(GameState.MainMenu);
    }

    public void QuitConfirmNo()
    {
        gameState = GameState.InGame;
        SwitchGameState(GameState.InGame);
    }

    public void RestartGame()
    {
        SwitchGameState(GameState.InGame);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ToggleMusic()
    {

    }

    public void InfoScreen_Button()
    {
        InfoScreen.SetActive(true);
    }

    public void CloseInfoScreen()
    {
        InfoScreen.SetActive(false);
    }

    public void SwitchGameState(GameState state)
    {
        MainMenuScreen.SetActive(false);
        GameOverScreen.SetActive(false);
        PauseScreen.SetActive(false);
        HUDScreen.SetActive(false);
        QuitConfirmation.SetActive(false);
        ToggleGamePauseState(true);

        if (state == GameState.MainMenu)
        {
            MainMenuScreen.SetActive(true);
        }

        if (state == GameState.GameOver)
        {
            GameOverScreen.SetActive(true);
        }

        if (state == GameState.InGame)
        {
            HUDScreen.SetActive(true);
            ToggleGamePauseState(false);
        }

        if (state == GameState.Pause)
        {
            PauseScreen.SetActive(true);
        }
    }

    public void ToggleGamePauseState(bool isPaused)
    {
        if (isPaused)
        {
            Time.timeScale = 0;
            isGamePaused = true;
        }
        else
        {
            Time.timeScale = 1;
            isGamePaused = false;
        }
    }
}