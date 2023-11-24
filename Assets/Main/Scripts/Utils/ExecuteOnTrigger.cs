using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecuteOnTrigger : MonoBehaviour
{
    public string eventName;
    public ParticleSystem particle;
    public AudioSource audioSource;
    public float shake;

    private void Start()
    {
        EventManager.StartListening(eventName, OnTrigger);
    }
    protected  void OnTrigger(EventData arg0)
    {
        if (particle != null)
        {
            particle.Play();
        }
        if (audioSource != null)
        {
            audioSource.Play();
        }
        EventManager.TriggerEvent(EventName.SHAKE_CAM_POS, EventManager.Instance.GetEventData().SetFloat(shake));

    }
    private void OnDestroy()
    {
        EventManager.StopListening(eventName, OnTrigger);

    }
}
