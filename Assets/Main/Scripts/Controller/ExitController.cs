using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitController : MonoBehaviour
{
    public Transform doorRotation;
    public GameObject triggerExit;
    public ParticleSystem particles;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
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
        
    }

   public void OnOpen()
    {
        audioSource.Play();
        particles.Play();
        triggerExit.SetActive(true);
        LeanTween.rotate(doorRotation.gameObject, Vector3.up * -120,1);
    }

   
}
