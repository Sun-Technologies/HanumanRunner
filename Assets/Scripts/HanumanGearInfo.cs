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

public class HanumanGearInfo : MonoBehaviour
{
    public string currentHanumanGear = string.Empty;
    void Start()
    {
         
    }

    public static string SaveToJsonString()
    {
        return null;
    }
}
