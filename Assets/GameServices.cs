using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameServices : MonoBehaviour
{
    static bool hasCalledInit = false;
    void Awake()
    {
        if (!hasCalledInit)
        {
            AvGameServices.Init();
            hasCalledInit = true;
        }
    }
}
