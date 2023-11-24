using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsController 
{
    public static AdsController instance;
    public IAdHandler adHandler;
    public static void Init()
    {
        instance = new AdsController();
    }
    public AdsController()
    {
#if UNITY_WEBGL
        adHandler = new AdHandlerWebgl();
#else
        adHandler = new AdHandlerMobile();

#endif
    }
    public AdsController GetInstance()
    {
        return instance;
    }
}
