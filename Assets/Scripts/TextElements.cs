using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextElements : MonoBehaviour
{
    public Text[] TextComponents;
    public Transform CanvasRootTransform;

    public Font EnglishFont;
    public Font HindiFont;

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
    public Text btn_Beach;
    public Text lbl_Store;
    public Text btn_GoldArmor;
    public Text btn_SilverArmor;
    public Text btn_NoArmor;
    public Text lbl_FbShareSettings;
    public Text lbl_FbShareGameOver;
    public Text btn_PurchasedSilver;
    public Text btn_PurchasedGold;
    public string txt_FbShareInGame;
    public Text[] lbl_NoLaddu;
    public Text[] lbl_Message;
    public Text[] lbl_Selected;
    public Text[] btn_Ok;

    void Awake()
    {
        UpdateLanguage();
    }

    public void UpdateLanguage()
    {
        string _lang = (PlayerPrefsStorage.GetStringData(GameData.KEY_LANGUAGE));
        Debug.Log("Current language = " + _lang);
        if (string.IsNullOrEmpty(_lang))
        {
            _lang = LocalizationText.GetLanguage();
        }
        LocalizationText.SetLanguage(_lang);
        SetDaysText();
        if (LocalizationText.GetLanguage() != _language)
        {
            _language = LocalizationText.GetLanguage();
            
            Debug.Log("Changed language to " + _language);
        }

        TextComponents = CanvasRootTransform.GetComponentsInChildren<Text>(true);
        foreach (Text item in TextComponents)
        {
            if (_language == GameData.LANG_ENGLISH)
            {
                if (item.name != "Text")
                {
                    item.font = EnglishFont;
                }
            }
            else if (_language == GameData.LANG_HINDI)
            {
                if (item.name != "Text")
                {
                    item.font = HindiFont;
                }
            }

            SetComponent(item);
        }

        SetTextsLanguage();
    }

    void SetTextsLanguage()
    {
        lbl_SelectLanguage.text = LocalizationText.GetText("lblSelectLanguage");
        //btn_English.text = LocalizationText.GetText("btnEnglish");
        //btn_Hindi.text = LocalizationText.GetText("btnHindi");
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
        //lbl_RewardsReceived.text = LocalizationText.GetText("lblRewardsReceived");
        lbl_Levels.text = LocalizationText.GetText("lblLevels");
        btn_Forest.text = LocalizationText.GetText("btnForest");
        btn_Snow.text = LocalizationText.GetText("btnSnow");
        btn_Lava.text = LocalizationText.GetText("btnLava");
        btn_Beach.text = LocalizationText.GetText("btnBeach");
        lbl_Store.text = LocalizationText.GetText("lblStore");
        //btn_GoldArmor.text = LocalizationText.GetText("btnGoldArmor");
        //btn_SilverArmor.text = LocalizationText.GetText("btnSilverArmor");
        //btn_NoArmor.text = LocalizationText.GetText("btnNoArmor");
        lbl_FbShareGameOver.text = LocalizationText.GetText("lblFbShareGameOver");
        lbl_FbShareSettings.text = LocalizationText.GetText("lblFbShareSettings");
        btn_PurchasedSilver.text = LocalizationText.GetText("btnPurchased");
        btn_PurchasedGold.text = LocalizationText.GetText("btnPurchased");
        //btn_Select.text = LocalizationText.GetText("btnSelect");
        //btn_Selected.text = LocalizationText.GetText("btnSelected");
        //lbl_NoLaddu.text = LocalizationText.GetText("lblNoLaddu");
        //txt_FbShareInGame = LocalizationText.GetText("txtFbShare");
    }

    void SetDaysText()
    {
        for (int i = 0; i < UiManager.instance.DaysButtons.Length; i++)
        {
            UiManager.instance.DaysButtons[i].text = string.Format("{0} {1}", LocalizationText.GetText("btnDay"), i+1);
        }

        for (int i = 0; i < lbl_NoLaddu.Length; i++)
        {
            lbl_NoLaddu[i].text = LocalizationText.GetText("lblNoLaddu");
        }

        for (int i = 0; i < lbl_Message.Length; i++)
        {
            lbl_Message[i].text = LocalizationText.GetText("lblMessage");
        }

        for (int i = 0; i < lbl_Selected.Length; i++)
        {
            lbl_Selected[i].text = LocalizationText.GetText("lblSelected");
        }

        for (int i = 0; i < btn_Ok.Length; i++)
        {
            btn_Ok[i].text = LocalizationText.GetText("btnOk");
        }
    }

    private void SetComponent(Text item)
    {
        if (item.name == "lbl_SelectLanguage")
        {
            lbl_SelectLanguage = item;
            return;
        }

        if (item.name == "btn_English")
        {
            btn_English = item;
            return;
        }

        if (item.name == "btn_Hindi")
        {
            btn_Hindi = item;
            return;
        }

        if (item.name == "btn_TapToFly")
        {
            btn_TapToFly = item;
            return;
        }

        if (item.name == "lbl_Reward")
        {
            lbl_Reward = item;
            return;
        }

        if (item.name == "lbl_Paused")
        {
            lbl_Paused = item;
            return;
        }

        if (item.name == "lbl_Score")
        {
            lbl_Score = item;
            return;
        }

        if (item.name == "lbl_HighScore")
        {
            lbl_HighScore = item;
            return;
        }

        if (item.name == "lbl_NewBest")
        {
            lbl_NewBest = item;
            return;
        }

        if (item.name == "lbl_ScoreShareGameOver")
        {
            lbl_Sharegame = item;
            return;
        }

        if (item.name == "lbl_Settings")
        {
            lbl_Settings = item;
            return;
        }

        if (item.name == "lbl_DailyBonus")
        {
            lbl_DailyBonus = item;
            return;
        }

        if (item.name == "lbl_RewardsHeader")
        {
            lbl_RewardsHeader = item;
            return;
        }

        if (item.name == "lbl_RewardsDescription")
        {
            lbl_RewardsDescription = item;
            return;
        }

        if (item.name == "lbl_RewardsReceived")
        {
            lbl_RewardsReceived = item;
            return;
        }

        if (item.name == "btn_Day")
        {
            btn_Day = item;
            return;
        }

        if (item.name == "lbl_Levels")
        {
            lbl_Levels = item;
            return;
        }

        if (item.name == "btn_Forest")
        {
            btn_Forest = item;
            return;
        }

        if (item.name == "btn_Snow")
        {
            btn_Snow = item;
            return;
        }

        if (item.name == "btn_Lava")
        {
            btn_Lava = item;
            return;
        }

        if (item.name == "btn_Beach")
        {
            btn_Beach = item;
            return;
        }

        if (item.name == "lbl_Store")
        {
            lbl_Store = item;
            return;
        }

        if (item.name == "btn_GoldArmor")
        {
            btn_GoldArmor = item;
            return;
        }

        if (item.name == "btn_SilverArmor")
        {
            btn_SilverArmor = item;
            return;
        }

        if (item.name == "btn_NoArmor")
        {
            btn_NoArmor = item;
            return;
        }

        if (item.name == "lbl_FbShareGameOver")
        {
            lbl_FbShareGameOver = item;
            return;
        }

        if (item.name == "lbl_FbShareSettings")
        {
            lbl_FbShareSettings = item;
            return;
        }

        if (item.name == "btn_PurchasedSilver")
        {
            btn_PurchasedSilver = item;
            return;
        }

        if (item.name == "btn_PurchasedGold")
        {
            btn_PurchasedGold = item;
            return;
        }

        //if (item.name == "btn_Selected")
        //{
        //    btn_Selected[1] = item;
        //    return;
        //}

        //if (item.name == "lbl_NoLaddu")
        //{
        //    lbl_NoLaddu = item;
        //    return;
        //}
    }
}
