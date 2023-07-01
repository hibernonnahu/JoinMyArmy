using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitController : MonoBehaviour
{
    public GameObject triggerExit;
    public ParticleSystem particles;
    private AudioSource audioSource;
    private bool open = false;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            throw new Exception("No audioSource on exit " );
        }
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
            open = true;
            audioSource.Play();
            particles.Play();
            Animation();
        }
    }

    protected virtual void Animation()
    {
       
    }

    protected void OnComplete()
    {
        triggerExit.SetActive(true);
    }
    private void OnDestroy()
    {
        EventManager.StopListening(EventName.EXIT_OPEN, OnExitOpen);

    }
}
