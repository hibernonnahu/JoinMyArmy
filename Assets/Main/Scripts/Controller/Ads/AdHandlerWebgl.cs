//using CrazyGames;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdHandlerWebgl: IAdHandler
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
        //CrazyAds.Instance.beginAdBreakRewarded(OnSuccess, OnFail);
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
