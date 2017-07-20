using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStoreScreen : MonoBehaviour
{
    const int SILVER_HANUMAN_COST = 2000;
    const int GOLD_HANUMAN_COST = 2000;
    const int SNOW_LEVEL_COST = 1500;
    const int LAVA_LEVEL_COST = 2000;

    public UiManager _UiManager;
    public HanumanGearInfo _HanumanGearInfo;

    public GameObject[] CostButtons;
    public GameObject[] PurchasedButtons;

    public GameObject brokeMessage;

    int ladduCount = 0;

    int savedGearType;

    void Start()
    {
        savedGearType = PlayerPrefsStorage.GetIntData(GameData.KEY_GEAR_TYPE, 0);
        _UiManager = GetComponent<UiManager>();
        _HanumanGearInfo = FindObjectOfType<HanumanGearInfo>();
        SetButtonsContent();
        Debug.Log("Laddu count = " + PlayerPrefsStorage.GetIntData(GameData.KEY_LADDUS_COLLECTED_COUNT, 0));
        ladduCount = PlayerPrefsStorage.GetIntData(GameData.KEY_LADDUS_COLLECTED_COUNT, 0);
    }

    void SetButtonsContent()
    {
        _UiManager.GetAndDisplayLadddusAvailable();
        if (PlayerPrefsStorage.GetIntData(GameData.KEY_SILVER_UNLOCKED, 0) == 0)
        {
            CostButtons[0].SetActive(true);
            PurchasedButtons[0].SetActive(false);
        }
        else
        {
            CostButtons[0].SetActive(false);
            PurchasedButtons[0].SetActive(true);

            if (savedGearType == 1 || savedGearType == 2)
            {
                PurchasedButtons[0].GetComponent<Text>().text = LocalizationText.GetText(GameData.STR_SELECT);
                if (PlayerPrefsStorage.GetIntData(GameData.KEY_GADA_UNLOCKED, 0) == 0)
                {
                    _HanumanGearInfo.SetAnimController(GearType.SilverArmor);
                }
                else
                {
                    _HanumanGearInfo.SetAnimController(GearType.SilverArmorWithGada);
                }
            }
            else
            {
                PurchasedButtons[0].GetComponent<Text>().text = LocalizationText.GetText(GameData.STR_SELECTED);
            }
        }

        if (PlayerPrefsStorage.GetIntData(GameData.KEY_GOLD_UNLOCKED, 0) == 0)
        {
            CostButtons[1].SetActive(true);
            PurchasedButtons[1].SetActive(false);
        }
        else
        {
            CostButtons[1].SetActive(false);
            PurchasedButtons[1].SetActive(true);

            if (savedGearType == 3 || savedGearType == 4)
            {
                PurchasedButtons[1].GetComponent<Text>().text = LocalizationText.GetText(GameData.STR_SELECT);
                if (PlayerPrefsStorage.GetIntData(GameData.KEY_GADA_UNLOCKED, 0) == 0)
                {
                    _HanumanGearInfo.SetAnimController(GearType.GoldArmor);
                }
                else
                {
                    _HanumanGearInfo.SetAnimController(GearType.GoldArmorWithGada);
                }

            }
            else
            {
                PurchasedButtons[1].GetComponent<Text>().text = LocalizationText.GetText(GameData.STR_SELECTED);
            }
        }
    }

    public void BuySilverHanuman()
    {
        if (PlayerPrefsStorage.GetIntData(GameData.KEY_SILVER_UNLOCKED, 0) == 0)
        {
            if (ladduCount >= SILVER_HANUMAN_COST)
            {
                ladduCount -= SILVER_HANUMAN_COST;
                PlayerPrefsStorage.SaveData(GameData.KEY_LADDUS_COLLECTED_COUNT, ladduCount);
                PurchasedButtons[0].GetComponent<Text>().text = LocalizationText.GetText(GameData.STR_SELECTED); ;
                PlayerPrefsStorage.SaveData(GameData.KEY_SILVER_UNLOCKED, 1);
                PlayerPrefsStorage.SaveData(GameData.KEY_GEAR_TYPE, 1);
                if (PlayerPrefsStorage.GetIntData(GameData.KEY_GADA_UNLOCKED, 0) == 0)
                {
                    _HanumanGearInfo.SetAnimController(GearType.SilverArmor);
                    CostButtons[0].SetActive(false);
                }
                else
                {
                    _HanumanGearInfo.SetAnimController(GearType.SilverArmorWithGada);
                    CostButtons[0].SetActive(false);
                }
            }
            else
            {
                ShowBrokeMessage(true);
            }
        }
        SetButtonsContent();
    }

    public void BuyGoldHanuman()
    {
        if (PlayerPrefsStorage.GetIntData(GameData.KEY_GOLD_UNLOCKED, 0) == 0)
        {
            if (ladduCount >= GOLD_HANUMAN_COST)
            {
                ladduCount -= GOLD_HANUMAN_COST;
                PlayerPrefsStorage.SaveData(GameData.KEY_LADDUS_COLLECTED_COUNT, ladduCount);
                PurchasedButtons[0].GetComponent<Text>().text =LocalizationText.GetText(GameData.STR_SELECTED);
                PlayerPrefsStorage.SaveData(GameData.KEY_GOLD_UNLOCKED, 1);
                PlayerPrefsStorage.SaveData(GameData.KEY_GEAR_TYPE, 3);
                if (PlayerPrefsStorage.GetIntData(GameData.KEY_GADA_UNLOCKED, 0) == 0)
                {
                    _HanumanGearInfo.SetAnimController(GearType.GoldArmor);
                    CostButtons[0].SetActive(false);
                }
                else
                {
                    _HanumanGearInfo.SetAnimController(GearType.GoldArmorWithGada);
                    CostButtons[0].SetActive(false);
                }
            }
            else
            {
                ShowBrokeMessage(true);
            }
        }
        SetButtonsContent();
    }

    public void ShowBrokeMessage(bool value)
    {
        brokeMessage.SetActive(value);
    }
}
