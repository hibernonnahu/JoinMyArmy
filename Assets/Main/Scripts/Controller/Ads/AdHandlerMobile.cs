using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdHandlerMobile : IAdHandler
{
    private Action onSuccess = () => { };
    private Action onFail = () => { };

    public bool IsAvailable()
    {
        return true;
    }
    public void RewardAd(Action onSuccess, Action onFail)
    {
        this.onSuccess = onSuccess;
        this.onFail = onFail;
        OnSuccess();
       
    }

    private void OnSuccess()
    {
        onSuccess();
    }

    private void OnFail()
    {
        onFail();
    }
}
