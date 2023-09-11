using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitController : MonoBehaviour
{
    public GameObject[] hideOnOpen;
    public GameObject triggerExit;
    public ParticleSystem particles;
    private AudioSource audioSource;
    private bool open = false;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (triggerExit.active)
        {
            throw new Exception("exit already enabled ");
        }
        EventManager.StartListening(EventName.EXIT_OPEN, OnExitOpen);

    }

    private void OnExitOpen(EventData arg0)
    {
        OnOpen();
    }

    public void OnOpen()
    {
        if (!open)
        {
            foreach (var item in hideOnOpen)
            {
                item.SetActive(false);
            }
            open = true;
            if (audioSource != null)
                audioSource.Play();
            particles.Play();
            Animation();
        }
    }

    protected virtual void Animation()
    {
        OnComplete();
    }

    protected void OnComplete()
    {
        triggerExit.SetActive(true);
    }
    private void OnDestroy()
    {
        EventManager.StopListening(EventName.EXIT_OPEN, OnExitOpen);

    }

    internal void Pause(bool v)
    {

        triggerExit.GetComponent<ExitTrigger>().pause = v;
    }
}
