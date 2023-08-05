using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBehaviour : MonoBehaviour
{
    public string triggerBy = "";
    public bool onEnable = false;
    public bool onStart = false;
    private const float SCALE = 1.4f;
    private const float SCALE_IN_TIME = 0.7f;
    private const float SCALE_OUT_TIME = 0.7f;
    private const float WIGGLE_TIME = 0.16f;
    private const float WIGGLE_TO = 20f;
  
    private short side = 1;
    private int wiggleTimes;
    private Vector3 initialScale;
    private bool started = false;
    // Start is called before the first frame update

    private void Awake()
    {
        initialScale = gameObject.transform.localScale;
    }
    void Start()
    {
        if (onStart)
        {
            ShowAnimation();
        }
        if (triggerBy != "")
        {
            EventManager.StartListening(triggerBy, ShowAnimation);
        }
    }
    private void OnEnable()
    {
        if (onEnable)
        {
            ShowAnimation();
        }
    }

    private void ShowAnimation(EventData arg0)
    {
        ShowAnimation();
    }

    // Update is called once per frame
    public void ShowAnimation()
    {
        if (!started)
        {
            wiggleTimes = Mathf.FloorToInt((SCALE_IN_TIME + SCALE_OUT_TIME) / WIGGLE_TIME);
            LeanTween.scale(gameObject, initialScale * SCALE, SCALE_IN_TIME).setIgnoreTimeScale(true);
            LeanTween.scale(gameObject, initialScale, SCALE_OUT_TIME).setDelay(SCALE_IN_TIME).setIgnoreTimeScale(true).setOnComplete(AddPulse);
            started = true;
            GoWiggle();
        }
    }

    private void AddPulse()
    {
        if (!gameObject.GetComponent<Pulse>())
        {
            var pulse = gameObject.AddComponent<Pulse>();
            pulse.SCALE_MULTIPLIER = 1.1f;
            pulse.TIME = 0.75f;
        }
    }

    public void GoWiggle(int times)
    {
        wiggleTimes = times;
        GoWiggle();
    }
    private void GoWiggle()
    {
        wiggleTimes--;
        if (wiggleTimes > 0)
        {
            LeanTween.rotateZ(this.gameObject, WIGGLE_TO * side, WIGGLE_TIME).setIgnoreTimeScale(true).setOnComplete(GoWiggle);
            side *= -1;
        }
        else
        {
            LeanTween.rotateZ(this.gameObject,0, WIGGLE_TIME).setIgnoreTimeScale(true);

        }
    }
    private void OnDestroy()
    {
        LeanTween.cancel(gameObject);
        if (triggerBy!="")
        {
            EventManager.StopListening(triggerBy, ShowAnimation);

        }
    }
}
