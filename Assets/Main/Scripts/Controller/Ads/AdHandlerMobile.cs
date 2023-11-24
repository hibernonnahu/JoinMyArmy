using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdHandlerMobile : IAdHandler
{
    public bool IsAvailable()
    {
        return true;
    }

    public void RewardAd(Action onSuccess, Action onFail)
    {
        
    }
}
