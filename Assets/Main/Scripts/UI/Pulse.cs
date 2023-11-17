using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulse : MonoBehaviour
{
    Vector3 initialScale;
    public float SCALE_MULTIPLIER = 1.2f;
    public float TIME = 0.7f;
    // Start is called before the first frame update
    void Awake()
    {
        OnPulseStart();
    }

    private void OnPulseStart()
    {

        initialScale = transform.localScale;
        PulseOut();

    }
    private void PulseOut()
    {
        LeanTween.scale(gameObject, initialScale * SCALE_MULTIPLIER, TIME).setIgnoreTimeScale(true).setOnComplete(PulseIn
            );
    }
    private void PulseIn()
    {
        LeanTween.scale(gameObject, initialScale, TIME).setIgnoreTimeScale(true).setOnComplete(PulseOut);
    }
    private void OnDestroy()
    {
        LeanTween.cancel(gameObject);
    }
    public void Pause(bool v)
    {
        if (v)
        {
            LeanTween.cancel(gameObject);
            LeanTween.scale(gameObject, initialScale, 0).setIgnoreTimeScale(true);
        }
        else
        {
            PulseIn();
        }
    }
}
