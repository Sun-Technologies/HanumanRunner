﻿using Facebook.Unity;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;
#if UNITY_ANDROID || UNITY_IOS
using UnityEngine.Advertisements;
#endif

public enum GameState { MainMenu, InGame, GameOver, Pause, Info };
public class UiManager : MonoBehaviour
{
    public static UiManager instance = null;
    public static GameState gameState = GameState.MainMenu;

    public List<UiPanels> UipanelsList;

    public GameObject TapToPlayObj;
    public int HighScore = 0;

    public int lives = 5;

    public int shareCount = 0;

    public int TapToPlayCount = 0;

    public bool isGamePaused = false;

    DateTime SavedDateTime = DateTime.Today;

    bool isMainMenuScreen = false;

    public TextElements _textElements;

    #region UI Screens

    public GameObject MainMenuScreen;
    public GameObject PauseScreen;
    public GameObject GameOverScreen;
    public GameObject InfoScreen;
    public GameObject HUDScreen;
    public GameObject QuitConfirmation;
    public GameObject StoreScreen;
    public GameObject LevelsScreen;
    public GameObject SettingsScreen;
    public GameObject DailyBonusScreen;
    public GameObject LanguageSelection;

    #endregion


    #region Game over screen items

    public GameObject HighScore_Panel;
    public GameObject NewBest_TextObj;
    public Text HighScore_Text;
    public Text Lives_Text;
    public Text Score_Text;
    public Text GameShare_Text;
    public Text ScoreShare_Text;

    #endregion

    #region PauseScreen items

    public Toggle MusicToggle_PauseMenu;

    #endregion

    #region MainMenu Screen items

    public Toggle MusicToggle_MainMenu;
    public GameObject PlayButtonObj;
    public GameObject ShareTextObj;

    #endregion

    #region FacebookVariables

    private string shareLink = "http://www.aavegainteractive.com/";
    private string shareTitle = "Hanuman The Run HD";
    private string shareImage = "https://s3.amazonaws.com/hanumanrun/facebookshare/facebookshare.png";
    private string feedMediaSource = string.Empty;

    #endregion

    #region Daily Bonus Screen Items
    public GameObject DaysButtonHolder;
    public Text[] DaysButtons;
    public int days_DailyBonus = 0;
    public static bool IsTodaysBonusShown = false;
    public Scrollbar bonusScroll;
    public GameObject[] LockObjs;
    #endregion

    #region Language Selection Screen

