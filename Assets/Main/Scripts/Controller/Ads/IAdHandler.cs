using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAdHandler 
{
    void RewardAd(Action onSuccess, Action onFail);
    bool IsAvailable();
}
