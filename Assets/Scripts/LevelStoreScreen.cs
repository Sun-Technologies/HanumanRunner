using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelStoreScreen : MonoBehaviour
{
    const int SNOW_COST = 200;
    const int LAVA_COST = 1000;
    const int BEACH_COST = 1500;
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

    private void RefreshLaddusCount()
    {
        ladduCount = PlayerPrefsStorage.GetIntData(GameData.KEY_LADDUS_COLLECTED_COUNT, 0);
    }

    void SelectButtonText()
    {
        string str = LocalizationText.GetText(GameData.STR_SELECT);
        for (int i = 0; i < PurchasedButtons.Length; i++)
        {
            PurchasedButtons[i].transform.GetComponentInChildren<Text>().text = str;
        }
    }

    public void SetButtonsContent()
    {
        SelectButtonText();
        if (PlayerPrefsStorage.GetIntData(GameData.KEY_MAPS_UNLOCKED, 0) == 0)
        {
            PurchasedButtons[0].SetActive(true);
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
        }
        if (PlayerPrefsStorage.GetIntData(GameData.KEY_MAP_BEACH_UNLOCKED, 0) == 0)  //BEACH
        {
            CostButtons[3].SetActive(true);
            PurchasedButtons[3].SetActive(false);
        }
        else
        {
            CostButtons[3].SetActive(false);
            PurchasedButtons[3].SetActive(true);
        }
    }

    void SelectSnowLevel()
    {
        RefreshLaddusCount();
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
        RefreshLaddusCount();
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

    void SelectBeachLevel()
    {
        RefreshLaddusCount();
        if (PlayerPrefsStorage.GetIntData(GameData.KEY_MAP_BEACH_UNLOCKED, 0) == 0)  //Not bought
        {
            
            if (ladduCount >= BEACH_COST)
            {
                ladduCount -= BEACH_COST;
                 PlayerPrefsStorage.SaveData(GameData.KEY_LADDUS_COLLECTED_COUNT, ladduCount);
                PlayerPrefsStorage.SaveData(GameData.KEY_MAP_BEACH_UNLOCKED, 1);
                PlayerPrefsStorage.SaveData(GameData.KEY_LEVEL_TYPE, 4);
                AchievementsScript.SaveDataAndUnlockAchievements(0, 1, 0, 0, 0);
                SetButtonsContent();
                _HanumanGearInfo.SetLevelType(LevelType.Beach);
            }
            else
            {
                ShowBrokeMessage(true);
            }
        }
        else
        {
            _HanumanGearInfo.SetLevelType(LevelType.Beach);  //bought
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
          case 4:
                PlayerPrefsStorage.SaveData(GameData.KEY_LEVEL_TYPE, 3);
                SelectBeachLevel();
                break;
        }
    }

    public void ShowBrokeMessage(bool value)
    {
        brokeMessage.SetActive(value);
    }
}
