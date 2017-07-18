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
    Lava
}

public class HanumanGearInfo : MonoBehaviour
{
    public string currentHanumanGear = string.Empty;
    public bool gadaUnlocked = false;
    public GearType _gearType;
    public LevelType _levelType;
    public HanumanController _HanumanController;
    public Animator HanumanAnimator;
    public AnimatorOverrideController[] AnimControllersList;

    void Start()
    {
        _levelType = (LevelType)PlayerPrefsStorage.GetIntData(GameData.KEY_LEVEL_TYPE, 0);
        _gearType = (GearType)PlayerPrefsStorage.GetIntData(GameData.KEY_GEAR_TYPE, 0);
        Debug.Log("Level type = " + _levelType);
        //_HanumanController = GetComponent<HanumanController>();
        SetLevelType();
        //SetAnimController((GearType)PlayerPrefsStorage.GetIntData(GameData.KEY_GEAR_TYPE, 0));
        //SetAnimController(_gearType);
    }

    void SetLevelType()
    {
        foreach (var item in _HanumanController.ParallaxObjects)
        {
            item.gameObject.SetActive(false);
        }
        _HanumanController.ParallaxObjects[(int)_levelType].gameObject.SetActive(true);
        //_HanumanController.LevelObjects[(int)_levelType].gameObject.SetActive(true);
    }

    public void SetAnimController(GearType gearType)
    {
        Debug.Log("Gear type = " + gearType);
        HanumanAnimator.runtimeAnimatorController = AnimControllersList[(int)gearType];
    }
}   