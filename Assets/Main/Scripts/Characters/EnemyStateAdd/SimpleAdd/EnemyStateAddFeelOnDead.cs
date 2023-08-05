using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateAddFeelOnDead : EnemyStateOnDead
{
    public ParticleSystem particle;
    public AudioSource audioSource;
    public float shake;
  
    protected override void ExecuteOnDead()
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

}
