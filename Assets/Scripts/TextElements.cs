using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextElements : MonoBehaviour
{
    public Text[] TextComponents;
    public Transform CanvasRootTransform;

    public string _language = LocalizationText.GetLanguage();

    public Text lbl_SelectLanguage;
    public Text btn_English;
    public Text btn_Hindi;
    public Text btn_TapToFly;
    public Text lbl_Reward;
    public Text lbl_Paused;
    public Text lbl_Score;
    public Text lbl_HighScore;
    public Text lbl_NewBest;
    public Text lbl_Sharegame;
    public Text lbl_Settings;
    public Text lbl_DailyBonus;
    public Text lbl_RewardsHeader;
    public Text lbl_RewardsDescription;
    public Text lbl_RewardsReceived;
    public Text btn_Day;
    public Text lbl_Levels;
    public Text btn_Forest;
    public Text btn_Snow;
    public Text btn_Lava;
    public Text lbl_Store;
    public Text btn_GoldArmor;
    public Text btn_SilverArmor;
    public Text btn_NoArmor;
    public Text lbl_FbShareSettings;
    public Text lbl_FbShareGameOver;
    public string txt_FbShareInGame;

    void Awake()
    {
        UpdateLanguage();
    }

    public void UpdateLanguage()
    {
        if (LocalizationText.GetLanguage() != _language)
        {
            _language = LocalizationText.GetLanguage();
            SetTextsLanguage();
        }
        
        Debug.Log("Changed language to " + _language);
        TextComponents = CanvasRootTransform.GetComponentsInChildren<Text>(true);
        foreach (Text item in TextComponents)
        {
            SetComponent(item);
        }
    }

    void Start()
    {

        SetTextsLanguage();
        SetDaysText();
    }

    void SetTextsLanguage()
    {
        lbl_SelectLanguage.text = LocalizationText.GetText("lblSelectLanguage");
        btn_English.text = LocalizationText.GetText("btnEnglish");
        btn_Hindi.text = LocalizationText.GetText("btnHindi");
        btn_TapToFly.text = LocalizationText.GetText("btnTapToFly");
        //lbl_Reward.text = LocalizationText.GetText("lblReward");
        lbl_Paused.text = LocalizationText.GetText("lblPaused");
        lbl_Score.text = LocalizationText.GetText("lblScore");
        lbl_HighScore.text = LocalizationText.GetText("lblHighScore");
        lbl_NewBest.text = LocalizationText.GetText("lblNewBest");
        //lbl_Sharegame.text = LocalizationText.GetText("lblSharegame");
        lbl_Settings.text = LocalizationText.GetText("lblSettings");
        lbl_DailyBonus.text = LocalizationText.GetText("lblDailyBonus");
        lbl_RewardsHeader.text = LocalizationText.GetText("lblRewardsHeader");
        lbl_RewardsDescription.text = LocalizationText.GetText("lblRewardsDescription");
        lbl_RewardsReceived.text = LocalizationText.GetText("lblRewardsReceived");
        lbl_Levels.text = LocalizationText.GetText("lblLevels");
        btn_Forest.text = LocalizationText.GetText("btnForest");
        btn_Snow.text = LocalizationText.GetText("btnSnow");
        btn_Lava.text = LocalizationText.GetText("btnLava");
        //lbl_Store.text = LocalizationText.GetText("lblStore");
        //btn_GoldArmor.text = LocalizationText.GetText("btnGoldArmor");
        //btn_SilverArmor.text = LocalizationText.GetText("btnSilverArmor");
        //btn_NoArmor.text = LocalizationText.GetText("btnNoArmor");
        //lbl_FbShareGameOver.text = LocalizationText.GetText("lblFbShareGameOver");
        //lbl_FbShareSettings.text = LocalizationText.GetText("lblFbShareSettings");
        //txt_FbShareInGame = LocalizationText.GetText("txtFbShare");
    }

    void SetDaysText()
    {
        for (int i = 0; i < UiManager.instance.DaysButtons.Length; i++)
        {
            UiManager.instance.DaysButtons[i].text = string.Format("{0} {1}", LocalizationText.GetText("btnDay"), i);
        }
    }

    private void SetComponent(Text item)
    {
        if (item.name == "lbl_SelectLanguage")
            lbl_SelectLanguage = item;

        if (item.name == "btn_English")
            btn_English = item;

        if (item.name == "btn_Hindi")
            btn_Hindi = item;

        if (item.name == "btn_TapToFly")
            btn_TapToFly = item;

        if (item.name == "lbl_Reward")
            lbl_Reward = item;

        if (item.name == "lbl_Paused")
            lbl_Paused = item;

        if (item.name == "lbl_Score")
            lbl_Score = item;

        if (item.name == "lbl_HighScore")
            lbl_HighScore = item;

        if (item.name == "lbl_NewBest")
            lbl_NewBest = item;

        if (item.name == "lbl_ScoreShareGameOver")
            lbl_Sharegame = item;

        if (item.name == "lbl_Settings")
            lbl_Settings = item;

        if (item.name == "lbl_DailyBonus")
            lbl_DailyBonus = item;

        if (item.name == "lbl_RewardsHeader")
            lbl_RewardsHeader = item;

        if (item.name == "lbl_RewardsDescription")
            lbl_RewardsDescription = item;

        if (item.name == "lbl_RewardsReceived")
            lbl_RewardsReceived = item;

        if (item.name == "btn_Day")
            btn_Day = item;

        if (item.name == "lbl_Levels")
            lbl_Levels = item;

        if (item.name == "btn_Forest")
            btn_Forest = item;

        if (item.name == "btn_Snow")
            btn_Snow = item;

        if (item.name == "btn_Lava")
            btn_Lava = item;

        if (item.name == "lbl_Store")
            lbl_Store = item;

        if (item.name == "btn_GoldArmor")
            btn_GoldArmor = item;

        if (item.name == "btn_SilverArmor")
            btn_SilverArmor = item;

        if (item.name == "btn_NoArmor")
            btn_NoArmor = item;

        if (item.name == "lbl_FbShareGameOver")
            lbl_FbShareGameOver = item;

        if (item.name == "lbl_FbShareSettings")
            lbl_FbShareSettings = item;
    }
}
