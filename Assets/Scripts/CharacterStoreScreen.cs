using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SelectedSkin
{
    Default,
    Silver,
    Gold
}

public enum ButtonState
{
    Buy,
    Select,
    Selected
}

public class CharacterStoreScreen : MonoBehaviour
{
    const int SILVER_HANUMAN_COST = 2000;
    const int GOLD_HANUMAN_COST = 5000;

    SelectedSkin _selectedSkin;
    ButtonState _buttonState;

    public UiManager _UiManager;
    public HanumanGearInfo _HanumanGearInfo;

    public GameObject[] CostButtons;
    public GameObject[] PurchasedButtons;

    public GameObject brokeMessage;

    int ladduCount = 0;

    int savedGearType;

    bool isGadaunlocked = false;

    void Start()
    {
        _selectedSkin = SelectedSkin.Default;
        savedGearType = PlayerPrefsStorage.GetIntData(GameData.KEY_GEAR_TYPE, 0);
        _UiManager = GetComponent<UiManager>();
        _HanumanGearInfo = FindObjectOfType<HanumanGearInfo>();
        SetSkinFromSavedData();
        ladduCount = PlayerPrefsStorage.GetIntData(GameData.KEY_LADDUS_COLLECTED_COUNT, 0);
    }

    public void SetSkinFromSavedData()
    {
        _UiManager.GetAndDisplayLadddusAvailable();
        savedGearType = PlayerPrefsStorage.GetIntData(GameData.KEY_GEAR_TYPE, 0);
        switch (savedGearType)
        {
            case 0:
                SwitchSkin(SelectedSkin.Default);
                break;
            case 1:
                SwitchSkin(SelectedSkin.Silver);
                break;
            case 2:
                SwitchSkin(SelectedSkin.Silver);
                break;
            case 3:
                SwitchSkin(SelectedSkin.Gold);
                break;
            case 4:
                SwitchSkin(SelectedSkin.Gold);
                break;
        }
    }

    public void SelectSkin(int _skin)
    {
        SwitchSkin((SelectedSkin)_skin);
    }

    void SetAnimController(SelectedSkin _skin)
    {

        if (PlayerPrefsStorage.GetIntData(GameData.KEY_GADA_UNLOCKED, 0) == 0)
        {
            isGadaunlocked = true;
        }
        else
        {
            isGadaunlocked = false;
        }

        switch (_skin)
        {
            case SelectedSkin.Default:
                _HanumanGearInfo.SetAnimController(GearType.Default);
                PlayerPrefsStorage.SaveData(GameData.KEY_GEAR_TYPE, (int)GearType.Default);
                break;

            case SelectedSkin.Silver:
                if (isGadaunlocked)
                {
                    _HanumanGearInfo.SetAnimController(GearType.SilverArmorWithGada);
                    PlayerPrefsStorage.SaveData(GameData.KEY_GEAR_TYPE, (int)GearType.SilverArmorWithGada);
                }
                else
                {
                    _HanumanGearInfo.SetAnimController(GearType.SilverArmor);
                    PlayerPrefsStorage.SaveData(GameData.KEY_GEAR_TYPE, (int)GearType.SilverArmor);
                }
                break;

            case SelectedSkin.Gold:
                if (isGadaunlocked)
                {
                    _HanumanGearInfo.SetAnimController(GearType.GoldArmorWithGada);
                    PlayerPrefsStorage.SaveData(GameData.KEY_GEAR_TYPE, (int)GearType.GoldArmorWithGada);
                }
                else
                {
                    _HanumanGearInfo.SetAnimController(GearType.GoldArmor);
                    PlayerPrefsStorage.SaveData(GameData.KEY_GEAR_TYPE, (int)GearType.GoldArmor);
                }
                break;
        }
    }

