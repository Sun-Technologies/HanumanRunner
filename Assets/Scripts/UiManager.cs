using Facebook.Unity;
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
    public int Score = 0;

    public int lives = 5;

    public int shareCount = 0;

    public int TapToPlayCount = 0;

    public const string SCOREKEY = "ScoreKey";

    public const string LIVESKEY = "LivesKey";

    public const string FBSHAREKEY = "FbShareKey";

    public const string DATETIMEKEY = "DateTimeKey";

    public const string TAPTOPLAYKEY = "TapToPlayKey";

    const int DAILYLIVES = 40;

    public bool isGamePaused = false;

    DateTime SavedDateTime = DateTime.Today;

    bool isMainMenuScreen = false;

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
    public GameObject PlayButtonObj;
    public GameObject ShareTextObj;

    #endregion

    #region FacebookVariables

    private string shareLink = "http://www.aavegainteractive.com/";
    private string shareTitle = "Hanuman The Run HD";
    private string shareImage = "https://s3.amazonaws.com/hanumanrun/facebookshare/facebookshare.png";
    private string feedMediaSource = string.Empty;
    

    #endregion

    void Awake()
    {
        if (!PlayerPrefs.HasKey(LIVESKEY))
        {
            PlayerPrefsStorage.SaveData(LIVESKEY, DAILYLIVES);
        }
        if (instance == null)
        {
            instance = this;
        }

        Debug.Log("State = " + gameState);
        SetInit();
    }

    void Start()
    {
        Debug.Log(SavedDateTime);
        //PlayerPrefsStorage.SaveData(LivesKey, 1);

        if (string.IsNullOrEmpty(PlayerPrefsStorage.GetStringData(DATETIMEKEY)))
        {
            PlayerPrefsStorage.SaveData(DATETIMEKEY, SavedDateTime.ToString());
        }

        SavedDateTime = DateTime.Parse(PlayerPrefsStorage.GetStringData(DATETIMEKEY));

        if (SavedDateTime.CompareTo(DateTime.Today) > 0)
        {
            Debug.Log("Date changer. Updating timer.");
            SavedDateTime.AddDays(1);
            PlayerPrefsStorage.SaveData(LIVESKEY, DAILYLIVES);
        }

        Score = PlayerPrefsStorage.GetIntData(SCOREKEY, 0);
        lives = PlayerPrefsStorage.GetIntData(LIVESKEY, DAILYLIVES);
        shareCount = PlayerPrefsStorage.GetIntData(FBSHAREKEY, 0);
        TapToPlayCount = PlayerPrefsStorage.GetIntData(TAPTOPLAYKEY, 0);
        if (gameState == GameState.MainMenu)
        {
            SwitchGameState(GameState.MainMenu);
        }
        else
        {
            SwitchGameState(GameState.InGame);
        }
        ToggleShareText();
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
                    string.Format("I have collected {0} Laddus. Can you beat my score?", Score),
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
                shareCount++;
                PlayerPrefsStorage.SaveData(FBSHAREKEY, shareCount);
                FacebookLog("Success Response:\n" + result.RawResult);
                PlayerPrefsStorage.SaveData(LIVESKEY, -1);
                ToggleShareText();
                Restart_Button.interactable = true;
                Home_Button.interactable = true;
                Lives_Text.text = 8.ToString();
                Lives_Text.rectTransform.localEulerAngles = new Vector3(0, 0, 90);
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

    private void ToggleShareText()
    {
        if (shareCount > 0)
        {
            ScoreShare_Text.gameObject.SetActive(true);
            GameShare_Text.gameObject.SetActive(false);
        }
        else
        {
            GameShare_Text.gameObject.SetActive(true);
            ScoreShare_Text.gameObject.SetActive(false);
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

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        gameState = GameState.InGame;
        SwitchGameState(GameState.InGame);
    }

    public void OnTapToPlay()
    {
        TapToPlayCount += 1;
        PlayerPrefsStorage.SaveData(TAPTOPLAYKEY, TapToPlayCount);
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
        foreach (UiPanels item in UipanelsList)
        {
            item.ObjectReference.SetActive(false);
        }

        MainMenuScreen.SetActive(false);
        GameOverScreen.SetActive(false);
        PauseScreen.SetActive(false);
        HUDScreen.SetActive(false);
        ToggleGamePauseState(true);

        if (state == GameState.MainMenu)
        {
            MainMenuScreen.SetActive(true);
            gameState = GameState.MainMenu;
            DisplayMainMenuItems();
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
        switch (num)
        {
            case 1:

                break;

            case 2:

                break;

            case 3:

                break;
        }
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

    void DisplayMainMenuItems()
    {
        //if (lives == 0)
        //{
        //    PlayButtonObj.SetActive(false);
        //    ShareTextObj.SetActive(true);
        //}
        //else
        //{
        PlayButtonObj.SetActive(true);
        ShareTextObj.SetActive(false);
        //}
    }

    void DisplayGameOverItems()
    {
        Analytics.CustomEvent("Game over");
#if UNITY_ANDROID || UNITY_IOS
        if (Advertisement.IsReady())
        {
            //Advertisement.Show();
        }
#endif
        if (HanumanController.currentScore > Score)     //New high score
        {
            Score = HanumanController.currentScore;
            PlayerPrefsStorage.SaveData(SCOREKEY, Score);
            NewBest_TextObj.SetActive(true);
            Analytics.CustomEvent("Highscore: " + Score.ToString());
        }
        else
        {
            NewBest_TextObj.SetActive(false);
        }

        HighScore_Text.text = Score.ToString();
        Score_Text.text = HanumanController.currentScore.ToString();

        if (lives > 0)
        {
            lives--;
            PlayerPrefsStorage.SaveData(LIVESKEY, lives);
        }

        //if (lives > 0 || lives == -1)
        //{
            Restart_Button.interactable = true;
            Home_Button.interactable = true;
        //}
        //else
        //{
        //    Restart_Button.interactable = false;
        //    Home_Button.interactable = false;
        //}

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