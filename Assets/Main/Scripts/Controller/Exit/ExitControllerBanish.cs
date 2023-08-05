using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitControllerBanish : ExitController
{

    private Action onUpdate = () => { };
    private Material material;
    private float currentTransparency = 0.01f;
    private float fadeSpeed = 0.25f;
    protected override void Start()
    {
        currentTransparency = 0.01f;
        base.Start();
        material = GetComponent<MeshRenderer>().material;
    }
    protected override void Animation()
    {
        onUpdate = BanishUpdate;

    }
    private void Update()
    {
        onUpdate();
    }
    void BanishUpdate()
    {
        currentTransparency += Time.deltaTime * fadeSpeed;
        material.SetFloat("_Cutoff", currentTransparency);
        if (currentTransparency > 0.49f)
        {
            OnComplete();
            onUpdate = () => { };
        }
    }
}
