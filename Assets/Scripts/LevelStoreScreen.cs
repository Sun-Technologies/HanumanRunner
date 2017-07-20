using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelStoreScreen : MonoBehaviour
{
    const int SNOW_COST = 1500;
    const int LAVA_COST = 2000;

    public UiManager _UiManager;
    public HanumanGearInfo _HanumanGearInfo;

    public GameObject[] CostButtons;
    public GameObject[] PurchasedButtons;

    public GameObject brokeMessage;

    int ladduCount = 0;

    int savedLevelsType;

    public TextElements _textElements;

    void Start()
    {
        _textElements = GetComponent<TextElements>();
        for (int i = 0; i < _textElements.lbl_Selected.Length; i++)
        {
            _textElements.lbl_Selected[i].text = LocalizationText.GetText(GameData.STR_SELECT);
        }
        savedLevelsType = PlayerPrefsStorage.GetIntData(GameData.KEY_LEVEL_TYPE, 0);
        _UiManager = GetComponent<UiManager>();
        _HanumanGearInfo = FindObjectOfType<HanumanGearInfo>();
        SetButtonsContent();
        Debug.Log("Laddu count = " + PlayerPrefsStorage.GetIntData(GameData.KEY_LADDUS_COLLECTED_COUNT, 0));
        ladduCount = PlayerPrefsStorage.GetIntData(GameData.KEY_LADDUS_COLLECTED_COUNT, 0);
    }

    void SelectButton(int index)
    {
        string str = LocalizationText.GetText(GameData.STR_SELECT);
        for (int i = 0; i < PurchasedButtons.Length; i++)
        {
            PurchasedButtons[i].transform.GetComponentInChildren<Text>().text = str;
            //if (i == index)
            //{
            //    PurchasedButtons[i].transform.GetComponentInChildren<Text>().text = LocalizationText.GetText(GameData.STR_SELECTED);
            //}
            //else
            //{
            //    PurchasedButtons[i].transform.GetComponentInChildren<Text>().text = LocalizationText.GetText(GameData.STR_SELECT);
            //}
        }
    }

    void SetButtonsContent()
    {
        if (PlayerPrefsStorage.GetIntData(GameData.KEY_MAPS_UNLOCKED, 0) == 0)
        {
            CostButtons[0].SetActive(true);
            PurchasedButtons[0].SetActive(false);
            if (savedLevelsType == 2)
            {
                SelectButton(0);
            }
        }


        if (PlayerPrefsStorage.GetIntData(GameData.KEY_MAP_SNOW_UNLOCKED, 0) == 0)  //Snow
        {
            CostButtons[1].SetActive(true);
            PurchasedButtons[1].SetActive(false);
        }
        else
        {
            CostButtons[1].SetActive(false);
            PurchasedButtons[1].SetActive(true);

            if (savedLevelsType == 2)
            {
                SelectButton(1);
            }
        }

        if (PlayerPrefsStorage.GetIntData(GameData.KEY_MAP_LAVA_UNLOCKED, 0) == 0)  //Lava
        {
            CostButtons[2].SetActive(true);
            PurchasedButtons[2].SetActive(false);
        }
        else
        {
            CostButtons[2].SetActive(false);
            PurchasedButtons[2].SetActive(true);

            if (savedLevelsType == 3)
            {
                SelectButton(2);

            }
        }
    }

    void SelectSnowLevel()
    {
        if (PlayerPrefsStorage.GetIntData(GameData.KEY_MAP_SNOW_UNLOCKED, 0) == 0)    //not bought
        {
            if (ladduCount >= SNOW_COST)
            {
                ladduCount -= SNOW_COST;
                PlayerPrefsStorage.SaveData(GameData.KEY_LADDUS_COLLECTED_COUNT, ladduCount);
                PlayerPrefsStorage.SaveData(GameData.KEY_MAP_SNOW_UNLOCKED, 1);
                PlayerPrefsStorage.SaveData(GameData.KEY_LEVEL_TYPE, 2);
                AchievementsScript.SaveDataAndUnlockAchievements(0, 1, 0, 0, 0);
                SetButtonsContent();
                _HanumanGearInfo.SetLevelType(LevelType.Snow);
            }
            else
            {
                ShowBrokeMessage(true);
            }
        }
        else
        {
            _HanumanGearInfo.SetLevelType(LevelType.Snow); //bought
        }
    }

    void SelectLavaLevel()
    {
        if (PlayerPrefsStorage.GetIntData(GameData.KEY_MAP_LAVA_UNLOCKED, 0) == 0)  //Not bought
        {
            if (ladduCount >= LAVA_COST)
            {
                ladduCount -= LAVA_COST;
                PlayerPrefsStorage.SaveData(GameData.KEY_LADDUS_COLLECTED_COUNT, ladduCount);
                PlayerPrefsStorage.SaveData(GameData.KEY_MAP_LAVA_UNLOCKED, 1);
                PlayerPrefsStorage.SaveData(GameData.KEY_LEVEL_TYPE, 3);
                AchievementsScript.SaveDataAndUnlockAchievements(0, 1, 0, 0, 0);
                SetButtonsContent();
                _HanumanGearInfo.SetLevelType(LevelType.Lava);
            }
            else
            {
                ShowBrokeMessage(true);
            }
        }
        else
        {
            _HanumanGearInfo.SetLevelType(LevelType.Lava);  //bought
        }
    }

    public void SelectLevel(int index)
    {
        switch (index)
        {
            case 1:
                PlayerPrefsStorage.SaveData(GameData.KEY_LEVEL_TYPE, 0);
                _HanumanGearInfo.SetLevelType(LevelType.Forest);
                break;

            case 2:
                PlayerPrefsStorage.SaveData(GameData.KEY_LEVEL_TYPE, 1);
                SelectSnowLevel();
                break;

            case 3:
                PlayerPrefsStorage.SaveData(GameData.KEY_LEVEL_TYPE, 2);
                SelectLavaLevel();
                break;
        }
    }

    public void ShowBrokeMessage(bool value)
    {
        brokeMessage.SetActive(value);
    }
}
