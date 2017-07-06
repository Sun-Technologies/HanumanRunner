using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GearType
{
    NoGear,
    SilverArmor,
    GoldArmor

};

public enum LevelType
{
    Forest,
    Snow,
    Lava
}


public class HanumanGearInfo
{
    public string currentHanumanGear = string.Empty;
    public bool gadaUnlocked = false;
    public static GearType _gearType;
    public static LevelType _levelType;

    void Start()
    {
        _gearType = GearType.NoGear;
        _levelType = LevelType.Forest;
    }

    public static string SaveToJsonString()
    {
        return null;
    }
}
