using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitControllerSlide : ExitController
{
    public Transform doorSlide;
    public float amount = 50;

    protected override void Animation()
    {
        LeanTween.moveLocalX(doorSlide.gameObject, doorSlide.localPosition.x + amount, 1).setOnComplete(OnComplete);
    }
}
