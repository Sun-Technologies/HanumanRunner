using Facebook.Unity;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameState { MainMenu, InGame, GameOver, Pause, Info };
enum DateTimer { SameDay, NewDay, Backdated};
public class UiManager : MonoBehaviour
{
    public static UiManager instance = null;
    public static GameState gameState = GameState.MainMenu;
    public AudioListener listener;
    public int Score = 0;

    public int lives = 5;

    public int shareCount = 0;

    public const string ScoreKey = "ScoreKey";

    public const string LivesKey = "LivesKey";

    public const string FbShareKey = "FbShareKey";

    public const string DateTimeKey = "DateTimeKey";

    public bool isGamePaused = false;

    DateTime SavedDateTime = DateTime.Today;
    DateTimer dateTimer;

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

    void OnEnable()
    {
        //SwitchGameState(GameState.MainMenu);
    }

    void Awake()
    {
        listener = FindObjectOfType<AudioListener>();
        if (instance == null)
        {
            instance = this;
        }
        //else if (instance != this)
        //{
        //    Destroy(gameObject);
        //}
        //DontDestroyOnLoad(gameObject);
        Debug.Log("State = " + gameState);
        SetInit();
    }

    void Start()
    {
        Debug.Log(SavedDateTime);

        if (string.IsNullOrEmpty(PlayerPrefsStorage.GetStringData(DateTimeKey)))
        {
            PlayerPrefsStorage.SaveData(DateTimeKey, SavedDateTime.ToString());
        }

        if (SavedDateTime.CompareTo(DateTime.Today) > 0)
        {
            dateTimer = DateTimer.NewDay;
            Debug.Log("Date changer. Updating timer.");
            SavedDateTime.AddDays(1);
        }
        if (SavedDateTime.CompareTo(DateTime.Today) == 0)
        {
            dateTimer = DateTimer.SameDay;
        }

        Score = PlayerPrefsStorage.GetIntData(ScoreKey, 0);
        lives = PlayerPrefsStorage.GetIntData(LivesKey, 5);
        shareCount = PlayerPrefsStorage.GetIntData(FbShareKey, 0);
        if (gameState == GameState.MainMenu)
        {
            SwitchGameState(GameState.MainMenu);
        }
        else
        {
            SwitchGameState(GameState.InGame);
        }
        
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
            listener.enabled = true;
        }
        else
        {
            listener.enabled = false;
        }
    }

    public void ToggleMusicPauseMenu()
    {
        if (MusicToggle_PauseMenu.isOn)
        {
            listener.enabled = true;
        }
        else
        {
            listener.enabled = false;
        }
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

    void DisplayGameOverItems()
    {
        if (HanumanController.currentScore > Score)     //New high score
        {
            Score = HanumanController.currentScore;
            PlayerPrefsStorage.SaveData(ScoreKey, Score);
            NewBest_TextObj.SetActive(true);
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
        }
        
        Lives_Text.text = lives.ToString();
        PlayerPrefsStorage.SaveData(LivesKey, lives);
    }
}