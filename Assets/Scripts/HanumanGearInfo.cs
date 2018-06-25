using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GearType
{
    Default,
    SilverArmor,
    SilverArmorWithGada,
    GoldArmor,
    GoldArmorWithGada
};

public enum LevelType
{
    Forest,
    Snow,
    Lava,
    Beach
}

public class HanumanGearInfo : MonoBehaviour
{
    public string currentHanumanGear = string.Empty;
    public bool gadaUnlocked = false;
    public static GearType _gearType = GearType.Default;
    public LevelType _levelType;
    public HanumanController _HanumanController;
    public Animator HanumanAnimator;
    public AnimatorOverrideController[] AnimControllersList;
    public static int levelIndex = 0;

    void Start()
    {
        //_levelType = (LevelType)PlayerPrefsStorage.GetIntData(GameData.KEY_LEVEL_TYPE, 0);
        //_gearType = (GearType)PlayerPrefsStorage.GetIntData(GameData.KEY_GEAR_TYPE, 0);
        //Debug.Log("Level type = " + _levelType);
        //_HanumanController = GetComponent<HanumanController>();
        //SetLevelType();
        //SetAnimController((GearType)PlayerPrefsStorage.GetIntData(GameData.KEY_GEAR_TYPE, 0));
        //SetAnimController(_gearType);
    }

    public void SetLevelType(LevelType levelType)
    {
        levelIndex = (int)levelType;
        foreach (var item in _HanumanController.ParallaxObjects)
        {
            item.gameObject.SetActive(false);
        }
        
        UiManager.instance.StartGame();
        Debug.Log("Level type = " + levelType);
        _levelType = levelType;

    }

    public void SetAnimController(GearType gearType)
    {
        Debug.Log("Gear type = " + gearType);
        _gearType = gearType;
        HanumanAnimator.runtimeAnimatorController = AnimControllersList[(int)gearType];
    }
}   