using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitControllerRotate : ExitController
{
    public Transform doorRotation;

    protected override void Animation()
    {
        LeanTween.rotate(doorRotation.gameObject, Vector3.up * -120, 1).setOnComplete(OnComplete);
    }
}