    bool IsSkinBought(SelectedSkin _skin)
    {
        switch (_skin)
        {
            case SelectedSkin.Default:
                return true;

            case SelectedSkin.Silver:
                if (PlayerPrefsStorage.GetIntData(GameData.KEY_SILVER_UNLOCKED, 0) == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            case SelectedSkin.Gold:
                if (PlayerPrefsStorage.GetIntData(GameData.KEY_GOLD_UNLOCKED, 0) == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
        }
        return false;
    }

    bool AttemptPurchasingSkin(SelectedSkin _skin)
    {
        ladduCount = PlayerPrefsStorage.GetIntData(GameData.KEY_LADDUS_COLLECTED_COUNT, 0);
        bool isPurchaseSuccessful = false;
        switch (_skin)
        {
            case SelectedSkin.Default:
                isPurchaseSuccessful = true;
                break;
            case SelectedSkin.Silver:
                if (ladduCount > SILVER_HANUMAN_COST)
                {
                    ladduCount -= SILVER_HANUMAN_COST;
                    isPurchaseSuccessful = true;
                    PlayerPrefsStorage.SaveData(GameData.KEY_SILVER_UNLOCKED, 1);
                    PlayerPrefsStorage.SaveData(GameData.KEY_GEAR_TYPE, 1);
                    AchievementsScript.SaveDataAndUnlockAchievements(0, 0, 1, 0, 0);
                }
                break;
            case SelectedSkin.Gold:
                if (ladduCount > GOLD_HANUMAN_COST)
                {
                    ladduCount -= GOLD_HANUMAN_COST;
                    isPurchaseSuccessful = true;
                    PlayerPrefsStorage.SaveData(GameData.KEY_GOLD_UNLOCKED, 1);
                    PlayerPrefsStorage.SaveData(GameData.KEY_GEAR_TYPE, 3);
                    AchievementsScript.SaveDataAndUnlockAchievements(0, 0, 0, 1, 0);
                }
                break;
        }
        PlayerPrefsStorage.SaveData(GameData.KEY_LADDUS_COLLECTED_COUNT, ladduCount);

        return isPurchaseSuccessful;
    }

    void SetThisButtonState(ButtonState _state, bool purchased, SelectedSkin _skin)
    {
        for (int i = 0; i < PurchasedButtons.Length; i++)
        {
            if (IsSkinBought((SelectedSkin)i))
            {
                CostButtons[i].SetActive(false);
                PurchasedButtons[i].SetActive(true);
                PurchasedButtons[i].GetComponent<Text>().text = LocalizationText.GetText(GameData.STR_SELECT);
            }
            else
            {
                CostButtons[i].SetActive(true);
                PurchasedButtons[i].SetActive(false);
            }
        }

        for (int i = 0; i < PurchasedButtons.Length; i++)
        {
            if ((int)_skin == i)
            {
                if (purchased)
                {
                    PurchasedButtons[i].GetComponent<Text>().text = LocalizationText.GetText(GameData.STR_SELECTED);
                    PurchasedButtons[i].SetActive(true);
                    CostButtons[i].SetActive(false);
                }
                else
                {
                    PurchasedButtons[i].SetActive(false);
                }
            }
        }
    }

    //void SetGearType(int skinType)
    //{
    //    bool gadaUnlocked = false;
    //    if (PlayerPrefsStorage.GetIntData(GameData.KEY_GADA_UNLOCKED, 0) == 0)
    //    {
    //        isGadaunlocked
    //    }
    //        switch (skinType)
    //    {
    //        case 1:
    //            PlayerPrefsStorage.SaveData(GameData.KEY_GEAR_TYPE, 0);
    //            break;
    //    }

    //    if (skinType == 0)  // Default
    //    {

    //    }

    //    if (skinType == 1 || skinType == 2) //Silver
    //    {
    //        PlayerPrefsStorage.SaveData(GameData.KEY_GEAR_TYPE, 3);
    //    }

    //    if (skinType == 3 || skinType == 4) //Silver
    //    {
    //        PlayerPrefsStorage.SaveData(GameData.KEY_GEAR_TYPE, 3);
    //    }

    //}

    void SwitchSkin(SelectedSkin _skin)
    {
        if (IsSkinBought(_skin) || AttemptPurchasingSkin(_skin))
        {
            SetAnimController(_skin);
            SetThisButtonState(ButtonState.Selected, true, _skin);
        }
        else
        {
            SetThisButtonState(ButtonState.Buy, false, _skin);
            ShowBrokeMessage(true);
        }
    }

    public void ShowBrokeMessage(bool value)
    {
        brokeMessage.SetActive(value);
    }
}