    public Toggle EnglishToggle;
    public Toggle HindiToggle;
    #endregion

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        Debug.Log("State = " + gameState);
        SetInit();
        DaysButtons = DaysButtonHolder.GetComponentsInChildren<Text>(true);
        _textElements = GetComponent<TextElements>();
    }

    void Start()
    {
        Debug.Log(SavedDateTime);
        AvGameServices.Init();
        //PlayerPrefsStorage.SaveData(LivesKey, 1);

        if (string.IsNullOrEmpty(PlayerPrefsStorage.GetStringData(GameData.KEY_DATETIME)))
        {
            PlayerPrefsStorage.SaveData(GameData.KEY_DATETIME, SavedDateTime.ToString());
            ToggleDailyBonusState(true);
        }
        else
        {
            SavedDateTime = DateTime.Parse(PlayerPrefsStorage.GetStringData(GameData.KEY_DATETIME));
            Debug.Log("Date = " + SavedDateTime);
        }
        //SavedDateTime = DateTime.Parse("7 / 19 / 2017 12:00:00 AM");    //TODO: remove later
        if (SavedDateTime < DateTime.Today)//if (SavedDateTime < DateTime.Parse("7 / 20 / 2017 12:00:00 AM"))                //
        {
            Debug.Log("Date changed. Updating timer.");
            SavedDateTime.AddDays(1);
            days_DailyBonus += 1;
            PlayerPrefsStorage.SaveData(GameData.KEY_DAYS, days_DailyBonus);
            PlayerPrefsStorage.SaveData(GameData.KEY_DATETIME, DateTime.Today.ToString());

            if (days_DailyBonus <= 7)
            {
                ToggleDailyBonusState(IsTodaysBonusShown);
                if (days_DailyBonus <= 1)
                {
                    bonusScroll.value = 0;
                }
                else if (days_DailyBonus > 1 && days_DailyBonus < 4)
                {
                    bonusScroll.value = 0.66f;
                }
                else
                {
                    bonusScroll.value = 1;
                }
            }
        }
        else
        {
            Debug.Log("Not a new day yet!");
        }
        Debug.Log("Days count " + days_DailyBonus);
        HighScore = PlayerPrefsStorage.GetIntData(GameData.KEY_HIGHSCORE, 0);
        shareCount = PlayerPrefsStorage.GetIntData(GameData.KEY_FBSHARE, 0);
        TapToPlayCount = PlayerPrefsStorage.GetIntData(GameData.KEY_TAPTOPLAY, 0);
        if (gameState == GameState.MainMenu)
        {
            SwitchGameState(GameState.MainMenu);
        }
        else
        {
            SwitchGameState(GameState.InGame);
        }

        //if (days_DailyBonus >= 7)
        //{
        //    ToggleDailyBonusState(false);
        //}
        //else
        //{
        //    CheckDailyBonus();
        //}
    }

    public void OpenDailyBonusScreen(bool flag)
    {
        DailyBonusScreen.SetActive(flag);
    }

    public void ToggleDailyBonusState(bool flag)
    {
        DailyBonusScreen.SetActive(!flag);
        if (!flag)
        {
            CheckDailyBonus();
        }
    }

    string GetDailyBonusText(int ladduCount)
    {
        string str = string.Empty;
        if (days_DailyBonus < 7)
        {
            str = string.Format("You have received {0} laddus", ladduCount);
        }
        else
        {
            str = "You have unlocked Gada!";
        }
        return str;
    }

    void CheckDailyBonus()
    {
        IsTodaysBonusShown = true;
        int ladduCount = PlayerPrefsStorage.GetIntData(GameData.KEY_LADDUS_COLLECTED_COUNT, 0);

        for (int i = 0; i < LockObjs.Length; i++)
        {
            LockObjs[i].SetActive(true);
        }

        for (int i = 0; i <= days_DailyBonus; i++)
        {
            LockObjs[i].SetActive(false);
        }

        switch (days_DailyBonus)
        {
            case 1:
                ladduCount += 5;
                _textElements.lbl_RewardsReceived.text = GetDailyBonusText(5);
                break;

            case 2:
                ladduCount += 10;
                _textElements.lbl_RewardsReceived.text = GetDailyBonusText(10);
                break;

            case 3:
                ladduCount += 30;
                _textElements.lbl_RewardsReceived.text = GetDailyBonusText(30);
                break;

            case 4:
                ladduCount += 50;
                _textElements.lbl_RewardsReceived.text = GetDailyBonusText(50);
                break;

            case 5:
                ladduCount += 70;
                _textElements.lbl_RewardsReceived.text = GetDailyBonusText(70);
                break;

            case 6:
                ladduCount += 90;
                _textElements.lbl_RewardsReceived.text = GetDailyBonusText(90);
                break;

            case 7:

                _textElements.lbl_RewardsReceived.text = GetDailyBonusText(ladduCount);
                PlayerPrefsStorage.SaveData(GameData.KEY_GADA_UNLOCKED, 1);
                break;

            default:
                break;
        }

        PlayerPrefsStorage.SaveData(GameData.KEY_LADDUS_COLLECTED_COUNT, ladduCount);

        Debug.Log("Days count = " + days_DailyBonus + " Total laddus count = " + ladduCount);

        PlayerPrefsStorage.SaveData(GameData.KEY_DAYS, days_DailyBonus);

        if (days_DailyBonus == 7)
        {
            PlayerPrefsStorage.SaveData(GameData.KEY_GADA_UNLOCKED, 1);
        }
    }

    public void DoLanguageToggle()
    {
        if (EnglishToggle.isOn)
        {
            LocalizationText.SetLanguage(GameData.LANG_ENGLISH);
            PlayerPrefsStorage.SaveData(GameData.KEY_LANGUAGE, GameData.LANG_ENGLISH);
        }
        else
        {
            LocalizationText.SetLanguage(GameData.LANG_HINDI);
            PlayerPrefsStorage.SaveData(GameData.KEY_LANGUAGE, GameData.LANG_ENGLISH);
        }
        Debug.Log("Active language = " + LocalizationText.GetLanguage());
        _textElements.UpdateLanguage();
    }

    string OpenGameLink()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            return "https://play.google.com/store/apps/details?id=com.aavega.hanumantherunhd";
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            return "https://itunes.apple.com/us/app/hanuman-the-run-hd/id1177930830?ls=1&mt=8";
        }
        else
        {
            return "http://www.aavegainteractive.com";
        }
    }

    void Update()
    {
        if (gameState == GameState.MainMenu && Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Application quit!");
            Application.Quit();
        }
    }

    public void PostToFacebook(bool isMainMenu)
    {
        if (isMainMenu)
        {
            isMainMenuScreen = true;
        }
        else
        {
            isMainMenuScreen = false;
        }
        if (FB.IsInitialized)
        {
            if (!FB.IsLoggedIn)
            {
                FB.LogInWithPublishPermissions(new List<string>() { "publish_actions" }, HandleResult);
            }
            else
            {
                FB.ShareLink(
                    new Uri(OpenGameLink()),
                    shareTitle,
                    string.Format("I have collected {0} Laddus. Can you beat my score?", HighScore),
                    new Uri(shareImage),
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
            Analytics.CustomEvent("Facebook share error: " + result.Error);
        }
        else if (result.Cancelled)
        {
            FacebookLog("Cancelled Response:\n" + result.RawResult);
            Analytics.CustomEvent("Facebook share cancelled");
        }
        else if (!string.IsNullOrEmpty(result.RawResult))
        {
            bool success = result.ResultDictionary.ContainsKey("postId") || (result.ResultDictionary.ContainsKey("posted") && (bool)result.ResultDictionary["posted"] == true);
            if (success)
            {
                Analytics.CustomEvent("Facebook share");
                FacebookLog("Success Response:\n" + result.RawResult);
                PlayerPrefsStorage.SaveData(GameData.KEY_LIVES, -1);
                if (isMainMenuScreen)
                {
                    //PlayButtonObj.SetActive(true);
                    //ShareTextObj.SetActive(false);
                }
            }
        }
        else
        {
            FacebookLog("Empty Response\n");
        }
    }

    private void SetInit()
    {
        FacebookLog("SetInit");
        if (!FB.IsInitialized)
        {
            FB.Init(OnInitComplete, OnHideUnity);
        }

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

    public void OnSelectLevel()
    {
        LevelsScreen.SetActive(true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        gameState = GameState.InGame;
        SwitchGameState(GameState.InGame);
    }

    public void OnTapToPlay()
    {
        TapToPlayCount += 1;
        PlayerPrefsStorage.SaveData(GameData.KEY_TAPTOPLAY, TapToPlayCount);
        TapToPlayObj.SetActive(false);
    }

    public void ResumeGame()
    {
        gameState = GameState.InGame;
        SwitchGameState(GameState.InGame);
    }

    public void QuitToMainMenu()
    {
        //gameState = GameState.MainMenu;
        SwitchGameState(GameState.MainMenu);
        //QuitConfirmation.SetActive(true);
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        gameState = GameState.InGame;
        SwitchGameState(GameState.InGame);
        ToggleGamePauseState(false);
    }

    public void OpenSettingsScreen(bool value)
    {
        if (value)
        {
            SettingsScreen.SetActive(true);
        }
        else
        {
            SettingsScreen.SetActive(false);
        }

    }

    public void OpenSelectLanguageScreen(bool value)
    {
        if (value)
        {
            LanguageSelection.SetActive(true);
        }
        else
        {
            LanguageSelection.SetActive(false);
        }

    }

    public void ShowAchievements()
    {
        AvGameServices.ShowAchievements();
    }

    public void ShowLeaderboards()
    {
        AvGameServices.ShowLeaderBoard();
    }

    public void OpenStoreScreen(bool value)
    {
        if (value)
        {
            StoreScreen.SetActive(true);
        }
        else
        {
            StoreScreen.SetActive(false);
        }

    }

    public void ToggleMusicMainMenu()
    {
        if (MusicToggle_MainMenu.isOn)
        {
            AudioListener.volume = 1;
        }
        else
        {
            AudioListener.volume = 0;
        }
    }

    public void ToggleMusicPauseMenu()
    {
        if (MusicToggle_PauseMenu.isOn)
        {
            AudioListener.volume = 1;
        }
        else
        {
            AudioListener.volume = 0;
        }
    }

    public void InfoScreen_Button()
    {
        InfoScreen.SetActive(true);
        Analytics.CustomEvent("Checked credits!");
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
        ToggleGamePauseState(true);

        if (state == GameState.MainMenu)
        {
            MainMenuScreen.SetActive(true);
            gameState = GameState.MainMenu;
        }

        if (state == GameState.GameOver)
        {
            GameOverScreen.SetActive(true);
            DisplayGameOverItems();
        }

        if (state == GameState.InGame)
        {
            HUDScreen.SetActive(true);
            ToggleGamePauseState(false);
            DisplayInGameItems();

        }

        if (state == GameState.Pause)
        {
            PauseScreen.SetActive(true);
        }
    }


    public void SelectLevel(int num)
    {

        PlayerPrefsStorage.SaveData(GameData.KEY_LEVEL_TYPE, num - 1);
        if (num == 2)
        {
          
        }
        StartGame();
    }

    public void CloseLevelSelection()
    {
        LevelsScreen.SetActive(false);
    }

    void DisplayInGameItems()
    {
        if (TapToPlayCount == 0)
        {
            TapToPlayObj.SetActive(true);
        }
        else
        {
            TapToPlayObj.SetActive(false);
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

    void DisplayGameOverItems()
    {
        Analytics.CustomEvent("Game over");
#if UNITY_ANDROID || UNITY_IOS
        if (Advertisement.IsReady())
        {
            int random = UnityEngine.Random.Range(1, 4);
            if (random == 3)
            {
                Debug.Log("Showing ad");
                Advertisement.Show();
            }
        }
#endif
        if (HanumanController.currentScore > HighScore)     //New high score
        {
            HighScore = HanumanController.currentScore;
            PlayerPrefsStorage.SaveData(GameData.KEY_HIGHSCORE, HighScore);
            NewBest_TextObj.SetActive(true);
            Analytics.CustomEvent("Highscore: " + HighScore.ToString());
            AvGameServices.SubmitScore(HighScore);
        }
        else
        {
            NewBest_TextObj.SetActive(false);
        }

        HighScore_Text.text = HighScore.ToString();
        Score_Text.text = HanumanController.currentScore.ToString();

        AchievementsScript.SaveDataAndUnlockAchievements(HanumanController.currentScore, 0, 0, 0, HanumanController.enemiesKilled);

        if (lives > 0)
        {
            lives--;
            PlayerPrefsStorage.SaveData(GameData.KEY_LIVES, lives);
        }

        if (lives == -1)
        {
            Lives_Text.text = 8.ToString();
            Lives_Text.rectTransform.localEulerAngles = new Vector3(0, 0, 90);
        }
        else
        {
            Lives_Text.text = lives.ToString();
            Lives_Text.rectTransform.localEulerAngles = new Vector3(0, 0, 0);
        }
    }

    [System.Serializable]
    public class UiPanels
    {
        public string Name;
        public GameObject ObjectReference;
    }
}